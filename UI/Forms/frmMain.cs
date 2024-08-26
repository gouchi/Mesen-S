﻿using Mesen.GUI.Config;
using Mesen.GUI.Config.Shortcuts;
using Mesen.GUI.Controls;
using Mesen.GUI.Debugger;
using Mesen.GUI.Debugger.Workspace;
using Mesen.GUI.Emulation;
using Mesen.GUI.Forms.Config;
using Mesen.GUI.Forms.NetPlay;
using Mesen.GUI.Interop;
using Mesen.GUI.Updates;
using Mesen.GUI.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Forms
{
	public partial class frmMain : BaseInputForm
	{
		private NotificationListener _notifListener;
		private ShortcutHandler _shortcuts;
		private DisplayManager _displayManager;
		private CommandLineHelper _commandLine;

		public static frmMain Instance { get; private set; }

		public frmMain(string[] args)
		{
			frmMain.Instance = this;

			InitializeComponent();
			if(DesignMode) {
				return;
			}

			_commandLine = new CommandLineHelper(args);

			ResourceHelper.LoadResources(Language.English);
			MonoThemeHelper.ExcludeFromTheme(pnlRenderer);
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			RestoreLocation(ConfigManager.Config.WindowLocation, ConfigManager.Config.WindowSize);
		}

		protected override void OnShown(EventArgs e)
		{
			base.OnShown(e);
			
#if HIDETESTMENU
			mnuTests.Visible = false;
#endif

			EmuApi.InitDll();
			bool showUpgradeMessage = UpdateHelper.PerformUpgrade();

			ConfigManager.Config.Video.ApplyConfig();
			EmuApi.InitializeEmu(ConfigManager.HomeFolder, Handle, ctrlRenderer.Handle, false, false, false);

			if(ConfigManager.Config.Preferences.OverrideGameFolder && Directory.Exists(ConfigManager.Config.Preferences.GameFolder)) {
				EmuApi.AddKnownGameFolder(ConfigManager.Config.Preferences.GameFolder);
			}
			foreach(RecentItem recentItem in ConfigManager.Config.RecentFiles.Items) {
				EmuApi.AddKnownGameFolder(recentItem.RomFile.Folder);
			}

			ConfigManager.Config.InitializeDefaults();
			ConfigManager.Config.ApplyConfig();

			_displayManager = new DisplayManager(this, ctrlRenderer, pnlRenderer, mnuMain, ctrlRecentGames);
			_displayManager.SetScaleBasedOnWindowSize();
			_shortcuts = new ShortcutHandler(_displayManager);

			_notifListener = new NotificationListener();
			_notifListener.OnNotification += OnNotificationReceived;
			
			_commandLine.LoadGameFromCommandLine();

			SaveStateManager.InitializeStateMenu(mnuSaveState, true, _shortcuts);
			SaveStateManager.InitializeStateMenu(mnuLoadState, false, _shortcuts);
			BindShortcuts();

			Task.Run(() => {
				Thread.Sleep(25);
				this.BeginInvoke((Action)(() => {

					if(!EmuRunner.IsRunning()) {
						ShowGameScreen(GameScreenMode.RecentGames);
					}
				}));
			});

			if(showUpgradeMessage) {
				MesenMsgBox.Show("UpgradeSuccess", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}

			if(ConfigManager.Config.Preferences.AutomaticallyCheckForUpdates) {
				UpdateHelper.CheckForUpdates(true);
			}

			InBackgroundHelper.StartBackgroundTimer();
			this.Resize += frmMain_Resize;
		}

		private bool _shuttingDown = false;
		protected override void OnFormClosing(FormClosingEventArgs e)
		{
			base.OnFormClosing(e);

			InBackgroundHelper.StopBackgroundTimer();

			if(_notifListener != null) {
				_notifListener.Dispose();
				_notifListener = null;
			}

			if(!_shuttingDown && Program.IsMono) {
				//This appears to prevent Mono from locking up when closing the form
				DebugApi.ResumeExecution();
				DebugWindowManager.CloseAll();

				Task.Run(() => {
					EmuApi.Stop();
					_shuttingDown = true;
					this.BeginInvoke((Action)(() => this.Close()));
				});
				e.Cancel = true;
				return;
			}

			DebugApi.ResumeExecution();
			DebugWindowManager.CloseAll();

			ConfigManager.Config.WindowLocation = this.WindowState == FormWindowState.Normal ? this.Location : this.RestoreBounds.Location;
			ConfigManager.Config.WindowSize = this.WindowState == FormWindowState.Normal ? this.Size : this.RestoreBounds.Size;
			ConfigManager.ApplyChanges();
			ConfigManager.SaveConfig();

			EmuApi.Stop();
			EmuApi.Release();
		}

		private void OnNotificationReceived(NotificationEventArgs e)
		{
			switch(e.NotificationType) {
				case ConsoleNotificationType.GameLoaded:
					CheatCodes.ApplyCheats();
					RomInfo romInfo = EmuApi.GetRomInfo();

					this.Invoke((Action)(() => {
						DebugWindowManager.CloseWindows(romInfo.CoprocessorType);
					}));

					Task.Run(() => {
						this.BeginInvoke((Action)(() => {
							UpdateDebuggerMenu();
							ctrlRecentGames.Visible = false;
							SaveStateManager.UpdateStateMenu(mnuLoadState, false);
							SaveStateManager.UpdateStateMenu(mnuSaveState, true);

							this.Text = "Mesen-S - " + romInfo.GetRomName();

							if(DebugWindowManager.HasOpenedWindow) {
								DebugWorkspaceManager.GetWorkspace();
							}
						}));
					});
					break;

				case ConsoleNotificationType.BeforeEmulationStop:
					this.Invoke((Action)(() => {
						DebugWindowManager.CloseAll();
					}));
					break;

				case ConsoleNotificationType.GameResumed:
					this.BeginInvoke((Action)(() => {
						//Ensure mouse is hidden when game is resumed
						CursorManager.OnMouseMove(ctrlRenderer);
					}));
					break;

				case ConsoleNotificationType.EmulationStopped:
					this.BeginInvoke((Action)(() => {
						this.Text = "Mesen-S";
						UpdateDebuggerMenu();
						ShowGameScreen(GameScreenMode.RecentGames);
						ResizeRecentGames();
						if(_displayManager.ExclusiveFullscreen) {
							_displayManager.SetFullscreenState(false);
						}
					}));
					break;

				case ConsoleNotificationType.ResolutionChanged:
					this.BeginInvoke((Action)(() => {
						_displayManager.UpdateViewerSize();
					}));
					break;

				case ConsoleNotificationType.ExecuteShortcut:
					this.BeginInvoke((Action)(() => {
						_shortcuts.ExecuteShortcut((EmulatorShortcut)e.Parameter);
					}));
					break;

				case ConsoleNotificationType.MissingFirmware:
					this.Invoke((Action)(() => {
						MissingFirmwareMessage msg = (MissingFirmwareMessage)Marshal.PtrToStructure(e.Parameter, typeof(MissingFirmwareMessage));
						FirmwareHelper.RequestFirmwareFile(msg);
					}));
					break;
			}
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			if(_displayManager.HideMenuStrip && (keyData & Keys.Alt) == Keys.Alt) {
				if(mnuMain.Visible && !mnuMain.ContainsFocus) {
					mnuMain.Visible = false;
				} else {
					mnuMain.Visible = true;
					mnuMain.Focus();
				}
			}

#if !HIDETESTMENU
			if(keyData == Keys.Pause && EmuRunner.IsRunning()) {
				if(TestApi.RomTestRecording()) {
					TestApi.RomTestStop();
				} else {
					TestApi.RomTestRecord(ConfigManager.TestFolder + "\\" + EmuApi.GetRomInfo().GetRomName() + ".mtp", true);
				}
			}
#endif

			return base.ProcessCmdKey(ref msg, keyData);
		}

		private void BindShortcuts()
		{
			Func<bool> notClient = () => { return !NetplayApi.IsConnected(); };
			Func<bool> running = () => { return EmuRunner.IsRunning(); };
			Func<bool> runningNotClient = () => { return EmuRunner.IsRunning() && !NetplayApi.IsConnected(); };
			Func<bool> runningNotClientNotMovie = () => { return EmuRunner.IsRunning() && !NetplayApi.IsConnected() && !RecordApi.MoviePlaying(); };

			_shortcuts.BindShortcut(mnuOpen, EmulatorShortcut.OpenFile);
			_shortcuts.BindShortcut(mnuReloadRom, EmulatorShortcut.ReloadRom, runningNotClientNotMovie);
			_shortcuts.BindShortcut(mnuExit, EmulatorShortcut.Exit);
			_shortcuts.BindShortcut(mnuIncreaseSpeed, EmulatorShortcut.IncreaseSpeed, notClient);
			_shortcuts.BindShortcut(mnuDecreaseSpeed, EmulatorShortcut.DecreaseSpeed, notClient);
			_shortcuts.BindShortcut(mnuEmuSpeedMaximumSpeed, EmulatorShortcut.MaxSpeed, notClient);

			_shortcuts.BindShortcut(mnuPause, EmulatorShortcut.Pause, runningNotClient);
			_shortcuts.BindShortcut(mnuReset, EmulatorShortcut.Reset, runningNotClientNotMovie);
			_shortcuts.BindShortcut(mnuPowerCycle, EmulatorShortcut.PowerCycle, runningNotClientNotMovie);
			_shortcuts.BindShortcut(mnuPowerOff, EmulatorShortcut.PowerOff, runningNotClient);

			_shortcuts.BindShortcut(mnuShowFPS, EmulatorShortcut.ToggleFps);

			_shortcuts.BindShortcut(mnuScale1x, EmulatorShortcut.SetScale1x);
			_shortcuts.BindShortcut(mnuScale2x, EmulatorShortcut.SetScale2x);
			_shortcuts.BindShortcut(mnuScale3x, EmulatorShortcut.SetScale3x);
			_shortcuts.BindShortcut(mnuScale4x, EmulatorShortcut.SetScale4x);
			_shortcuts.BindShortcut(mnuScale5x, EmulatorShortcut.SetScale5x);
			_shortcuts.BindShortcut(mnuScale6x, EmulatorShortcut.SetScale6x);

			_shortcuts.BindShortcut(mnuFullscreen, EmulatorShortcut.ToggleFullscreen);

			_shortcuts.BindShortcut(mnuTakeScreenshot, EmulatorShortcut.TakeScreenshot, running);
			_shortcuts.BindShortcut(mnuRandomGame, EmulatorShortcut.LoadRandomGame);
			
			mnuDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenDebugger));
			mnuSpcDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenSpcDebugger));
			mnuSa1Debugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenSa1Debugger));
			mnuGsuDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenGsuDebugger));
			mnuNecDspDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenNecDspDebugger));
			mnuCx4Debugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenCx4Debugger));
			mnuGbDebugger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenGameboyDebugger));
			mnuMemoryTools.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenMemoryTools));
			mnuEventViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenEventViewer));
			mnuTilemapViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTilemapViewer));
			mnuTileViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTileViewer));
			mnuSpriteViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenSpriteViewer));
			mnuPaletteViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenPaletteViewer));
			mnuTraceLogger.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenTraceLogger));
			mnuScriptWindow.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenScriptWindow));
			mnuRegisterViewer.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenRegisterViewer));
			mnuProfiler.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenProfiler));
			mnuAssembler.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenAssembler));
			mnuDebugLog.InitShortcut(this, nameof(DebuggerShortcutsConfig.OpenDebugLog));

			mnuNoneFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.None); };
			mnuNtscFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.NTSC); };

			mnuHQ2xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.HQ2x); };
			mnuHQ3xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.HQ3x); };
			mnuHQ4xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.HQ4x); };

			mnuPrescale2xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale2x); };
			mnuPrescale3xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale3x); };
			mnuPrescale4xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale4x); };
			mnuPrescale6xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale6x); };
			mnuPrescale8xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale8x); };
			mnuPrescale10xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Prescale10x); };

			mnuScale2xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Scale2x); };
			mnuScale3xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Scale3x); };
			mnuScale4xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Scale4x); };

			mnu2xSaiFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType._2xSai); };
			mnuSuper2xSaiFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.Super2xSai); };
			mnuSuperEagleFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.SuperEagle); };

			mnuXBRZ2xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.xBRZ2x); };
			mnuXBRZ3xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.xBRZ3x); };
			mnuXBRZ4xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.xBRZ4x); };
			mnuXBRZ5xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.xBRZ5x); };
			mnuXBRZ6xFilter.Click += (s, e) => { _shortcuts.SetVideoFilter(VideoFilterType.xBRZ6x); };

			mnuBilinearInterpolation.Click += (s, e) => { _shortcuts.ToggleBilinearInterpolation(); };
			mnuBlendHighResolutionModes.Click += (s, e) => { _shortcuts.ToggleBlendHighResolutionModes(); };

			mnuRegionAuto.Click += (s, e) => { _shortcuts.SetRegion(ConsoleRegion.Auto); };
			mnuRegionNtsc.Click += (s, e) => { _shortcuts.SetRegion(ConsoleRegion.Ntsc); };
			mnuRegionPal.Click += (s, e) => { _shortcuts.SetRegion(ConsoleRegion.Pal); };

			mnuCheats.Click += (s, e) => { frmCheatList.ShowWindow(); };

			mnuOptions.DropDownOpening += (s, e) => {
				bool isConnected = NetplayApi.IsConnected();
				mnuRegion.Enabled = !isConnected;
				mnuInputConfig.Enabled = !isConnected;
				mnuEmulationConfig.Enabled = !isConnected;
			};
			
			InitNetPlayMenus();

			Func<bool> isGameboyMode = () => EmuApi.GetRomInfo().CoprocessorType == CoprocessorType.Gameboy;
			mnuDebugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbDebugger : DebugWindow.Debugger); };
			mnuSpcDebugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.SpcDebugger); };
			mnuSa1Debugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.Sa1Debugger); };
			mnuGsuDebugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GsuDebugger); };
			mnuNecDspDebugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.NecDspDebugger); };
			mnuCx4Debugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.Cx4Debugger); };
			mnuGbDebugger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbDebugger); };
			mnuTraceLogger.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.TraceLogger); };
			mnuMemoryTools.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.MemoryTools); };
			mnuTilemapViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbTilemapViewer : DebugWindow.TilemapViewer); };
			mnuTileViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbTileViewer : DebugWindow.TileViewer); };
			mnuSpriteViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbSpriteViewer : DebugWindow.SpriteViewer); };
			mnuPaletteViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbPaletteViewer : DebugWindow.PaletteViewer); };
			mnuEventViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(isGameboyMode() ? DebugWindow.GbEventViewer : DebugWindow.EventViewer); };
			mnuScriptWindow.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.ScriptWindow); };
			mnuRegisterViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.RegisterViewer); };
			mnuProfiler.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.Profiler); };
			mnuAssembler.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.Assembler); };
			mnuDebugLog.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.DebugLog); };

			mnuGbTilemapViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbTilemapViewer); };
			mnuGbTileViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbTileViewer); };
			mnuGbSpriteViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbSpriteViewer); };
			mnuGbPaletteViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbPaletteViewer); };
			mnuGbEventViewer.Click += (s, e) => { DebugWindowManager.OpenDebugWindow(DebugWindow.GbEventViewer); };

			mnuTestRun.Click += (s, e) => { RomTestHelper.RunTest(); };
			mnuTestRecord.Click += (s, e) => { RomTestHelper.RecordTest(); };
			mnuTestStop.Click += (s, e) => { RomTestHelper.StopRecording(); };
			mnuRunAllTests.Click += (s, e) => { RomTestHelper.RunAllTests(); };

			UpdateDebuggerMenu();
		}

		public void ShowGameScreen(GameScreenMode mode)
		{
			ctrlRecentGames.ShowScreen(mode);
			ResizeRecentGames();
		}

		private void InitNetPlayMenus()
		{
			mnuConnect.Click += (s, e) => { NetPlayHelper.Connect(); };
			mnuStartServer.Click += (s, e) => { NetPlayHelper.ToggleServer(); };
			mnuProfile.Click += (s, e) => { using(frmPlayerProfile frm = new frmPlayerProfile()) { frm.ShowDialog(mnuProfile, this); } };
			mnuNetPlayPlayer1.Click += (s, e) => { NetplayApi.NetPlaySelectController(0); };
			mnuNetPlayPlayer2.Click += (s, e) => { NetplayApi.NetPlaySelectController(1); };
			mnuNetPlayPlayer3.Click += (s, e) => { NetplayApi.NetPlaySelectController(2); };
			mnuNetPlayPlayer4.Click += (s, e) => { NetplayApi.NetPlaySelectController(3); };
			mnuNetPlayPlayer5.Click += (s, e) => { NetplayApi.NetPlaySelectController(4); };
			mnuNetPlaySpectator.Click += (s, e) => { NetplayApi.NetPlaySelectController(0xFF); };

			mnuNetPlay.DropDownOpening += (s, e) => {
				bool runAheadDisabled = ConfigManager.Config.Emulation.RunAheadFrames == 0;
				bool isClient = NetplayApi.IsConnected();
				bool isServer = NetplayApi.IsServerRunning();
				mnuConnect.Text = ResourceHelper.GetMessage(isClient ? "Disconnect" : "ConnectToServer");
				mnuConnect.Enabled = runAheadDisabled && !isServer;
				mnuStartServer.Text = ResourceHelper.GetMessage(isServer ? "StopServer" : "StartServer");
				mnuStartServer.Enabled = runAheadDisabled && !isClient;
				mnuNetPlaySelectController.Enabled = isClient || isServer;
			};

			mnuNetPlaySelectController.DropDownOpening += (s, e) => {
				int availableControllers = NetplayApi.NetPlayGetAvailableControllers();
				int currentControllerPort = NetplayApi.NetPlayGetControllerPort();
				mnuNetPlayPlayer1.Enabled = (availableControllers & 0x01) == 0x01;
				mnuNetPlayPlayer2.Enabled = (availableControllers & 0x02) == 0x02;
				mnuNetPlayPlayer3.Enabled = (availableControllers & 0x04) == 0x04;
				mnuNetPlayPlayer4.Enabled = (availableControllers & 0x08) == 0x08;
				mnuNetPlayPlayer5.Enabled = (availableControllers & 0x10) == 0x10;

				Func<int, string> getControllerName = (int port) => {
					ControllerType type = ConfigApi.GetControllerType(port);
					if(type == ControllerType.Multitap) {
						type = ControllerType.SnesController;
					}
					return ResourceHelper.GetEnumText(type);
				};

				mnuNetPlayPlayer1.Text = ResourceHelper.GetMessage("PlayerNumber", "1") + " (" + getControllerName(0) + ")";
				mnuNetPlayPlayer2.Text = ResourceHelper.GetMessage("PlayerNumber", "2") + " (" + getControllerName(1) + ")";
				mnuNetPlayPlayer3.Text = ResourceHelper.GetMessage("PlayerNumber", "3") + " (" + getControllerName(2) + ")";
				mnuNetPlayPlayer4.Text = ResourceHelper.GetMessage("PlayerNumber", "4") + " (" + getControllerName(3) + ")";
				mnuNetPlayPlayer5.Text = ResourceHelper.GetMessage("PlayerNumber", "5") + " (" + getControllerName(4) + ")";

				mnuNetPlayPlayer1.Checked = (currentControllerPort == 0);
				mnuNetPlayPlayer2.Checked = (currentControllerPort == 1);
				mnuNetPlayPlayer3.Checked = (currentControllerPort == 2);
				mnuNetPlayPlayer4.Checked = (currentControllerPort == 3);
				mnuNetPlayPlayer5.Checked = (currentControllerPort == 4);
				mnuNetPlaySpectator.Checked = (currentControllerPort == 0xFF);
			};
		}

		private void UpdateDebuggerMenu()
		{
			bool running = EmuRunner.IsRunning();
			mnuDebugger.Enabled = running;
			mnuSpcDebugger.Enabled = running;

			CoprocessorType coprocessor = EmuApi.GetRomInfo().CoprocessorType;
			mnuSa1Debugger.Enabled = coprocessor == CoprocessorType.SA1;
			mnuSa1Debugger.Visible = coprocessor == CoprocessorType.SA1;

			mnuGsuDebugger.Enabled = coprocessor == CoprocessorType.GSU;
			mnuGsuDebugger.Visible = coprocessor == CoprocessorType.GSU;

			bool isNecDsp = (
				coprocessor == CoprocessorType.DSP1 ||
				coprocessor == CoprocessorType.DSP1B ||
				coprocessor == CoprocessorType.DSP2 ||
				coprocessor == CoprocessorType.DSP3 ||
				coprocessor == CoprocessorType.DSP4 ||
				coprocessor == CoprocessorType.ST010 ||
				coprocessor == CoprocessorType.ST011
			);

			mnuNecDspDebugger.Enabled = isNecDsp;
			mnuNecDspDebugger.Visible = isNecDsp;

			mnuCx4Debugger.Enabled = coprocessor == CoprocessorType.CX4;
			mnuCx4Debugger.Visible = coprocessor == CoprocessorType.CX4;

			mnuTraceLogger.Enabled = running;
			mnuScriptWindow.Enabled = running;
			mnuMemoryTools.Enabled = running;
			mnuTilemapViewer.Enabled = running;
			mnuTileViewer.Enabled = running;
			mnuSpriteViewer.Enabled = running;
			mnuPaletteViewer.Enabled = running;
			mnuEventViewer.Enabled = running;
			mnuRegisterViewer.Enabled = running;
			mnuProfiler.Enabled = running;
			mnuAssembler.Enabled = running;
			mnuDebugLog.Enabled = running;

			bool isGameboyMode = coprocessor == CoprocessorType.Gameboy;
			bool isSuperGameboy = coprocessor == CoprocessorType.SGB;
			
			//Only show in super gameboy mode
			mnuGbDebugger.Enabled = isSuperGameboy;
			mnuGbDebugger.Visible = isSuperGameboy;
			mnuGbEventViewer.Enabled = isSuperGameboy;
			mnuGbEventViewer.Visible = isSuperGameboy;
			mnuGbPaletteViewer.Enabled = isSuperGameboy;
			mnuGbPaletteViewer.Visible = isSuperGameboy;
			mnuGbSpriteViewer.Enabled = isSuperGameboy;
			mnuGbSpriteViewer.Visible = isSuperGameboy;
			mnuGbTilemapViewer.Enabled = isSuperGameboy;
			mnuGbTilemapViewer.Visible = isSuperGameboy;
			mnuGbTileViewer.Enabled = isSuperGameboy;
			mnuGbTileViewer.Visible = isSuperGameboy;
			sepGameboyDebugger.Visible = isSuperGameboy;

			//Hide in gameboy-only mode
			mnuSpcDebugger.Enabled = running && !isGameboyMode;
			mnuSpcDebugger.Visible = !isGameboyMode;
			sepCoprocessors.Visible = !isGameboyMode;
		}
		
		private void ResizeRecentGames()
		{
			ctrlRecentGames.Height = this.ClientSize.Height - ctrlRecentGames.Top - (ctrlRecentGames.Mode == GameScreenMode.RecentGames ? 25 : 0);
		}

		private void frmMain_Resize(object sender, EventArgs e)
		{
			ResizeRecentGames();
		}
		
		private void mnuVideoConfig_Click(object sender, EventArgs e)
		{
			using(frmVideoConfig frm = new frmVideoConfig()) {
				frm.ShowDialog(sender, this);
			}
			ConfigManager.Config.Video.ApplyConfig();
		}

		private void mnuAudioConfig_Click(object sender, EventArgs e)
		{
			using(frmAudioConfig frm = new frmAudioConfig()) {
				frm.ShowDialog(sender, this);
			}
			ConfigManager.Config.Audio.ApplyConfig();
		}
		
		private void mnuEmulationConfig_Click(object sender, EventArgs e)
		{
			using(frmEmulationConfig frm = new frmEmulationConfig()) {
				frm.ShowDialog(sender, this);
			}
			ConfigManager.Config.Emulation.ApplyConfig();
		}

		private void mnuGameboyConfig_Click(object sender, EventArgs e)
		{
			using(frmGameboyConfig frm = new frmGameboyConfig()) {
				frm.ShowDialog(sender, this);
			}
			ConfigManager.Config.Gameboy.ApplyConfig();
		}

		private void mnuInputConfig_Click(object sender, EventArgs e)
		{
			using(frmInputConfig frm = new frmInputConfig()) {
				frm.ShowDialog(sender, this);
			}
			ConfigManager.Config.Input.ApplyConfig();
		}

		private void mnuPreferences_Click(object sender, EventArgs e)
		{
			using(frmPreferences frm = new frmPreferences()) {
				frm.ShowDialog(sender, this);
				ConfigManager.Config.Preferences.ApplyConfig();
				if(!EmuRunner.IsRunning()) {
					ShowGameScreen(GameScreenMode.RecentGames);
				}
				if(frm.NeedRestart) {
					this.Close();
				}
			}
		}

		protected override void OnDragDrop(DragEventArgs e)
		{
			base.OnDragDrop(e);

			try {
				string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
				if(File.Exists(files[0])) {
					EmuRunner.LoadFile(files[0]);
					this.Activate();
				} else {
					EmuApi.DisplayMessage("Error", "File not found: " + files[0]);
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		protected override void OnDragEnter(DragEventArgs e)
		{
			base.OnDragEnter(e);

			try {
				if(e.Data != null && e.Data.GetDataPresent(DataFormats.FileDrop)) {
					e.Effect = DragDropEffects.Copy;
				} else {
					EmuApi.DisplayMessage("Error", "Unsupported operation.");
				}
			} catch(Exception ex) {
				MesenMsgBox.Show("UnexpectedError", MessageBoxButtons.OK, MessageBoxIcon.Error, ex.ToString());
			}
		}

		private void mnuLogWindow_Click(object sender, EventArgs e)
		{
			new frmLogWindow().Show();
		}

		private void mnuCheckForUpdates_Click(object sender, EventArgs e)
		{
			UpdateHelper.CheckForUpdates(false);
		}

		private void mnuReportBug_Click(object sender, EventArgs e)
		{
			Process.Start("https://www.mesen.ca/snes/ReportBug.php");
		}

		private void mnuAbout_Click(object sender, EventArgs e)
		{
			using(frmAbout frm = new frmAbout()) {
				frm.ShowDialog(this);
			}
		}

		private void mnuFile_DropDownOpening(object sender, EventArgs e)
		{
			mnuRecentFiles.DropDownItems.Clear();
			mnuRecentFiles.DropDownItems.AddRange(ConfigManager.Config.RecentFiles.GetMenuItems().ToArray());
			mnuRecentFiles.Enabled = ConfigManager.Config.RecentFiles.Items.Count > 0;

			mnuSaveState.Enabled = EmuRunner.IsRunning();
			mnuLoadState.Enabled = EmuRunner.IsRunning() && !NetplayApi.IsConnected();
		}

		private void mnuVideoFilter_DropDownOpening(object sender, EventArgs e)
		{
			VideoFilterType filterType = ConfigManager.Config.Video.VideoFilter;
			mnuNoneFilter.Checked = (filterType == VideoFilterType.None);
			mnuNtscFilter.Checked = (filterType == VideoFilterType.NTSC);
			mnuXBRZ2xFilter.Checked = (filterType == VideoFilterType.xBRZ2x);
			mnuXBRZ3xFilter.Checked = (filterType == VideoFilterType.xBRZ3x);
			mnuXBRZ4xFilter.Checked = (filterType == VideoFilterType.xBRZ4x);
			mnuXBRZ5xFilter.Checked = (filterType == VideoFilterType.xBRZ5x);
			mnuXBRZ6xFilter.Checked = (filterType == VideoFilterType.xBRZ6x);
			mnuHQ2xFilter.Checked = (filterType == VideoFilterType.HQ2x);
			mnuHQ3xFilter.Checked = (filterType == VideoFilterType.HQ3x);
			mnuHQ4xFilter.Checked = (filterType == VideoFilterType.HQ4x);
			mnuScale2xFilter.Checked = (filterType == VideoFilterType.Scale2x);
			mnuScale3xFilter.Checked = (filterType == VideoFilterType.Scale3x);
			mnuScale4xFilter.Checked = (filterType == VideoFilterType.Scale4x);
			mnu2xSaiFilter.Checked = (filterType == VideoFilterType._2xSai);
			mnuSuper2xSaiFilter.Checked = (filterType == VideoFilterType.Super2xSai);
			mnuSuperEagleFilter.Checked = (filterType == VideoFilterType.SuperEagle);
			mnuPrescale2xFilter.Checked = (filterType == VideoFilterType.Prescale2x);
			mnuPrescale3xFilter.Checked = (filterType == VideoFilterType.Prescale3x);
			mnuPrescale4xFilter.Checked = (filterType == VideoFilterType.Prescale4x);
			mnuPrescale6xFilter.Checked = (filterType == VideoFilterType.Prescale6x);
			mnuPrescale8xFilter.Checked = (filterType == VideoFilterType.Prescale8x);
			mnuPrescale10xFilter.Checked = (filterType == VideoFilterType.Prescale10x);

			mnuBilinearInterpolation.Checked = ConfigManager.Config.Video.UseBilinearInterpolation;
			mnuBlendHighResolutionModes.Checked = ConfigManager.Config.Video.BlendHighResolutionModes;
		}

		private void mnuVideoScale_DropDownOpening(object sender, EventArgs e)
		{
			double scale = ConfigManager.Config.Video.VideoScale;
			mnuScale1x.Checked = (scale == 1.0);
			mnuScale2x.Checked = (scale == 2.0);
			mnuScale3x.Checked = (scale == 3.0);
			mnuScale4x.Checked = (scale == 4.0);
			mnuScale5x.Checked = (scale == 5.0);
			mnuScale6x.Checked = (scale == 6.0);

			mnuFullscreen.Checked = _displayManager.Fullscreen;
		}

		private void mnuEmulationSpeed_DropDownOpening(object sender, EventArgs e)
		{
			uint emulationSpeed = ConfigManager.Config.Emulation.EmulationSpeed;
			mnuEmuSpeedNormal.Checked = emulationSpeed == 100;
			mnuEmuSpeedQuarter.Checked = emulationSpeed == 25;
			mnuEmuSpeedHalf.Checked = emulationSpeed == 50;
			mnuEmuSpeedDouble.Checked = emulationSpeed == 200;
			mnuEmuSpeedTriple.Checked = emulationSpeed == 300;
			mnuEmuSpeedMaximumSpeed.Checked = emulationSpeed == 0;

			mnuShowFPS.Checked = ConfigManager.Config.Preferences.ShowFps;
		}

		private void mnuLoadState_DropDownOpening(object sender, EventArgs e)
		{
			SaveStateManager.UpdateStateMenu(mnuLoadState, false);
		}

		private void mnuSaveState_DropDownOpening(object sender, EventArgs e)
		{
			SaveStateManager.UpdateStateMenu(mnuSaveState, true);
		}

		private void mnuRegion_DropDownOpening(object sender, EventArgs e)
		{
			mnuRegionAuto.Checked = ConfigManager.Config.Emulation.Region == ConsoleRegion.Auto;
			mnuRegionNtsc.Checked = ConfigManager.Config.Emulation.Region == ConsoleRegion.Ntsc;
			mnuRegionPal.Checked = ConfigManager.Config.Emulation.Region == ConsoleRegion.Pal;
		}

		private void mnuTools_DropDownOpening(object sender, EventArgs e)
		{
			bool isClient = NetplayApi.IsConnected();
			bool runAheadDisabled = ConfigManager.Config.Emulation.RunAheadFrames == 0;
			bool isGameboyMode = EmuApi.GetRomInfo().CoprocessorType == CoprocessorType.Gameboy;

			mnuNetPlay.Enabled = runAheadDisabled && !isGameboyMode;

			mnuMovies.Enabled = runAheadDisabled && EmuRunner.IsRunning();
			mnuPlayMovie.Enabled = runAheadDisabled && EmuRunner.IsRunning() && !RecordApi.MoviePlaying() && !RecordApi.MovieRecording() && !isClient;
			mnuRecordMovie.Enabled = runAheadDisabled && EmuRunner.IsRunning() && !RecordApi.MoviePlaying() && !RecordApi.MovieRecording();
			mnuStopMovie.Enabled = runAheadDisabled && EmuRunner.IsRunning() && (RecordApi.MoviePlaying() || RecordApi.MovieRecording());

			mnuSoundRecorder.Enabled = EmuRunner.IsRunning();
			mnuWaveRecord.Enabled = EmuRunner.IsRunning() && !RecordApi.WaveIsRecording();
			mnuWaveStop.Enabled = EmuRunner.IsRunning() && RecordApi.WaveIsRecording();

			mnuVideoRecorder.Enabled = EmuRunner.IsRunning();
			mnuAviRecord.Enabled = EmuRunner.IsRunning() && !RecordApi.AviIsRecording();
			mnuAviStop.Enabled = EmuRunner.IsRunning() && RecordApi.AviIsRecording();

			mnuCheats.Enabled = EmuRunner.IsRunning() && !isClient && !isGameboyMode;
		}

		private void mnuAviRecord_Click(object sender, EventArgs e)
		{
			using(frmRecordAvi frm = new frmRecordAvi()) {
				if(frm.ShowDialog(mnuVideoRecorder, this) == DialogResult.OK) {
					RecordApi.AviRecord(frm.Filename, ConfigManager.Config.AviRecord.Codec, ConfigManager.Config.AviRecord.CompressionLevel);
				}
			}
		}

		private void mnuAviStop_Click(object sender, EventArgs e)
		{
			RecordApi.AviStop();
		}

		private void mnuWaveRecord_Click(object sender, EventArgs e)
		{
			using(SaveFileDialog sfd = new SaveFileDialog()) {
				sfd.SetFilter(ResourceHelper.GetMessage("FilterWave"));
				sfd.InitialDirectory = ConfigManager.WaveFolder;
				sfd.FileName = EmuApi.GetRomInfo().GetRomName() + ".wav";
				if(sfd.ShowDialog(this) == DialogResult.OK) {
					RecordApi.WaveRecord(sfd.FileName);
				}
			}
		}

		private void mnuWaveStop_Click(object sender, EventArgs e)
		{
			RecordApi.WaveStop();
		}

		private void mnuPlayMovie_Click(object sender, EventArgs e)
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterMovie"));
				ofd.InitialDirectory = ConfigManager.MovieFolder;
				if(ofd.ShowDialog(this) == DialogResult.OK) {
					RecordApi.MoviePlay(ofd.FileName);
				}
			}
		}

		private void mnuStopMovie_Click(object sender, EventArgs e)
		{
			RecordApi.MovieStop();
		}

		private void mnuRecordMovie_Click(object sender, EventArgs e)
		{
			using(frmRecordMovie frm = new frmRecordMovie()) {
				frm.ShowDialog(mnuMovies, this);
			}
		}

		private void mnu_DropDownOpened(object sender, EventArgs e)
		{
			Interlocked.Increment(ref _inMenu);
		}

		private void mnu_DropDownClosed(object sender, EventArgs e)
		{
			Task.Run(() => {
				Thread.Sleep(100);
				Interlocked.Decrement(ref _inMenu);
			});
		}

		private void mnuOnlineHelp_Click(object sender, EventArgs e)
		{
			string platform = Program.IsMono ? "linux" : "win";
			Process.Start("http://www.mesen.ca/snes/docs/?v=" + EmuApi.GetMesenVersion() + "&p=" + platform + "&l=" + ResourceHelper.GetLanguageCode());
		}
	}
}
