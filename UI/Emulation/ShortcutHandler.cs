﻿using Mesen.GUI.Config;
using Mesen.GUI.Config.Shortcuts;
using Mesen.GUI.Controls;
using Mesen.GUI.Forms;
using Mesen.GUI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Emulation
{
	public class ShortcutHandler
	{
		private DisplayManager _displayManager;
		private Dictionary<EmulatorShortcut, Func<bool>> _actionEnabledFuncs = new Dictionary<EmulatorShortcut, Func<bool>>();
		private List<uint> _speedValues = new List<uint> { 1, 3, 6, 12, 25, 50, 75, 100, 150, 200, 250, 300, 350, 400, 450, 500, 750, 1000, 2000, 4000 };

		public ShortcutHandler(DisplayManager displayManager)
		{
			_displayManager = displayManager;
		}

		public void BindShortcut(ToolStripMenuItem item, EmulatorShortcut shortcut, Func<bool> isActionEnabled = null)
		{
			item.Click += (object sender, EventArgs e) => {
				if(isActionEnabled == null || isActionEnabled()) {
					ExecuteShortcut(shortcut);
				}
			};

			_actionEnabledFuncs[shortcut] = isActionEnabled;

			if(item.OwnerItem is ToolStripMenuItem) {
				Action updateShortcut = () => {
					PreferencesConfig cfg = ConfigManager.Config.Preferences;
					int keyIndex = cfg.ShortcutKeys1.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
					if(keyIndex >= 0) {
						item.ShortcutKeyDisplayString = cfg.ShortcutKeys1[keyIndex].KeyCombination.ToString();
					} else {
						keyIndex = cfg.ShortcutKeys2.FindIndex((ShortcutKeyInfo shortcutInfo) => shortcutInfo.Shortcut == shortcut);
						if(keyIndex >= 0) {
							item.ShortcutKeyDisplayString = cfg.ShortcutKeys2[keyIndex].KeyCombination.ToString();
						} else {
							item.ShortcutKeyDisplayString = "";
						}
					}
					item.Enabled = isActionEnabled == null || isActionEnabled();
				};

				updateShortcut();

				//Update item shortcut text when its parent opens
				((ToolStripMenuItem)item.OwnerItem).DropDownOpening += (object sender, EventArgs e) => { updateShortcut(); };
			}
		}

		public void ExecuteShortcut(EmulatorShortcut shortcut)
		{
			Func<bool> isActionEnabled;
			if(_actionEnabledFuncs.TryGetValue(shortcut, out isActionEnabled)) {
				isActionEnabled = _actionEnabledFuncs[shortcut];
				if(isActionEnabled != null && !isActionEnabled()) {
					//Action disabled
					return;
				}
			}

			bool restoreFullscreen = _displayManager.ExclusiveFullscreen;

			switch(shortcut) {
				case EmulatorShortcut.Pause: TogglePause(); break;
				case EmulatorShortcut.Reset: EmuApi.Reset(); break;
				case EmulatorShortcut.PowerCycle: EmuApi.PowerCycle(); break;
				case EmulatorShortcut.ReloadRom: Task.Run(() => EmuApi.ReloadRom()); break;
				case EmulatorShortcut.PowerOff: Task.Run(() => EmuApi.Stop()); restoreFullscreen = false; break;
				case EmulatorShortcut.Exit: frmMain.Instance.Close(); restoreFullscreen = false; break;

				case EmulatorShortcut.ToggleAudio: ToggleAudio(); break;
				case EmulatorShortcut.IncreaseVolume: IncreaseVolume(); break;
				case EmulatorShortcut.DecreaseVolume: DecreaseVolume(); break;

				case EmulatorShortcut.ToggleFps: ToggleFps(); break;
				case EmulatorShortcut.ToggleGameTimer: ToggleGameTimer(); break;
				case EmulatorShortcut.ToggleFrameCounter: ToggleFrameCounter(); break;
				case EmulatorShortcut.ToggleOsd: ToggleOsd(); break;
				case EmulatorShortcut.ToggleAlwaysOnTop: ToggleAlwaysOnTop(); break;
				case EmulatorShortcut.ToggleDebugInfo: ToggleDebugInfo(); break;
				case EmulatorShortcut.ToggleCheats: ToggleCheats(); break;
				case EmulatorShortcut.MaxSpeed: ToggleMaxSpeed(); break;
				case EmulatorShortcut.ToggleFullscreen: _displayManager.ToggleFullscreen(); restoreFullscreen = false; break;

				case EmulatorShortcut.OpenFile: OpenFile(); break;
				case EmulatorShortcut.IncreaseSpeed: IncreaseEmulationSpeed(); break;
				case EmulatorShortcut.DecreaseSpeed: DecreaseEmulationSpeed(); break;

				case EmulatorShortcut.LoadRandomGame: RandomGameHelper.LoadRandomGame(); break;

				case EmulatorShortcut.SetScale1x: _displayManager.SetScale(1, true); break;
				case EmulatorShortcut.SetScale2x: _displayManager.SetScale(2, true); break;
				case EmulatorShortcut.SetScale3x: _displayManager.SetScale(3, true); break;
				case EmulatorShortcut.SetScale4x: _displayManager.SetScale(4, true); break;
				case EmulatorShortcut.SetScale5x: _displayManager.SetScale(5, true); break;
				case EmulatorShortcut.SetScale6x: _displayManager.SetScale(6, true); break;
		
				case EmulatorShortcut.ToggleBgLayer0: ToggleBgLayer0(); break;
				case EmulatorShortcut.ToggleBgLayer1: ToggleBgLayer1(); break;
				case EmulatorShortcut.ToggleBgLayer2: ToggleBgLayer2(); break;
				case EmulatorShortcut.ToggleBgLayer3: ToggleBgLayer3(); break;
				case EmulatorShortcut.ToggleSprites: ToggleSprites(); break;
				case EmulatorShortcut.EnableAllLayers: EnableAllLayers(); break;

				case EmulatorShortcut.ToggleRecordVideo: ToggleRecordVideo(); break;
				case EmulatorShortcut.ToggleRecordAudio: ToggleRecordAudio(); break;
				case EmulatorShortcut.ToggleRecordMovie: ToggleRecordMovie(); break;

				case EmulatorShortcut.TakeScreenshot: EmuApi.TakeScreenshot(); break;

				case EmulatorShortcut.LoadStateFromFile: SaveStateManager.LoadStateFromFile(); break;
				case EmulatorShortcut.SaveStateToFile: SaveStateManager.SaveStateToFile(); break;

				case EmulatorShortcut.SaveStateSlot1: SaveStateManager.SaveState(1); break;
				case EmulatorShortcut.SaveStateSlot2: SaveStateManager.SaveState(2); break;
				case EmulatorShortcut.SaveStateSlot3: SaveStateManager.SaveState(3); break;
				case EmulatorShortcut.SaveStateSlot4: SaveStateManager.SaveState(4); break;
				case EmulatorShortcut.SaveStateSlot5: SaveStateManager.SaveState(5); break;
				case EmulatorShortcut.SaveStateSlot6: SaveStateManager.SaveState(6); break;
				case EmulatorShortcut.SaveStateSlot7: SaveStateManager.SaveState(7); break;
				case EmulatorShortcut.SaveStateSlot8: SaveStateManager.SaveState(8); break;
				case EmulatorShortcut.SaveStateSlot9: SaveStateManager.SaveState(9); break;
				case EmulatorShortcut.SaveStateSlot10: SaveStateManager.SaveState(10); break;
				case EmulatorShortcut.LoadStateSlot1: SaveStateManager.LoadState(1); break;
				case EmulatorShortcut.LoadStateSlot2: SaveStateManager.LoadState(2); break;
				case EmulatorShortcut.LoadStateSlot3: SaveStateManager.LoadState(3); break;
				case EmulatorShortcut.LoadStateSlot4: SaveStateManager.LoadState(4); break;
				case EmulatorShortcut.LoadStateSlot5: SaveStateManager.LoadState(5); break;
				case EmulatorShortcut.LoadStateSlot6: SaveStateManager.LoadState(6); break;
				case EmulatorShortcut.LoadStateSlot7: SaveStateManager.LoadState(7); break;
				case EmulatorShortcut.LoadStateSlot8: SaveStateManager.LoadState(8); break;
				case EmulatorShortcut.LoadStateSlot9: SaveStateManager.LoadState(9); break;
				case EmulatorShortcut.LoadStateSlot10: SaveStateManager.LoadState(10); break;
				case EmulatorShortcut.LoadStateSlotAuto: SaveStateManager.LoadState(11); break;

				case EmulatorShortcut.LoadStateDialog:
					if(_displayManager.ExclusiveFullscreen) {
						_displayManager.SetFullscreenState(false);
						restoreFullscreen = false;
					}
					frmMain.Instance.ShowGameScreen(GameScreenMode.LoadState);
					break;

				case EmulatorShortcut.SaveStateDialog:
					if(_displayManager.ExclusiveFullscreen) {
						_displayManager.SetFullscreenState(false);
						restoreFullscreen = false;
					}
					frmMain.Instance.ShowGameScreen(GameScreenMode.SaveState);
					break;
			}

			if(restoreFullscreen && !_displayManager.ExclusiveFullscreen) {
				//Need to restore fullscreen mode after showing a dialog
				_displayManager.SetFullscreenState(true);
			}
		}

		private static void ToggleRecordVideo()
		{
			if(!EmuApi.IsRunning()) {
				return;
			}

			if(RecordApi.AviIsRecording()) {
				RecordApi.AviStop();
			} else {
				string filename = GetOutputFilename(ConfigManager.AviFolder, ConfigManager.Config.AviRecord.Codec == VideoCodec.GIF ? ".gif" : ".avi");
				RecordApi.AviRecord(filename, ConfigManager.Config.AviRecord.Codec, ConfigManager.Config.AviRecord.CompressionLevel);
			}
		}

		private static void ToggleRecordAudio()
		{
			if(!EmuApi.IsRunning()) {
				return;
			}

			if(RecordApi.WaveIsRecording()) {
				RecordApi.WaveStop();
			} else {
				string filename = GetOutputFilename(ConfigManager.WaveFolder, ".wav");
				RecordApi.WaveRecord(filename);
			}
		}

		private static void ToggleRecordMovie()
		{
			if(!EmuApi.IsRunning()) {
				return;
			}

			if(!RecordApi.MoviePlaying() && !NetplayApi.IsConnected()) {
				if(RecordApi.MovieRecording()) {
					RecordApi.MovieStop();
				} else {
					RecordMovieOptions options = new RecordMovieOptions(
						GetOutputFilename(ConfigManager.MovieFolder, ".msm"),
						ConfigManager.Config.MovieRecord.Author,
						ConfigManager.Config.MovieRecord.Description,
						ConfigManager.Config.MovieRecord.RecordFrom
					);
					RecordApi.MovieRecord(ref options);
				}
			}
		}

		private static string GetOutputFilename(string folder, string ext)
		{
			DateTime now = DateTime.Now;
			string baseName = EmuApi.GetRomInfo().GetRomName();
			string dateTime = " " + now.ToShortDateString() + " " + now.ToLongTimeString();
			string filename = baseName + dateTime + ext;

			//Replace any illegal chars with _
			filename = string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));

			return Path.Combine(folder, filename);
		}

		private void OpenFile()
		{
			using(OpenFileDialog ofd = new OpenFileDialog()) {
				ofd.SetFilter(ResourceHelper.GetMessage("FilterRom"));
				
				if(ConfigManager.Config.Preferences.OverrideGameFolder && Directory.Exists(ConfigManager.Config.Preferences.GameFolder)) {
					ofd.InitialDirectory = ConfigManager.Config.Preferences.GameFolder;
				} else if(ConfigManager.Config.RecentFiles.Items.Count > 0) {
					ofd.InitialDirectory = ConfigManager.Config.RecentFiles.Items[0].RomFile.Folder;
				}

				if(ofd.ShowDialog(frmMain.Instance) == DialogResult.OK) {
					EmuRunner.LoadFile(ofd.FileName);
				}
			}
		}
				
		public void SetRegion(ConsoleRegion region)
		{
			ConfigManager.Config.Emulation.Region = region;
			ConfigManager.Config.Emulation.ApplyConfig();
			ConfigManager.ApplyChanges();
		}
		
		public void SetVideoFilter(VideoFilterType filter)
		{
			ConfigManager.Config.Video.VideoFilter = filter;
			ConfigManager.Config.Video.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void InvertConfigFlag(ref bool flag)
		{
			flag = !flag;
			ConfigManager.Config.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		public void ToggleBilinearInterpolation()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.UseBilinearInterpolation);
		}

		public void ToggleBlendHighResolutionModes()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.BlendHighResolutionModes);
		}

		private void ToggleBgLayer0()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.HideBgLayer0);
			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage(ConfigManager.Config.Video.HideBgLayer0 ? "BgLayerDisabled" : "BgLayerEnabled", "1"));
		}

		private void ToggleBgLayer1()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.HideBgLayer1);
			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage(ConfigManager.Config.Video.HideBgLayer1 ? "BgLayerDisabled" : "BgLayerEnabled", "2"));
		}

		private void ToggleBgLayer2()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.HideBgLayer2);
			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage(ConfigManager.Config.Video.HideBgLayer2 ? "BgLayerDisabled" : "BgLayerEnabled", "3"));
		}

		private void ToggleBgLayer3()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.HideBgLayer3);
			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage(ConfigManager.Config.Video.HideBgLayer3 ? "BgLayerDisabled" : "BgLayerEnabled", "4"));
		}

		private void ToggleSprites()
		{
			InvertConfigFlag(ref ConfigManager.Config.Video.HideSprites);
			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage(ConfigManager.Config.Video.HideSprites ? "SpriteLayerDisabled" : "SpriteLayerEnabled"));
		}
		
		private void EnableAllLayers()
		{
			ConfigManager.Config.Video.HideBgLayer0 = false;
			ConfigManager.Config.Video.HideBgLayer1 = false;
			ConfigManager.Config.Video.HideBgLayer2 = false;
			ConfigManager.Config.Video.HideBgLayer3 = false;
			ConfigManager.Config.Video.HideSprites = false;
			ConfigManager.Config.ApplyConfig();
			ConfigManager.ApplyChanges();

			EmuApi.DisplayMessage("Debug", ResourceHelper.GetMessage("AllLayersEnabled"));
		}

		private void SetEmulationSpeed(uint emulationSpeed)
		{
			ConfigManager.Config.Emulation.EmulationSpeed = emulationSpeed;
			ConfigManager.Config.Emulation.ApplyConfig();
			ConfigManager.ApplyChanges();

			if(emulationSpeed == 0) {
				EmuApi.DisplayMessage("EmulationSpeed", "EmulationMaximumSpeed");
			} else {
				EmuApi.DisplayMessage("EmulationSpeed", "EmulationSpeedPercent", emulationSpeed.ToString());
			}
		}

		private void IncreaseEmulationSpeed()
		{
			uint emulationSpeed = ConfigManager.Config.Emulation.EmulationSpeed;
			if(emulationSpeed == _speedValues[_speedValues.Count - 1]) {
				SetEmulationSpeed(0);
			} else if(emulationSpeed != 0) {
				for(int i = 0; i < _speedValues.Count; i++) {
					if(_speedValues[i] > emulationSpeed) {
						SetEmulationSpeed(_speedValues[i]);
						break;
					}
				}
			}
		}

		private void DecreaseEmulationSpeed()
		{
			uint emulationSpeed = ConfigManager.Config.Emulation.EmulationSpeed;
			if(emulationSpeed == 0) {
				SetEmulationSpeed(_speedValues[_speedValues.Count - 1]);
			} else if(emulationSpeed > _speedValues[0]) {
				for(int i = _speedValues.Count - 1; i >= 0; i--) {
					if(_speedValues[i] < emulationSpeed) {
						SetEmulationSpeed(_speedValues[i]);
						break;
					}
				}
			}
		}
		
		private void ToggleMaxSpeed()
		{
			if(ConfigManager.Config.Emulation.EmulationSpeed == 0) {
				SetEmulationSpeed(100);
			} else {
				SetEmulationSpeed(0);
			}
		}

		private void ToggleCheats()
		{
			InvertConfigFlag(ref ConfigManager.Config.Cheats.DisableAllCheats);
			CheatCodes.ApplyCheats();
		}

		private void ToggleOsd()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.DisableOsd);
		}

		private void ToggleFps()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.ShowFps);
		}

		private void ToggleAudio()
		{
			InvertConfigFlag(ref ConfigManager.Config.Audio.EnableAudio);
		}

		private void IncreaseVolume()
		{
			ConfigManager.Config.Audio.MasterVolume = (uint)Math.Min(100, (int)ConfigManager.Config.Audio.MasterVolume + 5);
			ConfigManager.Config.Audio.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void DecreaseVolume()
		{
			ConfigManager.Config.Audio.MasterVolume = (uint)Math.Max(0, (int)ConfigManager.Config.Audio.MasterVolume - 5);
			ConfigManager.Config.Audio.ApplyConfig();
			ConfigManager.ApplyChanges();
		}

		private void ToggleFrameCounter()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.ShowFrameCounter);
		}
		
		private void ToggleGameTimer()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.ShowGameTimer);
		}

		private void ToggleAlwaysOnTop()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.AlwaysOnTop);
		}

		private void ToggleDebugInfo()
		{
			InvertConfigFlag(ref ConfigManager.Config.Preferences.ShowDebugInfo);
		}

		private void TogglePause()
		{
			if(EmuApi.IsPaused()) {
				EmuApi.Resume();
			} else {
				EmuApi.Pause();
			}
		}
	}
}
