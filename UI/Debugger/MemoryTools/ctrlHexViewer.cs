﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Mesen.GUI.Config;
using Be.Windows.Forms;
using Mesen.GUI.Controls;
using static Be.Windows.Forms.DynamicByteProvider;
using Mesen.GUI.Forms;
using Mesen.GUI.Debugger.Labels;

namespace Mesen.GUI.Debugger.Controls
{
	public partial class ctrlHexViewer : BaseControl
	{
		private FindOptions _findOptions;
		private StaticByteProvider _byteProvider;
		private SnesMemoryType _memoryType;

		private int SelectionStartAddress { get { return (int)ctrlHexBox.SelectionStart; } }
		private int SelectionEndAddress { get { return (int)(ctrlHexBox.SelectionStart + (ctrlHexBox.SelectionLength == 0 ? 0 : (ctrlHexBox.SelectionLength - 1))); } }

		public ctrlHexViewer()
		{
			InitializeComponent();

			this.BaseFont = new Font(BaseControl.MonospaceFontFamily, 10, FontStyle.Regular);
			this.ctrlHexBox.ContextMenuStrip = this.ctxMenuStrip;
			this.ctrlHexBox.SelectionForeColor = Color.White;
			this.ctrlHexBox.SelectionBackColor = Color.FromArgb(31, 123, 205);
			this.ctrlHexBox.ShadowSelectionColor = Color.FromArgb(100, 60, 128, 200);
			this.ctrlHexBox.InfoBackColor = Color.FromArgb(235, 235, 235);
			this.ctrlHexBox.InfoForeColor = Color.Gray;
			this.ctrlHexBox.HighlightCurrentRowColumn = true;
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad(e);

			if(!IsDesignMode) {
				cboNumberColumns.SelectedIndex = ConfigManager.Config.Debug.HexEditor.ColumnCount;
				InitShortcuts();
			}
		}

		private void InitShortcuts()
		{
			mnuAddToWatch.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_AddToWatch));
			mnuEditBreakpoint.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_EditBreakpoint));
			mnuEditLabel.InitShortcut(this, nameof(DebuggerShortcutsConfig.MemoryViewer_EditLabel));

			mnuMarkAsCode.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsCode));
			mnuMarkAsCode.Click += (s, e) => MarkSelectionAs(CdlFlags.Code);
			mnuMarkAsData.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsData));
			mnuMarkAsData.Click += (s, e) => MarkSelectionAs(CdlFlags.Data);
			mnuMarkAsUnidentifiedData.InitShortcut(this, nameof(DebuggerShortcutsConfig.MarkAsUnidentified));
			mnuMarkAsUnidentifiedData.Click += (s, e) => MarkSelectionAs(CdlFlags.None);
		}

		public new void Focus()
		{
			this.ctrlHexBox.Focus();
		}

		public byte[] GetData()
		{
			return this._byteProvider != null ? this._byteProvider.Bytes.ToArray() : new byte[0];
		}
		
		public void RefreshData(SnesMemoryType memoryType)
		{
			if(_memoryType != memoryType) {
				_memoryType = memoryType;
				_byteProvider = null;
			}

			byte[] data = DebugApi.GetMemoryState(this._memoryType);

			if(data != null) {
				bool changed = true;
				if(this._byteProvider != null && data.Length == this._byteProvider.Length) {
					changed = false;
					for(int i = 0; i < this._byteProvider.Length; i++) {
						if(this._byteProvider.Bytes[i] != data[i]) {
							changed = true;
							break;
						}
					}
				}

				if(changed) {
					if(_byteProvider == null || _byteProvider.Length != data.Length) {
						_byteProvider = new StaticByteProvider(data);
						_byteProvider.ByteChanged += (int byteIndex, byte newValue, byte oldValue) => {
							DebugApi.SetMemoryValue(_memoryType, (UInt32)byteIndex, newValue);
						};
						_byteProvider.BytesChanged += (int byteIndex, byte[] values) => {
							DebugApi.SetMemoryValues(_memoryType, (UInt32)byteIndex, values, values.Length);
						};
						this.ctrlHexBox.ByteProvider = _byteProvider;
					} else {
						_byteProvider.SetData(data);
					}
					this.ctrlHexBox.Refresh();
				}
			}
		}

		private int ColumnCount
		{
			get { return Int32.Parse(this.cboNumberColumns.Text); }
		}

		public int RequiredWidth
		{
			get { return this.ctrlHexBox.RequiredWidth;	}
		}
				
		private void cboNumberColumns_SelectedIndexChanged(object sender, EventArgs e)
		{
			this.ctrlHexBox.Focus();

			this.ctrlHexBox.BytesPerLine = this.ColumnCount;
			this.ctrlHexBox.UseFixedBytesPerLine = true;

			ConfigManager.Config.Debug.HexEditor.ColumnCount = this.cboNumberColumns.SelectedIndex;
			ConfigManager.ApplyChanges();
		}

		public Font HexFont
		{
			get { return this.ctrlHexBox.Font; }
		}

		private int _textZoom = 100;
		public int TextZoom
		{
			get { return _textZoom; }
			set
			{
				if(value >= 30 && value <= 500) {
					_textZoom = value;
					this.UpdateFont();
				}
			}
		}

		private Font _baseFont = new Font(BaseControl.MonospaceFontFamily, BaseControl.DefaultFontSize, FontStyle.Regular); 
		public Font BaseFont {
			get { return _baseFont; }
			set
			{
				if(!value.Equals(_baseFont)) {
					_baseFont = value;
					this.UpdateFont();
				}
			}
		}

		public void UpdateFont()
		{
			this.ctrlHexBox.Font = new Font(BaseFont.FontFamily, BaseFont.Size * _textZoom / 100f, BaseFont.Style);
		}
		
		public void GoToAddress(int address)
		{
			this.ctrlHexBox.ScrollByteIntoView(GetData().Length - 1);
			this.ctrlHexBox.ScrollByteIntoView(address);
			this.ctrlHexBox.Select(address, 0);
			this.ctrlHexBox.Focus();
		}

		public void GoToAddress()
		{
			GoToAddress address = new GoToAddress();

			int currentAddr = (int)(this.ctrlHexBox.CurrentLine - 1) * this.ctrlHexBox.BytesPerLine;
			address.Address = (UInt32)currentAddr;

			using(frmGoToLine frm = new frmGoToLine(address, (_byteProvider.Length - 1).ToString("X").Length)) {
				frm.StartPosition = FormStartPosition.Manual;
				Point topLeft = this.PointToScreen(new Point(0, 0));
				frm.Location = new Point(topLeft.X + (this.Width - frm.Width) / 2, topLeft.Y + (this.Height - frm.Height) / 2);
				if(frm.ShowDialog() == DialogResult.OK) {
					GoToAddress((int)address.Address);
				}
			}
		}

		public void OpenSearchBox(bool forceFocus = false)
		{
			this._findOptions = new Be.Windows.Forms.FindOptions();
			this._findOptions.Type = chkTextSearch.Checked ? FindType.Text : FindType.Hex;
			this._findOptions.MatchCase = false;
			this._findOptions.Text = this.cboSearch.Text;
			this._findOptions.WrapSearch = true;

			bool focus = !this.panelSearch.Visible;
			this.panelSearch.Visible = true;

			if(Program.IsMono) {
				//Mono doesn't resize the TLP properly for some reason when set to autosize
				this.tlpMain.RowStyles[2].SizeType = System.Windows.Forms.SizeType.Absolute;
				this.tlpMain.RowStyles[2].Height = 30;
			}
			if(focus || forceFocus) {
				this.cboSearch.Focus();
				this.cboSearch.SelectAll();
			}
		}

		private void CloseSearchBox()
		{
			this.panelSearch.Visible = false;
			if(Program.IsMono) {
				//Mono doesn't resize the TLP properly for some reason when set to autosize
				this.tlpMain.RowStyles[2].SizeType = System.Windows.Forms.SizeType.Absolute;
				this.tlpMain.RowStyles[2].Height = 0;
			}			
			this.Focus();
		}

		protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
		{
			UpdateActionAvailability();

			if(keyData == ConfigManager.Config.Debug.Shortcuts.Find) {
				this.OpenSearchBox(true);
				return true;
			} else if(keyData == ConfigManager.Config.Debug.Shortcuts.IncreaseFontSize) {
				this.TextZoom += 10;
				return true;
			} else if(keyData == ConfigManager.Config.Debug.Shortcuts.DecreaseFontSize) {
				this.TextZoom -= 10;
				return true;
			} else if(keyData == ConfigManager.Config.Debug.Shortcuts.ResetFontSize) {
				this.TextZoom = 100;
				return true;
			}

			if(this.cboSearch.Focused) {
				if(keyData == Keys.Escape) {
					this.CloseSearchBox();
					return true;
				}
			}

			return base.ProcessCmdKey(ref msg, keyData);
		}

		public void FindNext()
		{
			this.OpenSearchBox();
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Next) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		public void FindPrevious()
		{
			this.OpenSearchBox();
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Previous) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		private void picCloseSearch_Click(object sender, EventArgs e)
		{
			this.CloseSearchBox();
		}

		private void picSearchPrevious_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindPrevious();
		}

		private void picSearchNext_MouseUp(object sender, MouseEventArgs e)
		{
			this.FindNext();
		}

		private byte[] GetByteArray(string hexText, ref bool hasWildcard)
		{
			hexText = hexText.Replace(" ", "");

			try {
				List<byte> bytes = new List<byte>(hexText.Length/2);
				for(int i = 0; i < hexText.Length; i+=2) {
					if(i == hexText.Length - 1) {
						bytes.Add((byte)(Convert.ToByte(hexText.Substring(i, 1), 16) << 4));
						hasWildcard = true;
					} else {
						bytes.Add(Convert.ToByte(hexText.Substring(i, 2), 16));
					}
				}
				return bytes.ToArray();
			} catch {
				return new byte[0];
			}
		}

		private bool UpdateSearchOptions()
		{
			bool invalidSearchString = false;

			this._findOptions.MatchCase = this.chkMatchCase.Checked;

			if(this.chkTextSearch.Checked) {
				this._findOptions.Type = FindType.Text;
				this._findOptions.Text = this.cboSearch.Text;
				this._findOptions.HasWildcard = false;
			} else {
				this._findOptions.Type = FindType.Hex;
				bool hasWildcard = false;
				this._findOptions.Hex = this.GetByteArray(this.cboSearch.Text, ref hasWildcard);
				this._findOptions.HasWildcard = hasWildcard;
				invalidSearchString = this._findOptions.Hex.Length == 0 && this.cboSearch.Text.Trim().Length > 0;
			}

			this.lblSearchWarning.Text = "";

			bool emptySearch = this._findOptions.Text.Length == 0 || (!this.chkTextSearch.Checked && (this._findOptions.Hex == null || this._findOptions.Hex.Length == 0));
			if(invalidSearchString) {
				this.lblSearchWarning.Text = "Invalid search string";
			} else if(!emptySearch) {
				return true;
			}
			return false;
		}

		private void cboSearch_TextUpdate(object sender, EventArgs e)
		{
			if(this.UpdateSearchOptions()) {
				if(this.ctrlHexBox.Find(this._findOptions, HexBox.eSearchDirection.Incremental) == -1) {
					this.lblSearchWarning.Text = "No matches found!";
				}
			}
		}

		private void cboSearch_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter) {
				this.FindNext();
				if(this.cboSearch.Items.Contains(this.cboSearch.Text)) {
					this.cboSearch.Items.Remove(this.cboSearch.Text);
				}
				this.cboSearch.Items.Insert(0, this.cboSearch.Text);

				e.Handled = true;
				e.SuppressKeyPress = true;
			}
		}

		private void chkTextSearch_CheckedChanged(object sender, EventArgs e)
		{
			this.UpdateSearchOptions();
		}
		
		public event EventHandler RequiredWidthChanged
		{
			add { this.ctrlHexBox.RequiredWidthChanged += value; }
			remove { this.ctrlHexBox.RequiredWidthChanged -= value; }
		}
		
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IByteCharConverter ByteCharConverter
		{
			get { return this.ctrlHexBox.ByteCharConverter; }
			set { this.ctrlHexBox.ByteCharConverter = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public IByteColorProvider ByteColorProvider
		{
			get { return this.ctrlHexBox.ByteColorProvider; }
			set { this.ctrlHexBox.ByteColorProvider = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool StringViewVisible
		{
			get { return this.ctrlHexBox.StringViewVisible; }
			set { this.ctrlHexBox.StringViewVisible = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ReadOnly
		{
			get { return this.ctrlHexBox.ReadOnly; }
			set { this.ctrlHexBox.ReadOnly = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HighDensityMode
		{
			get { return this.ctrlHexBox.HighDensityMode; }
			set { this.ctrlHexBox.HighDensityMode = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool EnablePerByteNavigation
		{
			get { return this.ctrlHexBox.EnablePerByteNavigation; }
			set { this.ctrlHexBox.EnablePerByteNavigation = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool ByteEditingMode
		{
			get { return this.ctrlHexBox.ByteEditingMode; }
			set { this.ctrlHexBox.ByteEditingMode = value; }
		}

		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public bool HighlightCurrentRowColumn
		{
			get { return this.ctrlHexBox.HighlightCurrentRowColumn; }
			set { this.ctrlHexBox.HighlightCurrentRowColumn = value; }
		}

		public delegate void ByteMouseHoverHandler(int address, Point position);
		public event ByteMouseHoverHandler ByteMouseHover; 
		private void ctrlHexBox_MouseMove(object sender, MouseEventArgs e)
		{
			BytePositionInfo? bpi = ctrlHexBox.GetRestrictedHexBytePositionInfo(e.Location);
			if(bpi.HasValue) {
				Point position = ctrlHexBox.GetBytePosition(bpi.Value.Index);
				ByteMouseHover?.Invoke((int)bpi.Value.Index, new Point(position.X + (int)(ctrlHexBox.CharSize.Width * 2.5), position.Y + (int)(ctrlHexBox.CharSize.Height * 1.1)));
			} else {
				ByteMouseHover?.Invoke(-1, Point.Empty);
			}
		}

		private void ctrlHexBox_MouseLeave(object sender, EventArgs e)
		{
			ByteMouseHover?.Invoke(-1, Point.Empty);
		}

		private void UpdateLocationLabel()
		{
			if(ctrlHexBox.SelectionLength > 0) {
				this.lblLocation.Text = $"Selection: ${ctrlHexBox.SelectionStart.ToString("X4")} - ${(ctrlHexBox.SelectionStart + ctrlHexBox.SelectionLength - 1).ToString("X4")}, {ctrlHexBox.SelectionLength} bytes (${ctrlHexBox.SelectionLength.ToString("X2")})";
			} else {
				this.lblLocation.Text = $"Location: ${ctrlHexBox.SelectionStart.ToString("X4")}";
			}
		}

		private void ctrlHexBox_SelectionStartChanged(object sender, EventArgs e)
		{
			UpdateLocationLabel();
		}

		private void ctrlHexBox_SelectionLengthChanged(object sender, EventArgs e)
		{
			UpdateLocationLabel();
		}

		private void mnuCopy_Click(object sender, EventArgs e)
		{
			ctrlHexBox.CopyHex();
		}

		private void mnuPaste_Click(object sender, EventArgs e)
		{
			ctrlHexBox.Paste();
		}

		private void mnuSelectAll_Click(object sender, EventArgs e)
		{
			ctrlHexBox.SelectAll();
		}

		private void UpdateActionAvailability()
		{
			UInt32 startAddress = (UInt32)SelectionStartAddress;
			UInt32 endAddress = (UInt32)SelectionEndAddress;

			string address = "$" + startAddress.ToString("X4");
			string addressRange;
			if(startAddress != endAddress) {
				addressRange = "$" + startAddress.ToString("X4") + "-$" + endAddress.ToString("X4");
			} else {
				addressRange = address;
			}

			mnuEditLabel.Text = $"Edit Label ({address})";
			mnuEditBreakpoint.Text = $"Edit Breakpoint ({addressRange})";
			mnuAddToWatch.Text = $"Add to Watch ({addressRange})";

			if(_memoryType.IsRelativeMemory()) {
				AddressInfo relAddress = new AddressInfo() {
					Address = (int)startAddress,
					Type = _memoryType
				};

				AddressInfo absAddress = DebugApi.GetAbsoluteAddress(relAddress);
				mnuEditLabel.Enabled = absAddress.Address != -1 && absAddress.Type.SupportsLabels();
				mnuAddToWatch.Enabled = _memoryType.SupportsWatch();
			} else {
				mnuEditLabel.Enabled = _memoryType.SupportsLabels();
				mnuAddToWatch.Enabled = false;
			}

			if(_memoryType == SnesMemoryType.CpuMemory || _memoryType == SnesMemoryType.GameboyMemory) {
				AddressInfo start = DebugApi.GetAbsoluteAddress(new AddressInfo() { Address = (int)startAddress, Type = _memoryType });
				AddressInfo end = DebugApi.GetAbsoluteAddress(new AddressInfo() { Address = (int)endAddress, Type = _memoryType });

				if(start.Address >= 0 && end.Address >= 0 && start.Address <= end.Address && ((start.Type == SnesMemoryType.PrgRom && end.Type == SnesMemoryType.PrgRom) || (start.Type == SnesMemoryType.GbPrgRom && end.Type == SnesMemoryType.GbPrgRom))) {
					mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
					mnuMarkSelectionAs.Enabled = true;
				} else {
					mnuMarkSelectionAs.Text = "Mark selection as...";
					mnuMarkSelectionAs.Enabled = false;
				}
			} else if(_memoryType == SnesMemoryType.PrgRom || _memoryType == SnesMemoryType.GbPrgRom) {
				mnuMarkSelectionAs.Text = "Mark selection as... (" + addressRange + ")";
				mnuMarkSelectionAs.Enabled = true;
			} else {
				mnuMarkSelectionAs.Text = "Mark selection as...";
				mnuMarkSelectionAs.Enabled = false;
			}

			mnuEditBreakpoint.Enabled = true;
		}

		private void MarkSelectionAs(CdlFlags type)
		{
			if(_memoryType != SnesMemoryType.CpuMemory && _memoryType != SnesMemoryType.PrgRom && _memoryType != SnesMemoryType.GameboyMemory && _memoryType != SnesMemoryType.GbPrgRom) {
				return;
			}

			int start = SelectionStartAddress;
			int end = SelectionEndAddress;

			if(_memoryType == SnesMemoryType.CpuMemory || _memoryType == SnesMemoryType.GameboyMemory) {
				start = DebugApi.GetAbsoluteAddress(new AddressInfo() { Address = start, Type = _memoryType }).Address;
				end = DebugApi.GetAbsoluteAddress(new AddressInfo() { Address = end, Type = _memoryType }).Address;
			}

			if(start >= 0 && end >= 0 && start <= end) {
				DebugApi.MarkBytesAs(_memoryType.ToCpuType(), (UInt32)start, (UInt32)end, type);
				DebugWindowManager.GetDebugger(_memoryType.ToCpuType())?.RefreshDisassembly();
			}

			RefreshData(_memoryType);
		}

		private void mnuAddToWatch_Click(object sender, EventArgs e)
		{
			if(_memoryType.SupportsWatch()) {
				string[] toAdd = Enumerable.Range(SelectionStartAddress, SelectionEndAddress - SelectionStartAddress + 1).Select((num) => $"[${num.ToString("X6")}]").ToArray();
				WatchManager.GetWatchManager(_memoryType.ToCpuType()).AddWatch(toAdd);
			}
		}

		private void mnuEditBreakpoint_Click(object sender, EventArgs e)
		{
			UInt32 startAddress = (UInt32)SelectionStartAddress;
			UInt32 endAddress = (UInt32)SelectionEndAddress;
			BreakpointAddressType addressType = startAddress == endAddress ? BreakpointAddressType.SingleAddress : BreakpointAddressType.AddressRange;

			Breakpoint bp = BreakpointManager.GetMatchingBreakpoint(startAddress, endAddress, _memoryType);
			if(bp == null) {
				bp = new Breakpoint() { Address = startAddress, MemoryType = _memoryType, CpuType = _memoryType.ToCpuType(), StartAddress = startAddress, EndAddress = endAddress, AddressType = addressType, BreakOnWrite = true, BreakOnRead = true };
				if(bp.IsCpuBreakpoint) {
					bp.BreakOnExec = true;
				}
			}
			BreakpointManager.EditBreakpoint(bp);
		}

		private void mnuEditLabel_Click(object sender, EventArgs e)
		{
			UInt32 address = (UInt32)ctrlHexBox.SelectionStart;
			SnesMemoryType memType = _memoryType;
			if(!memType.SupportsLabels()) {
				AddressInfo relAddress = new AddressInfo() {
					Address = (int)address,
					Type = memType
				};
				AddressInfo absAddress = DebugApi.GetAbsoluteAddress(relAddress);
				if(absAddress.Address < 0 || !absAddress.Type.SupportsLabels()) {
					return;
				}
				address = (uint)absAddress.Address;
				memType = absAddress.Type;
			}

			CodeLabel label = LabelManager.GetLabel(address, memType);
			if(label == null) {
				label = new CodeLabel() {
					Address = address,
					MemoryType = memType
				};
			}

			ctrlLabelList.EditLabel(label);
		}

		private void ctxMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			UpdateActionAvailability();
		}
	}
}
