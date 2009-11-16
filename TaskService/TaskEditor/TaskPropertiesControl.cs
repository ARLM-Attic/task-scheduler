﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security.Principal;
using System.Windows.Forms;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Control which allows for the editing of all properties of a <see cref="TaskDefinition"/>.
	/// </summary>
    public partial class TaskPropertiesControl : UserControl
    {
        internal static global::System.Resources.ResourceManager taskSchedResources;

        private bool editable = false;
        //private bool flagExecutorIsCurrentUser, flagExecutorIsTheMachineAdministrator;
        private bool flagUserIsAnAdmin, flagExecutorIsServiceAccount, flagRunOnlyWhenUserIsLoggedOn, flagExecutorIsGroup;
        private Task task = null;
        private TaskDefinition td = null;
		private TaskService service = null;

        static TaskPropertiesControl()
        {
            taskSchedResources = new global::System.Resources.ResourceManager("Microsoft.Win32.TaskScheduler.Properties.Resources", typeof(Trigger).Assembly);
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="TaskPropertiesControl"/> class.
		/// </summary>
        public TaskPropertiesControl()
        {
            InitializeComponent();

            // Setup images on action up and down buttons
            Bitmap bmpUp = new Bitmap(6, 3);
            using (Graphics g = Graphics.FromImage(bmpUp))
            {
                g.Clear(actionUpButton.BackColor);
                using (SolidBrush b = new SolidBrush(SystemColors.ControlText))
                    g.FillPolygon(b, new Point[] { new Point(0, 0), new Point(6, 0), new Point(3, 3), new Point(2, 3) });
            }
            Bitmap bmpDn = (Bitmap)bmpUp.Clone();
            bmpDn.RotateFlip(RotateFlipType.RotateNoneFlipY);
            actionUpButton.Image = bmpDn;
            actionDownButton.Image = bmpUp;

            taskIdleDurationCombo.Items.AddRange(new TimeSpan[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30), TimeSpan.FromMinutes(60) });
            taskIdleWaitTimeoutCombo.FormattedZero = Properties.Resources.TimeSpanDoNotWait;
            taskIdleWaitTimeoutCombo.Items.AddRange(new TimeSpan[] { TimeSpan.Zero, TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30), TimeSpan.FromHours(1), TimeSpan.FromHours(2) });
            taskRestartIntervalCombo.Items.AddRange(new TimeSpan[] { TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(15), TimeSpan.FromMinutes(30), TimeSpan.FromHours(1), TimeSpan.FromHours(2) });
            taskExecutionTimeLimitCombo.Items.AddRange(new TimeSpan[] { TimeSpan.FromHours(1), TimeSpan.FromHours(2), TimeSpan.FromHours(4), TimeSpan.FromHours(8), TimeSpan.FromHours(12), TimeSpan.FromDays(1), TimeSpan.FromDays(3) });
            taskDeleteAfterCombo.FormattedZero = Properties.Resources.TimeSpanImmediately;
            taskDeleteAfterCombo.Items.AddRange(new TimeSpan[] { TimeSpan.Zero, TimeSpan.FromDays(30), TimeSpan.FromDays(90), TimeSpan.FromDays(180), TimeSpan.FromDays(365) });
        }

		/// <summary>
		/// Initializes the control for the editing of a new <see cref="TaskDefintion"/>.
		/// </summary>
		/// <param name="service">A <see cref="TaskService"/> instance.</param>
		public void Initialize(TaskService service)
		{
			this.TaskService = service;
			this.TaskDefinition = service.NewTask();
		}

		/// <summary>
		/// Initializes the control for the editing of an existing <see cref="Task"/>.
		/// </summary>
		/// <param name="task">A <see cref="Task"/> instance.</param>
		public void Initialize(Task task)
		{
			this.Task = task;
		}

		/// <summary>
		/// Gets or sets a value indicating whether this <see cref="TaskPropertiesControl"/> is editable.
		/// </summary>
		/// <value><c>true</c> if editable; otherwise, <c>false</c>.</value>
        public bool Editable
        {
            get { return editable; }
            set
            {
                editable = value;
                bool v2 = true;
                System.Security.Principal.WindowsIdentity wid = System.Security.Principal.WindowsIdentity.GetCurrent();
                if (task != null)
                {
                    v2 = td.Settings.Compatibility == TaskCompatibility.V2;
					try { wid = new System.Security.Principal.WindowsIdentity(td.Principal.UserId); }
					catch { }
                }

                // General tab
                taskDescText.ReadOnly = !value;
                changePrincipalButton.Visible = taskHiddenCheck.Enabled = taskRunLevelCheck.Enabled = taskVersionCombo.Enabled = value;
                SetUserControls(task != null ? td.Principal.LogonType : TaskLogonType.InteractiveTokenOrPassword);

                // Triggers tab
                triggerDeleteButton.Visible = triggerEditButton.Visible = triggerNewButton.Visible = value;
                triggerListView.Enabled = value;

                // Actions tab
                actionDeleteButton.Visible = actionEditButton.Visible = actionNewButton.Visible = value;
                actionListView.Enabled = actionUpButton.Visible = actionDownButton.Visible = value;

                // Conditions tab
                foreach (Control ctrl in conditionsTab.Controls)
                    foreach (Control sub in ctrl.Controls)
                        if (sub is CheckBox || sub is ComboBox)
                            sub.Enabled = value;

                // Settings tab
                foreach (Control ctrl in settingsTab.Controls)
                    ctrl.Enabled = value;
            }
        }

		/// <summary>
		/// Gets the current <see cref="Task"/>. This is only the task used to initialize this control. The updates made to the control are not registered.
		/// </summary>
		/// <value>The task.</value>
        public Task Task
        {
            get
            {
                return task;
            }
            private set
            {
                task = value;
				if (task != null)
				{
					TaskService = task.TaskService;
					TaskDefinition = task.Definition;
				}
            }
        }

		/// <summary>
		/// Gets the <see cref="TaskDefinition"/> in its edited state.
		/// </summary>
		/// <value>The task definition.</value>
        public TaskDefinition TaskDefinition
        {
            get { return td; }
            private set
            {
				if (service == null)
					throw new ArgumentNullException("TaskDefinition cannot be set until TaskService has been set with a valid object.");

				td = value;

                this.flagUserIsAnAdmin = NativeMethods.AccountUtils.CurrentUserIsAdmin(service.TargetServer);
                //this.flagExecutorIsCurrentUser = this.UserIsExecutor(td.Principal.UserId);
                this.flagExecutorIsServiceAccount = NativeMethods.AccountUtils.UserIsServiceAccount(service.ConnectedUser);
                //this.flagExecutorIsTheMachineAdministrator = this.ExecutorIsTheMachineAdministrator(executor);

                // Set General tab
                SetUserControls(td.Principal.LogonType);
                if (task != null) taskNameText.Text = task.Name;
                taskAuthorText.Text = td.RegistrationInfo.Author;
                taskDescText.Text = td.RegistrationInfo.Description;
                taskLoggedOnRadio.Checked = flagRunOnlyWhenUserIsLoggedOn;
                taskLoggedOptionalRadio.Checked = !flagRunOnlyWhenUserIsLoggedOn;
                taskLocalOnlyCheck.Checked = !flagRunOnlyWhenUserIsLoggedOn && td.Principal.LogonType == TaskLogonType.S4U;
                taskRunLevelCheck.Checked = td.Principal.RunLevel == TaskRunLevel.Highest;
                taskHiddenCheck.Checked = td.Settings.Hidden;
                taskVersionCombo.SelectedIndex = td.Settings.Compatibility == TaskCompatibility.V1 ? 0 : 1;

                // Set Triggers tab
                foreach (Trigger tr in td.Triggers)
                {
                    AddTriggerToList(tr);
                }

                // Set Actions tab
                foreach (Action act in td.Actions)
                {
                    AddActionToList(act, -1);
                }
                SetActionUpDnState();

                // Set Conditions tab
                taskIdleDurationCheck.Checked = td.Settings.IdleSettings.IdleDuration != TimeSpan.Zero;
                if (taskIdleDurationCheck.Checked)
                {
                    taskIdleDurationCombo.Value = td.Settings.IdleSettings.IdleDuration;
                    taskIdleWaitTimeoutCombo.Value = td.Settings.IdleSettings.WaitTimeout;
                    taskIdleDurationCombo.Enabled = taskIdleWaitTimeoutCombo.Enabled = editable;
                }
                taskStopOnIdleEndCheck.Checked = td.Settings.IdleSettings.StopOnIdleEnd;
                taskRestartOnIdleCheck.Enabled = editable && td.Settings.IdleSettings.StopOnIdleEnd;
                taskRestartOnIdleCheck.Checked = td.Settings.IdleSettings.RestartOnIdle;
                taskDisallowStartIfOnBatteriesCheck.Checked = td.Settings.DisallowStartIfOnBatteries;
                taskStopIfGoingOnBatteriesCheck.Enabled = editable && td.Settings.DisallowStartIfOnBatteries;
                taskStopIfGoingOnBatteriesCheck.Checked = td.Settings.StopIfGoingOnBatteries;
                taskWakeToRunCheck.Checked = td.Settings.WakeToRun;
                taskStartIfConnectionCheck.Checked = td.Settings.RunOnlyIfNetworkAvailable;
                availableConnectionsCombo.Enabled = editable && td.Settings.RunOnlyIfNetworkAvailable;
                if (taskStartIfConnectionCheck.Checked) availableConnectionsCombo.SelectedItem = td.Settings.NetworkSettings.Name;

                // Set Settings tab
                taskAllowDemandStartCheck.Checked = td.Settings.AllowDemandStart;
                taskStartWhenAvailableCheck.Checked = td.Settings.StartWhenAvailable;
                taskRestartIntervalCheck.Checked = taskRestartIntervalCombo.Enabled = td.Settings.RestartInterval != TimeSpan.Zero;
                if (taskRestartIntervalCheck.Checked)
                {
                    taskRestartIntervalCombo.Value = td.Settings.RestartInterval;
                    taskRestartCountText.Value = td.Settings.RestartCount;
                }
                taskExecutionTimeLimitCheck.Checked = taskExecutionTimeLimitCombo.Enabled = td.Settings.ExecutionTimeLimit != TimeSpan.Zero;
                if (taskExecutionTimeLimitCombo.Enabled) taskExecutionTimeLimitCombo.Value = td.Settings.ExecutionTimeLimit;
                taskAllowHardTerminateCheck.Checked = td.Settings.AllowHardTerminate;
                taskDeleteAfterCheck.Checked = taskDeleteAfterCombo.Enabled = td.Settings.DeleteExpiredTaskAfter != TimeSpan.Zero;
                if (taskDeleteAfterCombo.Enabled) taskDeleteAfterCombo.Value = td.Settings.DeleteExpiredTaskAfter;
                taskMultInstCombo.SelectedIndex = (int)td.Settings.MultipleInstances;
            }
        }

		/// <summary>
		/// Gets the <see cref="TaskService"/> assigned at initialization.
		/// </summary>
		/// <value>The task service.</value>
		public TaskService TaskService
		{
			get { return service; }
			private set { service = value; }
		}

        internal static string BuildEnumString(string preface, object enumValue)
        {
            return BuildEnumString(Properties.Resources.ResourceManager, preface, enumValue);
        }

        internal static string BuildEnumString(System.Resources.ResourceManager mgr, string preface, object enumValue)
        {
            string[] vals = enumValue.ToString().Split(new string[] { ", " }, StringSplitOptions.None);
            if (vals.Length == 0)
                return string.Empty;

            for (int i = 0; i < vals.Length; i++)
            {
                vals[i] = mgr.GetString(preface + vals[i]);
            }
            return string.Join(", ", vals);
        }

        private void AddActionToList(Action act, int index)
        {
            TaskActionType t = TaskActionType.Execute;
            switch (act.GetType().Name)
            {
                case "ComHandlerAction":
                    t = TaskActionType.ComHandler;
                    break;
                case "EmailAction":
                    t = TaskActionType.SendEmail;
                    break;
                case "ShowMessageAction":
                    t = TaskActionType.ShowMessage;
                    break;
                case "ExecAction":
                default:
                    break;
            }
            ListViewItem lvi = new ListViewItem(new string[] {
                    BuildEnumString(taskSchedResources, "ActionType", t),
                    act.ToString() }) { Tag = act };
            if (index < 0)
                actionListView.Items.Add(lvi);
            else
                actionListView.Items.Insert(index, lvi);
        }

        private void AddTriggerToList(Trigger tr)
        {
            triggerListView.Items.Add(new ListViewItem(new string[] {
                    BuildEnumString(taskSchedResources, "TriggerType", tr.TriggerType),
                    tr.ToString(),
                    tr.Enabled ? Properties.Resources.Enabled : Properties.Resources.Disabled
                }));
        }

        private void InvokeObjectPicker(string targetComputerName)
        {
            string acct = String.Empty;
            if (!NativeMethods.AccountUtils.SelectAccount(this, targetComputerName, ref acct, ref this.flagExecutorIsGroup, ref this.flagExecutorIsServiceAccount))
                return;

            if (this.flagExecutorIsServiceAccount)
            {
                this.flagExecutorIsGroup = false;
                td.Principal.UserId = acct;
                td.Principal.LogonType = TaskLogonType.ServiceAccount;
                //this.flagExecutorIsCurrentUser = false;
            }
            else if (this.flagExecutorIsGroup)
            {
                td.Principal.GroupId = acct;
                td.Principal.LogonType = TaskLogonType.Group;
                //this.flagExecutorIsCurrentUser = false;
            }
            else
            {
                td.Principal.UserId = acct;
                //this.flagExecutorIsCurrentUser = this.UserIsExecutor(objArray[0].ObjectName);
                if (td.Principal.LogonType == TaskLogonType.Group)
                {
                    td.Principal.LogonType = TaskLogonType.InteractiveToken;
                }
                else if (td.Principal.LogonType == TaskLogonType.ServiceAccount)
                {
                    td.Principal.LogonType = TaskLogonType.Password;
                }
            }
            SetUserControls(td.Principal.LogonType);
        }

        private void SetActionUpDnState()
        {
            actionUpButton.Enabled = actionDownButton.Enabled = actionListView.Items.Count > 1;
        }

        private void SetUserControls(TaskLogonType logonType)
        {
            switch (logonType)
            {
                case TaskLogonType.InteractiveToken:
                    this.flagRunOnlyWhenUserIsLoggedOn = true;
                    this.flagExecutorIsServiceAccount = false;
                    this.flagExecutorIsGroup = false;
                    break;
                case TaskLogonType.Group:
                    this.flagRunOnlyWhenUserIsLoggedOn = true;
                    this.flagExecutorIsServiceAccount = false;
                    this.flagExecutorIsGroup = true;
                    break;
                case TaskLogonType.ServiceAccount:
                    this.flagRunOnlyWhenUserIsLoggedOn = false;
                    this.flagExecutorIsServiceAccount = true;
                    this.flagExecutorIsGroup = false;
                    break;
                default:
                    this.flagRunOnlyWhenUserIsLoggedOn = false;
                    this.flagExecutorIsServiceAccount = false;
                    this.flagExecutorIsGroup = false;
                    break;
            }

            if (this.flagExecutorIsServiceAccount)
            {
                taskLoggedOnRadio.Enabled = false;
                taskLoggedOptionalRadio.Enabled = false;
                taskLocalOnlyCheck.Enabled = false;
            }
            else if (this.flagExecutorIsGroup)
            {
                taskLoggedOnRadio.Enabled = editable;
                taskLoggedOptionalRadio.Enabled = false;
                taskLocalOnlyCheck.Enabled = false;
            }
            else if (this.flagRunOnlyWhenUserIsLoggedOn)
            {
                taskLoggedOnRadio.Enabled = editable;
                taskLoggedOptionalRadio.Enabled = editable;
                taskLocalOnlyCheck.Enabled = false;
            }
            else
            {
                taskLoggedOnRadio.Enabled = editable;
                taskLoggedOptionalRadio.Enabled = editable;
                taskLocalOnlyCheck.Enabled = editable && (task == null || td.Settings.Compatibility == TaskCompatibility.V2);
            }
            if (task != null)
                taskPrincipalText.Text = this.flagExecutorIsGroup ? td.Principal.GroupId : td.Principal.UserId;
            else
                taskPrincipalText.Text = WindowsIdentity.GetCurrent().Name;
        }

        private void actionDeleteButton_Click(object sender, EventArgs e)
        {
            int idx = actionListView.SelectedIndices[0];
            td.Actions.RemoveAt(idx);
            actionListView.Items.RemoveAt(idx);
            SetActionUpDnState();
        }

        private void actionDownButton_Click(object sender, EventArgs e)
        {
            if ((this.actionListView.SelectedIndices.Count == 1) && (this.actionListView.SelectedIndices[0] != (this.actionListView.Items.Count - 1)))
            {
                int index = actionListView.SelectedIndices[0];
                actionListView.BeginUpdate();
                ListViewItem lvi = this.actionListView.Items[index];
                actionListView.Items.RemoveAt(index);
                actionListView.Items.Insert(index + 1, lvi);
                actionListView.EndUpdate();
            }
        }

        private void actionEditButton_Click(object sender, EventArgs e)
        {
            int idx = actionListView.SelectedIndices[0];
            ActionEditDialog dlg = new ActionEditDialog(td.Settings.Compatibility == TaskCompatibility.V2);
            dlg.Action = actionListView.Items[idx].Tag as Action;
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                td.Actions.RemoveAt(idx);
                actionListView.Items.RemoveAt(idx);
                td.Actions.Add(dlg.Action);
                AddActionToList(dlg.Action, idx);
            }
        }

        private void actionListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            actionEditButton_Click(sender, EventArgs.Empty);
        }

        private void actionNewButton_Click(object sender, EventArgs e)
        {
            ActionEditDialog dlg = new ActionEditDialog(td.Settings.Compatibility == TaskCompatibility.V2);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                td.Actions.Add(dlg.Action);
                AddActionToList(dlg.Action, -1);
                SetActionUpDnState();
            }
        }

        private void actionUpButton_Click(object sender, EventArgs e)
        {
            if ((this.actionListView.SelectedIndices.Count == 1) && (this.actionListView.SelectedIndices[0] != 0))
            {
                int index = actionListView.SelectedIndices[0];
                actionListView.BeginUpdate();
                ListViewItem lvi = this.actionListView.Items[index];
                actionListView.Items.RemoveAt(index);
                actionListView.Items.Insert(index - 1, lvi);
                actionListView.EndUpdate();
            }
        }

        private void availableConnectionsCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool onNet = td.Settings.RunOnlyIfNetworkAvailable = taskStartIfConnectionCheck.Checked && availableConnectionsCombo.SelectedIndex != -1;
            if (onNet && availableConnectionsCombo.SelectedIndex > 0)
                td.Settings.NetworkSettings.Name = availableConnectionsCombo.SelectedItem.ToString();
            else
                td.Settings.NetworkSettings.Name = null;
        }

        private void changePrincipalButton_Click(object sender, EventArgs e)
        {
            InvokeObjectPicker(service.TargetServer);
        }

        private void conditionsTab_Enter(object sender, EventArgs e)
        {
            // Load network connections
            availableConnectionsCombo.Items.Clear();
            availableConnectionsCombo.Items.Add(Properties.Resources.AnyConnection);
            foreach (var n in System.Net.NetworkInformation.NetworkInterface.GetAllNetworkInterfaces())
                availableConnectionsCombo.Items.Add(n.Name);
            if (task == null)
                availableConnectionsCombo.SelectedIndex = -1;
            else
                availableConnectionsCombo.SelectedText = td.Settings.NetworkSettings.Name;
        }

        private void historyBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            TaskEventLog log = new TaskEventLog(task.Path);
            List<ListViewItem> c = new List<ListViewItem>(100);
            foreach (TaskEvent item in log)
                c.Add(new ListViewItem(new string[] { item.Level, item.TimeCreated.ToString(), item.EventId.ToString(),
                    item.TaskCategory, item.OpCode, item.ActivityId.ToString() }));
            c.Reverse();
            e.Result = c.ToArray();
        }

        private void historyBackgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
        {
            historyListView.Items.AddRange(e.Result as ListViewItem[]);
            historyListView.Cursor = Cursors.Default;
        }

        private void historyTab_Enter(object sender, EventArgs e)
        {
            if (task == null)
                return;

            historyListView.Items.Clear();
            historyListView.Cursor = Cursors.WaitCursor;
            historyBackgroundWorker.RunWorkerAsync();
        }

        private void taskAllowDemandStartCheck_CheckedChanged(object sender, EventArgs e)
        {
			if (td.Settings.Compatibility == TaskCompatibility.V2)
				td.Settings.AllowDemandStart = taskAllowDemandStartCheck.Checked;
        }

        private void taskAllowHardTerminateCheck_CheckedChanged(object sender, EventArgs e)
        {
			if (td.Settings.Compatibility == TaskCompatibility.V2)
				td.Settings.AllowHardTerminate = taskAllowHardTerminateCheck.Checked;
        }

        private void taskDeleteAfterCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskDeleteAfterCombo.Enabled = editable && taskDeleteAfterCheck.Checked;
            if (taskDeleteAfterCheck.Checked)
                taskDeleteAfterCombo.Value = TimeSpan.FromDays(30);
            else
                taskDeleteAfterCombo.Value = TimeSpan.Zero;
        }

        private void taskDeleteAfterCombo_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.DeleteExpiredTaskAfter = taskDeleteAfterCombo.Value;
            taskDeleteAfterCheck.Checked = taskDeleteAfterCombo.Value != TimeSpan.Zero;
        }

        private void taskDescText_Leave(object sender, EventArgs e)
        {
            td.RegistrationInfo.Description = taskDescText.Text;
        }

        private void taskDisallowStartIfOnBatteriesCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskStopIfGoingOnBatteriesCheck.Enabled = editable && taskDisallowStartIfOnBatteriesCheck.Checked;
            td.Settings.DisallowStartIfOnBatteries = taskDisallowStartIfOnBatteriesCheck.Checked;
        }

        private void taskExecutionTimeLimitCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskExecutionTimeLimitCombo.Enabled = editable && taskExecutionTimeLimitCheck.Checked;
            if (taskExecutionTimeLimitCheck.Checked)
                taskExecutionTimeLimitCombo.Value = TimeSpan.FromDays(3);
            else
                taskExecutionTimeLimitCombo.Value = TimeSpan.Zero;
        }

        private void taskExecutionTimeLimitCombo_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.ExecutionTimeLimit = taskExecutionTimeLimitCombo.Value;
            taskExecutionTimeLimitCheck.Checked = taskExecutionTimeLimitCombo.Value != TimeSpan.Zero;
        }

        private void taskHiddenCheck_CheckedChanged(object sender, EventArgs e)
        {
            td.Settings.Hidden = taskHiddenCheck.Checked;
        }

        private void taskIdleDelayCombo_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.IdleSettings.WaitTimeout = taskIdleWaitTimeoutCombo.Value;
        }

        private void taskIdleDurationCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskIdleDurationCombo.Enabled = taskIdleWaitTimeoutCombo.Enabled = taskStopOnIdleEndCheck.Enabled = editable && taskIdleDurationCheck.Checked;
            if (taskIdleDurationCheck.Checked)
            {
                taskIdleDurationCombo.Value = TimeSpan.FromMinutes(10);
                taskIdleWaitTimeoutCombo.Value = TimeSpan.FromHours(1);
            }
            else
                taskIdleDurationCombo.Value = taskIdleWaitTimeoutCombo.Value = TimeSpan.Zero;
            taskRestartOnIdleCheck_CheckedChanged(sender, e);
        }

        private void taskLoggedOptionalRadio_CheckedChanged(object sender, EventArgs e)
        {
            taskLocalOnlyCheck.Enabled = editable && taskLoggedOptionalRadio.Checked;
        }

        private void taskMultInstCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
			if (td.Settings.Compatibility == TaskCompatibility.V2)
				td.Settings.MultipleInstances = (TaskInstancesPolicy)taskMultInstCombo.SelectedIndex;
        }

        private void taskRestartCountText_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.RestartCount = Convert.ToInt32(taskRestartCountText.Value);
        }

        private void taskRestartIntervalCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskRestartIntervalCombo.Enabled = taskRestartCountText.Enabled = editable && taskRestartIntervalCheck.Checked;
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

        private void taskRestartIntervalCombo_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.RestartInterval = taskRestartIntervalCombo.Value;
            taskRestartIntervalCheck.Checked = td.Settings.RestartInterval != TimeSpan.Zero;
        }

        private void taskRestartOnIdleCheck_CheckedChanged(object sender, EventArgs e)
        {
            td.Settings.IdleSettings.RestartOnIdle = taskRestartOnIdleCheck.Checked;
        }

        private void taskStartAfterIdleCombo_ValueChanged(object sender, EventArgs e)
        {
            td.Settings.IdleSettings.IdleDuration = taskIdleDurationCombo.Value;
        }

        private void taskStartIfConnectionCheck_CheckedChanged(object sender, EventArgs e)
        {
            availableConnectionsCombo.Enabled = editable && taskStartIfConnectionCheck.Checked;
        }

        private void taskStartWhenAvailableCheck_CheckedChanged(object sender, EventArgs e)
        {
            td.Settings.StartWhenAvailable = taskStartWhenAvailableCheck.Checked;
        }

        private void taskStopIfGoingOnBatteriesCheck_CheckedChanged(object sender, EventArgs e)
        {
            td.Settings.StopIfGoingOnBatteries = taskStopIfGoingOnBatteriesCheck.Checked;
        }

        private void taskStopOnIdleEndCheck_CheckedChanged(object sender, EventArgs e)
        {
            taskRestartOnIdleCheck.Enabled = editable && (taskStopOnIdleEndCheck.Checked && taskStopOnIdleEndCheck.Enabled);
            td.Settings.IdleSettings.StopOnIdleEnd = taskStopOnIdleEndCheck.Checked;
        }

        private void taskVersionCombo_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool isV2 = taskVersionCombo.SelectedIndex != 0;
			if (isV2)
	            td.Settings.Compatibility = TaskCompatibility.V2;

            taskRestartOnIdleCheck.Enabled = isV2;
            taskStartIfConnectionCheck.Enabled = availableConnectionsCombo.Enabled = isV2;
            taskAllowDemandStartCheck.Enabled = taskStartWhenAvailableCheck.Enabled = isV2;
            taskRestartIntervalCheck.Enabled = taskRestartIntervalCombo.Enabled = isV2;
            taskRestartAttemptsLabel.Enabled = taskRestartAttemptTimesLabel.Enabled = taskRestartCountText.Enabled = isV2;
            taskAllowHardTerminateCheck.Enabled = taskRunningRuleLabel.Enabled = taskMultInstCombo.Enabled = isV2;
        }

        private void taskWakeToRunCheck_CheckedChanged(object sender, EventArgs e)
        {
            td.Settings.WakeToRun = taskWakeToRunCheck.Checked;
        }

        private void triggerDeleteButton_Click(object sender, EventArgs e)
        {
            int idx = triggerListView.SelectedIndices[0];
            td.Triggers.RemoveAt(idx);
            triggerListView.Items.RemoveAt(idx);
        }

        private void triggerEditButton_Click(object sender, EventArgs e)
        {
            int idx = triggerListView.SelectedIndices[0];
            TriggerEditDialog dlg = new TriggerEditDialog(td, td.Triggers[idx]);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                td.Triggers.RemoveAt(idx);
                triggerListView.Items.RemoveAt(idx);
                td.Triggers.Add(dlg.Trigger);
                AddTriggerToList(dlg.Trigger);
            }
        }

        private void triggerListView_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            triggerEditButton_Click(sender, EventArgs.Empty);
        }

        private void triggerNewButton_Click(object sender, EventArgs e)
        {
            TriggerEditDialog dlg = new TriggerEditDialog(td, null);
            if (dlg.ShowDialog() == DialogResult.OK)
            {
                td.Triggers.Add(dlg.Trigger);
                AddTriggerToList(dlg.Trigger);
            }
        }
    }
}