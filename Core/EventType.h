#pragma once

enum class EventType
{
	Nmi,
	Irq,
	StartFrame,
	EndFrame,
	Reset,
	ScriptEnded,
	InputPolled,
	StateLoaded,
	StateSaved,
	GbStartFrame,
	GbEndFrame,
	EventTypeSize
};