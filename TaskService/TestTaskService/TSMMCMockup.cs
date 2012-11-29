﻿using System;
using System.Windows.Forms;
using Microsoft.Win32.TaskScheduler;

namespace TestTaskService
{
	public interface ISupportTasks
	{
		TaskService TaskService { get; set; }
		ContextMenuStrip MenuItems { get; }
	}

	public partial class TSMMCMockup : Form
	{
		private FolderPanel folderPanel;
		private HomePanel homePanel;
		private Control curPanel;

		public TSMMCMockup()
		{
			InitializeComponent();
		}

		private void SetActionMenu(ContextMenuStrip menuItems)
		{
			ToolStripItemCollection coll = this.libraryMenuStrip.Items;
			if (menuItems != null)
				coll = menuItems.Items;
			actionToolStripMenuItem.DropDownItems.Clear();
			actionToolStripMenuItem.DropDownItems.AddRange(coll);
			actionToolStrip.Items.Clear();
			actionToolStrip.Items.AddRange(coll);
		}

		private void ShowPanel(Control panel)
		{
			if (curPanel != null)
				curPanel.Hide();
			curPanel = panel;
			if (panel is ISupportTasks)
			{
				((ISupportTasks)panel).TaskService = this.TaskService;
				SetActionMenu(((ISupportTasks)panel).MenuItems);
			}
			panel.Show();
		}

		private void ShowHome()
		{
			if (homePanel == null)
			{
				homePanel = new HomePanel();
				splitContainer1.Panel2.Controls.Add(homePanel);
				homePanel.Dock = DockStyle.Fill;
			}
			ShowPanel(homePanel);
		}

		private void ShowFolder(TaskFolder folder)
		{
			if (folderPanel == null)
			{
				folderPanel = new FolderPanel();
				splitContainer1.Panel2.Controls.Add(folderPanel);
				folderPanel.Dock = DockStyle.Fill;
			}
			folderPanel.Tasks = folder.Tasks;
			ShowPanel(folderPanel);
		}

		private void treeView1_AfterSelect(object sender, TreeViewEventArgs e)
		{
			if (e.Node.Tag == null)
				ShowHome();
			else
				ShowFolder(e.Node.Tag as TaskFolder);
		}

		private void TSMMCMockup_Load(object sender, EventArgs e)
		{
			imageList1.Images.Add(Properties.Resources.Properties);
			imageList1.Images.Add(Properties.Resources.TaskLibraryRootNode);
			imageList1.Images.Add(Properties.Resources.Folder_16);
			imageList1.Images.Add(Properties.Resources.ScheduledTaskWizard);
			imageList1.Images.Add(Properties.Resources.CreateNewTask);
			imageList1.Images.Add(Properties.Resources.ReviewRunningTasks);
			imageList1.Images.Add(Properties.Resources.AdminLog);
			imageList1.Images.Add(Properties.Resources.NewFolder);
			imageList1.Images.Add(Properties.Resources.Refresh);
			imageList1.Images.Add(Properties.Resources.RunNow);
			imageList1.Images.Add(Properties.Resources.EndTask);
			imageList1.Images.Add(Properties.Resources.Disable);
			imageList1.Images.Add(Properties.Resources.DeleteTask);
			imageList1.Images.Add(Properties.Resources.Help);

			libraryMenuStrip.ImageList = imageList1;
			createBasicTaskToolStripMenuItem.ImageIndex = 3;
			createTaskToolStripMenuItem.ImageIndex = 4;
			displayAllRunningTasksToolStripMenuItem.ImageIndex = 5;
			newFolderToolStripMenuItem.ImageIndex = 7;
			refreshToolStripMenuItem.ImageIndex = 8;

			itemMenuStrip.ImageList = imageList1;
			runToolStripMenuItem.ImageIndex = 9;
			endToolStripMenuItem.ImageIndex = 10;
			disableToolStripMenuItem.ImageIndex = 11;
			propertiesToolStripMenuItem.ImageIndex = 0;
			deleteToolStripMenuItem.ImageIndex = 12;

			RefreshList();
		}

		private void RefreshList()
		{
			treeView1.Nodes.Clear();
			TreeNode n = treeView1.Nodes.Add(null, string.Format("Task Scheduler ({0})", TaskService.TargetServer == null || TaskService.TargetServer.Equals(Environment.MachineName, StringComparison.InvariantCultureIgnoreCase) ? "Local" : TaskService.TargetServer), 0, 0);
			TreeNode p = n.Nodes.Add(null, "Task Scheduler Library", 1, 1);
			p.Tag = TaskService.RootFolder;
			n.Expand();
			LoadChildren(p);
			treeView1.SelectedNode = n;
		}

		private void LoadChildren(TreeNode p)
		{
			foreach (var item in ((TaskFolder)p.Tag).SubFolders)
			{
				TreeNode n = p.Nodes.Add(null, item.Name, 2, 2);
				n.Tag = item;
				LoadChildren(n);
			}
		}

		private void connectToAnotherComputerToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (taskServiceConnectDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
				RefreshList();
		}

		private void createBasicTaskMenu_Click(object sender, EventArgs e)
		{
			taskSchedulerWizard1.ShowDialog(this);
		}

		private void createTaskMenu_Click(object sender, EventArgs e)
		{
			taskEditDialog1.Initialize(TaskService);
			taskEditDialog1.ShowDialog(this);
		}

		private void importTaskMenu_Click(object sender, EventArgs e)
		{
			if (openFileDialog1.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				try
				{
					TaskDefinition td = TaskService.NewTaskFromFile(openFileDialog1.FileName);
					taskEditDialog1.Initialize(TaskService, td);
					taskEditDialog1.TaskName = System.IO.Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
					taskEditDialog1.ShowDialog(this);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error: " + ex.ToString());
				}
			}
		}

		private void displayAllRunningTasksToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
		{
			RefreshList();
		}

		private void newFolderToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void runToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void endToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void disableToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void exportToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void propertiesToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}
	}
}
