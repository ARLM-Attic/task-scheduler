﻿using System;

namespace Microsoft.Win32.TaskScheduler.OptionPanels
{
	internal partial class RuntimeOptionPanel : Microsoft.Win32.TaskScheduler.OptionPanels.OptionPanel
	{
		public RuntimeOptionPanel()
		{
			InitializeComponent();
			long allVal;
			ComboBoxExtension.InitializeFromEnum(taskPriorityCombo.Items, typeof(System.Diagnostics.ProcessPriorityClass), EditorProperties.Resources.ResourceManager, "ProcessPriority", out allVal);
			taskRestartIntervalCombo.Items.AddRange(new TimeSpan2[] { TimeSpan2.FromMinutes(1), TimeSpan2.FromMinutes(5), TimeSpan2.FromMinutes(10), TimeSpan2.FromMinutes(15), TimeSpan2.FromMinutes(30), TimeSpan2.FromHours(1), TimeSpan2.FromHours(2) });
			taskExecutionTimeLimitCombo.Items.AddRange(new TimeSpan2[] { TimeSpan2.FromHours(1), TimeSpan2.FromHours(2), TimeSpan2.FromHours(4), TimeSpan2.FromHours(8), TimeSpan2.FromHours(12), TimeSpan2.FromDays(1), TimeSpan2.FromDays(3) });
			taskDeleteAfterCombo.FormattedZero = EditorProperties.Resources.TimeSpanImmediately;
			taskDeleteAfterCombo.Items.AddRange(new TimeSpan2[] { TimeSpan2.Zero, TimeSpan2.FromDays(30), TimeSpan2.FromDays(90), TimeSpan2.FromDays(180), TimeSpan2.FromDays(365) });
		}

		protected override void InitializePanel()
		{
			bool editable = parent.Editable;
			bool v2 = parent.IsV2;

			taskAllowDemandStartCheck.Enabled = taskStartWhenAvailableCheck.Enabled =
				taskRestartIntervalCheck.Enabled = taskRestartAttemptTimesLabel.Enabled = 
				taskRunningRuleLabel.Enabled = taskMultInstCombo.Enabled = editable && v2;
			taskAllowHardTerminateCheck.Enabled = editable && v2 && !td.Settings.UseUnifiedSchedulingEngine;

			taskAllowDemandStartCheck.Checked = td.Settings.AllowDemandStart;

			// Update Multiple Instances policy combo
			taskMultInstCombo.BeginUpdate();
			long allVal;
			ComboBoxExtension.InitializeFromEnum(taskMultInstCombo.Items, typeof(TaskInstancesPolicy), EditorProperties.Resources.ResourceManager, "TaskInstances", out allVal);
			if (td.Settings.UseUnifiedSchedulingEngine)
				taskMultInstCombo.Items.RemoveAt(taskMultInstCombo.Items.IndexOf((long)TaskInstancesPolicy.StopExisting));
			taskMultInstCombo.SelectedIndex = taskMultInstCombo.Items.IndexOf((long)td.Settings.MultipleInstances);
			taskMultInstCombo.EndUpdate();

			taskAllowDemandStartCheck.Checked = td.Settings.AllowDemandStart;
			taskStartWhenAvailableCheck.Checked = td.Settings.StartWhenAvailable;
			taskRestartIntervalCheck.Checked = td.Settings.RestartInterval != TimeSpan.Zero;
			taskRestartIntervalCheck_CheckedChanged(null, EventArgs.Empty);
			if (taskRestartIntervalCheck.Checked)
			{
				taskRestartIntervalCombo.Value = td.Settings.RestartInterval;
				taskRestartCountText.Value = td.Settings.RestartCount;
			}
			taskExecutionTimeLimitCheck.Checked = td.Settings.ExecutionTimeLimit != TimeSpan.Zero;
			taskExecutionTimeLimitCombo.Enabled = editable && taskExecutionTimeLimitCheck.Checked;
			taskExecutionTimeLimitCombo.Value = td.Settings.ExecutionTimeLimit;
			taskAllowHardTerminateCheck.Checked = td.Settings.AllowHardTerminate;
			taskDeleteAfterCheck.Checked = td.Settings.DeleteExpiredTaskAfter != TimeSpan.Zero;
			taskDeleteAfterCombo.Enabled = v2 && editable && taskDeleteAfterCheck.Checked;
			taskDeleteAfterCombo.Value = td.Settings.DeleteExpiredTaskAfter == TimeSpan.FromSeconds(1) ? TimeSpan.Zero : td.Settings.DeleteExpiredTaskAfter;
			taskMultInstCombo.SelectedIndex = taskMultInstCombo.Items.IndexOf((long)td.Settings.MultipleInstances);

			taskPriorityCombo.SelectedIndex = taskPriorityCombo.Items.IndexOf((long)td.Settings.Priority);
		}

		private void taskAllowDemandStartCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!onAssignment && parent.IsV2)
				td.Settings.AllowDemandStart = taskAllowDemandStartCheck.Checked;
		}

		private void taskAllowHardTerminateCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!onAssignment && parent.IsV2)
				td.Settings.AllowHardTerminate = taskAllowHardTerminateCheck.Checked;
		}

		private void taskDeleteAfterCheck_CheckedChanged(object sender, EventArgs e)
		{
			taskDeleteAfterCombo.Enabled = parent.Editable && taskDeleteAfterCheck.Checked;
			if (!onAssignment)
			{
				if (taskDeleteAfterCheck.Checked)
					taskDeleteAfterCombo.Value = TimeSpan.FromDays(30);
				else
					taskDeleteAfterCombo.Value = TimeSpan.Zero;
			}
		}

		private void taskDeleteAfterCombo_ValueChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.DeleteExpiredTaskAfter = taskDeleteAfterCheck.Checked ? (taskDeleteAfterCombo.Value == TimeSpan2.Zero ? TimeSpan.FromSeconds(1) : (TimeSpan)taskDeleteAfterCombo.Value) : TimeSpan.Zero;
		}

		private void taskExecutionTimeLimitCheck_CheckedChanged(object sender, EventArgs e)
		{
			taskExecutionTimeLimitCombo.Enabled = parent.Editable && taskExecutionTimeLimitCheck.Checked;
			if (!onAssignment)
			{
				if (taskExecutionTimeLimitCheck.Checked)
					taskExecutionTimeLimitCombo.Value = TimeSpan.FromDays(3);
				else
					taskExecutionTimeLimitCombo.Value = TimeSpan.Zero;
			}
		}

		private void taskExecutionTimeLimitCombo_ValueChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.ExecutionTimeLimit = taskExecutionTimeLimitCombo.Value;
			taskExecutionTimeLimitCheck.Checked = taskExecutionTimeLimitCombo.Value != TimeSpan2.Zero;
		}

		private void taskMultInstCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!onAssignment && parent.IsV2 && td != null)
				td.Settings.MultipleInstances = (TaskInstancesPolicy)((DropDownCheckListItem)taskMultInstCombo.SelectedItem).Value;
		}

		private void taskPriorityCombo_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.Priority = (System.Diagnostics.ProcessPriorityClass)Convert.ToInt32(((DropDownCheckListItem)taskPriorityCombo.SelectedItem).Value);
		}

		private void taskRestartCountText_ValueChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.RestartCount = Convert.ToInt32(taskRestartCountText.Value);
		}

		private void taskRestartIntervalCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
			{
				if (taskRestartIntervalCheck.Checked)
				{
					taskRestartIntervalCombo.Value = TimeSpan.FromMinutes(1);
					taskRestartCountText.Value = 3;
				}
				else
				{
					taskRestartIntervalCombo.Value = TimeSpan.Zero;
					taskRestartCountText.Value = 0;
				}
			}
			taskRestartIntervalCombo.Enabled = taskRestartCountLabel.Enabled = taskRestartCountText.Enabled = parent.IsV2 && parent.Editable && taskRestartIntervalCheck.Checked;
		}

		private void taskRestartIntervalCombo_ValueChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.RestartInterval = taskRestartIntervalCombo.Value;
		}

		private void taskStartWhenAvailableCheck_CheckedChanged(object sender, EventArgs e)
		{
			if (!onAssignment)
				td.Settings.StartWhenAvailable = taskStartWhenAvailableCheck.Checked;
		}
	}
}
