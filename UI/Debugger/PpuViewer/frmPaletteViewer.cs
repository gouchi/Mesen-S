﻿using Mesen.GUI.Debugger.Controls;
using Mesen.GUI.Forms;
using System;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public partial class frmPaletteViewer : BaseForm, IRefresh, IDebuggerWindow
	{
		private WindowRefreshManager _refreshManager;
		public CpuType CpuType { get; private set; }
		public ctrlScanlineCycleSelect ScanlineCycleSelect { get { return this.ctrlScanlineCycleSelect; } }

		public frmPaletteViewer(CpuType cpuType)
		{
			this.CpuType = cpuType;
			InitializeComponent();
			ctrlPaletteViewer.CpuType = cpuType;
			if(cpuType == CpuType.Gameboy) {
				this.Text = "GB " + this.Text;
			}
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);
			if(DesignMode) {
				return;
			}

			_refreshManager = new WindowRefreshManager(this);
			_refreshManager.AutoRefresh = true;
			ctrlScanlineCycleSelect.Initialize(241, 0, this.CpuType);

			double scale = (double)ctrlPaletteViewer.Width / 256;
			ctrlPaletteViewer.PaletteScale = (int)(16 * scale);

			ctrlPaletteViewer.RefreshData();
			ctrlPaletteViewer.RefreshViewer();
			UpdateFields();
		}

		private void UpdateFields()
		{
			int index = ctrlPaletteViewer.SelectedPalette;
			byte[] cgram = ctrlPaletteViewer.CgRam;
			int color = (cgram[index * 2] | (cgram[index * 2 + 1] << 8));
			txtIndex.Text = index.ToString();
			txtValue.Text = color.ToString("X4");
			txtR.Text = (color & 0x1F).ToString();
			txtG.Text = ((color >> 5) & 0x1F).ToString();
			txtB.Text = ((color >> 10) & 0x1F).ToString();
			txtRgb.Text = (ctrlPaletteViewer.ToArgb(color) & 0xFFFFFF).ToString("X6");
		}

		protected override void OnFormClosed(FormClosedEventArgs e)
		{
			_refreshManager?.Dispose();
			base.OnFormClosed(e);
		}

		public void RefreshData()
		{
			ctrlPaletteViewer.RefreshData();
		}

		public void RefreshViewer()
		{
			ctrlPaletteViewer.RefreshViewer();
			UpdateFields();
		}

		private void ctrlPaletteViewer_SelectionChanged()
		{
			UpdateFields();
		}
	}
}