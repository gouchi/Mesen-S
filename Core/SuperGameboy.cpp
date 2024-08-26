#include "stdafx.h"
#include "SuperGameboy.h"
#include "Console.h"
#include "MemoryManager.h"
#include "EmuSettings.h"
#include "BaseCartridge.h"
#include "Spc.h"
#include "Gameboy.h"
#include "GbApu.h"
#include "GbPpu.h"
#include "MessageManager.h"
#include "../Utilities/HexUtilities.h"
#include "../Utilities/HermiteResampler.h"

SuperGameboy::SuperGameboy(Console* console) : BaseCoprocessor(SnesMemoryType::Register)
{
	_mixBuffer = new int16_t[0x10000];

	_console = console;
	_memoryManager = console->GetMemoryManager().get();
	_cart = _console->GetCartridge().get();
	_spc = _console->GetSpc().get();
	
	_gameboy = _cart->GetGameboy();
	_ppu = _gameboy->GetPpu();

	_control = 0x01; //Divider = 5, gameboy = not running
	UpdateClockRatio();
	
	MemoryMappings* cpuMappings = _memoryManager->GetMemoryMappings();
	for(int i = 0; i <= 0x3F; i++) {
		cpuMappings->RegisterHandler(i, i, 0x6000, 0x7FFF, this);
		cpuMappings->RegisterHandler(i + 0x80, i + 0x80, 0x6000, 0x7FFF, this);
	}

	_gameboy->PowerOn(this);
}

SuperGameboy::~SuperGameboy()
{
	delete[] _mixBuffer;
}

void SuperGameboy::Reset()
{
	_control = 0;
	_resetClock = 0;

	memset(_input, 0, sizeof(_input));
	_inputIndex = 0;

	_listeningForPacket = false;
	_waitForHigh = true;
	_packetReady = false;
	_inputWriteClock = 0;
	_inputValue = 0;
	memset(_packetData, 0, sizeof(_packetData));
	_packetByte = 0;
	_packetBit = 0;

	_lcdRowSelect = 0;
	_readPosition = 0;
	memset(_lcdBuffer, 0, sizeof(_lcdBuffer));
}

uint8_t SuperGameboy::Read(uint32_t addr)
{
	addr &= 0xF80F;
	
	if(addr >= 0x7000 && addr <= 0x700F) {
		_packetReady = false;
		return _packetData[addr & 0x0F];
	} else if(addr >= 0x7800 && addr <= 0x780F) {
		if(_readPosition >= 320) {
			//Return 0xFF for 320..511 and then wrap to 0
			_readPosition = (_readPosition + 1) & 0x1FF;
			return 0xFF;
		}

		uint8_t* start = _lcdBuffer[_lcdRowSelect];
		start += ((_readPosition >> 1) & 0x07) * 160;
		start += (_readPosition >> 4) * 8;

		uint8_t data = 0;
		uint8_t shift = _readPosition & 0x01;
		for(int i = 0; i < 8; i++) {
			data |= ((start[i] >> shift) & 0x01) << (7 - i);
		}
		_readPosition++;
		return data;
	} else {
		switch(addr & 0xFFFF) {
			case 0x6000: return (GetLcdRow() << 3) | GetLcdBufferRow();
			case 0x6002: return _packetReady;
			case 0x600F: return 0x21; //or 0x61
		}
	}

	return 0;
}

void SuperGameboy::Write(uint32_t addr, uint8_t value)
{
	addr &= 0xF80F;

	switch(addr & 0xFFFF) {
		case 0x6001:
			_lcdRowSelect = value & 0x03; 
			_readPosition = 0;
			break;

		case 0x6003: {
			if(!(_control & 0x80) && (value & 0x80)) {
				_resetClock = _memoryManager->GetMasterClock();
				_gameboy->PowerOn(this);
				_ppu = _gameboy->GetPpu();
			}
			_control = value;
			_inputIndex %= GetPlayerCount();

			UpdateClockRatio();
			break;
		}

		case 0x6004: _input[0] = value; break;
		case 0x6005: _input[1] = value; break;
		case 0x6006: _input[2] = value; break;
		case 0x6007: _input[3] = value; break;
	}
}

void SuperGameboy::ProcessInputPortWrite(uint8_t value)
{
	if(_inputValue == value) {
		return;
	}

	if(value == 0x00) {
		//Reset pulse
		_waitForHigh = true;
		_packetByte = 0;
		_packetBit = 0;
	} else if(_waitForHigh) {
		if(value == 0x10 || value == 0x20) {
			//Invalid sequence (should be 0x00 -> 0x30 -> 0x10/0x20 -> 0x30 -> 0x10/0x20, etc.)
			_waitForHigh = false;
			_listeningForPacket = false;
		} else if(value == 0x30) {
			_waitForHigh = false;
			_listeningForPacket = true;
		}
	} else if(_listeningForPacket) {
		if(value == 0x20) {
			//0 bit
			if(_packetByte >= 16 && _packetBit == 0) {
				_packetReady = true;
				_listeningForPacket = false;

				if(_console->IsDebugging()) {
					LogPacket();
				}
			} else {
				_packetData[_packetByte] &= ~(1 << _packetBit);
			}
			_packetBit++;
			if(_packetBit == 8) {
				_packetBit = 0;
				_packetByte++;
			}
		} else if(value == 0x10) {
			//1 bit
			if(_packetByte >= 16) {
				//Invalid bit
				_listeningForPacket = false;
			} else {
				_packetData[_packetByte] |= (1 << _packetBit);
				_packetBit++;
				if(_packetBit == 8) {
					_packetBit = 0;
					_packetByte++;
				}
			}
		}
		_waitForHigh = _listeningForPacket;
	} else if(!(_inputValue & 0x20) && (value & 0x20)) {
		_inputIndex = (_inputIndex + 1) % GetPlayerCount();
	}

	_inputValue = value;
	_inputWriteClock = _memoryManager->GetMasterClock();
}

void SuperGameboy::LogPacket()
{
	uint8_t commandId = _packetData[0] >> 3;
	string name;
	switch(commandId) {
		case 0: name = "PAL01"; break; //Set SGB Palette 0, 1 Data
		case 1: name = "PAL23"; break; //Set SGB Palette 2, 3 Data
		case 2: name = "PAL03"; break; //Set SGB Palette 0, 3 Data
		case 3: name = "PAL12"; break; //Set SGB Palette 1, 2 Data
		case 4: name = "ATTR_BLK"; break; //"Block" Area Designation Mode
		case 5: name = "ATTR_LIN"; break; //"Line" Area Designation Mode
		case 6: name = "ATTR_DIV"; break; //"Divide" Area Designation Mode
		case 7: name = "ATTR_CHR"; break; //"1CHR" Area Designation Mode
		case 8: name = "SOUND"; break; //Sound On / Off
		case 9: name = "SOU_TRN"; break; //Transfer Sound PRG / DATA
		case 0xA: name = "PAL_SET"; break; //Set SGB Palette Indirect
		case 0xB: name = "PAL_TRN"; break; //Set System Color Palette Data
		case 0xC: name = "ATRC_EN"; break; //Enable / disable Attraction Mode
		case 0xD: name = "TEST_EN"; break; //Speed Function
		case 0xE: name = "ICON_EN"; break; //SGB Function
		case 0xF: name = "DATA_SND"; break; //SUPER NES WRAM Transfer 1
		case 0x10: name = "DATA_TRN"; break; //SUPER NES WRAM Transfer 2
		case 0x11: name = "MLT_REG"; break; //Controller 2 Request
		case 0x12: name = "JUMP"; break; //Set SNES Program Counter
		case 0x13: name = "CHR_TRN"; break; //Transfer Character Font Data
		case 0x14: name = "PCT_TRN"; break; //Set Screen Data Color Data
		case 0x15: name = "ATTR_TRN"; break; //Set Attribute from ATF
		case 0x16: name = "ATTR_SET"; break; //Set Data to ATF
		case 0x17: name = "MASK_EN"; break; //Game Boy Window Mask
		case 0x18: name = "OBJ_TRN"; break; //Super NES OBJ Mode
		
		case 0x1E: name = "Header Data"; break;
		case 0x1F: name = "Header Data"; break;

		default: name = "Unknown"; break;
	}

	string log = "SGB Command: " + HexUtilities::ToHex(commandId) + " - " + name + " (Len: " + std::to_string(_packetData[0] & 0x07) + ") - ";
	for(int i = 0; i < 16; i++) {
		log += HexUtilities::ToHex(_packetData[i]) + " ";
	}
	_console->DebugLog(log);
}

void SuperGameboy::WriteLcdColor(uint8_t scanline, uint8_t pixel, uint8_t color)
{
	_lcdBuffer[GetLcdBufferRow()][(scanline & 0x07) * 160 + pixel] = color;
}

uint8_t SuperGameboy::GetLcdRow()
{
	uint8_t scanline = _ppu->GetScanline();
	uint8_t row = scanline / 8;
	if(row >= 18) {
		row = 0;
	}
	return row;
}

uint8_t SuperGameboy::GetLcdBufferRow()
{
	return (_ppu->GetFrameCount() * 18 + GetLcdRow()) & 0x03;
}

uint8_t SuperGameboy::GetPlayerCount()
{
	uint8_t playerCount = ((_control >> 4) & 0x03) + 1;
	if(playerCount >= 3) {
		//Unknown: 2 and 3 both mean 4 players?
		return 4;
	}
	return playerCount;
}

void SuperGameboy::MixAudio(uint32_t targetRate, int16_t* soundSamples, uint32_t sampleCount)
{
	int16_t* gbSamples = nullptr;
	uint32_t gbSampleCount = 0;
	_gameboy->GetSoundSamples(gbSamples, gbSampleCount);
	_resampler.SetSampleRates(GbApu::SampleRate, targetRate);
	
	int32_t outCount = (int32_t)_resampler.Resample(gbSamples, gbSampleCount, _mixBuffer + _mixSampleCount) * 2;
	_mixSampleCount += outCount;

	int32_t copyCount = (int32_t)std::min(_mixSampleCount, sampleCount*2);
	if(!_spc->IsMuted()) {
		for(int32_t i = 0; i < copyCount; i++) {
			soundSamples[i] += _mixBuffer[i];
		}
	}

	int32_t remainingSamples = (int32_t)_mixSampleCount - copyCount;
	if(remainingSamples > 0) {
		memmove(_mixBuffer, _mixBuffer + copyCount, remainingSamples*sizeof(int16_t));
		_mixSampleCount = remainingSamples;
	} else {
		_mixSampleCount = 0;
	}
}

void SuperGameboy::Run()
{
	if(!(_control & 0x80)) {
		return;
	}

	_gameboy->Run((uint64_t)((_memoryManager->GetMasterClock() - _resetClock) * _clockRatio));
}

void SuperGameboy::UpdateClockRatio()
{
	bool isSgb2 = _console->GetSettings()->GetGameboyConfig().UseSgb2;
	uint32_t masterRate = isSgb2 ? 20971520 : _console->GetMasterClockRate();
	uint8_t divider = 5;

	//TODO: This doesn't actually work properly if the speed is changed while the SGB is running (but this most likely never happens?)
	switch(_control & 0x03) {
		case 0: divider = 4; break;
		case 1: divider = 5; break;
		case 2: divider = 7; break;
		case 3: divider = 9; break;
	}

	double effectiveRate = (double)masterRate / divider;
	_clockRatio = effectiveRate / _console->GetMasterClockRate();
}

uint32_t SuperGameboy::GetClockRate()
{
	return (uint32_t)(_console->GetMasterClockRate() * _clockRatio);
}

uint8_t SuperGameboy::GetInputIndex()
{
	return 0xF - _inputIndex;
}

uint8_t SuperGameboy::GetInput()
{
	return _input[_inputIndex];
}

uint8_t SuperGameboy::Peek(uint32_t addr)
{
	return 0;
}

void SuperGameboy::PeekBlock(uint32_t addr, uint8_t* output)
{
	memset(output, 0, 0x1000);
}

AddressInfo SuperGameboy::GetAbsoluteAddress(uint32_t address)
{
	return { -1, SnesMemoryType::Register };
}

void SuperGameboy::Serialize(Serializer& s)
{
	s.Stream(
		_control, _resetClock, _input[0], _input[1], _input[2], _input[3], _inputIndex, _listeningForPacket, _packetReady,
		_inputWriteClock, _inputValue, _packetByte, _packetBit, _lcdRowSelect, _readPosition, _waitForHigh, _clockRatio
	);

	s.StreamArray(_packetData, 16);
	s.StreamArray(_lcdBuffer[0], 1280);
	s.StreamArray(_lcdBuffer[1], 1280);
	s.StreamArray(_lcdBuffer[2], 1280);
	s.StreamArray(_lcdBuffer[3], 1280);
}
