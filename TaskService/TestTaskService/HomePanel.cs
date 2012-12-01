﻿using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;
using System;
using System.Collections.Generic;

namespace TestTaskService
{
	public partial class HomePanel : UserControl, ISupportTasks
	{
		private TaskService ts;

		public HomePanel()
		{
			InitializeComponent();
			comboBox1.SelectedIndex = 0;
		}

		public TaskService TaskService
		{
			get
			{
				return ts;
			}
			set
			{
				ts = value;
				RefreshPanels();
			}
		}

		private void RefreshPanels()
		{
			DateTime now = DateTime.Now;

			// Update summary
			summaryLabel.Text = string.Format("Task Scheduler Summary (Last refreshed: {0:G})", now);
			footerLabel.Text = string.Format("Last refreshed at {0:G}", now);

			if (ts == null)
				return;

			// Update status list
			UpdateStatusList();

			// Update active list
			activeListView.Items.Clear();
			activeListView.UseWaitCursor = true;
			activeBackgroundWorker.RunWorkerAsync();
		}

		private void UpdateStatusList()
		{
			statusListView.Items.Clear();
			statusListView.Groups.Clear();
			statusListView.UseWaitCursor = true;
			statusBackgroundWorker.CancelAsync();
			while (statusBackgroundWorker.IsBusy)
				System.Threading.Thread.Sleep(250);
			statusBackgroundWorker.RunWorkerAsync(comboBox1.SelectedIndex);
		}

		public ContextMenuStrip MenuItems
		{
			get { return new ContextMenuStrip(); }
		}

		protected override void WndProc(ref Message m)
		{
			switch (m.Msg)
			{
				case 0x0202: // WM_LBUTTONUP
					base.DefWndProc(ref m);
					break;
				default:
					base.WndProc(ref m);
					break;
			}
		}

		private void refreshButton_Click(object sender, EventArgs e)
		{
			RefreshPanels();
		}

		private void statusBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			List<ListViewItem> list = new List<ListViewItem>();
			TimeSpan span = TimeSpan.Zero;
			switch ((int)e.Argument)
			{
				case 0:
					span = TimeSpan.FromHours(1);
					break;
				case 1:
					span = TimeSpan.FromDays(1);
					break;
				case 2:
					span = TimeSpan.FromDays(7);
					break;
				case 3:
					span = TimeSpan.FromDays(30);
					break;
				default:
					break;
			}
			DateTime st = DateTime.Now.Subtract(span);
			CorrelatedTaskEventLog log = new CorrelatedTaskEventLog(st, null, ts.TargetServer);
			foreach (var t in log)
			{
				if (((System.ComponentModel.BackgroundWorker)sender).CancellationPending)
				{
					e.Cancel = true;
					return;
				}
				list.Add(new ListViewItem(new string[] { t.TaskName, t.RunResult.ToString(), t.RunStart.ToString(), t.RunEnd.ToString(), t.TriggeredBy.ToString() }) { Tag = t });
			}
			e.Result = list.ToArray();
		}

		private void statusBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			ListViewItem[] items = e.Result as ListViewItem[];
			if (items == null)
				return;
			ListViewGroup lvgroup = new ListViewGroup();
			statusListView.UseWaitCursor = false;
			statusListView.BeginUpdate();
			for (int i = 0; i < items.Length; i++)
			{
				if (lvgroup.Header != items[i].Text)
				{
					lvgroup = new ListViewGroup(items[i].Text);
					statusListView.Groups.Add(lvgroup);
					lvgroup.SetCollapsible(true);
				}
				items[i].Group = lvgroup;
				statusListView.Items.Add(items[i]);
			}
			statusListView.EndUpdate();
		}

		private void activeBackgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
		{
			using (TaskService lts = new TaskService(ts.TargetServer, ts.UserName, ts.UserAccountDomain, ts.UserPassword, ts.HighestSupportedVersion.Minor == 1))
			{
				List<ListViewItem> list = new List<ListViewItem>();
				foreach (var t in lts.FindAllTasks(null))
					if (t.IsActive)
						list.Add(new ListViewItem(new string[] { t.Name, t.NextRunTime.ToString("G"), t.Definition.Triggers.ToString(), t.Path }) { Tag = t });
				e.Result = list.ToArray();
			}
		}

		private void activeBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
		{
			activeListView.UseWaitCursor = false;
			activeListView.Items.AddRange(e.Result as ListViewItem[]);
		}

		private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (ts != null)
				UpdateStatusList();
		}
	}
}
