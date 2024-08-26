#include "stdafx.h"
#include "EventManager.h"
#include "DebugTypes.h"
#include "Cpu.h"
#include "Ppu.h"
#include "DmaController.h"
#include "MemoryManager.h"
#include "Debugger.h"
#include "DebugBreakHelper.h"
#include "DefaultVideoFilter.h"
#include "BaseEventManager.h"

EventManager::EventManager(Debugger *debugger, Cpu *cpu, Ppu *ppu, MemoryManager *memoryManager, DmaController *dmaController)
{
	_debugger = debugger;
	_cpu = cpu;
	_ppu = ppu;
	_memoryManager = memoryManager;
	_dmaController = dmaController;

	_ppuBuffer = new uint16_t[512 * 478];
	memset(_ppuBuffer, 0, 512 * 478 * sizeof(uint16_t));
}

EventManager::~EventManager()
{
	delete[] _ppuBuffer;
}

void EventManager::AddEvent(DebugEventType type, MemoryOperationInfo &operation, int32_t breakpointId)
{
	DebugEventInfo evt = {};
	evt.Type = type;
	evt.Operation = operation;
	evt.Scanline = _ppu->GetScanline();
	evt.Cycle = _memoryManager->GetHClock();
	evt.BreakpointId = breakpointId;

	if(operation.Type == MemoryOperationType::DmaRead || operation.Type == MemoryOperationType::DmaWrite) {
		evt.DmaChannel = _dmaController->GetActiveChannel();
		evt.DmaChannelInfo = _dmaController->GetChannelConfig(evt.DmaChannel & 0x07);
	} else {
		evt.DmaChannel = -1;
	}

	CpuState state = _cpu->GetState();
	evt.ProgramCounter = (state.K << 16) | state.PC;

	_debugEvents.push_back(evt);
}

void EventManager::AddEvent(DebugEventType type)
{
	DebugEventInfo evt = {};
	evt.Type = type;
	evt.Scanline = _ppu->GetScanline();
	evt.Cycle = _memoryManager->GetHClock();
	evt.BreakpointId = -1;
	evt.DmaChannel = -1;
	
	CpuState state = _cpu->GetState();
	evt.ProgramCounter = (state.K << 16) | state.PC;

	_debugEvents.push_back(evt);
}

void EventManager::GetEvents(DebugEventInfo *eventArray, uint32_t &maxEventCount)
{
	auto lock = _lock.AcquireSafe();
	uint32_t eventCount = std::min(maxEventCount, (uint32_t)_sentEvents.size());
	memcpy(eventArray, _sentEvents.data(), eventCount * sizeof(DebugEventInfo));
	maxEventCount = eventCount;
}

DebugEventInfo EventManager::GetEvent(uint16_t scanline, uint16_t cycle, EventViewerDisplayOptions &options)
{
	auto lock = _lock.AcquireSafe();

	for(DebugEventInfo &evt : _sentEvents) {
		if(evt.Cycle == cycle && evt.Scanline == scanline) {
			return evt;
		}
	}

	DebugEventInfo empty = {};
	empty.ProgramCounter = 0xFFFFFFFF;
	return empty;
}

uint32_t EventManager::GetEventCount(EventViewerDisplayOptions options)
{
	auto lock = _lock.AcquireSafe();
	FilterEvents(options);
	return (uint32_t)_sentEvents.size();
}

void EventManager::ClearFrameEvents()
{
	_prevDebugEvents = _debugEvents;
	_debugEvents.clear();
}

void EventManager::FilterEvents(EventViewerDisplayOptions &options)
{
	auto lock = _lock.AcquireSafe();
	_sentEvents.clear();

	vector<DebugEventInfo> events = _snapshot;
	if(options.ShowPreviousFrameEvents && _snapshotScanline != 0) {
		uint32_t key = (_snapshotScanline << 16) + _snapshotCycle;
		for(DebugEventInfo &evt : _prevDebugEvents) {
			uint32_t evtKey = (evt.Scanline << 16) + evt.Cycle;
			if(evtKey > key) {
				events.push_back(evt);
			}
		}
	}

	for(DebugEventInfo &evt : events) {
		bool isWrite = evt.Operation.Type == MemoryOperationType::Write || evt.Operation.Type == MemoryOperationType::DmaWrite;
		bool isDma = evt.Operation.Type == MemoryOperationType::DmaWrite || evt.Operation.Type == MemoryOperationType::DmaRead;
		bool showEvent = false;
		switch(evt.Type) {
			case DebugEventType::Breakpoint: showEvent = options.ShowMarkedBreakpoints;break;
			case DebugEventType::Irq: showEvent = options.ShowIrq; break;
			case DebugEventType::Nmi: showEvent = options.ShowNmi; break;
			case DebugEventType::Register:
				if(isDma && !options.ShowDmaChannels[evt.DmaChannel & 0x07]) {
					showEvent = false;
					break;
				}

				uint16_t reg = evt.Operation.Address & 0xFFFF;
				if(reg <= 0x213F) {
					if(isWrite) {
						if(reg >= 0x2101 && reg <= 0x2104) {
							showEvent = options.ShowPpuRegisterOamWrites;
						} else if(reg >= 0x2105 && reg <= 0x210C) {
							showEvent = options.ShowPpuRegisterBgOptionWrites;
						} else if(reg >= 0x210D && reg <= 0x2114) {
							showEvent = options.ShowPpuRegisterBgScrollWrites;
						} else if(reg >= 0x2115 && reg <= 0x2119) {
							showEvent = options.ShowPpuRegisterVramWrites;
						} else if(reg >= 0x211A && reg <= 0x2120) {
							showEvent = options.ShowPpuRegisterMode7Writes;
						} else if(reg >= 0x2121 && reg <= 0x2122) {
							showEvent = options.ShowPpuRegisterCgramWrites;
						} else if(reg >= 0x2123 && reg <= 0x212B) {
							showEvent = options.ShowPpuRegisterWindowWrites;
						} else {
							showEvent = options.ShowPpuRegisterOtherWrites;
						}
					} else {
						showEvent = options.ShowPpuRegisterReads;
					}
				} else if(reg <= 0x217F) {
					showEvent = isWrite ? options.ShowApuRegisterWrites : options.ShowApuRegisterReads;
				} else if(reg <= 0x2183) {
					showEvent = isWrite ? options.ShowWorkRamRegisterWrites : options.ShowWorkRamRegisterReads;
				} else if(reg >= 0x4000) {
					showEvent = isWrite ? options.ShowCpuRegisterWrites : options.ShowCpuRegisterReads;
				}
				break;
		}

		if(showEvent) {
			_sentEvents.push_back(evt);
		}
	}
}

void EventManager::DrawEvent(DebugEventInfo &evt, bool drawBackground, uint32_t *buffer, EventViewerDisplayOptions &options)
{
	bool isWrite = evt.Operation.Type == MemoryOperationType::Write || evt.Operation.Type == MemoryOperationType::DmaWrite;
	uint32_t color = 0;
	switch(evt.Type) {
		case DebugEventType::Breakpoint: color = options.BreakpointColor; break;
		case DebugEventType::Irq: color = options.IrqColor; break;
		case DebugEventType::Nmi: color = options.NmiColor; break;
		case DebugEventType::Register:
			uint16_t reg = evt.Operation.Address & 0xFFFF;
			if(reg <= 0x213F) {
				if(isWrite) {
					if(reg >= 0x2101 && reg <= 0x2104) {
						color = options.PpuRegisterWriteOamColor;
					} else if(reg >= 0x2105 && reg <= 0x210C) {
						color = options.PpuRegisterWriteBgOptionColor;
					} else if(reg >= 0x210D && reg <= 0x2114) {
						color = options.PpuRegisterWriteBgScrollColor;
					} else if(reg >= 0x2115 && reg <= 0x2119) {
						color = options.PpuRegisterWriteVramColor;
					} else if(reg >= 0x211A && reg <= 0x2120) {
						color = options.PpuRegisterWriteMode7Color;
					} else if(reg >= 0x2121 && reg <= 0x2122) {
						color = options.PpuRegisterWriteCgramColor;
					} else if(reg >= 0x2123 && reg <= 0x212B) {
						color = options.PpuRegisterWriteWindowColor;
					} else {
						color = options.PpuRegisterWriteOtherColor;
					}
				} else {
					color = options.PpuRegisterReadColor;
				}
			} else if(reg <= 0x217F) {
				color = isWrite ? options.ApuRegisterWriteColor : options.ApuRegisterReadColor;
			} else if(reg <= 0x2183) {
				color = isWrite ? options.WorkRamRegisterWriteColor : options.WorkRamRegisterReadColor;
			} else if(reg >= 0x4000) {
				color = isWrite ? options.CpuRegisterWriteColor : options.CpuRegisterReadColor;
			}
			break;
	}

	if(drawBackground){
		color = 0xFF000000 | ((color >> 1) & 0x7F7F7F);
	} else {
		color |= 0xFF000000;
	}

	int iMin = drawBackground ? -2 : 0;
	int iMax = drawBackground ? 3 : 1;
	int jMin = drawBackground ? -2 : 0;
	int jMax = drawBackground ? 3 : 1;
	uint32_t y = std::min<uint32_t>(evt.Scanline * 2, _scanlineCount * 2);
	uint32_t x = evt.Cycle / 2;

	for(int i = iMin; i <= iMax; i++) {
		for(int j = jMin; j <= jMax; j++) {
			int32_t pos = (y + i) * EventManager::ScanlineWidth + x + j;
			if(pos < 0 || pos >= EventManager::ScanlineWidth * (int)_scanlineCount * 2) {
				continue;
			}
			buffer[pos] = color;
		}
	}
}

uint32_t EventManager::TakeEventSnapshot(EventViewerDisplayOptions options)
{
	DebugBreakHelper breakHelper(_debugger);
	auto lock = _lock.AcquireSafe();
	_snapshot.clear();

	uint16_t cycle = _memoryManager->GetHClock();
	uint16_t scanline = _ppu->GetScanline();

	_overscanMode = _ppu->GetState().OverscanMode;
	_useHighResOutput = _ppu->IsHighResOutput();

	if(scanline >= _ppu->GetNmiScanline() || scanline == 0) {
		memcpy(_ppuBuffer, _ppu->GetScreenBuffer(), (_useHighResOutput ? (512 * 478) : (256*239)) * sizeof(uint16_t));
	} else {
		uint16_t adjustedScanline = scanline + (_overscanMode ? 0 : 7);
		uint32_t size = _useHighResOutput ? (512 * 478) : (256 * 239);
		uint32_t offset = _useHighResOutput ? (512 * adjustedScanline * 2) : (256 * adjustedScanline);
		memcpy(_ppuBuffer, _ppu->GetScreenBuffer(), offset * sizeof(uint16_t));
		memcpy(_ppuBuffer+offset, _ppu->GetPreviousScreenBuffer()+offset, (size - offset) * sizeof(uint16_t));
	}

	_snapshot = _debugEvents;
	_snapshotScanline = scanline;
	_snapshotCycle = cycle;
	_scanlineCount = _ppu->GetVblankEndScanline() + 1;
	return _scanlineCount;
}

void EventManager::GetDisplayBuffer(uint32_t *buffer, uint32_t bufferSize, EventViewerDisplayOptions options)
{
	auto lock = _lock.AcquireSafe();

	if(_snapshotScanline < 0 || bufferSize < _scanlineCount * 2 * EventManager::ScanlineWidth * 4) {
		return;
	}

	for(int i = 0; i < EventManager::ScanlineWidth * (int)_scanlineCount * 2; i++) {
		buffer[i] = 0xFF555555;
	}

	//Skip the first 7 blank lines in the buffer when overscan mode is off
	uint16_t *src = _ppuBuffer + (_overscanMode ? 0 : (_useHighResOutput ? (512 * 14) : (256 * 7)));

	for(uint32_t y = 0, len = _overscanMode ? 239*2 : 224*2; y < len; y++) {
		for(uint32_t x = 0; x < 512; x++) {
			int srcOffset = _useHighResOutput ? ((y << 9) | x) : (((y >> 1) << 8) | (x >> 1));
			buffer[(y + 2)*EventManager::ScanlineWidth + x + 22*2] = DefaultVideoFilter::ToArgb(src[srcOffset]);
		}
	}

	constexpr uint32_t nmiColor = 0xFF55FFFF;
	constexpr uint32_t currentScanlineColor = 0xFFFFFF55;
	int nmiScanline = (_overscanMode ? 240 : 225) * 2 * EventManager::ScanlineWidth;
	uint32_t scanlineOffset = _snapshotScanline * 2 * EventManager::ScanlineWidth;
	for(int i = 0; i < EventManager::ScanlineWidth; i++) {
		buffer[nmiScanline + i] = nmiColor;
		buffer[nmiScanline + EventManager::ScanlineWidth + i] = nmiColor;
		if(_snapshotScanline != 0) {
			buffer[scanlineOffset + i] = currentScanlineColor;
			buffer[scanlineOffset + EventManager::ScanlineWidth + i] = currentScanlineColor;
		}
	}

	FilterEvents(options);
	for(DebugEventInfo &evt : _sentEvents) {
		DrawEvent(evt, true, buffer, options);
	}
	for(DebugEventInfo &evt : _sentEvents) {
		DrawEvent(evt, false, buffer, options);
	}
}
