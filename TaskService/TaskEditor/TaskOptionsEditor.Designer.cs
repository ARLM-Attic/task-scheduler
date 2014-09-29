﻿namespace Microsoft.Win32.TaskScheduler
{
	partial class TaskOptionsEditor
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskOptionsEditor));
			this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
			this.cancelButton = new System.Windows.Forms.Button();
			this.okButton = new System.Windows.Forms.Button();
			this.menuPanel = new System.Windows.Forms.Panel();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.generalItem = new System.Windows.Forms.ToolStripMenuItem();
			this.triggersItem = new System.Windows.Forms.ToolStripMenuItem();
			this.actionsItem = new System.Windows.Forms.ToolStripMenuItem();
			this.bodyPanel = new System.Windows.Forms.Panel();
			this.splitterPanel = new System.Windows.Forms.Panel();
			this.tableLayoutPanel1.SuspendLayout();
			this.menuPanel.SuspendLayout();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tableLayoutPanel1
			// 
			resources.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
			this.tableLayoutPanel1.Controls.Add(this.cancelButton, 2, 0);
			this.tableLayoutPanel1.Controls.Add(this.okButton, 1, 0);
			this.tableLayoutPanel1.Name = "tableLayoutPanel1";
			// 
			// cancelButton
			// 
			resources.ApplyResources(this.cancelButton, "cancelButton");
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// okButton
			// 
			resources.ApplyResources(this.okButton, "okButton");
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Name = "okButton";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// menuPanel
			// 
			this.menuPanel.BackColor = System.Drawing.SystemColors.Window;
			this.menuPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.menuPanel.Controls.Add(this.menuStrip1);
			resources.ApplyResources(this.menuPanel, "menuPanel");
			this.menuPanel.Name = "menuPanel";
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generalItem,
            this.triggersItem,
            this.actionsItem});
			this.menuStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.Table;
			resources.ApplyResources(this.menuStrip1, "menuStrip1");
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.ShowItemToolTips = true;
			// 
			// generalItem
			// 
			this.generalItem.Name = "generalItem";
			this.generalItem.Padding = new System.Windows.Forms.Padding(4);
			resources.ApplyResources(this.generalItem, "generalItem");
			this.generalItem.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// triggersItem
			// 
			this.triggersItem.Name = "triggersItem";
			this.triggersItem.Padding = new System.Windows.Forms.Padding(4);
			resources.ApplyResources(this.triggersItem, "triggersItem");
			this.triggersItem.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// actionsItem
			// 
			this.actionsItem.Name = "actionsItem";
			this.actionsItem.Padding = new System.Windows.Forms.Padding(4);
			resources.ApplyResources(this.actionsItem, "actionsItem");
			this.actionsItem.Click += new System.EventHandler(this.menuItem_Click);
			// 
			// bodyPanel
			// 
			this.bodyPanel.BackColor = System.Drawing.SystemColors.Window;
			this.bodyPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			resources.ApplyResources(this.bodyPanel, "bodyPanel");
			this.bodyPanel.Name = "bodyPanel";
			// 
			// splitterPanel
			// 
			resources.ApplyResources(this.splitterPanel, "splitterPanel");
			this.splitterPanel.Name = "splitterPanel";
			// 
			// TaskOptionsEditor
			// 
			this.AcceptButton = this.okButton;
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ControlBox = false;
			this.Controls.Add(this.bodyPanel);
			this.Controls.Add(this.splitterPanel);
			this.Controls.Add(this.menuPanel);
			this.Controls.Add(this.tableLayoutPanel1);
			this.MainMenuStrip = this.menuStrip1;
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "TaskOptionsEditor";
			this.ShowInTaskbar = false;
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.tableLayoutPanel1.ResumeLayout(false);
			this.menuPanel.ResumeLayout(false);
			this.menuPanel.PerformLayout();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Panel menuPanel;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem generalItem;
		private System.Windows.Forms.ToolStripMenuItem triggersItem;
		private System.Windows.Forms.ToolStripMenuItem actionsItem;
		private System.Windows.Forms.Panel bodyPanel;
		private System.Windows.Forms.Panel splitterPanel;
	}
}