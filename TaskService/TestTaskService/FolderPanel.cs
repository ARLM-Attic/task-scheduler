﻿using Microsoft.Win32.TaskScheduler;
using System;
using System.Windows.Forms;

namespace TestTaskService
{
	public partial class FolderPanel : UserControl, ISupportTasks
	{
		private Task selTask;

		public FolderPanel()
		{
			InitializeComponent();
			InitMenus();
		}

		public ToolStrip MenuItems => itemMenuStrip;

		public TaskFolder TaskFolder
		{
			get { return TaskListView.Folder; }
			set
			{
				TaskListView.Folder = value;
				if (TaskListView.Tasks.Count > 0)
					TaskListView.SelectedIndex = 0;
				else
					taskListView_TaskSelected(null, TaskListView.TaskSelectedEventArgs.Empty);
			}
		}

		public TaskService TaskService { get; set; }

		private void deleteMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null)
			{
				try
				{
					TaskService.GetFolder(System.IO.Path.GetDirectoryName(selTask.Path)).DeleteTask(selTask.Name, false);
				}
				catch (System.IO.FileNotFoundException)
				{
				}
				catch (Exception ex)
				{
					MessageBox.Show(this, ex.Message, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
				TaskListView.Refresh();
			}
		}

		private void disableMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null)
			{
				selTask.Enabled = !selTask.Enabled;
				TaskListView.Refresh();
			}
		}

		private void endMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null && MessageBox.Show("Do you want to end all instances of this task?", "Task Scheduler", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
				selTask.Stop();
		}

		private void exportMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null)
			{
				saveFileDialog1.FileName = selTask.Name;
				if (saveFileDialog1.ShowDialog(this) == DialogResult.OK)
					selTask.Export(saveFileDialog1.FileName);
			}
		}

		private void InitMenus()
		{
			imageList1.Images.Add(Properties.Resources.NewFolder);
			imageList1.Images.Add(Properties.Resources.DeleteTask);
			imageList1.Images.Add(Properties.Resources.RunNow);
			imageList1.Images.Add(Properties.Resources.EndTask);
			imageList1.Images.Add(Properties.Resources.Disable);
			imageList1.Images.Add(Properties.Resources.statusUnknown);
			imageList1.Images.Add(Properties.Resources.Properties);
			imageList1.Images.Add(Properties.Resources.DeleteTask);

			folderMenu.ImageList = imageList1;
			newFolderToolStripMenuItem.ImageIndex = 0;
			deleteFolderToolStripMenuItem.ImageIndex = 1;

			itemMenu.ImageList = itemMenuStrip.ImageList = imageList1;
			runMenuItem.ImageIndex = runMenuItem2.ImageIndex = 2;
			endMenuItem.ImageIndex = endMenuItem2.ImageIndex = 3;
			disableMenuItem.ImageIndex = disableMenuItem2.ImageIndex = 4;
			exportMenuItem.ImageIndex = exportMenuItem2.ImageIndex = 5;
			propertiesMenuItem.ImageIndex = propertiesMenuItem2.ImageIndex = 6;
			deleteMenuItem.ImageIndex = deleteMenuItem2.ImageIndex = 7;
		}

		private void propMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null)
			{
				taskEditDialog1.Initialize(selTask);
				taskEditDialog1.ShowDialog(this);
			}
		}

		private void runMenu_Click(object sender, EventArgs e)
		{
			if (selTask != null)
				selTask.Run();
		}

		private void taskListView_MouseDoubleClick(object sender, MouseEventArgs e)
		{
			propMenu_Click(TaskListView, e);
		}

		private void taskListView_TaskSelected(object sender, Microsoft.Win32.TaskScheduler.TaskListView.TaskSelectedEventArgs e)
		{
			if (itemMenuStrip.Enabled != (e.Task != null))
				itemMenuStrip.Enabled = (e.Task != null);
			bool hasValidTask = true;
			try { var d = e.Task.Definition; } catch { hasValidTask = false; }
			if (!hasValidTask)
			{
				TaskPropertiesControl.Hide();
				selTask = null;
			}
			else
			{
				TaskPropertiesControl.Show();
				TaskPropertiesControl.Initialize(e.Task);
				selTask = e.Task;

				runMenuItem.Enabled = runMenuItem2.Enabled = (selTask.Definition.Settings.AllowDemandStart);
				endMenuItem.Enabled = endMenuItem2.Enabled = (selTask.State == TaskState.Running);
				disableMenuItem.Enabled = disableMenuItem2.Enabled = (selTask.Enabled);
			}
		}
	}
}