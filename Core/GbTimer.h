#pragma once
#include "stdafx.h"
#include "../Utilities/ISerializable.h"
#include "../Utilities/Serializer.h"
#include "GbTypes.h"

class GbMemoryManager;
class GbApu;

class GbTimer : public ISerializable
{
private:
	GbMemoryManager* _memoryManager = nullptr;
	GbApu* _apu = nullptr;
	GbTimerState _state = {};
	
	void SetDivider(uint16_t value);
	void ReloadCounter();

public:
	virtual ~GbTimer();

	void Init(GbMemoryManager* memoryManager, GbApu* apu);

	GbTimerState GetState();

	void Exec();

	uint8_t Read(uint16_t addr);
	void Write(uint16_t addr, uint8_t value);

	void Serialize(Serializer& s) override;
};