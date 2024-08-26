﻿using Mesen.GUI.Config;
using Mesen.GUI.Debugger.Labels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Mesen.GUI.Debugger
{
	public class RgbdsSymbolFile
	{
		private static bool IsValidRow(string row)
		{
			return GetBankAddressLabel(row, out _, out _, out _);
		}

		public static bool IsValidFile(string path, bool silent = false)
		{
			string[] content = File.ReadAllLines(path, Encoding.UTF8);
			int errorCount = 0;
			for(int i = 0; i < 20 && i < content.Length; i++) {
				if(!IsValidRow(content[i])) {
					errorCount++;
				}
			}
			return errorCount < 5;
		}

		private static bool GetBankAddressLabel(string row, out UInt32 address, out UInt32 bank, out string labelName)
		{
			address = 0;
			bank = 0;
			labelName = null;

			string lineData = row;
			int commentIndex = lineData.IndexOf(';');
			if(commentIndex >= 0) {
				lineData = lineData.Substring(0, commentIndex);
			}
			lineData = lineData.Trim();
			if(lineData.Length == 0) {
				return true;
			}

			int splitIndex = lineData.IndexOf(' ');
			if(splitIndex < 0) {
				return false;
			}

			string[] bankAddressStr = lineData.Substring(0, splitIndex).Split(':');
			if(bankAddressStr.Length != 2) {
				return false;
			}

			if(!UInt32.TryParse(bankAddressStr[0], NumberStyles.HexNumber, null, out bank)) {
				return false;
			}

			if(!UInt32.TryParse(bankAddressStr[1], NumberStyles.HexNumber, null, out address)) {
				return false;
			}

			labelName = lineData.Substring(splitIndex + 1).Replace('.', '_');
			if(string.IsNullOrEmpty(labelName) || !LabelManager.LabelRegex.IsMatch(labelName)) {
				return false;
			}
			return true;
		}

		public static void Import(string path, bool silent = false)
		{
			const int prgBankSize = 0x4000;
			const int wramBankSize = 0x1000;
			const int sramBankSize = 0x2000;

			List<CodeLabel> labels = new List<CodeLabel>(1000);

			int errorCount = 0;
			foreach(string row in File.ReadAllLines(path, Encoding.UTF8)) {
				UInt32 address;
				UInt32 bank;
				string labelName;

				if(!GetBankAddressLabel(row, out address, out bank, out labelName)) {
					errorCount++;
					continue;
				} else if(labelName == null) {
					//Empty line/comment
					continue;
				}

				UInt32 fullAddress = 0;
				AddressInfo absAddress;
				if(address <= 0x7FFF) {
					fullAddress = bank * prgBankSize + (address & (prgBankSize - 1));
					absAddress = new AddressInfo() { Address = (int)fullAddress, Type = SnesMemoryType.GbPrgRom };
				} else if(address >= 0xA000 && address <= 0xCFFF) {
					fullAddress = bank * sramBankSize + (address & (sramBankSize - 1));
					absAddress = new AddressInfo() { Address = (int)fullAddress, Type = SnesMemoryType.GbCartRam };
				} else if(address >= 0xC000 && address <= 0xDFFF) {
					fullAddress = bank * wramBankSize + (address & (wramBankSize - 1));
					absAddress = new AddressInfo() { Address = (int)fullAddress, Type = SnesMemoryType.GbWorkRam };
				} else {
					absAddress = DebugApi.GetAbsoluteAddress(new AddressInfo() { Address = (int)address, Type = SnesMemoryType.GameboyMemory });
				}

				if(absAddress.Address >= 0) {
					CodeLabel label = new CodeLabel();
					label.Address = (UInt32)absAddress.Address;
					label.MemoryType = absAddress.Type;
					label.Comment = "";
					label.Label = labelName;
					labels.Add(label);
				} else {
					errorCount++;
				}
			}

			LabelManager.SetLabels(labels);

			if(!silent) {
				string message = $"Import completed with {labels.Count} labels imported";
				if(errorCount > 0) {
					message += $" and {errorCount} error(s)";
				}
				MessageBox.Show(message, "Mesen-S", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
