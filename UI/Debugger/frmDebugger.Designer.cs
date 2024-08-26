﻿namespace Mesen.GUI.Debugger
{
	partial class frmDebugger
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if(disposing && (components != null)) {
				components.Dispose();
			}
			if(this._notifListener != null) {
				this._notifListener.Dispose();
				this._notifListener = null;
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.ctrlDisassemblyView = new Mesen.GUI.Debugger.Controls.ctrlDisassemblyView();
			this.ctrlMesenMenuStrip1 = new Mesen.GUI.Controls.ctrlMesenMenuStrip();
			this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuReloadRom = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem16 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSaveRomAs = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuSaveAsIps = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem14 = new System.Windows.Forms.ToolStripSeparator();
			this.importExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDbgIntegrationSettings = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem15 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuImportLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuExportLabels = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem7 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuCodeDataLogger = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetCdlLog = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCdlGenerateRom = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCdlStripUnusedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuCdlStripUsedData = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuAutoResetCdl = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem13 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuExit = new System.Windows.Forms.ToolStripMenuItem();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuContinue = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuStepInto = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOver = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepOut = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuStepBack = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPowerCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem24 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuToggleBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuEnableDisableBreakpoint = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuRunPpuCycle = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunScanline = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuRunOneFrame = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem8 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakIn = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOn = new System.Windows.Forms.ToolStripMenuItem();
			this.searchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAll = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem11 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFind = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindNext = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuFindPrev = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem9 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFindAllOccurrences = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoTo = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToAddress = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem23 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToProgramCounter = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem22 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGoToResetHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToIrqHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToNmiHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToBrkHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGoToCopHandler = new System.Windows.Forms.ToolStripMenuItem();
			this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassemblyOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUnidentifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideUnident = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleUnident = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowUnident = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuVerifiedData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuHideData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDisassembleData = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuShowData = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem6 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowByteCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUseLowerCaseDisassembly = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuUseAltSpcOpNames = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnPowerCycleReset = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnOpen = new System.Windows.Forms.ToolStripMenuItem();
			this.sepBrkCopStpWdm = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnBrk = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnCop = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnStp = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBreakOnWdm = new System.Windows.Forms.ToolStripMenuItem();
			this.sepBreakOnUnitRead = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBreakOnUnitRead = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem10 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuBringToFrontOnBreak = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuBringToFrontOnPause = new System.Windows.Forms.ToolStripMenuItem();
			this.sepGameboyBreak = new System.Windows.Forms.ToolStripSeparator();
			this.mnuGbBreakOnOamCorruption = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGbBreakOnInvalidOamAccess = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGbBreakOnInvalidVramAccess = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGbBreakOnDisableLcdOutsideVblank = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGbBreakOnInvalidOpCode = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuGbBreakOnNopLoad = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem5 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuFontOptions = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuIncreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuDecreaseFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuResetFontSize = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem21 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuSelectFont = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem4 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuConfigureColors = new System.Windows.Forms.ToolStripMenuItem();
			this.mnuPreferences = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem12 = new System.Windows.Forms.ToolStripSeparator();
			this.mnuShowMemoryMappings = new System.Windows.Forms.ToolStripMenuItem();
			this.ctrlSplitContainer = new Mesen.GUI.Controls.ctrlSplitContainer();
			this.pnlStatus = new System.Windows.Forms.Panel();
			this.ctrlLabelList = new Mesen.GUI.Debugger.Controls.ctrlLabelList();
			this.ctrlPpuStatus = new Mesen.GUI.Debugger.Controls.ctrlPpuStatus();
			this.tlpBottomPanel = new System.Windows.Forms.TableLayoutPanel();
			this.grpWatch = new System.Windows.Forms.GroupBox();
			this.picWatchHelp = new System.Windows.Forms.PictureBox();
			this.ctrlWatch = new Mesen.GUI.Debugger.ctrlWatch();
			this.grpBreakpoints = new System.Windows.Forms.GroupBox();
			this.ctrlBreakpoints = new Mesen.GUI.Debugger.Controls.ctrlBreakpoints();
			this.grpCallstack = new System.Windows.Forms.GroupBox();
			this.ctrlCallstack = new Mesen.GUI.Debugger.Controls.ctrlCallstack();
			this.tsToolbar = new Mesen.GUI.Controls.ctrlMesenToolStrip();
			this.ctrlMesenMenuStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplitContainer)).BeginInit();
			this.ctrlSplitContainer.Panel1.SuspendLayout();
			this.ctrlSplitContainer.Panel2.SuspendLayout();
			this.ctrlSplitContainer.SuspendLayout();
			this.pnlStatus.SuspendLayout();
			this.tlpBottomPanel.SuspendLayout();
			this.grpWatch.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).BeginInit();
			this.grpBreakpoints.SuspendLayout();
			this.grpCallstack.SuspendLayout();
			this.SuspendLayout();
			// 
			// ctrlDisassemblyView
			// 
			this.ctrlDisassemblyView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlDisassemblyView.Location = new System.Drawing.Point(0, 0);
			this.ctrlDisassemblyView.Name = "ctrlDisassemblyView";
			this.ctrlDisassemblyView.Size = new System.Drawing.Size(484, 433);
			this.ctrlDisassemblyView.TabIndex = 0;
			// 
			// ctrlMesenMenuStrip1
			// 
			this.ctrlMesenMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.debugToolStripMenuItem,
            this.searchToolStripMenuItem,
            this.optionsToolStripMenuItem});
			this.ctrlMesenMenuStrip1.Location = new System.Drawing.Point(0, 0);
			this.ctrlMesenMenuStrip1.Name = "ctrlMesenMenuStrip1";
			this.ctrlMesenMenuStrip1.Size = new System.Drawing.Size(832, 24);
			this.ctrlMesenMenuStrip1.TabIndex = 1;
			this.ctrlMesenMenuStrip1.Text = "ctrlMesenMenuStrip1";
			// 
			// fileToolStripMenuItem
			// 
			this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuReloadRom,
            this.toolStripMenuItem16,
            this.mnuSaveRomAs,
            this.mnuSaveAsIps,
            this.toolStripMenuItem14,
            this.importExportToolStripMenuItem,
            this.toolStripMenuItem7,
            this.mnuCodeDataLogger,
            this.toolStripMenuItem13,
            this.mnuExit});
			this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
			this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
			this.fileToolStripMenuItem.Text = "File";
			// 
			// mnuReloadRom
			// 
			this.mnuReloadRom.Image = global::Mesen.GUI.Properties.Resources.Refresh;
			this.mnuReloadRom.Name = "mnuReloadRom";
			this.mnuReloadRom.Size = new System.Drawing.Size(201, 22);
			this.mnuReloadRom.Text = "Reload ROM";
			// 
			// toolStripMenuItem16
			// 
			this.toolStripMenuItem16.Name = "toolStripMenuItem16";
			this.toolStripMenuItem16.Size = new System.Drawing.Size(198, 6);
			// 
			// mnuSaveRomAs
			// 
			this.mnuSaveRomAs.Image = global::Mesen.GUI.Properties.Resources.SaveFloppy;
			this.mnuSaveRomAs.Name = "mnuSaveRomAs";
			this.mnuSaveRomAs.Size = new System.Drawing.Size(201, 22);
			this.mnuSaveRomAs.Text = "Save ROM as...";
			// 
			// mnuSaveAsIps
			// 
			this.mnuSaveAsIps.Image = global::Mesen.GUI.Properties.Resources.CheatCode;
			this.mnuSaveAsIps.Name = "mnuSaveAsIps";
			this.mnuSaveAsIps.Size = new System.Drawing.Size(201, 22);
			this.mnuSaveAsIps.Text = "Save edits as IPS patch...";
			// 
			// toolStripMenuItem14
			// 
			this.toolStripMenuItem14.Name = "toolStripMenuItem14";
			this.toolStripMenuItem14.Size = new System.Drawing.Size(198, 6);
			// 
			// importExportToolStripMenuItem
			// 
			this.importExportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDbgIntegrationSettings,
            this.toolStripMenuItem15,
            this.mnuImportLabels,
            this.mnuExportLabels});
			this.importExportToolStripMenuItem.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.importExportToolStripMenuItem.Name = "importExportToolStripMenuItem";
			this.importExportToolStripMenuItem.Size = new System.Drawing.Size(201, 22);
			this.importExportToolStripMenuItem.Text = "Import/Export";
			// 
			// mnuDbgIntegrationSettings
			// 
			this.mnuDbgIntegrationSettings.Image = global::Mesen.GUI.Properties.Resources.Settings;
			this.mnuDbgIntegrationSettings.Name = "mnuDbgIntegrationSettings";
			this.mnuDbgIntegrationSettings.Size = new System.Drawing.Size(180, 22);
			this.mnuDbgIntegrationSettings.Text = "Integration Settings";
			this.mnuDbgIntegrationSettings.Click += new System.EventHandler(this.mnuDbgIntegrationSettings_Click);
			// 
			// toolStripMenuItem15
			// 
			this.toolStripMenuItem15.Name = "toolStripMenuItem15";
			this.toolStripMenuItem15.Size = new System.Drawing.Size(177, 6);
			// 
			// mnuImportLabels
			// 
			this.mnuImportLabels.Image = global::Mesen.GUI.Properties.Resources.Import;
			this.mnuImportLabels.Name = "mnuImportLabels";
			this.mnuImportLabels.Size = new System.Drawing.Size(180, 22);
			this.mnuImportLabels.Text = "Import Labels";
			this.mnuImportLabels.Click += new System.EventHandler(this.mnuImportLabels_Click);
			// 
			// mnuExportLabels
			// 
			this.mnuExportLabels.Image = global::Mesen.GUI.Properties.Resources.Export;
			this.mnuExportLabels.Name = "mnuExportLabels";
			this.mnuExportLabels.Size = new System.Drawing.Size(180, 22);
			this.mnuExportLabels.Text = "Export Labels";
			this.mnuExportLabels.Click += new System.EventHandler(this.mnuExportLabels_Click);
			// 
			// toolStripMenuItem7
			// 
			this.toolStripMenuItem7.Name = "toolStripMenuItem7";
			this.toolStripMenuItem7.Size = new System.Drawing.Size(198, 6);
			// 
			// mnuCodeDataLogger
			// 
			this.mnuCodeDataLogger.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuResetCdlLog,
            this.mnuCdlGenerateRom,
            this.toolStripSeparator1,
            this.mnuAutoResetCdl});
			this.mnuCodeDataLogger.Image = global::Mesen.GUI.Properties.Resources.VerifiedData;
			this.mnuCodeDataLogger.Name = "mnuCodeDataLogger";
			this.mnuCodeDataLogger.Size = new System.Drawing.Size(201, 22);
			this.mnuCodeDataLogger.Text = "Code/Data Logger";
			this.mnuCodeDataLogger.DropDownOpening += new System.EventHandler(this.mnuCodeDataLogger_DropDownOpening);
			// 
			// mnuResetCdlLog
			// 
			this.mnuResetCdlLog.Image = global::Mesen.GUI.Properties.Resources.Refresh;
			this.mnuResetCdlLog.Name = "mnuResetCdlLog";
			this.mnuResetCdlLog.Size = new System.Drawing.Size(264, 22);
			this.mnuResetCdlLog.Text = "Reset log";
			this.mnuResetCdlLog.Click += new System.EventHandler(this.mnuResetCdlLog_Click);
			// 
			// mnuCdlGenerateRom
			// 
			this.mnuCdlGenerateRom.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuCdlStripUnusedData,
            this.mnuCdlStripUsedData});
			this.mnuCdlGenerateRom.Image = global::Mesen.GUI.Properties.Resources.Copy;
			this.mnuCdlGenerateRom.Name = "mnuCdlGenerateRom";
			this.mnuCdlGenerateRom.Size = new System.Drawing.Size(264, 22);
			this.mnuCdlGenerateRom.Text = "Generate stripped ROM";
			// 
			// mnuCdlStripUnusedData
			// 
			this.mnuCdlStripUnusedData.Name = "mnuCdlStripUnusedData";
			this.mnuCdlStripUnusedData.Size = new System.Drawing.Size(166, 22);
			this.mnuCdlStripUnusedData.Text = "Strip unused data";
			// 
			// mnuCdlStripUsedData
			// 
			this.mnuCdlStripUsedData.Name = "mnuCdlStripUsedData";
			this.mnuCdlStripUsedData.Size = new System.Drawing.Size(166, 22);
			this.mnuCdlStripUsedData.Text = "Strip used data";
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(261, 6);
			// 
			// mnuAutoResetCdl
			// 
			this.mnuAutoResetCdl.Name = "mnuAutoResetCdl";
			this.mnuAutoResetCdl.Size = new System.Drawing.Size(264, 22);
			this.mnuAutoResetCdl.Text = "Auto-reset CDL when ROM changes";
			// 
			// toolStripMenuItem13
			// 
			this.toolStripMenuItem13.Name = "toolStripMenuItem13";
			this.toolStripMenuItem13.Size = new System.Drawing.Size(198, 6);
			// 
			// mnuExit
			// 
			this.mnuExit.Image = global::Mesen.GUI.Properties.Resources.Exit;
			this.mnuExit.Name = "mnuExit";
			this.mnuExit.Size = new System.Drawing.Size(201, 22);
			this.mnuExit.Text = "Exit";
			this.mnuExit.Click += new System.EventHandler(this.mnuExit_Click);
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuContinue,
            this.mnuBreak,
            this.toolStripMenuItem3,
            this.mnuStepInto,
            this.mnuStepOver,
            this.mnuStepOut,
            this.mnuStepBack,
            this.toolStripMenuItem1,
            this.mnuReset,
            this.mnuPowerCycle,
            this.toolStripMenuItem24,
            this.mnuToggleBreakpoint,
            this.mnuEnableDisableBreakpoint,
            this.toolStripMenuItem2,
            this.mnuRunPpuCycle,
            this.mnuRunScanline,
            this.mnuRunOneFrame,
            this.toolStripMenuItem8,
            this.mnuBreakIn,
            this.mnuBreakOn});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.debugToolStripMenuItem.Text = "Debug";
			// 
			// mnuContinue
			// 
			this.mnuContinue.Image = global::Mesen.GUI.Properties.Resources.MediaPlay;
			this.mnuContinue.Name = "mnuContinue";
			this.mnuContinue.Size = new System.Drawing.Size(212, 22);
			this.mnuContinue.Text = "Continue";
			// 
			// mnuBreak
			// 
			this.mnuBreak.Enabled = false;
			this.mnuBreak.Image = global::Mesen.GUI.Properties.Resources.MediaPause;
			this.mnuBreak.Name = "mnuBreak";
			this.mnuBreak.ShortcutKeyDisplayString = "";
			this.mnuBreak.Size = new System.Drawing.Size(212, 22);
			this.mnuBreak.Text = "Break";
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuStepInto
			// 
			this.mnuStepInto.Image = global::Mesen.GUI.Properties.Resources.StepInto;
			this.mnuStepInto.Name = "mnuStepInto";
			this.mnuStepInto.Size = new System.Drawing.Size(212, 22);
			this.mnuStepInto.Text = "Step Into";
			// 
			// mnuStepOver
			// 
			this.mnuStepOver.Image = global::Mesen.GUI.Properties.Resources.StepOver;
			this.mnuStepOver.Name = "mnuStepOver";
			this.mnuStepOver.Size = new System.Drawing.Size(212, 22);
			this.mnuStepOver.Text = "Step Over";
			// 
			// mnuStepOut
			// 
			this.mnuStepOut.Image = global::Mesen.GUI.Properties.Resources.StepOut;
			this.mnuStepOut.Name = "mnuStepOut";
			this.mnuStepOut.Size = new System.Drawing.Size(212, 22);
			this.mnuStepOut.Text = "Step Out";
			// 
			// mnuStepBack
			// 
			this.mnuStepBack.Image = global::Mesen.GUI.Properties.Resources.StepBack;
			this.mnuStepBack.Name = "mnuStepBack";
			this.mnuStepBack.Size = new System.Drawing.Size(212, 22);
			this.mnuStepBack.Text = "Step Back";
			this.mnuStepBack.Visible = false;
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuReset
			// 
			this.mnuReset.Image = global::Mesen.GUI.Properties.Resources.Refresh;
			this.mnuReset.Name = "mnuReset";
			this.mnuReset.Size = new System.Drawing.Size(212, 22);
			this.mnuReset.Text = "Reset";
			// 
			// mnuPowerCycle
			// 
			this.mnuPowerCycle.Image = global::Mesen.GUI.Properties.Resources.PowerCycle;
			this.mnuPowerCycle.Name = "mnuPowerCycle";
			this.mnuPowerCycle.Size = new System.Drawing.Size(212, 22);
			this.mnuPowerCycle.Text = "Power Cycle";
			// 
			// toolStripMenuItem24
			// 
			this.toolStripMenuItem24.Name = "toolStripMenuItem24";
			this.toolStripMenuItem24.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuToggleBreakpoint
			// 
			this.mnuToggleBreakpoint.Image = global::Mesen.GUI.Properties.Resources.Breakpoint;
			this.mnuToggleBreakpoint.Name = "mnuToggleBreakpoint";
			this.mnuToggleBreakpoint.Size = new System.Drawing.Size(212, 22);
			this.mnuToggleBreakpoint.Text = "Toggle Breakpoint";
			// 
			// mnuEnableDisableBreakpoint
			// 
			this.mnuEnableDisableBreakpoint.Image = global::Mesen.GUI.Properties.Resources.BreakpointDisabled;
			this.mnuEnableDisableBreakpoint.Name = "mnuEnableDisableBreakpoint";
			this.mnuEnableDisableBreakpoint.Size = new System.Drawing.Size(212, 22);
			this.mnuEnableDisableBreakpoint.Text = "Disable/Enable Breakpoint";
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuRunPpuCycle
			// 
			this.mnuRunPpuCycle.Image = global::Mesen.GUI.Properties.Resources.RunPpuCycle;
			this.mnuRunPpuCycle.Name = "mnuRunPpuCycle";
			this.mnuRunPpuCycle.Size = new System.Drawing.Size(212, 22);
			this.mnuRunPpuCycle.Text = "Run one PPU cycle";
			// 
			// mnuRunScanline
			// 
			this.mnuRunScanline.Image = global::Mesen.GUI.Properties.Resources.RunPpuScanline;
			this.mnuRunScanline.Name = "mnuRunScanline";
			this.mnuRunScanline.Size = new System.Drawing.Size(212, 22);
			this.mnuRunScanline.Text = "Run one scanline";
			// 
			// mnuRunOneFrame
			// 
			this.mnuRunOneFrame.Image = global::Mesen.GUI.Properties.Resources.RunPpuFrame;
			this.mnuRunOneFrame.Name = "mnuRunOneFrame";
			this.mnuRunOneFrame.Size = new System.Drawing.Size(212, 22);
			this.mnuRunOneFrame.Text = "Run one frame";
			// 
			// toolStripMenuItem8
			// 
			this.toolStripMenuItem8.Name = "toolStripMenuItem8";
			this.toolStripMenuItem8.Size = new System.Drawing.Size(209, 6);
			// 
			// mnuBreakIn
			// 
			this.mnuBreakIn.Name = "mnuBreakIn";
			this.mnuBreakIn.Size = new System.Drawing.Size(212, 22);
			this.mnuBreakIn.Text = "Break in...";
			// 
			// mnuBreakOn
			// 
			this.mnuBreakOn.Name = "mnuBreakOn";
			this.mnuBreakOn.Size = new System.Drawing.Size(212, 22);
			this.mnuBreakOn.Text = "Break on...";
			// 
			// searchToolStripMenuItem
			// 
			this.searchToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToAll,
            this.toolStripMenuItem11,
            this.mnuFind,
            this.mnuFindNext,
            this.mnuFindPrev,
            this.toolStripMenuItem9,
            this.mnuFindAllOccurrences,
            this.mnuGoTo});
			this.searchToolStripMenuItem.Name = "searchToolStripMenuItem";
			this.searchToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.searchToolStripMenuItem.Text = "Search";
			// 
			// mnuGoToAll
			// 
			this.mnuGoToAll.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuGoToAll.Name = "mnuGoToAll";
			this.mnuGoToAll.Size = new System.Drawing.Size(183, 22);
			this.mnuGoToAll.Text = "Go to All";
			this.mnuGoToAll.Click += new System.EventHandler(this.mnuGoToAll_Click);
			// 
			// toolStripMenuItem11
			// 
			this.toolStripMenuItem11.Name = "toolStripMenuItem11";
			this.toolStripMenuItem11.Size = new System.Drawing.Size(180, 6);
			// 
			// mnuFind
			// 
			this.mnuFind.Image = global::Mesen.GUI.Properties.Resources.Find;
			this.mnuFind.Name = "mnuFind";
			this.mnuFind.Size = new System.Drawing.Size(183, 22);
			this.mnuFind.Text = "Find...";
			// 
			// mnuFindNext
			// 
			this.mnuFindNext.Image = global::Mesen.GUI.Properties.Resources.NextArrow;
			this.mnuFindNext.Name = "mnuFindNext";
			this.mnuFindNext.Size = new System.Drawing.Size(183, 22);
			this.mnuFindNext.Text = "Find Next";
			// 
			// mnuFindPrev
			// 
			this.mnuFindPrev.Image = global::Mesen.GUI.Properties.Resources.PreviousArrow;
			this.mnuFindPrev.Name = "mnuFindPrev";
			this.mnuFindPrev.Size = new System.Drawing.Size(183, 22);
			this.mnuFindPrev.Text = "Find Previous";
			// 
			// toolStripMenuItem9
			// 
			this.toolStripMenuItem9.Name = "toolStripMenuItem9";
			this.toolStripMenuItem9.Size = new System.Drawing.Size(180, 6);
			// 
			// mnuFindAllOccurrences
			// 
			this.mnuFindAllOccurrences.Name = "mnuFindAllOccurrences";
			this.mnuFindAllOccurrences.Size = new System.Drawing.Size(183, 22);
			this.mnuFindAllOccurrences.Text = "Find All Occurrences";
			this.mnuFindAllOccurrences.Visible = false;
			// 
			// mnuGoTo
			// 
			this.mnuGoTo.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuGoToAddress,
            this.toolStripMenuItem23,
            this.mnuGoToProgramCounter,
            this.toolStripMenuItem22,
            this.mnuGoToResetHandler,
            this.mnuGoToIrqHandler,
            this.mnuGoToNmiHandler,
            this.mnuGoToBrkHandler,
            this.mnuGoToCopHandler});
			this.mnuGoTo.Name = "mnuGoTo";
			this.mnuGoTo.Size = new System.Drawing.Size(183, 22);
			this.mnuGoTo.Text = "Go To...";
			this.mnuGoTo.DropDownOpening += new System.EventHandler(this.mnuGoTo_DropDownOpening);
			// 
			// mnuGoToAddress
			// 
			this.mnuGoToAddress.Name = "mnuGoToAddress";
			this.mnuGoToAddress.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToAddress.Text = "Address";
			// 
			// toolStripMenuItem23
			// 
			this.toolStripMenuItem23.Name = "toolStripMenuItem23";
			this.toolStripMenuItem23.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuGoToProgramCounter
			// 
			this.mnuGoToProgramCounter.Name = "mnuGoToProgramCounter";
			this.mnuGoToProgramCounter.ShortcutKeyDisplayString = "";
			this.mnuGoToProgramCounter.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToProgramCounter.Text = "Program Counter";
			// 
			// toolStripMenuItem22
			// 
			this.toolStripMenuItem22.Name = "toolStripMenuItem22";
			this.toolStripMenuItem22.Size = new System.Drawing.Size(163, 6);
			// 
			// mnuGoToResetHandler
			// 
			this.mnuGoToResetHandler.Name = "mnuGoToResetHandler";
			this.mnuGoToResetHandler.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToResetHandler.Text = "Reset Handler";
			// 
			// mnuGoToIrqHandler
			// 
			this.mnuGoToIrqHandler.Name = "mnuGoToIrqHandler";
			this.mnuGoToIrqHandler.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToIrqHandler.Text = "IRQ Handler";
			// 
			// mnuGoToNmiHandler
			// 
			this.mnuGoToNmiHandler.Name = "mnuGoToNmiHandler";
			this.mnuGoToNmiHandler.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToNmiHandler.Text = "NMI Handler";
			// 
			// mnuGoToBrkHandler
			// 
			this.mnuGoToBrkHandler.Name = "mnuGoToBrkHandler";
			this.mnuGoToBrkHandler.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToBrkHandler.Text = "BRK Handler";
			// 
			// mnuGoToCopHandler
			// 
			this.mnuGoToCopHandler.Name = "mnuGoToCopHandler";
			this.mnuGoToCopHandler.Size = new System.Drawing.Size(166, 22);
			this.mnuGoToCopHandler.Text = "COP Handler";
			// 
			// optionsToolStripMenuItem
			// 
			this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuDisassemblyOptions,
            this.mnuBreakOptions,
            this.toolStripMenuItem5,
            this.mnuFontOptions,
            this.toolStripMenuItem4,
            this.mnuConfigureColors,
            this.mnuPreferences,
            this.toolStripMenuItem12,
            this.mnuShowMemoryMappings});
			this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
			this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
			this.optionsToolStripMenuItem.Text = "Options";
			// 
			// mnuDisassemblyOptions
			// 
			this.mnuDisassemblyOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuUnidentifiedData,
            this.mnuVerifiedData,
            this.toolStripMenuItem6,
            this.mnuShowByteCode,
            this.mnuUseLowerCaseDisassembly,
            this.mnuUseAltSpcOpNames});
			this.mnuDisassemblyOptions.Name = "mnuDisassemblyOptions";
			this.mnuDisassemblyOptions.Size = new System.Drawing.Size(209, 22);
			this.mnuDisassemblyOptions.Text = "Disassembly Options";
			// 
			// mnuUnidentifiedData
			// 
			this.mnuUnidentifiedData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHideUnident,
            this.mnuDisassembleUnident,
            this.mnuShowUnident});
			this.mnuUnidentifiedData.Image = global::Mesen.GUI.Properties.Resources.UnidentifiedData;
			this.mnuUnidentifiedData.Name = "mnuUnidentifiedData";
			this.mnuUnidentifiedData.Size = new System.Drawing.Size(217, 22);
			this.mnuUnidentifiedData.Text = "Unidentified Code/Data";
			this.mnuUnidentifiedData.DropDownOpening += new System.EventHandler(this.mnuUnidentifiedData_DropDownOpening);
			// 
			// mnuHideUnident
			// 
			this.mnuHideUnident.Name = "mnuHideUnident";
			this.mnuHideUnident.Size = new System.Drawing.Size(170, 22);
			this.mnuHideUnident.Text = "Hide";
			// 
			// mnuDisassembleUnident
			// 
			this.mnuDisassembleUnident.Name = "mnuDisassembleUnident";
			this.mnuDisassembleUnident.Size = new System.Drawing.Size(170, 22);
			this.mnuDisassembleUnident.Text = "Show disassembly";
			// 
			// mnuShowUnident
			// 
			this.mnuShowUnident.Name = "mnuShowUnident";
			this.mnuShowUnident.Size = new System.Drawing.Size(170, 22);
			this.mnuShowUnident.Text = "Show as data";
			// 
			// mnuVerifiedData
			// 
			this.mnuVerifiedData.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuHideData,
            this.mnuDisassembleData,
            this.mnuShowData});
			this.mnuVerifiedData.Image = global::Mesen.GUI.Properties.Resources.VerifiedData;
			this.mnuVerifiedData.Name = "mnuVerifiedData";
			this.mnuVerifiedData.Size = new System.Drawing.Size(217, 22);
			this.mnuVerifiedData.Text = "Verified Data";
			this.mnuVerifiedData.DropDownOpening += new System.EventHandler(this.mnuVerifiedData_DropDownOpening);
			// 
			// mnuHideData
			// 
			this.mnuHideData.Name = "mnuHideData";
			this.mnuHideData.Size = new System.Drawing.Size(170, 22);
			this.mnuHideData.Text = "Hide";
			// 
			// mnuDisassembleData
			// 
			this.mnuDisassembleData.Name = "mnuDisassembleData";
			this.mnuDisassembleData.Size = new System.Drawing.Size(170, 22);
			this.mnuDisassembleData.Text = "Show disassembly";
			// 
			// mnuShowData
			// 
			this.mnuShowData.Name = "mnuShowData";
			this.mnuShowData.Size = new System.Drawing.Size(170, 22);
			this.mnuShowData.Text = "Show as data";
			// 
			// toolStripMenuItem6
			// 
			this.toolStripMenuItem6.Name = "toolStripMenuItem6";
			this.toolStripMenuItem6.Size = new System.Drawing.Size(214, 6);
			// 
			// mnuShowByteCode
			// 
			this.mnuShowByteCode.CheckOnClick = true;
			this.mnuShowByteCode.Name = "mnuShowByteCode";
			this.mnuShowByteCode.Size = new System.Drawing.Size(217, 22);
			this.mnuShowByteCode.Text = "Show byte code";
			// 
			// mnuUseLowerCaseDisassembly
			// 
			this.mnuUseLowerCaseDisassembly.CheckOnClick = true;
			this.mnuUseLowerCaseDisassembly.Name = "mnuUseLowerCaseDisassembly";
			this.mnuUseLowerCaseDisassembly.Size = new System.Drawing.Size(217, 22);
			this.mnuUseLowerCaseDisassembly.Text = "Show in lower case";
			// 
			// mnuUseAltSpcOpNames
			// 
			this.mnuUseAltSpcOpNames.CheckOnClick = true;
			this.mnuUseAltSpcOpNames.Name = "mnuUseAltSpcOpNames";
			this.mnuUseAltSpcOpNames.Size = new System.Drawing.Size(217, 22);
			this.mnuUseAltSpcOpNames.Text = "Use alternative mnemonics";
			// 
			// mnuBreakOptions
			// 
			this.mnuBreakOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuBreakOnPowerCycleReset,
            this.mnuBreakOnOpen,
            this.sepBrkCopStpWdm,
            this.mnuBreakOnBrk,
            this.mnuBreakOnCop,
            this.mnuBreakOnStp,
            this.mnuBreakOnWdm,
            this.sepBreakOnUnitRead,
            this.mnuBreakOnUnitRead,
            this.toolStripMenuItem10,
            this.mnuBringToFrontOnBreak,
            this.mnuBringToFrontOnPause,
            this.sepGameboyBreak,
            this.mnuGbBreakOnOamCorruption,
            this.mnuGbBreakOnInvalidOamAccess,
            this.mnuGbBreakOnInvalidVramAccess,
            this.mnuGbBreakOnDisableLcdOutsideVblank,
            this.mnuGbBreakOnInvalidOpCode,
            this.mnuGbBreakOnNopLoad});
			this.mnuBreakOptions.Name = "mnuBreakOptions";
			this.mnuBreakOptions.Size = new System.Drawing.Size(209, 22);
			this.mnuBreakOptions.Text = "Break Options";
			this.mnuBreakOptions.DropDownOpening += new System.EventHandler(this.mnuBreakOptions_DropDownOpening);
			// 
			// mnuBreakOnPowerCycleReset
			// 
			this.mnuBreakOnPowerCycleReset.Name = "mnuBreakOnPowerCycleReset";
			this.mnuBreakOnPowerCycleReset.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnPowerCycleReset.Text = "Break on power/reset";
			// 
			// mnuBreakOnOpen
			// 
			this.mnuBreakOnOpen.Name = "mnuBreakOnOpen";
			this.mnuBreakOnOpen.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnOpen.Text = "Break when debugger is opened";
			// 
			// sepBrkCopStpWdm
			// 
			this.sepBrkCopStpWdm.Name = "sepBrkCopStpWdm";
			this.sepBrkCopStpWdm.Size = new System.Drawing.Size(262, 6);
			// 
			// mnuBreakOnBrk
			// 
			this.mnuBreakOnBrk.Name = "mnuBreakOnBrk";
			this.mnuBreakOnBrk.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnBrk.Text = "Break on BRK";
			// 
			// mnuBreakOnCop
			// 
			this.mnuBreakOnCop.Name = "mnuBreakOnCop";
			this.mnuBreakOnCop.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnCop.Text = "Break on COP";
			// 
			// mnuBreakOnStp
			// 
			this.mnuBreakOnStp.Name = "mnuBreakOnStp";
			this.mnuBreakOnStp.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnStp.Text = "Break on STP";
			// 
			// mnuBreakOnWdm
			// 
			this.mnuBreakOnWdm.Name = "mnuBreakOnWdm";
			this.mnuBreakOnWdm.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnWdm.Text = "Break on WDM";
			// 
			// sepBreakOnUnitRead
			// 
			this.sepBreakOnUnitRead.Name = "sepBreakOnUnitRead";
			this.sepBreakOnUnitRead.Size = new System.Drawing.Size(262, 6);
			// 
			// mnuBreakOnUnitRead
			// 
			this.mnuBreakOnUnitRead.Name = "mnuBreakOnUnitRead";
			this.mnuBreakOnUnitRead.Size = new System.Drawing.Size(265, 22);
			this.mnuBreakOnUnitRead.Text = "Break on uninitialized memory read";
			// 
			// toolStripMenuItem10
			// 
			this.toolStripMenuItem10.Name = "toolStripMenuItem10";
			this.toolStripMenuItem10.Size = new System.Drawing.Size(262, 6);
			// 
			// mnuBringToFrontOnBreak
			// 
			this.mnuBringToFrontOnBreak.Name = "mnuBringToFrontOnBreak";
			this.mnuBringToFrontOnBreak.Size = new System.Drawing.Size(265, 22);
			this.mnuBringToFrontOnBreak.Text = "Bring debugger to front on break";
			// 
			// mnuBringToFrontOnPause
			// 
			this.mnuBringToFrontOnPause.Name = "mnuBringToFrontOnPause";
			this.mnuBringToFrontOnPause.Size = new System.Drawing.Size(265, 22);
			this.mnuBringToFrontOnPause.Text = "Bring debugger to front on pause";
			this.mnuBringToFrontOnPause.Visible = false;
			// 
			// sepGameboyBreak
			// 
			this.sepGameboyBreak.Name = "sepGameboyBreak";
			this.sepGameboyBreak.Size = new System.Drawing.Size(262, 6);
			this.sepGameboyBreak.Visible = false;
			// 
			// mnuGbBreakOnOamCorruption
			// 
			this.mnuGbBreakOnOamCorruption.Name = "mnuGbBreakOnOamCorruption";
			this.mnuGbBreakOnOamCorruption.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnOamCorruption.Text = "Break on OAM corruption";
			this.mnuGbBreakOnOamCorruption.Visible = false;
			// 
			// mnuGbBreakOnInvalidOamAccess
			// 
			this.mnuGbBreakOnInvalidOamAccess.Name = "mnuGbBreakOnInvalidOamAccess";
			this.mnuGbBreakOnInvalidOamAccess.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnInvalidOamAccess.Text = "Break on invalid OAM access";
			this.mnuGbBreakOnInvalidOamAccess.Visible = false;
			// 
			// mnuGbBreakOnInvalidVramAccess
			// 
			this.mnuGbBreakOnInvalidVramAccess.Name = "mnuGbBreakOnInvalidVramAccess";
			this.mnuGbBreakOnInvalidVramAccess.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnInvalidVramAccess.Text = "Break on invalid VRAM access";
			this.mnuGbBreakOnInvalidVramAccess.Visible = false;
			// 
			// mnuGbBreakOnDisableLcdOutsideVblank
			// 
			this.mnuGbBreakOnDisableLcdOutsideVblank.Name = "mnuGbBreakOnDisableLcdOutsideVblank";
			this.mnuGbBreakOnDisableLcdOutsideVblank.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnDisableLcdOutsideVblank.Text = "Break on LCD disable outside vblank";
			this.mnuGbBreakOnDisableLcdOutsideVblank.Visible = false;
			// 
			// mnuGbBreakOnInvalidOpCode
			// 
			this.mnuGbBreakOnInvalidOpCode.Name = "mnuGbBreakOnInvalidOpCode";
			this.mnuGbBreakOnInvalidOpCode.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnInvalidOpCode.Text = "Break on invalid instructions";
			this.mnuGbBreakOnInvalidOpCode.Visible = false;
			// 
			// mnuGbBreakOnNopLoad
			// 
			this.mnuGbBreakOnNopLoad.Name = "mnuGbBreakOnNopLoad";
			this.mnuGbBreakOnNopLoad.Size = new System.Drawing.Size(265, 22);
			this.mnuGbBreakOnNopLoad.Text = "Break on LD B, B (nop)";
			this.mnuGbBreakOnNopLoad.Visible = false;
			// 
			// toolStripMenuItem5
			// 
			this.toolStripMenuItem5.Name = "toolStripMenuItem5";
			this.toolStripMenuItem5.Size = new System.Drawing.Size(206, 6);
			// 
			// mnuFontOptions
			// 
			this.mnuFontOptions.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.mnuIncreaseFontSize,
            this.mnuDecreaseFontSize,
            this.mnuResetFontSize,
            this.toolStripMenuItem21,
            this.mnuSelectFont});
			this.mnuFontOptions.Image = global::Mesen.GUI.Properties.Resources.Font;
			this.mnuFontOptions.Name = "mnuFontOptions";
			this.mnuFontOptions.Size = new System.Drawing.Size(209, 22);
			this.mnuFontOptions.Text = "Font Options";
			// 
			// mnuIncreaseFontSize
			// 
			this.mnuIncreaseFontSize.Name = "mnuIncreaseFontSize";
			this.mnuIncreaseFontSize.ShortcutKeyDisplayString = "";
			this.mnuIncreaseFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuIncreaseFontSize.Text = "Increase Size";
			// 
			// mnuDecreaseFontSize
			// 
			this.mnuDecreaseFontSize.Name = "mnuDecreaseFontSize";
			this.mnuDecreaseFontSize.ShortcutKeyDisplayString = "";
			this.mnuDecreaseFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuDecreaseFontSize.Text = "Decrease Size";
			// 
			// mnuResetFontSize
			// 
			this.mnuResetFontSize.Name = "mnuResetFontSize";
			this.mnuResetFontSize.ShortcutKeyDisplayString = "";
			this.mnuResetFontSize.Size = new System.Drawing.Size(157, 22);
			this.mnuResetFontSize.Text = "Reset to Default";
			// 
			// toolStripMenuItem21
			// 
			this.toolStripMenuItem21.Name = "toolStripMenuItem21";
			this.toolStripMenuItem21.Size = new System.Drawing.Size(154, 6);
			// 
			// mnuSelectFont
			// 
			this.mnuSelectFont.Name = "mnuSelectFont";
			this.mnuSelectFont.Size = new System.Drawing.Size(157, 22);
			this.mnuSelectFont.Text = "Select Font...";
			this.mnuSelectFont.Click += new System.EventHandler(this.mnuSelectFont_Click);
			// 
			// toolStripMenuItem4
			// 
			this.toolStripMenuItem4.Name = "toolStripMenuItem4";
			this.toolStripMenuItem4.Size = new System.Drawing.Size(206, 6);
			// 
			// mnuConfigureColors
			// 
			this.mnuConfigureColors.Image = global::Mesen.GUI.Properties.Resources.PipetteSmall;
			this.mnuConfigureColors.Name = "mnuConfigureColors";
			this.mnuConfigureColors.Size = new System.Drawing.Size(209, 22);
			this.mnuConfigureColors.Text = "Configure colors";
			this.mnuConfigureColors.Click += new System.EventHandler(this.mnuConfigureColors_Click);
			// 
			// mnuPreferences
			// 
			this.mnuPreferences.Image = global::Mesen.GUI.Properties.Resources.Settings;
			this.mnuPreferences.Name = "mnuPreferences";
			this.mnuPreferences.Size = new System.Drawing.Size(209, 22);
			this.mnuPreferences.Text = "Configure shortcut keys...";
			this.mnuPreferences.Click += new System.EventHandler(this.mnuPreferences_Click);
			// 
			// toolStripMenuItem12
			// 
			this.toolStripMenuItem12.Name = "toolStripMenuItem12";
			this.toolStripMenuItem12.Size = new System.Drawing.Size(206, 6);
			// 
			// mnuShowMemoryMappings
			// 
			this.mnuShowMemoryMappings.CheckOnClick = true;
			this.mnuShowMemoryMappings.Name = "mnuShowMemoryMappings";
			this.mnuShowMemoryMappings.Size = new System.Drawing.Size(209, 22);
			this.mnuShowMemoryMappings.Text = "Show memory mappings";
			// 
			// ctrlSplitContainer
			// 
			this.ctrlSplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlSplitContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.ctrlSplitContainer.HidePanel2 = false;
			this.ctrlSplitContainer.Location = new System.Drawing.Point(0, 49);
			this.ctrlSplitContainer.Name = "ctrlSplitContainer";
			this.ctrlSplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
			// 
			// ctrlSplitContainer.Panel1
			// 
			this.ctrlSplitContainer.Panel1.Controls.Add(this.ctrlDisassemblyView);
			this.ctrlSplitContainer.Panel1.Controls.Add(this.pnlStatus);
			this.ctrlSplitContainer.Panel1MinSize = 275;
			// 
			// ctrlSplitContainer.Panel2
			// 
			this.ctrlSplitContainer.Panel2.Controls.Add(this.tlpBottomPanel);
			this.ctrlSplitContainer.Panel2MinSize = 100;
			this.ctrlSplitContainer.Size = new System.Drawing.Size(832, 595);
			this.ctrlSplitContainer.SplitterDistance = 433;
			this.ctrlSplitContainer.TabIndex = 2;
			// 
			// pnlStatus
			// 
			this.pnlStatus.Controls.Add(this.ctrlLabelList);
			this.pnlStatus.Controls.Add(this.ctrlPpuStatus);
			this.pnlStatus.Dock = System.Windows.Forms.DockStyle.Right;
			this.pnlStatus.Location = new System.Drawing.Point(484, 0);
			this.pnlStatus.Name = "pnlStatus";
			this.pnlStatus.Size = new System.Drawing.Size(348, 433);
			this.pnlStatus.TabIndex = 2;
			// 
			// ctrlLabelList
			// 
			this.ctrlLabelList.CpuType = Mesen.GUI.CpuType.Cpu;
			this.ctrlLabelList.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlLabelList.Location = new System.Drawing.Point(0, 47);
			this.ctrlLabelList.Name = "ctrlLabelList";
			this.ctrlLabelList.Padding = new System.Windows.Forms.Padding(3);
			this.ctrlLabelList.Size = new System.Drawing.Size(348, 386);
			this.ctrlLabelList.TabIndex = 4;
			// 
			// ctrlPpuStatus
			// 
			this.ctrlPpuStatus.Dock = System.Windows.Forms.DockStyle.Top;
			this.ctrlPpuStatus.Location = new System.Drawing.Point(0, 0);
			this.ctrlPpuStatus.Name = "ctrlPpuStatus";
			this.ctrlPpuStatus.Padding = new System.Windows.Forms.Padding(3, 0, 3, 0);
			this.ctrlPpuStatus.Size = new System.Drawing.Size(348, 47);
			this.ctrlPpuStatus.TabIndex = 3;
			// 
			// tlpBottomPanel
			// 
			this.tlpBottomPanel.ColumnCount = 3;
			this.tlpBottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tlpBottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tlpBottomPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
			this.tlpBottomPanel.Controls.Add(this.grpWatch, 0, 0);
			this.tlpBottomPanel.Controls.Add(this.grpBreakpoints, 1, 0);
			this.tlpBottomPanel.Controls.Add(this.grpCallstack, 2, 0);
			this.tlpBottomPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tlpBottomPanel.Location = new System.Drawing.Point(0, 0);
			this.tlpBottomPanel.Name = "tlpBottomPanel";
			this.tlpBottomPanel.RowCount = 1;
			this.tlpBottomPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
			this.tlpBottomPanel.Size = new System.Drawing.Size(832, 158);
			this.tlpBottomPanel.TabIndex = 0;
			// 
			// grpWatch
			// 
			this.grpWatch.Controls.Add(this.picWatchHelp);
			this.grpWatch.Controls.Add(this.ctrlWatch);
			this.grpWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpWatch.Location = new System.Drawing.Point(2, 0);
			this.grpWatch.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.grpWatch.Name = "grpWatch";
			this.grpWatch.Size = new System.Drawing.Size(273, 158);
			this.grpWatch.TabIndex = 1;
			this.grpWatch.TabStop = false;
			this.grpWatch.Text = "Watch";
			// 
			// picWatchHelp
			// 
			this.picWatchHelp.Image = global::Mesen.GUI.Properties.Resources.Help;
			this.picWatchHelp.Location = new System.Drawing.Point(44, 0);
			this.picWatchHelp.Name = "picWatchHelp";
			this.picWatchHelp.Size = new System.Drawing.Size(16, 16);
			this.picWatchHelp.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
			this.picWatchHelp.TabIndex = 4;
			this.picWatchHelp.TabStop = false;
			// 
			// ctrlWatch
			// 
			this.ctrlWatch.CpuType = Mesen.GUI.CpuType.Cpu;
			this.ctrlWatch.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlWatch.Location = new System.Drawing.Point(3, 16);
			this.ctrlWatch.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlWatch.Name = "ctrlWatch";
			this.ctrlWatch.Size = new System.Drawing.Size(267, 139);
			this.ctrlWatch.TabIndex = 0;
			// 
			// grpBreakpoints
			// 
			this.grpBreakpoints.Controls.Add(this.ctrlBreakpoints);
			this.grpBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpBreakpoints.Location = new System.Drawing.Point(279, 0);
			this.grpBreakpoints.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.grpBreakpoints.Name = "grpBreakpoints";
			this.grpBreakpoints.Size = new System.Drawing.Size(273, 158);
			this.grpBreakpoints.TabIndex = 2;
			this.grpBreakpoints.TabStop = false;
			this.grpBreakpoints.Text = "Breakpoints";
			// 
			// ctrlBreakpoints
			// 
			this.ctrlBreakpoints.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlBreakpoints.Location = new System.Drawing.Point(3, 16);
			this.ctrlBreakpoints.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlBreakpoints.Name = "ctrlBreakpoints";
			this.ctrlBreakpoints.Size = new System.Drawing.Size(267, 139);
			this.ctrlBreakpoints.TabIndex = 0;
			this.ctrlBreakpoints.BreakpointNavigation += new Mesen.GUI.Debugger.Controls.ctrlBreakpoints.BreakpointNavigationHandler(this.ctrlBreakpoints_BreakpointNavigation);
			// 
			// grpCallstack
			// 
			this.grpCallstack.Controls.Add(this.ctrlCallstack);
			this.grpCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.grpCallstack.Location = new System.Drawing.Point(556, 0);
			this.grpCallstack.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
			this.grpCallstack.Name = "grpCallstack";
			this.grpCallstack.Size = new System.Drawing.Size(274, 158);
			this.grpCallstack.TabIndex = 3;
			this.grpCallstack.TabStop = false;
			this.grpCallstack.Text = "Call Stack";
			// 
			// ctrlCallstack
			// 
			this.ctrlCallstack.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ctrlCallstack.Location = new System.Drawing.Point(3, 16);
			this.ctrlCallstack.Margin = new System.Windows.Forms.Padding(0);
			this.ctrlCallstack.Name = "ctrlCallstack";
			this.ctrlCallstack.Size = new System.Drawing.Size(268, 139);
			this.ctrlCallstack.TabIndex = 0;
			this.ctrlCallstack.FunctionSelected += new Mesen.GUI.Debugger.Controls.ctrlCallstack.NavigateToAddressHandler(this.ctrlCallstack_FunctionSelected);
			// 
			// tsToolbar
			// 
			this.tsToolbar.Location = new System.Drawing.Point(0, 24);
			this.tsToolbar.Name = "tsToolbar";
			this.tsToolbar.Size = new System.Drawing.Size(832, 25);
			this.tsToolbar.TabIndex = 3;
			this.tsToolbar.Text = "ctrlMesenToolStrip1";
			// 
			// frmDebugger
			// 
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(832, 644);
			this.Controls.Add(this.ctrlSplitContainer);
			this.Controls.Add(this.tsToolbar);
			this.Controls.Add(this.ctrlMesenMenuStrip1);
			this.MinimumSize = new System.Drawing.Size(620, 630);
			this.Name = "frmDebugger";
			this.Text = "Debugger";
			this.Controls.SetChildIndex(this.ctrlMesenMenuStrip1, 0);
			this.Controls.SetChildIndex(this.tsToolbar, 0);
			this.Controls.SetChildIndex(this.ctrlSplitContainer, 0);
			this.ctrlMesenMenuStrip1.ResumeLayout(false);
			this.ctrlMesenMenuStrip1.PerformLayout();
			this.ctrlSplitContainer.Panel1.ResumeLayout(false);
			this.ctrlSplitContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.ctrlSplitContainer)).EndInit();
			this.ctrlSplitContainer.ResumeLayout(false);
			this.pnlStatus.ResumeLayout(false);
			this.tlpBottomPanel.ResumeLayout(false);
			this.grpWatch.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.picWatchHelp)).EndInit();
			this.grpBreakpoints.ResumeLayout(false);
			this.grpCallstack.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private Controls.ctrlDisassemblyView ctrlDisassemblyView;
		private GUI.Controls.ctrlMesenMenuStrip ctrlMesenMenuStrip1;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuContinue;
		private System.Windows.Forms.ToolStripMenuItem mnuBreak;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem mnuStepInto;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOver;
		private System.Windows.Forms.ToolStripMenuItem mnuStepOut;
		private System.Windows.Forms.ToolStripMenuItem mnuStepBack;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem mnuReset;
		private System.Windows.Forms.ToolStripMenuItem mnuPowerCycle;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem24;
		private System.Windows.Forms.ToolStripMenuItem mnuToggleBreakpoint;
		private System.Windows.Forms.ToolStripMenuItem mnuEnableDisableBreakpoint;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ToolStripMenuItem mnuRunPpuCycle;
		private System.Windows.Forms.ToolStripMenuItem mnuRunScanline;
		private System.Windows.Forms.ToolStripMenuItem mnuRunOneFrame;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem8;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakIn;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOn;
		private GUI.Controls.ctrlSplitContainer ctrlSplitContainer;
		private System.Windows.Forms.TableLayoutPanel tlpBottomPanel;
		private ctrlWatch ctrlWatch;
		private System.Windows.Forms.GroupBox grpWatch;
		private System.Windows.Forms.GroupBox grpBreakpoints;
		private Controls.ctrlBreakpoints ctrlBreakpoints;
		private GUI.Controls.ctrlMesenToolStrip tsToolbar;
		private System.Windows.Forms.GroupBox grpCallstack;
		private Controls.ctrlCallstack ctrlCallstack;
		private System.Windows.Forms.ToolStripMenuItem searchToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuFind;
		private System.Windows.Forms.ToolStripMenuItem mnuFindNext;
		private System.Windows.Forms.ToolStripMenuItem mnuFindPrev;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem9;
		private System.Windows.Forms.ToolStripMenuItem mnuFindAllOccurrences;
		private System.Windows.Forms.ToolStripMenuItem mnuGoTo;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToAddress;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem23;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToProgramCounter;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem22;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToResetHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToIrqHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToNmiHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToBrkHandler;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToCopHandler;
		private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuPreferences;
		private System.Windows.Forms.PictureBox picWatchHelp;
		private System.Windows.Forms.ToolStripMenuItem mnuFontOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuIncreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuDecreaseFontSize;
		private System.Windows.Forms.ToolStripMenuItem mnuResetFontSize;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem21;
		private System.Windows.Forms.ToolStripMenuItem mnuSelectFont;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem4;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem5;
		private System.Windows.Forms.Panel pnlStatus;
		private Controls.ctrlPpuStatus ctrlPpuStatus;
		private Controls.ctrlLabelList ctrlLabelList;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassemblyOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuUnidentifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuHideUnident;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassembleUnident;
		private System.Windows.Forms.ToolStripMenuItem mnuShowUnident;
		private System.Windows.Forms.ToolStripMenuItem mnuVerifiedData;
		private System.Windows.Forms.ToolStripMenuItem mnuHideData;
		private System.Windows.Forms.ToolStripMenuItem mnuDisassembleData;
		private System.Windows.Forms.ToolStripMenuItem mnuShowData;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem6;
		private System.Windows.Forms.ToolStripMenuItem mnuShowByteCode;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOptions;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnBrk;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnCop;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnWdm;
		private System.Windows.Forms.ToolStripSeparator sepBreakOnUnitRead;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnUnitRead;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnOpen;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem10;
		private System.Windows.Forms.ToolStripMenuItem mnuBringToFrontOnBreak;
		private System.Windows.Forms.ToolStripMenuItem mnuBringToFrontOnPause;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnPowerCycleReset;
		private System.Windows.Forms.ToolStripSeparator sepBrkCopStpWdm;
		private System.Windows.Forms.ToolStripMenuItem mnuBreakOnStp;
		private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importExportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem mnuDbgIntegrationSettings;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem7;
		private System.Windows.Forms.ToolStripMenuItem mnuCodeDataLogger;
		private System.Windows.Forms.ToolStripMenuItem mnuResetCdlLog;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem13;
		private System.Windows.Forms.ToolStripMenuItem mnuExit;
		private System.Windows.Forms.ToolStripMenuItem mnuUseAltSpcOpNames;
		private System.Windows.Forms.ToolStripMenuItem mnuUseLowerCaseDisassembly;
		private System.Windows.Forms.ToolStripMenuItem mnuGoToAll;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem11;
		private System.Windows.Forms.ToolStripMenuItem mnuConfigureColors;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveRomAs;
		private System.Windows.Forms.ToolStripMenuItem mnuSaveAsIps;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem14;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.ToolStripMenuItem mnuCdlGenerateRom;
		private System.Windows.Forms.ToolStripMenuItem mnuCdlStripUnusedData;
		private System.Windows.Forms.ToolStripMenuItem mnuCdlStripUsedData;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem15;
		private System.Windows.Forms.ToolStripMenuItem mnuImportLabels;
		private System.Windows.Forms.ToolStripMenuItem mnuExportLabels;
		private System.Windows.Forms.ToolStripMenuItem mnuReloadRom;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem16;
	  private System.Windows.Forms.ToolStripMenuItem mnuAutoResetCdl;
	  private System.Windows.Forms.ToolStripSeparator toolStripMenuItem12;
	  private System.Windows.Forms.ToolStripMenuItem mnuShowMemoryMappings;
	  private System.Windows.Forms.ToolStripSeparator sepGameboyBreak;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnInvalidOamAccess;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnInvalidVramAccess;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnDisableLcdOutsideVblank;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnInvalidOpCode;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnNopLoad;
	  private System.Windows.Forms.ToolStripMenuItem mnuGbBreakOnOamCorruption;
   }
}