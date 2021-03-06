#pragma once
#include "stdafx.h"
#include "DebugTypes.h"
#include "../Utilities/ISerializable.h"

class Gameboy;
class GbCart;
class GbPpu;
class GbApu;
class GbTimer;

class EmuSettings;
class Console;
class ControlManager;

class GbMemoryManager : public ISerializable
{
private:
	Console* _console = nullptr;
	EmuSettings* _settings = nullptr;
	ControlManager* _controlManager = nullptr;

	Gameboy* _gameboy = nullptr;
	GbCart* _cart = nullptr;
	GbApu* _apu = nullptr;
	GbPpu* _ppu = nullptr;
	GbTimer* _timer;

	uint8_t* _prgRom = nullptr;
	uint32_t _prgRomSize = 0;
	uint8_t* _workRam = nullptr;
	uint32_t _workRamSize = 0;
	uint8_t* _cartRam = nullptr;
	uint32_t _cartRamSize = 0;
	uint8_t* _highRam = nullptr;
	
	uint8_t* _reads[0x100] = {};
	uint8_t* _writes[0x100] = {};

	GbMemoryManagerState _state;

public:
	virtual ~GbMemoryManager();

	GbMemoryManagerState GetState();

	void Init(Console* console, Gameboy* gameboy, GbCart* cart, GbPpu* ppu, GbApu* apu, GbTimer* timer);
	void MapRegisters(uint16_t start, uint16_t end, RegisterAccess access);
	void Map(uint16_t start, uint16_t end, GbMemoryType type, uint32_t offset, bool readonly);
	void Unmap(uint16_t start, uint16_t end);
	void RefreshMappings();

	void Exec();

	uint8_t Read(uint16_t addr, MemoryOperationType opType);
	void Write(uint16_t addr, uint8_t value);

	uint8_t ReadRegister(uint16_t addr);
	void WriteRegister(uint16_t addr, uint8_t value);

	void RequestIrq(uint8_t source);
	void ClearIrqRequest(uint8_t source);
	uint8_t ProcessIrqRequests();

	void ToggleSpeed();
	bool IsHighSpeed();
	uint64_t GetApuCycleCount();
	
	uint8_t ReadInputPort();

	uint8_t DebugRead(uint16_t addr);
	void DebugWrite(uint16_t addr, uint8_t value);

	uint8_t* GetMappedBlock(uint16_t addr);

	void Serialize(Serializer& s) override;
};
