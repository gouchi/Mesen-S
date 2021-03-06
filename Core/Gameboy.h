#pragma once
#include "stdafx.h"
#include "DebugTypes.h"
#include "../Utilities/ISerializable.h"

class Console;
class GbPpu;
class GbApu;
class GbCpu;
class GbCart;
class GbTimer;
class GbMemoryManager;
class VirtualFile;

class Gameboy : public ISerializable
{
private:
	static constexpr int SpriteRamSize = 0xA0;
	static constexpr int HighRamSize = 0x7F;

	Console* _console;

	unique_ptr<GbMemoryManager> _memoryManager;
	unique_ptr<GbCpu> _cpu;
	unique_ptr<GbPpu> _ppu;
	unique_ptr<GbApu> _apu;
	unique_ptr<GbCart> _cart;
	unique_ptr<GbTimer> _timer;

	bool _hasBattery;
	bool _cgbMode;

	uint8_t* _prgRom;
	uint32_t _prgRomSize;

	uint8_t* _cartRam;
	uint32_t _cartRamSize;

	uint8_t* _workRam;
	uint32_t _workRamSize;

	uint8_t* _videoRam;
	uint32_t _videoRamSize;

	uint8_t* _spriteRam;
	uint8_t* _highRam;

public:
	static Gameboy* Create(Console* console, VirtualFile& romFile);
	virtual ~Gameboy();

	void PowerOn();

	void Exec();
	void Run(uint64_t masterClock);
	
	void LoadBattery();
	void SaveBattery();

	GbPpu* GetPpu();
	GbCpu* GetCpu();
	GbState GetState();

	uint32_t DebugGetMemorySize(SnesMemoryType type);
	uint8_t* DebugGetMemory(SnesMemoryType type);
	GbMemoryManager* GetMemoryManager();
	AddressInfo GetAbsoluteAddress(uint16_t addr);
	int32_t GetRelativeAddress(AddressInfo& absAddress);

	bool IsCgb();
	uint64_t GetCycleCount();
	uint64_t GetApuCycleCount();

	void Serialize(Serializer& s) override;
};