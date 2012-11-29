﻿namespace TestTaskService
{
	partial class TSMMCMockup
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TSMMCMockup));
			this.mainMenuStrip = new System.Windows.Forms.MenuStrip();
			this.actionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainToolStrip = new System.Windows.Forms.ToolStrip();
			this.statusStrip = new System.Windows.Forms.StatusStrip();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.TaskService = new Microsoft.Win32.TaskScheduler.TaskService();
			this.taskServiceConnectDialog1 = new Microsoft.Win32.TaskScheduler.TaskServiceConnectDialog();
			this.actionToolStrip = new System.Windows.Forms.ToolStrip();
			this.splitContainer1 = new System.Windows.Forms.SplitContainer();
			this.treeView1 = new System.Windows.Forms.TreeView();
			this.libraryMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.connectToAnotherComputerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createBasicTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.createTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.importTaskToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.displayAllRunningTasksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
			this.newFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
			this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
			this.taskEditDialog1 = new Microsoft.Win32.TaskScheduler.TaskEditDialog();
			this.taskSchedulerWizard1 = new Microsoft.Win32.TaskScheduler.TaskSchedulerWizard();
			this.itemMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.runToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.endToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.disableToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.propertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripMenuItem3 = new System.Windows.Forms.ToolStripSeparator();
			this.deleteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.mainMenuStrip.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.TaskService)).BeginInit();
			this.splitContainer1.Panel1.SuspendLayout();
			this.splitContainer1.SuspendLayout();
			this.libraryMenuStrip.SuspendLayout();
			this.itemMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// mainMenuStrip
			// 
			this.mainMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.actionToolStripMenuItem});
			this.mainMenuStrip.Location = new System.Drawing.Point(0, 0);
			this.mainMenuStrip.Name = "mainMenuStrip";
			this.mainMenuStrip.Size = new System.Drawing.Size(1126, 24);
			this.mainMenuStrip.TabIndex = 0;
			this.mainMenuStrip.Text = "mainMenuStrip";
			// 
			// actionToolStripMenuItem
			// 
			this.actionToolStripMenuItem.Name = "actionToolStripMenuItem";
			this.actionToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
			this.actionToolStripMenuItem.Text = "Action";
			// 
			// mainToolStrip
			// 
			this.mainToolStrip.Location = new System.Drawing.Point(0, 24);
			this.mainToolStrip.Name = "mainToolStrip";
			this.mainToolStrip.Size = new System.Drawing.Size(1126, 25);
			this.mainToolStrip.TabIndex = 1;
			this.mainToolStrip.Text = "mainToolStrip";
			// 
			// statusStrip
			// 
			this.statusStrip.Location = new System.Drawing.Point(0, 664);
			this.statusStrip.Name = "statusStrip";
			this.statusStrip.Size = new System.Drawing.Size(1126, 22);
			this.statusStrip.TabIndex = 2;
			this.statusStrip.Text = "statusStrip1";
			// 
			// imageList1
			// 
			this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
			this.imageList1.ImageSize = new System.Drawing.Size(16, 16);
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			// 
			// taskServiceConnectDialog1
			// 
			this.taskServiceConnectDialog1.AutoSize = true;
			this.taskServiceConnectDialog1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.taskServiceConnectDialog1.ClientSize = new System.Drawing.Size(444, 181);
			this.taskServiceConnectDialog1.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.taskServiceConnectDialog1.Location = new System.Drawing.Point(100, 100);
			this.taskServiceConnectDialog1.Name = "TaskServiceConnectDialog";
			this.taskServiceConnectDialog1.TaskService = this.TaskService;
			this.taskServiceConnectDialog1.Text = "Select Computer";
			this.taskServiceConnectDialog1.Visible = false;
			// 
			// actionToolStrip
			// 
			this.actionToolStrip.Dock = System.Windows.Forms.DockStyle.Right;
			this.actionToolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow;
			this.actionToolStrip.Location = new System.Drawing.Point(1100, 49);
			this.actionToolStrip.Name = "actionToolStrip";
			this.actionToolStrip.Size = new System.Drawing.Size(26, 615);
			this.actionToolStrip.TabIndex = 5;
			this.actionToolStrip.Text = "Actions";
			// 
			// splitContainer1
			// 
			this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer1.Location = new System.Drawing.Point(0, 49);
			this.splitContainer1.Name = "splitContainer1";
			// 
			// splitContainer1.Panel1
			// 
			this.splitContainer1.Panel1.Controls.Add(this.treeView1);
			this.splitContainer1.Size = new System.Drawing.Size(1100, 615);
			this.splitContainer1.SplitterDistance = 365;
			this.splitContainer1.TabIndex = 6;
			// 
			// treeView1
			// 
			this.treeView1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.treeView1.FullRowSelect = true;
			this.treeView1.HideSelection = false;
			this.treeView1.ImageIndex = 0;
			this.treeView1.ImageList = this.imageList1;
			this.treeView1.Location = new System.Drawing.Point(0, 0);
			this.treeView1.Name = "treeView1";
			this.treeView1.SelectedImageIndex = 0;
			this.treeView1.ShowLines = false;
			this.treeView1.ShowPlusMinus = false;
			this.treeView1.ShowRootLines = false;
			this.treeView1.Size = new System.Drawing.Size(365, 615);
			this.treeView1.StateImageList = this.imageList1;
			this.treeView1.TabIndex = 4;
			this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
			// 
			// libraryMenuStrip
			// 
			this.libraryMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.connectToAnotherComputerToolStripMenuItem,
            this.createBasicTaskToolStripMenuItem,
            this.createTaskToolStripMenuItem,
            this.importTaskToolStripMenuItem,
            this.displayAllRunningTasksToolStripMenuItem,
            this.toolStripMenuItem1,
            this.newFolderToolStripMenuItem,
            this.toolStripMenuItem2,
            this.refreshToolStripMenuItem});
			this.libraryMenuStrip.Name = "contextMenuStrip1";
			this.libraryMenuStrip.Size = new System.Drawing.Size(242, 170);
			// 
			// connectToAnotherComputerToolStripMenuItem
			// 
			this.connectToAnotherComputerToolStripMenuItem.Name = "connectToAnotherComputerToolStripMenuItem";
			this.connectToAnotherComputerToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.connectToAnotherComputerToolStripMenuItem.Text = "Connect to another computer...";
			this.connectToAnotherComputerToolStripMenuItem.Click += new System.EventHandler(this.connectToAnotherComputerToolStripMenuItem_Click);
			// 
			// createBasicTaskToolStripMenuItem
			// 
			this.createBasicTaskToolStripMenuItem.Name = "createBasicTaskToolStripMenuItem";
			this.createBasicTaskToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.createBasicTaskToolStripMenuItem.Text = "Create Basic Task...";
			this.createBasicTaskToolStripMenuItem.Click += new System.EventHandler(this.createBasicTaskMenu_Click);
			// 
			// createTaskToolStripMenuItem
			// 
			this.createTaskToolStripMenuItem.Name = "createTaskToolStripMenuItem";
			this.createTaskToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.createTaskToolStripMenuItem.Text = "Create Task...";
			this.createTaskToolStripMenuItem.Click += new System.EventHandler(this.createTaskMenu_Click);
			// 
			// importTaskToolStripMenuItem
			// 
			this.importTaskToolStripMenuItem.Name = "importTaskToolStripMenuItem";
			this.importTaskToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.importTaskToolStripMenuItem.Text = "Import Task...";
			this.importTaskToolStripMenuItem.Click += new System.EventHandler(this.importTaskMenu_Click);
			// 
			// displayAllRunningTasksToolStripMenuItem
			// 
			this.displayAllRunningTasksToolStripMenuItem.Enabled = false;
			this.displayAllRunningTasksToolStripMenuItem.Name = "displayAllRunningTasksToolStripMenuItem";
			this.displayAllRunningTasksToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.displayAllRunningTasksToolStripMenuItem.Text = "Display All Running Tasks";
			this.displayAllRunningTasksToolStripMenuItem.Click += new System.EventHandler(this.displayAllRunningTasksToolStripMenuItem_Click);
			// 
			// toolStripMenuItem1
			// 
			this.toolStripMenuItem1.Name = "toolStripMenuItem1";
			this.toolStripMenuItem1.Size = new System.Drawing.Size(238, 6);
			// 
			// newFolderToolStripMenuItem
			// 
			this.newFolderToolStripMenuItem.Name = "newFolderToolStripMenuItem";
			this.newFolderToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.newFolderToolStripMenuItem.Text = "New Folder...";
			this.newFolderToolStripMenuItem.Click += new System.EventHandler(this.newFolderToolStripMenuItem_Click);
			// 
			// toolStripMenuItem2
			// 
			this.toolStripMenuItem2.Name = "toolStripMenuItem2";
			this.toolStripMenuItem2.Size = new System.Drawing.Size(238, 6);
			// 
			// refreshToolStripMenuItem
			// 
			this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
			this.refreshToolStripMenuItem.Size = new System.Drawing.Size(241, 22);
			this.refreshToolStripMenuItem.Text = "Refresh";
			this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
			// 
			// openFileDialog1
			// 
			this.openFileDialog1.DefaultExt = "xml";
			this.openFileDialog1.Filter = "Xml files (*.xml)|*.xml";
			this.openFileDialog1.Title = "Import Task";
			// 
			// taskEditDialog1
			// 
			this.taskEditDialog1.AutoSize = true;
			this.taskEditDialog1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.taskEditDialog1.ClientSize = new System.Drawing.Size(600, 462);
			this.taskEditDialog1.Editable = true;
			this.taskEditDialog1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.taskEditDialog1.Location = new System.Drawing.Point(75, 75);
			this.taskEditDialog1.MaximizeBox = false;
			this.taskEditDialog1.Name = "TaskEditDialog";
			this.taskEditDialog1.RegisterTaskOnAccept = true;
			this.taskEditDialog1.ShowIcon = false;
			this.taskEditDialog1.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.taskEditDialog1.Text = "Create Task";
			this.taskEditDialog1.Title = "Create Task";
			this.taskEditDialog1.Visible = false;
			// 
			// taskSchedulerWizard1
			// 
			this.taskSchedulerWizard1.AvailableTriggers = ((Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers)(((((((((((Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Event | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Time) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Daily) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Weekly) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Monthly) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.MonthlyDOW) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Idle) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Registration) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Boot) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.Logon) 
            | Microsoft.Win32.TaskScheduler.TaskSchedulerWizard.AvailableWizardTriggers.SessionStateChange)));
			this.taskSchedulerWizard1.ClientSize = new System.Drawing.Size(538, 391);
			this.taskSchedulerWizard1.Font = new System.Drawing.Font("Segoe UI", 9F);
			this.taskSchedulerWizard1.Icon = ((System.Drawing.Icon)(resources.GetObject("taskSchedulerWizard1.Icon")));
			this.taskSchedulerWizard1.Location = new System.Drawing.Point(50, 50);
			this.taskSchedulerWizard1.MinimumSize = new System.Drawing.Size(477, 374);
			this.taskSchedulerWizard1.Name = "TaskSchedulerWizard";
			this.taskSchedulerWizard1.RegisterTaskOnFinish = true;
			this.taskSchedulerWizard1.Title = "Create Task Wizard";
			this.taskSchedulerWizard1.Visible = false;
			// 
			// itemMenuStrip
			// 
			this.itemMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.runToolStripMenuItem,
            this.endToolStripMenuItem,
            this.disableToolStripMenuItem,
            this.exportToolStripMenuItem,
            this.propertiesToolStripMenuItem,
            this.toolStripMenuItem3,
            this.deleteToolStripMenuItem});
			this.itemMenuStrip.Name = "itemMenuStrip";
			this.itemMenuStrip.Size = new System.Drawing.Size(128, 142);
			// 
			// runToolStripMenuItem
			// 
			this.runToolStripMenuItem.Name = "runToolStripMenuItem";
			this.runToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.runToolStripMenuItem.Text = "Run";
			this.runToolStripMenuItem.Click += new System.EventHandler(this.runToolStripMenuItem_Click);
			// 
			// endToolStripMenuItem
			// 
			this.endToolStripMenuItem.Name = "endToolStripMenuItem";
			this.endToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.endToolStripMenuItem.Text = "End";
			this.endToolStripMenuItem.Click += new System.EventHandler(this.endToolStripMenuItem_Click);
			// 
			// disableToolStripMenuItem
			// 
			this.disableToolStripMenuItem.Name = "disableToolStripMenuItem";
			this.disableToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.disableToolStripMenuItem.Text = "Disable";
			this.disableToolStripMenuItem.Click += new System.EventHandler(this.disableToolStripMenuItem_Click);
			// 
			// exportToolStripMenuItem
			// 
			this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
			this.exportToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.exportToolStripMenuItem.Text = "Export...";
			this.exportToolStripMenuItem.Click += new System.EventHandler(this.exportToolStripMenuItem_Click);
			// 
			// propertiesToolStripMenuItem
			// 
			this.propertiesToolStripMenuItem.Name = "propertiesToolStripMenuItem";
			this.propertiesToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.propertiesToolStripMenuItem.Text = "Properties";
			this.propertiesToolStripMenuItem.Click += new System.EventHandler(this.propertiesToolStripMenuItem_Click);
			// 
			// toolStripMenuItem3
			// 
			this.toolStripMenuItem3.Name = "toolStripMenuItem3";
			this.toolStripMenuItem3.Size = new System.Drawing.Size(124, 6);
			// 
			// deleteToolStripMenuItem
			// 
			this.deleteToolStripMenuItem.Name = "deleteToolStripMenuItem";
			this.deleteToolStripMenuItem.Size = new System.Drawing.Size(127, 22);
			this.deleteToolStripMenuItem.Text = "Delete";
			this.deleteToolStripMenuItem.Click += new System.EventHandler(this.deleteToolStripMenuItem_Click);
			// 
			// TSMMCMockup
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1126, 686);
			this.Controls.Add(this.splitContainer1);
			this.Controls.Add(this.actionToolStrip);
			this.Controls.Add(this.statusStrip);
			this.Controls.Add(this.mainToolStrip);
			this.Controls.Add(this.mainMenuStrip);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MainMenuStrip = this.mainMenuStrip;
			this.Name = "TSMMCMockup";
			this.Text = "Task Scheduler";
			this.Load += new System.EventHandler(this.TSMMCMockup_Load);
			this.mainMenuStrip.ResumeLayout(false);
			this.mainMenuStrip.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.TaskService)).EndInit();
			this.splitContainer1.Panel1.ResumeLayout(false);
			this.splitContainer1.ResumeLayout(false);
			this.libraryMenuStrip.ResumeLayout(false);
			this.itemMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.MenuStrip mainMenuStrip;
		private System.Windows.Forms.ToolStrip mainToolStrip;
		private System.Windows.Forms.StatusStrip statusStrip;
		private Microsoft.Win32.TaskScheduler.TaskServiceConnectDialog taskServiceConnectDialog1;
		private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem;
		private System.Windows.Forms.ImageList imageList1;
		private System.Windows.Forms.ToolStrip actionToolStrip;
		private System.Windows.Forms.SplitContainer splitContainer1;
		private System.Windows.Forms.TreeView treeView1;
		private Microsoft.Win32.TaskScheduler.TaskService TaskService;
		private System.Windows.Forms.ToolStripMenuItem actionToolStripMenuItem1;
		private System.Windows.Forms.ContextMenuStrip libraryMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem connectToAnotherComputerToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createBasicTaskToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem createTaskToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importTaskToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem displayAllRunningTasksToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
		private System.Windows.Forms.OpenFileDialog openFileDialog1;
		private Microsoft.Win32.TaskScheduler.TaskEditDialog taskEditDialog1;
		private Microsoft.Win32.TaskScheduler.TaskSchedulerWizard taskSchedulerWizard1;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
		private System.Windows.Forms.ToolStripMenuItem newFolderToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
		private System.Windows.Forms.ContextMenuStrip itemMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem runToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem endToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem disableToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem propertiesToolStripMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripMenuItem3;
		private System.Windows.Forms.ToolStripMenuItem deleteToolStripMenuItem;
	}
}