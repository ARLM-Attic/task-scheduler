﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler.OptionPanels
{
	internal partial class TriggersOptionPanel : Microsoft.Win32.TaskScheduler.OptionPanels.OptionPanel
	{
		public TriggersOptionPanel()
		{
			InitializeComponent();
		}

		protected override void InitializePanel()
		{
			triggerCollectionUI1.RefreshState();
		}
	}
}
