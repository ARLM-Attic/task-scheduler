﻿using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler
{
	internal static class ControlProcessing
	{
		public static void EnableChildren(this Control ctl, bool enabled)
		{
			foreach (Control sub in ctl.Controls)
			{
				if (sub is ButtonBase || sub is ListControl || sub is TextBoxBase)
					sub.Enabled = enabled;
				sub.EnableChildren(enabled);
			}
		}

		public static RightToLeft GetRightToLeftProperty(this Control ctl)
		{
			if (ctl.RightToLeft == RightToLeft.Inherit)
			{
				return GetRightToLeftProperty(ctl.Parent);
			}
			return ctl.RightToLeft;
		}

		public static bool IsDesignMode(this Control ctrl)
		{
			if (ctrl.Parent == null)
				return true;
			Control p = ctrl.Parent;
			while (p != null)
			{
				var site = p.Site;
				if (site != null && site.DesignMode)
					return true;
				p = p.Parent;
			}
			return false;
		}
	}
}
