﻿using System;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler.UIComponents
{
	[System.ComponentModel.DefaultEvent("KeyValueChanged"), System.ComponentModel.DefaultProperty("Action")]
	internal partial class ComHandlerActionUI : UserControl, IActionHandler
	{
		public ComHandlerActionUI()
		{
			InitializeComponent();
		}

		private void getCLSIDButton_Click(object sender, EventArgs e)
		{
			try
			{
				ComObjectSelectionDialog dlg = new ComObjectSelectionDialog();
				dlg.SupportedInterface = new Guid("839D7762-5121-4009-9234-4F0D19394F04");
				if (dlg.ShowDialog(this) == DialogResult.OK)
					ComCLSID = dlg.CLSID;
			}
			catch { }
		}

		private Guid ComCLSID
		{
			get
			{
				try { return new Guid(toolTip.GetToolTip(comCLSIDText)); }
				catch { return Guid.Empty; }
			}
			set
			{
				if (value == Guid.Empty)
				{
					comCLSIDText.Clear();
					toolTip.SetToolTip(comCLSIDText, null);
				}
				else
				{
					toolTip.SetToolTip(comCLSIDText, value.ToString());
					comCLSIDText.Text = GetNameForCLSID(value) ?? value.ToString();
				}

				KeyValueChanged?.Invoke(this, EventArgs.Empty);
			}
		}

		internal static string GetNameForCLSID(Guid guid)
		{
			using (RegistryKey k = Registry.ClassesRoot.OpenSubKey("CLSID", false))
			{
				if (k != null)
				{
					using (RegistryKey k2 = k.OpenSubKey(guid.ToString("B"), false))
						return k2 != null ? k2.GetValue(null) as string : null;
				}
			}
			return null;
		}

		[System.ComponentModel.Browsable(false), System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
		public Action Action
		{
			get
			{
				return new ComHandlerAction(ComCLSID, comDataText.TextLength > 0 ? comDataText.Text : null);
			}
			set
			{
				ComCLSID = ((ComHandlerAction)value).ClassId;
				comDataText.Text = ((ComHandlerAction)value).Data;
			}
		}

		public void Run()
		{
			//var dlg = new ActionEditDialog();
			//dlg.Show(this);
			//TaskService.RunComHandlerActionAsync(ComCLSID, i => { dlg?.Close(); dlg = null; }, comDataText.Text, -1, (i, m) => { if (dlg != null) { dlg.Text = m; } });
		}

		public bool IsActionValid() => ComCLSID != Guid.Empty;

		public event EventHandler KeyValueChanged;
	}
}