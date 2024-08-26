#pragma once
#include "stdafx.h"
#include "DebugTypes.h"
#include "IDebugger.h"

class Disassembler;
class Debugger;
class TraceLogger;
class Gameboy;
class CallstackManager;
class MemoryAccessCounter;
class Console;
class BreakpointManager;
class EmuSettings;
class GbEventManager;
class GbAssembler;
class CodeDataLogger;

class GbDebugger final : public IDebugger
{
	Debugger* _debugger;
	Console* _console;
	Disassembler* _disassembler;
	TraceLogger* _traceLogger;
	MemoryAccessCounter* _memoryAccessCounter;
	Gameboy* _gameboy;
	EmuSettings* _settings;

	shared_ptr<GbEventManager> _eventManager;
	shared_ptr<CallstackManager> _callstackManager;
	shared_ptr<CodeDataLogger> _codeDataLogger;
	unique_ptr<BreakpointManager> _breakpointManager;
	unique_ptr<StepRequest> _step;
	shared_ptr<GbAssembler> _assembler;

	uint8_t _prevOpCode = 0xFF;
	uint32_t _prevProgramCounter = 0;
	bool _enableBreakOnUninitRead = false;

public:
	GbDebugger(Debugger* debugger);
	~GbDebugger();

	void Reset();

	void ProcessRead(uint16_t addr, uint8_t value, MemoryOperationType type);
	void ProcessWrite(uint16_t addr, uint8_t value, MemoryOperationType type);
	void Run();
	void Step(int32_t stepCount, StepType type);
	void ProcessInterrupt(uint32_t originalPc, uint32_t currentPc);
	void ProcessPpuCycle(uint16_t scanline, uint16_t cycle);

	shared_ptr<GbEventManager> GetEventManager();
	shared_ptr<GbAssembler> GetAssembler();
	shared_ptr<CallstackManager> GetCallstackManager();
	shared_ptr<CodeDataLogger> GetCodeDataLogger();
	BreakpointManager* GetBreakpointManager();
};