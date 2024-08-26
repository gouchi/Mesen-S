﻿using Mesen.GUI.Debugger.Workspace;
using Mesen.GUI.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public class DebugWindowManager
	{
		private static HashSet<Form> _openedWindows = new HashSet<Form>();

		public static List<Form> GetWindows()
		{
			return new List<Form>(_openedWindows);
		}
		public static Form OpenDebugWindow(DebugWindow window)
		{
			Form existingWindow = GetExistingSingleInstanceWindow(window);
			if(existingWindow != null) {
				BringToFront(existingWindow);
				return existingWindow;
			} else {
				BaseForm frm = null;
				 
				switch(window) {
					case DebugWindow.Debugger: frm = new frmDebugger(CpuType.Cpu); frm.Icon = Properties.Resources.Debugger; break;
					case DebugWindow.SpcDebugger: frm = new frmDebugger(CpuType.Spc); frm.Icon = Properties.Resources.SpcDebugger; break;
					case DebugWindow.Sa1Debugger: frm = new frmDebugger(CpuType.Sa1); frm.Icon = Properties.Resources.Sa1Debugger; break;
					case DebugWindow.GsuDebugger: frm = new frmDebugger(CpuType.Gsu); frm.Icon = Properties.Resources.GsuDebugger; break;
					case DebugWindow.NecDspDebugger: frm = new frmDebugger(CpuType.NecDsp); frm.Icon = Properties.Resources.NecDspDebugger; break;
					case DebugWindow.Cx4Debugger: frm = new frmDebugger(CpuType.Cx4); frm.Icon = Properties.Resources.Cx4Debugger; break;
					case DebugWindow.GbDebugger: frm = new frmDebugger(CpuType.Gameboy); frm.Icon = Properties.Resources.GbDebugger; break;
					case DebugWindow.TraceLogger: frm = new frmTraceLogger(); frm.Icon = Properties.Resources.LogWindow; break;
					case DebugWindow.MemoryTools: frm = new frmMemoryTools(); frm.Icon = Properties.Resources.CheatCode; break;
					case DebugWindow.TileViewer: frm = new frmTileViewer(CpuType.Cpu); frm.Icon = Properties.Resources.VerticalLayout; break;
					case DebugWindow.TilemapViewer: frm = new frmTilemapViewer(CpuType.Cpu); frm.Icon = Properties.Resources.VideoOptions; break;
					case DebugWindow.PaletteViewer: frm = new frmPaletteViewer(CpuType.Cpu); frm.Icon = Properties.Resources.VideoFilter; break;
					case DebugWindow.SpriteViewer: frm = new frmSpriteViewer(CpuType.Cpu); frm.Icon = Properties.Resources.PerfTracker; break;
					case DebugWindow.EventViewer: frm = new frmEventViewer(CpuType.Cpu); frm.Icon = Properties.Resources.NesEventViewer; break;
					case DebugWindow.ScriptWindow: frm = new frmScript(); frm.Icon = Properties.Resources.Script; break;
					case DebugWindow.RegisterViewer: frm = new frmRegisterViewer(); frm.Icon = Properties.Resources.RegisterIcon; break;
					case DebugWindow.Profiler: frm = new frmProfiler(); frm.Icon = Properties.Resources.PerfTracker; break;
					case DebugWindow.Assembler: frm = new frmAssembler(); frm.Icon = Properties.Resources.Chip; break;
					case DebugWindow.DebugLog: frm = new frmDebugLog(); frm.Icon = Properties.Resources.LogWindow; break;
					case DebugWindow.GbTileViewer: frm = new frmTileViewer(CpuType.Gameboy); frm.Icon = Properties.Resources.VerticalLayout; break;
					case DebugWindow.GbTilemapViewer: frm = new frmTilemapViewer(CpuType.Gameboy); frm.Icon = Properties.Resources.VideoOptions; break;
					case DebugWindow.GbPaletteViewer: frm = new frmPaletteViewer(CpuType.Gameboy); frm.Icon = Properties.Resources.VideoFilter; break;
					case DebugWindow.GbSpriteViewer: frm = new frmSpriteViewer(CpuType.Gameboy); frm.Icon = Properties.Resources.PerfTracker; break;
					case DebugWindow.GbEventViewer: frm = new frmEventViewer(CpuType.Gameboy); frm.Icon = Properties.Resources.NesEventViewer; break;
				}

				if(_openedWindows.Count == 0) {
					DebugWorkspaceManager.GetWorkspace();
				}
				
				_openedWindows.Add(frm);
				frm.FormClosed += Debugger_FormClosed;
				frm.Show();
				return frm;
			}
		}
		
		private static frmMemoryTools OpenMemoryViewer()
		{
			frmMemoryTools frm = GetMemoryViewer();
			if(frm == null) {
				frm = new frmMemoryTools();
				frm.Icon = Properties.Resources.CheatCode;
				frm.FormClosed += Debugger_FormClosed;
				_openedWindows.Add(frm);
			} else {
				if(frm.WindowState == FormWindowState.Minimized) {
					//Unminimize window if it was minimized
					frm.WindowState = FormWindowState.Normal;
				}
				frm.BringToFront();
			}
			frm.Show();
			return frm;
		}

		public static frmMemoryTools OpenMemoryViewer(AddressInfo address)
		{
			frmMemoryTools frm = OpenMemoryViewer();
			frm.ShowAddress(address.Address, address.Type);
			return frm;
		}

		public static frmDebugger OpenDebugger(CpuType type)
		{
			switch(type) {
				case CpuType.Cpu: return (frmDebugger)OpenDebugWindow(DebugWindow.Debugger);
				case CpuType.Spc: return (frmDebugger)OpenDebugWindow(DebugWindow.SpcDebugger);
				case CpuType.NecDsp: return (frmDebugger)OpenDebugWindow(DebugWindow.NecDspDebugger);
				case CpuType.Sa1: return (frmDebugger)OpenDebugWindow(DebugWindow.Sa1Debugger);
				case CpuType.Gsu: return (frmDebugger)OpenDebugWindow(DebugWindow.GsuDebugger);
				case CpuType.Cx4: return (frmDebugger)OpenDebugWindow(DebugWindow.Cx4Debugger);
				case CpuType.Gameboy: return (frmDebugger)OpenDebugWindow(DebugWindow.GbDebugger);
			}
			throw new Exception("Invalid CPU type");			
		}

		public static frmDebugger GetDebugger(CpuType type)
		{
			switch(type) {
				case CpuType.Cpu: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.Debugger);
				case CpuType.Spc: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.SpcDebugger);
				case CpuType.NecDsp: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.NecDspDebugger);
				case CpuType.Sa1: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.Sa1Debugger);
				case CpuType.Gsu: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.GsuDebugger);
				case CpuType.Cx4: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.Cx4Debugger);
				case CpuType.Gameboy: return (frmDebugger)GetExistingSingleInstanceWindow(DebugWindow.GbDebugger);
			}
			throw new Exception("Invalid CPU type");
		}

		public static void OpenAssembler(CpuType cpuType, string code = "", int startAddress = 0, int blockLength = 0)
		{
			if(_openedWindows.Count == 0) {
				DebugWorkspaceManager.GetWorkspace();
			}

			frmAssembler frm = new frmAssembler(cpuType, code, startAddress, blockLength);
			frm.Icon = Properties.Resources.Chip;
			_openedWindows.Add(frm);
			frm.FormClosed += Debugger_FormClosed;
			frm.Show();
		}

		public static frmMemoryTools GetMemoryViewer()
		{
			return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmMemoryTools)) as frmMemoryTools;
		}

		public static bool HasOpenedWindow
		{
			get
			{
				return _openedWindows.Count > 0;
			}
		}

		public static void CloseWindows(CpuType cpuType)
		{
			List<Form> openedWindows = new List<Form>(_openedWindows);
			foreach(Form frm in openedWindows) {
				if(frm is IDebuggerWindow && ((IDebuggerWindow)frm).CpuType == cpuType) {
					frm.Close();
				}
			}
		}

		public static void CloseWindows(CoprocessorType coprocessorType)
		{
			if(coprocessorType != CoprocessorType.CX4) {
				CloseWindows(CpuType.Cx4);
			}
			if(coprocessorType != CoprocessorType.GSU) {
				CloseWindows(CpuType.Gsu);
			}
			if(coprocessorType != CoprocessorType.SA1) {
				CloseWindows(CpuType.Sa1);
			}
			if(coprocessorType < CoprocessorType.DSP1 && coprocessorType > CoprocessorType.DSP4 && coprocessorType != CoprocessorType.ST010 && coprocessorType != CoprocessorType.ST011) {
				CloseWindows(CpuType.NecDsp);
			}

			if(coprocessorType == CoprocessorType.Gameboy) {
				CloseWindows(CpuType.Cpu);
				CloseWindows(CpuType.Spc);
			}

			if(coprocessorType != CoprocessorType.Gameboy && coprocessorType != CoprocessorType.SGB) {
				CloseWindows(CpuType.Gameboy);
			}
		}

		public static void CloseAll()
		{
			List<Form> openedWindows = new List<Form>(_openedWindows);
			foreach(Form frm in openedWindows) {
				frm.Close();
			}
		}

		private static Form GetExistingSingleInstanceWindow(DebugWindow window)
		{
			//Only one of each of these windows can be opened at once, check if one is already opened
			switch(window) {
				case DebugWindow.Debugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Cpu);
				case DebugWindow.SpcDebugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Spc);
				case DebugWindow.Sa1Debugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Sa1);
				case DebugWindow.GsuDebugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Gsu);
				case DebugWindow.NecDspDebugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.NecDsp);
				case DebugWindow.Cx4Debugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Cx4);
				case DebugWindow.GbDebugger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugger) && ((frmDebugger)form).CpuType == CpuType.Gameboy);
				case DebugWindow.TraceLogger: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmTraceLogger));
				case DebugWindow.EventViewer: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmEventViewer) && ((frmEventViewer)form).CpuType == CpuType.Cpu);
				case DebugWindow.GbEventViewer: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmEventViewer) && ((frmEventViewer)form).CpuType == CpuType.Gameboy);
				case DebugWindow.Profiler: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmProfiler));
				case DebugWindow.DebugLog: return _openedWindows.ToList().Find((form) => form.GetType() == typeof(frmDebugLog));
			}

			return null;
		}

		public static void CleanupDebugger()
		{
			if(_openedWindows.Count == 0) {
				//All windows have been closed, disable debugger
				DebugWorkspaceManager.SaveWorkspace();
				DebugWorkspaceManager.Clear();
				DebugApi.ReleaseDebugger();
			}
		}

		private static void Debugger_FormClosed(object sender, FormClosedEventArgs e)
		{
			_openedWindows.Remove((Form)sender);
			CleanupDebugger();
		}

		public static void BringToFront(Form form)
		{
			form.BringToFront();
			if(form.WindowState == FormWindowState.Minimized) {
				//Unminimize window if it was minimized
				form.WindowState = FormWindowState.Normal;
			}
			form.Focus();
		}
	}

	public interface IDebuggerWindow
	{
		CpuType CpuType { get; }
	}

	public enum DebugWindow
	{
		Debugger,
		SpcDebugger,
		Sa1Debugger,
		GsuDebugger,
		NecDspDebugger,
		Cx4Debugger,
		GbDebugger,
		MemoryTools,
		TraceLogger,
		TileViewer,
		TilemapViewer,
		PaletteViewer,
		SpriteViewer,
		EventViewer,
		ScriptWindow,
		RegisterViewer,
		Profiler,
		Assembler,
		DebugLog,

		GbTileViewer,
		GbTilemapViewer,
		GbPaletteViewer,
		GbSpriteViewer,
		GbEventViewer,
	}
}
