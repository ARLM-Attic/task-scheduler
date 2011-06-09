﻿using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Dialog that allows tasks to be edited
	/// </summary>
	[ToolboxItem(true), ToolboxItemFilter("System.Windows.Forms.Control.TopLevel"), Description("Dialog allowing the editing of a task.")]
	[Designer("System.ComponentModel.Design.ComponentDesigner, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a")]
	[DesignTimeVisible(true)]
	public partial class TaskEditDialog :
#if DEBUG
		Form
#else
		DialogBase
#endif
	{
		private bool titleSet = false;

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskEditDialog"/> class.
		/// </summary>
		public TaskEditDialog()
		{
			InitializeComponent();
			RegisterTaskOnAccept = false;
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskEditDialog"/> class.
		/// </summary>
		/// <param name="task">The task.</param>
		/// <param name="editable">If set to <c>true</c> the task will be editable in the dialog.</param>
		/// <param name="registerOnAccept">If set to <c>true</c> the task will be registered when Ok is pressed.</param>
		public TaskEditDialog(Task task, bool editable = true, bool registerOnAccept = true)
		{
			InitializeComponent();
			this.Editable = editable;
			this.Initialize(task);
			this.RegisterTaskOnAccept = registerOnAccept;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TaskEditDialog"/> is editable.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
		[DefaultValue(false), Category("Behavior"), Description("Determines whether the task can be edited.")]
		public bool Editable
		{
			get { return taskPropertiesControl1.Editable; }
			set { taskPropertiesControl1.Editable = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether to register task when Accept (Ok) button pressed.
		/// </summary>
		/// <value><c>true</c> if updated task is to be registered; otherwise, <c>false</c>.</value>
		[Category("Behavior"), DefaultValue(false)]
		public bool RegisterTaskOnAccept { get; set; }

		/// <summary>
		/// Gets the current <see cref="Task"/>. This is only the task used to initialize this control. The updates made to the control are not registered.
		/// </summary>
		/// <value>The task.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public Task Task
		{
			get { return taskPropertiesControl1.Task; }
			set { taskPropertiesControl1.Initialize(value); }
		}

		/// <summary>
		/// Gets the <see cref="TaskDefinition"/> in its edited state.
		/// </summary>
		/// <value>The task definition.</value>
		[Browsable(false), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public TaskDefinition TaskDefinition
		{
			get { return taskPropertiesControl1.TaskDefinition; }
		}

		/// <summary>
		/// Gets the <see cref="TaskService"/> assigned at initialization.
		/// </summary>
		/// <value>The task service.</value>
		[DefaultValue(null), Category("Data"), Description("The TaskService for this dialog.")]
		public TaskService TaskService
		{
			get { return taskPropertiesControl1.TaskService; }
			set { taskPropertiesControl1.Initialize(value); }
		}

		/// <summary>
		/// Gets or sets the title.
		/// </summary>
		/// <value>The title.</value>
		[Category("Appearance"), Description("A string to display in the title bar of the dialog box."), Localizable(true)]
		public string Title
		{
			get { return base.Text; }
			set { base.Text = value; titleSet = true; }
		}

		/// <summary>
		/// Initializes the control for the editing of a new <see cref="TaskDefinition"/>.
		/// </summary>
		/// <param name="service">A <see cref="TaskService"/> instance.</param>
		public void Initialize(TaskService service)
		{
			if (!titleSet)
				this.Text = string.Format(Properties.Resources.TaskEditDlgTitle, "New Task", GetServerString(service));
			taskPropertiesControl1.Initialize(service);
		}

		/// <summary>
		/// Initializes the control for the editing of an existing <see cref="Task"/>.
		/// </summary>
		/// <param name="task">A <see cref="Task"/> instance.</param>
		public void Initialize(Task task)
		{
			if (!titleSet)
				this.Text = string.Format(Properties.Resources.TaskEditDlgTitle, task.Name, GetServerString(task.TaskService));
			taskPropertiesControl1.Initialize(task);
		}

		private string GetServerString(TaskService service)
		{
			return service.TargetServer == null || Environment.MachineName.Equals(service.TargetServer, StringComparison.CurrentCultureIgnoreCase) ?
				Properties.Resources.LocalMachine : service.TargetServer;
		}

		private string InvokeCredentialDialog(string userName)
		{
			CredentialsDialog dlg = new CredentialsDialog(Properties.Resources.TaskSchedulerName,
				Properties.Resources.CredentialPromptMessage, userName);
			dlg.Options |= CredentialsDialogOptions.Persist;
			if (dlg.ShowDialog(this.ParentForm) == DialogResult.OK)
				return dlg.Password;
			return null;
		}

		private void okBtn_Click(object sender, System.EventArgs e)
		{
			if (this.TaskDefinition.Actions.Count == 0)
			{
				MessageBox.Show(Properties.Resources.TaskMustHaveActionsError, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (this.TaskDefinition.Settings.DeleteExpiredTaskAfter != TimeSpan.Zero && !ValidateOneTriggerExpires())
			{
				MessageBox.Show(Properties.Resources.Error_TaskDeleteMustHaveExpiringTrigger, null, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			if (RegisterTaskOnAccept)
			{
				string user = this.TaskDefinition.Principal.UserId;
				string pwd = null;
				if (this.TaskDefinition.Principal.LogonType == TaskLogonType.InteractiveTokenOrPassword || this.TaskDefinition.Principal.LogonType == TaskLogonType.Password)
				{
					pwd = InvokeCredentialDialog(user);
					if (pwd == null)
						throw new System.Security.Authentication.AuthenticationException(Properties.Resources.UserAuthenticationError);
				}
				if (this.TaskDefinition.Principal.LogonType == TaskLogonType.Group)
					user = this.TaskDefinition.Principal.GroupId;
				this.TaskService.RootFolder.RegisterTaskDefinition(this.taskPropertiesControl1.TaskName, this.TaskDefinition, TaskCreation.CreateOrUpdate,
					user, pwd, this.TaskDefinition.Principal.LogonType);
			}
			this.DialogResult = DialogResult.OK;
			Close();
		}

		private void ResetTitle()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskEditDialog));
			base.Text = resources.GetString("$this.Text");
		}

		private bool ShouldSerializeTitle()
		{
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskEditDialog));
			return base.Text != resources.GetString("$this.Text");
		}

		private bool ValidateOneTriggerExpires()
		{
			foreach (var tr in this.TaskDefinition.Triggers)
			{
				if (tr.EndBoundary != DateTime.MaxValue)
					return true;
			}
			return false;
		}
	}
}