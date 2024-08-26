#include "stdafx.h"
#include "GbDebugger.h"
#include "DisassemblyInfo.h"
#include "Disassembler.h"
#include "Gameboy.h"
#include "TraceLogger.h"
#include "CallstackManager.h"
#include "BreakpointManager.h"
#include "MemoryManager.h"
#include "Debugger.h"
#include "Console.h"
#include "MemoryAccessCounter.h"
#include "ExpressionEvaluator.h"
#include "EmuSettings.h"
#include "BaseCartridge.h"
#include "GameboyDisUtils.h"
#include "CodeDataLogger.h"
#include "GbEventManager.h"
#include "BaseEventManager.h"
#include "GbAssembler.h"
#include "../Utilities/HexUtilities.h"

GbDebugger::GbDebugger(Debugger* debugger)
{
	_debugger = debugger;
	_console = debugger->GetConsole().get();
	_traceLogger = debugger->GetTraceLogger().get();
	_disassembler = debugger->GetDisassembler().get();
	_memoryAccessCounter = debugger->GetMemoryAccessCounter().get();
	_gameboy = debugger->GetConsole()->GetCartridge()->GetGameboy();
	_settings = debugger->GetConsole()->GetSettings().get();
	_codeDataLogger.reset(new CodeDataLogger(_gameboy->DebugGetMemorySize(SnesMemoryType::GbPrgRom), CpuType::Gameboy));

	_eventManager.reset(new GbEventManager(debugger, _gameboy->GetCpu(), _gameboy->GetPpu()));
	_callstackManager.reset(new CallstackManager(debugger));
	_breakpointManager.reset(new BreakpointManager(debugger, CpuType::Gameboy, _eventManager.get()));
	_step.reset(new StepRequest());
	_assembler.reset(new GbAssembler(debugger->GetLabelManager()));

	if(_gameboy->GetState().MemoryManager.ApuCycleCount == 0) {
		//Enable breaking on uninit reads when debugger is opened at power on
		_enableBreakOnUninitRead = true;
	}
}

GbDebugger::~GbDebugger()
{
}

void GbDebugger::Reset()
{
	_callstackManager.reset(new CallstackManager(_debugger));
	_prevOpCode = 0;
}

void GbDebugger::ProcessRead(uint16_t addr, uint8_t value, MemoryOperationType type)
{
	AddressInfo addressInfo = _gameboy->GetAbsoluteAddress(addr);
	MemoryOperationInfo operation { addr, value, type };
	BreakSource breakSource = BreakSource::Unspecified;

	if(type == MemoryOperationType::ExecOpCode) {
		GbCpuState gbState = _gameboy->GetState().Cpu;

		if(_traceLogger->IsCpuLogged(CpuType::Gameboy) || _settings->CheckDebuggerFlag(DebuggerFlags::GbDebuggerEnabled)) {
			if(addressInfo.Address >= 0) {
				if(addressInfo.Type == SnesMemoryType::GbPrgRom) {
					_codeDataLogger->SetFlags(addressInfo.Address, CdlFlags::Code);
				}
				_disassembler->BuildCache(addressInfo, 0, CpuType::Gameboy);
			}

			if(_traceLogger->IsCpuLogged(CpuType::Gameboy)) {
				DebugState debugState;
				_debugger->GetState(debugState, true);

				DisassemblyInfo disInfo = _disassembler->GetDisassemblyInfo(addressInfo, addr, 0, CpuType::Gameboy);
				_traceLogger->Log(CpuType::Gameboy, debugState, disInfo);
			}
		}

		if(GameboyDisUtils::IsJumpToSub(_prevOpCode) && gbState.PC != _prevProgramCounter + GameboyDisUtils::GetOpSize(_prevOpCode)) {
			//CALL and RST, and PC doesn't match the next instruction, so the call was (probably) done
			uint8_t opSize = DisassemblyInfo::GetOpSize(_prevOpCode, 0, CpuType::Gameboy);
			uint16_t returnPc = _prevProgramCounter + opSize;
			AddressInfo src = _gameboy->GetAbsoluteAddress(_prevProgramCounter);
			AddressInfo ret = _gameboy->GetAbsoluteAddress(returnPc);
			_callstackManager->Push(src, _prevProgramCounter, addressInfo, gbState.PC, ret, returnPc, StackFrameFlags::None);
		} else if(GameboyDisUtils::IsReturnInstruction(_prevOpCode) && gbState.PC != _prevProgramCounter + GameboyDisUtils::GetOpSize(_prevOpCode)) {
			//RET used, and PC doesn't match the next instruction, so the ret was (probably) taken
			_callstackManager->Pop(addressInfo, gbState.PC);
		}

		if(_step->BreakAddress == (int32_t)gbState.PC && GameboyDisUtils::IsReturnInstruction(_prevOpCode)) {
			//RET/RETI found, if we're on the expected return address, break immediately (for step over/step out)
			_step->StepCount = 0;
		}

		if(_settings->CheckDebuggerFlag(DebuggerFlags::GbDebuggerEnabled)) {
			bool needBreak = false;
			switch(value) {
				case 0x40:
					needBreak = _settings->CheckDebuggerFlag(DebuggerFlags::GbBreakOnNopLoad);
					breakSource = BreakSource::GbNopLoad;
					break;

				case 0xD3: case 0xDB: case 0xDD: case 0xE3: case 0xE4: case 0xEB: case 0xEC: case 0xED: case 0xF4: case 0xFC: case 0xFD:
					needBreak = _settings->CheckDebuggerFlag(DebuggerFlags::GbBreakOnInvalidOpCode);
					breakSource = BreakSource::GbInvalidOpCode;
					break;
			}

			if(needBreak) {
				_step->StepCount = 0;
			}
		}

		_prevOpCode = value;
		_prevProgramCounter = gbState.PC;

		if(_step->StepCount > 0) {
			_step->StepCount--;
		}

		_memoryAccessCounter->ProcessMemoryExec(addressInfo, _console->GetMasterClock());
	} else if(type == MemoryOperationType::ExecOperand) {
		if(addressInfo.Address >= 0 && addressInfo.Type == SnesMemoryType::GbPrgRom) {
			_codeDataLogger->SetFlags(addressInfo.Address, CdlFlags::Code);
		}
		_memoryAccessCounter->ProcessMemoryExec(addressInfo, _console->GetMasterClock());
	} else {
		if(addressInfo.Address >= 0 && addressInfo.Type == SnesMemoryType::GbPrgRom) {
			_codeDataLogger->SetFlags(addressInfo.Address, CdlFlags::Data);
		}

		if(addr < 0xFE00 || addr >= 0xFF80) {
			if(_memoryAccessCounter->ProcessMemoryRead(addressInfo, _console->GetMasterClock())) {
				//Memory access was a read on an uninitialized memory address
				if(_enableBreakOnUninitRead) {
					if(_memoryAccessCounter->GetReadCount(addressInfo) == 1) {
						//Only warn the first time
						_debugger->Log("[GB] Uninitialized memory read: $" + HexUtilities::ToHex(addr));
					}
					if(_settings->CheckDebuggerFlag(DebuggerFlags::GbDebuggerEnabled) && _settings->CheckDebuggerFlag(DebuggerFlags::BreakOnUninitRead)) {
						breakSource = BreakSource::BreakOnUninitMemoryRead;
						_step->StepCount = 0;
					}
				}
			}
		}

		if(addr == 0xFFFF || (addr >= 0xFE00 && addr < 0xFF80) || (addr >= 0x8000 && addr <= 0x9FFF)) {
			_eventManager->AddEvent(DebugEventType::Register, operation);
		}
	}

	_debugger->ProcessBreakConditions(_step->StepCount == 0, GetBreakpointManager(), operation, addressInfo, breakSource);
}

void GbDebugger::ProcessWrite(uint16_t addr, uint8_t value, MemoryOperationType type)
{
	AddressInfo addressInfo = _gameboy->GetAbsoluteAddress(addr);
	MemoryOperationInfo operation { addr, value, type };
	_debugger->ProcessBreakConditions(false, GetBreakpointManager(), operation, addressInfo);

	if(addressInfo.Type == SnesMemoryType::GbWorkRam || addressInfo.Type == SnesMemoryType::GbCartRam || addressInfo.Type == SnesMemoryType::GbHighRam) {
		_disassembler->InvalidateCache(addressInfo, CpuType::Gameboy);
	}

	if(addr == 0xFFFF || (addr >= 0xFE00 && addr < 0xFF80) || (addr >= 0x8000 && addr <= 0x9FFF)) {
		_eventManager->AddEvent(DebugEventType::Register, operation);
	}

	_memoryAccessCounter->ProcessMemoryWrite(addressInfo, _console->GetMasterClock());
}

void GbDebugger::Run()
{
	_step.reset(new StepRequest());
}

void GbDebugger::Step(int32_t stepCount, StepType type)
{
	StepRequest step;

	GbCpuState gbState = _gameboy->GetState().Cpu;
	if((type == StepType::StepOver || type == StepType::StepOut || type == StepType::Step) && gbState.Halted) {
		//CPU isn't running - use the PPU to break execution instead
		step.PpuStepCount = 1;
	} else {
		switch(type) {
			case StepType::Step: step.StepCount = stepCount; break;
			case StepType::StepOut: step.BreakAddress = _callstackManager->GetReturnAddress(); break;
			case StepType::StepOver:
				if(GameboyDisUtils::IsJumpToSub(_prevOpCode)) {
					step.BreakAddress = _prevProgramCounter + DisassemblyInfo::GetOpSize(_prevOpCode, 0, CpuType::Gameboy);
				} else {
					//For any other instruction, step over is the same as step into
					step.StepCount = 1;
				}
				break;

			case StepType::PpuStep: step.PpuStepCount = stepCount; break;
			case StepType::SpecificScanline: step.BreakScanline = stepCount; break;
		}
	}
	_step.reset(new StepRequest(step));
}

void GbDebugger::ProcessInterrupt(uint32_t originalPc, uint32_t currentPc)
{
	AddressInfo src = _gameboy->GetAbsoluteAddress(_prevProgramCounter);
	AddressInfo ret = _gameboy->GetAbsoluteAddress(originalPc);
	AddressInfo dest = _gameboy->GetAbsoluteAddress(currentPc);
	_callstackManager->Push(src, _prevProgramCounter, dest, currentPc, ret, originalPc, StackFrameFlags::Irq);
	_eventManager->AddEvent(DebugEventType::Irq);
}

void GbDebugger::ProcessPpuCycle(uint16_t scanline, uint16_t cycle)
{
	if(_step->PpuStepCount > 0) {
		_step->PpuStepCount--;
		if(_step->PpuStepCount == 0) {
			_debugger->SleepUntilResume(BreakSource::PpuStep);
		}
	}

	if(cycle == 0 && scanline == _step->BreakScanline) {
		_step->BreakScanline = -1;
		_debugger->SleepUntilResume(BreakSource::PpuStep);
	}
}

shared_ptr<GbEventManager> GbDebugger::GetEventManager()
{
	return _eventManager;
}

shared_ptr<GbAssembler> GbDebugger::GetAssembler()
{
	return _assembler;
}

shared_ptr<CallstackManager> GbDebugger::GetCallstackManager()
{
	return _callstackManager;
}

shared_ptr<CodeDataLogger> GbDebugger::GetCodeDataLogger()
{
	return _codeDataLogger;
}

BreakpointManager* GbDebugger::GetBreakpointManager()
{
	return _breakpointManager.get();
}