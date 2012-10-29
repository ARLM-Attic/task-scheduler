﻿namespace Microsoft.Win32.TaskScheduler
{
	partial class TaskPropertiesControl
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

		#region Component Designer generated code

		/// <summary> 
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TaskPropertiesControl));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.generalTab = new System.Windows.Forms.TabPage();
			this.taskNameLabel = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.taskAuthorLabel = new System.Windows.Forms.Label();
			this.taskDescLabel = new System.Windows.Forms.Label();
			this.taskNameText = new System.Windows.Forms.TextBox();
			this.taskLocationText = new System.Windows.Forms.Label();
			this.taskAuthorText = new System.Windows.Forms.Label();
			this.taskDescText = new System.Windows.Forms.TextBox();
			this.taskVersionCombo = new System.Windows.Forms.ComboBox();
			this.taskVersionLabel = new System.Windows.Forms.Label();
			this.taskHiddenCheck = new System.Windows.Forms.CheckBox();
			this.taskSecurityGroupBox = new System.Windows.Forms.GroupBox();
			this.taskRunLevelCheck = new System.Windows.Forms.CheckBox();
			this.taskLocalOnlyCheck = new System.Windows.Forms.CheckBox();
			this.taskLoggedOptionalRadio = new System.Windows.Forms.RadioButton();
			this.taskLoggedOnRadio = new System.Windows.Forms.RadioButton();
			this.taskPrincipalText = new System.Windows.Forms.TextBox();
			this.changePrincipalButton = new System.Windows.Forms.Button();
			this.taskUserAcctLabel = new System.Windows.Forms.Label();
			this.triggersTab = new System.Windows.Forms.TabPage();
			this.triggerDeleteButton = new System.Windows.Forms.Button();
			this.triggerEditButton = new System.Windows.Forms.Button();
			this.triggerNewButton = new System.Windows.Forms.Button();
			this.triggerListView = new System.Windows.Forms.ListView();
			this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader3 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.taskTriggerIntroLabel = new System.Windows.Forms.Label();
			this.actionsTab = new System.Windows.Forms.TabPage();
			this.actionDownButton = new System.Windows.Forms.Button();
			this.actionUpButton = new System.Windows.Forms.Button();
			this.actionDeleteButton = new System.Windows.Forms.Button();
			this.actionEditButton = new System.Windows.Forms.Button();
			this.actionNewButton = new System.Windows.Forms.Button();
			this.actionListView = new System.Windows.Forms.ListView();
			this.columnHeader4 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader5 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.actionIntroLabel = new System.Windows.Forms.Label();
			this.conditionsTab = new System.Windows.Forms.TabPage();
			this.networkConditionGroupBox = new System.Windows.Forms.GroupBox();
			this.availableConnectionsCombo = new System.Windows.Forms.ComboBox();
			this.taskStartIfConnectionCheck = new System.Windows.Forms.CheckBox();
			this.powerConditionGroupBox = new System.Windows.Forms.GroupBox();
			this.taskStopIfGoingOnBatteriesCheck = new System.Windows.Forms.CheckBox();
			this.taskWakeToRunCheck = new System.Windows.Forms.CheckBox();
			this.taskDisallowStartIfOnBatteriesCheck = new System.Windows.Forms.CheckBox();
			this.idleConditionGroupBox = new System.Windows.Forms.GroupBox();
			this.taskIdleWaitTimeoutCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskIdleDurationCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskRestartOnIdleCheck = new System.Windows.Forms.CheckBox();
			this.taskStopOnIdleEndCheck = new System.Windows.Forms.CheckBox();
			this.taskIdleWaitTimeoutLabel = new System.Windows.Forms.Label();
			this.taskIdleDurationCheck = new System.Windows.Forms.CheckBox();
			this.conditionIntroLabel = new System.Windows.Forms.Label();
			this.settingsTab = new System.Windows.Forms.TabPage();
			this.taskRestartCountText = new System.Windows.Forms.NumericUpDown();
			this.taskMultInstCombo = new System.Windows.Forms.ComboBox();
			this.taskRunningRuleLabel = new System.Windows.Forms.Label();
			this.taskRestartAttemptTimesLabel = new System.Windows.Forms.Label();
			this.taskRestartCountLabel = new System.Windows.Forms.Label();
			this.taskDeleteAfterCheck = new System.Windows.Forms.CheckBox();
			this.taskAllowHardTerminateCheck = new System.Windows.Forms.CheckBox();
			this.taskExecutionTimeLimitCheck = new System.Windows.Forms.CheckBox();
			this.taskRestartIntervalCheck = new System.Windows.Forms.CheckBox();
			this.taskStartWhenAvailableCheck = new System.Windows.Forms.CheckBox();
			this.taskAllowDemandStartCheck = new System.Windows.Forms.CheckBox();
			this.settingsIntroLabel = new System.Windows.Forms.Label();
			this.taskDeleteAfterCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskExecutionTimeLimitCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskRestartIntervalCombo = new System.Windows.Forms.TimeSpanPicker();
			this.regInfoTab = new System.Windows.Forms.TabPage();
			this.label5 = new System.Windows.Forms.Label();
			this.taskRegSDDLLabel = new System.Windows.Forms.Label();
			this.taskRegVersionLabel = new System.Windows.Forms.Label();
			this.taskRegURILabel = new System.Windows.Forms.Label();
			this.taskRegSourceLabel = new System.Windows.Forms.Label();
			this.taskRegDocLabel = new System.Windows.Forms.Label();
			this.taskRegSDDLText = new System.Windows.Forms.TextBox();
			this.taskRegVersionText = new System.Windows.Forms.TextBox();
			this.taskRegURIText = new System.Windows.Forms.TextBox();
			this.taskRegSourceText = new System.Windows.Forms.TextBox();
			this.taskRegDocText = new System.Windows.Forms.TextBox();
			this.taskRegSDDLBtn = new System.Windows.Forms.Button();
			this.addPropTab = new System.Windows.Forms.TabPage();
			this.secHardGroup = new System.Windows.Forms.GroupBox();
			this.principalSIDTypeLabel = new System.Windows.Forms.Label();
			this.principalSIDTypeCombo = new System.Windows.Forms.ComboBox();
			this.principalReqPrivilegesLabel = new System.Windows.Forms.Label();
			this.principalReqPrivilegesDropDown = new Microsoft.Win32.TaskScheduler.DropDownCheckList();
			this.autoMaintGroup = new System.Windows.Forms.GroupBox();
			this.taskMaintenanceDeadlineCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskMaintenanceExclusiveCheck = new System.Windows.Forms.CheckBox();
			this.taskMaintenanceDeadlineLabel = new System.Windows.Forms.Label();
			this.taskMaintenancePeriodLabel = new System.Windows.Forms.Label();
			this.taskMaintenancePeriodCombo = new System.Windows.Forms.TimeSpanPicker();
			this.taskPriorityCombo = new System.Windows.Forms.ComboBox();
			this.label8 = new System.Windows.Forms.Label();
			this.taskVolatileCheck = new System.Windows.Forms.CheckBox();
			this.taskUseUnifiedSchedulingEngineCheck = new System.Windows.Forms.CheckBox();
			this.taskDisallowStartOnRemoteAppSessionCheck = new System.Windows.Forms.CheckBox();
			this.taskEnabledCheck = new System.Windows.Forms.CheckBox();
			this.label4 = new System.Windows.Forms.Label();
			this.runTimesTab = new System.Windows.Forms.TabPage();
			this.taskRunTimesControl1 = new Microsoft.Win32.TaskScheduler.TaskRunTimesControl();
			this.runTimesErrorLabel = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.historyTab = new System.Windows.Forms.TabPage();
			this.historyListView = new System.Windows.Forms.ListView();
			this.columnHeader6 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader7 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader8 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader9 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader10 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.columnHeader11 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.historyBackgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.errorProvider = new System.Windows.Forms.ErrorProvider(this.components);
			this.helpProvider = new System.Windows.Forms.HelpProvider();
			this.tabControl.SuspendLayout();
			this.generalTab.SuspendLayout();
			this.taskSecurityGroupBox.SuspendLayout();
			this.triggersTab.SuspendLayout();
			this.actionsTab.SuspendLayout();
			this.conditionsTab.SuspendLayout();
			this.networkConditionGroupBox.SuspendLayout();
			this.powerConditionGroupBox.SuspendLayout();
			this.idleConditionGroupBox.SuspendLayout();
			this.settingsTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.taskRestartCountText)).BeginInit();
			this.regInfoTab.SuspendLayout();
			this.addPropTab.SuspendLayout();
			this.secHardGroup.SuspendLayout();
			this.autoMaintGroup.SuspendLayout();
			this.runTimesTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.taskRunTimesControl1)).BeginInit();
			this.historyTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).BeginInit();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Controls.Add(this.generalTab);
			this.tabControl.Controls.Add(this.triggersTab);
			this.tabControl.Controls.Add(this.actionsTab);
			this.tabControl.Controls.Add(this.conditionsTab);
			this.tabControl.Controls.Add(this.settingsTab);
			this.tabControl.Controls.Add(this.regInfoTab);
			this.tabControl.Controls.Add(this.addPropTab);
			this.tabControl.Controls.Add(this.runTimesTab);
			this.tabControl.Controls.Add(this.historyTab);
			resources.ApplyResources(this.tabControl, "tabControl");
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.helpProvider.SetShowHelp(this.tabControl, ((bool)(resources.GetObject("tabControl.ShowHelp"))));
			// 
			// generalTab
			// 
			this.generalTab.Controls.Add(this.taskNameLabel);
			this.generalTab.Controls.Add(this.label2);
			this.generalTab.Controls.Add(this.taskAuthorLabel);
			this.generalTab.Controls.Add(this.taskDescLabel);
			this.generalTab.Controls.Add(this.taskNameText);
			this.generalTab.Controls.Add(this.taskLocationText);
			this.generalTab.Controls.Add(this.taskAuthorText);
			this.generalTab.Controls.Add(this.taskDescText);
			this.generalTab.Controls.Add(this.taskVersionCombo);
			this.generalTab.Controls.Add(this.taskVersionLabel);
			this.generalTab.Controls.Add(this.taskHiddenCheck);
			this.generalTab.Controls.Add(this.taskSecurityGroupBox);
			this.helpProvider.SetHelpKeyword(this.generalTab, resources.GetString("generalTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.generalTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("generalTab.HelpNavigator"))));
			resources.ApplyResources(this.generalTab, "generalTab");
			this.generalTab.Name = "generalTab";
			this.helpProvider.SetShowHelp(this.generalTab, ((bool)(resources.GetObject("generalTab.ShowHelp"))));
			this.generalTab.UseVisualStyleBackColor = true;
			this.generalTab.Enter += new System.EventHandler(this.generalTab_Enter);
			// 
			// taskNameLabel
			// 
			resources.ApplyResources(this.taskNameLabel, "taskNameLabel");
			this.taskNameLabel.Name = "taskNameLabel";
			this.helpProvider.SetShowHelp(this.taskNameLabel, ((bool)(resources.GetObject("taskNameLabel.ShowHelp"))));
			// 
			// label2
			// 
			resources.ApplyResources(this.label2, "label2");
			this.label2.Name = "label2";
			this.helpProvider.SetShowHelp(this.label2, ((bool)(resources.GetObject("label2.ShowHelp"))));
			// 
			// taskAuthorLabel
			// 
			resources.ApplyResources(this.taskAuthorLabel, "taskAuthorLabel");
			this.taskAuthorLabel.Name = "taskAuthorLabel";
			this.helpProvider.SetShowHelp(this.taskAuthorLabel, ((bool)(resources.GetObject("taskAuthorLabel.ShowHelp"))));
			// 
			// taskDescLabel
			// 
			resources.ApplyResources(this.taskDescLabel, "taskDescLabel");
			this.taskDescLabel.Name = "taskDescLabel";
			this.helpProvider.SetShowHelp(this.taskDescLabel, ((bool)(resources.GetObject("taskDescLabel.ShowHelp"))));
			// 
			// taskNameText
			// 
			resources.ApplyResources(this.taskNameText, "taskNameText");
			this.taskNameText.Name = "taskNameText";
			this.taskNameText.ReadOnly = true;
			this.helpProvider.SetShowHelp(this.taskNameText, ((bool)(resources.GetObject("taskNameText.ShowHelp"))));
			// 
			// taskLocationText
			// 
			resources.ApplyResources(this.taskLocationText, "taskLocationText");
			this.taskLocationText.Name = "taskLocationText";
			this.helpProvider.SetShowHelp(this.taskLocationText, ((bool)(resources.GetObject("taskLocationText.ShowHelp"))));
			// 
			// taskAuthorText
			// 
			resources.ApplyResources(this.taskAuthorText, "taskAuthorText");
			this.taskAuthorText.Name = "taskAuthorText";
			this.helpProvider.SetShowHelp(this.taskAuthorText, ((bool)(resources.GetObject("taskAuthorText.ShowHelp"))));
			// 
			// taskDescText
			// 
			resources.ApplyResources(this.taskDescText, "taskDescText");
			this.taskDescText.Name = "taskDescText";
			this.helpProvider.SetShowHelp(this.taskDescText, ((bool)(resources.GetObject("taskDescText.ShowHelp"))));
			this.taskDescText.Leave += new System.EventHandler(this.taskDescText_Leave);
			// 
			// taskVersionCombo
			// 
			resources.ApplyResources(this.taskVersionCombo, "taskVersionCombo");
			this.taskVersionCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.taskVersionCombo.Name = "taskVersionCombo";
			this.helpProvider.SetShowHelp(this.taskVersionCombo, ((bool)(resources.GetObject("taskVersionCombo.ShowHelp"))));
			this.taskVersionCombo.SelectedIndexChanged += new System.EventHandler(this.taskVersionCombo_SelectedIndexChanged);
			// 
			// taskVersionLabel
			// 
			resources.ApplyResources(this.taskVersionLabel, "taskVersionLabel");
			this.taskVersionLabel.Name = "taskVersionLabel";
			this.helpProvider.SetShowHelp(this.taskVersionLabel, ((bool)(resources.GetObject("taskVersionLabel.ShowHelp"))));
			// 
			// taskHiddenCheck
			// 
			resources.ApplyResources(this.taskHiddenCheck, "taskHiddenCheck");
			this.taskHiddenCheck.Name = "taskHiddenCheck";
			this.helpProvider.SetShowHelp(this.taskHiddenCheck, ((bool)(resources.GetObject("taskHiddenCheck.ShowHelp"))));
			this.taskHiddenCheck.UseVisualStyleBackColor = true;
			this.taskHiddenCheck.CheckedChanged += new System.EventHandler(this.taskHiddenCheck_CheckedChanged);
			// 
			// taskSecurityGroupBox
			// 
			resources.ApplyResources(this.taskSecurityGroupBox, "taskSecurityGroupBox");
			this.taskSecurityGroupBox.Controls.Add(this.taskRunLevelCheck);
			this.taskSecurityGroupBox.Controls.Add(this.taskLocalOnlyCheck);
			this.taskSecurityGroupBox.Controls.Add(this.taskLoggedOptionalRadio);
			this.taskSecurityGroupBox.Controls.Add(this.taskLoggedOnRadio);
			this.taskSecurityGroupBox.Controls.Add(this.taskPrincipalText);
			this.taskSecurityGroupBox.Controls.Add(this.changePrincipalButton);
			this.taskSecurityGroupBox.Controls.Add(this.taskUserAcctLabel);
			this.taskSecurityGroupBox.Name = "taskSecurityGroupBox";
			this.helpProvider.SetShowHelp(this.taskSecurityGroupBox, ((bool)(resources.GetObject("taskSecurityGroupBox.ShowHelp"))));
			this.taskSecurityGroupBox.TabStop = false;
			// 
			// taskRunLevelCheck
			// 
			resources.ApplyResources(this.taskRunLevelCheck, "taskRunLevelCheck");
			this.taskRunLevelCheck.Name = "taskRunLevelCheck";
			this.helpProvider.SetShowHelp(this.taskRunLevelCheck, ((bool)(resources.GetObject("taskRunLevelCheck.ShowHelp"))));
			this.taskRunLevelCheck.UseVisualStyleBackColor = true;
			this.taskRunLevelCheck.CheckedChanged += new System.EventHandler(this.taskRunLevelCheck_CheckedChanged);
			// 
			// taskLocalOnlyCheck
			// 
			resources.ApplyResources(this.taskLocalOnlyCheck, "taskLocalOnlyCheck");
			this.taskLocalOnlyCheck.Name = "taskLocalOnlyCheck";
			this.helpProvider.SetShowHelp(this.taskLocalOnlyCheck, ((bool)(resources.GetObject("taskLocalOnlyCheck.ShowHelp"))));
			this.taskLocalOnlyCheck.UseVisualStyleBackColor = true;
			this.taskLocalOnlyCheck.CheckedChanged += new System.EventHandler(this.taskLocalOnlyCheck_CheckedChanged);
			// 
			// taskLoggedOptionalRadio
			// 
			resources.ApplyResources(this.taskLoggedOptionalRadio, "taskLoggedOptionalRadio");
			this.taskLoggedOptionalRadio.Name = "taskLoggedOptionalRadio";
			this.helpProvider.SetShowHelp(this.taskLoggedOptionalRadio, ((bool)(resources.GetObject("taskLoggedOptionalRadio.ShowHelp"))));
			this.taskLoggedOptionalRadio.TabStop = true;
			this.taskLoggedOptionalRadio.UseVisualStyleBackColor = true;
			this.taskLoggedOptionalRadio.CheckedChanged += new System.EventHandler(this.taskLoggedOptionalRadio_CheckedChanged);
			// 
			// taskLoggedOnRadio
			// 
			resources.ApplyResources(this.taskLoggedOnRadio, "taskLoggedOnRadio");
			this.taskLoggedOnRadio.Name = "taskLoggedOnRadio";
			this.helpProvider.SetShowHelp(this.taskLoggedOnRadio, ((bool)(resources.GetObject("taskLoggedOnRadio.ShowHelp"))));
			this.taskLoggedOnRadio.TabStop = true;
			this.taskLoggedOnRadio.UseVisualStyleBackColor = true;
			this.taskLoggedOnRadio.CheckedChanged += new System.EventHandler(this.taskLoggedOnRadio_CheckedChanged);
			// 
			// taskPrincipalText
			// 
			resources.ApplyResources(this.taskPrincipalText, "taskPrincipalText");
			this.taskPrincipalText.Name = "taskPrincipalText";
			this.taskPrincipalText.ReadOnly = true;
			this.helpProvider.SetShowHelp(this.taskPrincipalText, ((bool)(resources.GetObject("taskPrincipalText.ShowHelp"))));
			// 
			// changePrincipalButton
			// 
			resources.ApplyResources(this.changePrincipalButton, "changePrincipalButton");
			this.changePrincipalButton.Name = "changePrincipalButton";
			this.helpProvider.SetShowHelp(this.changePrincipalButton, ((bool)(resources.GetObject("changePrincipalButton.ShowHelp"))));
			this.changePrincipalButton.UseVisualStyleBackColor = true;
			this.changePrincipalButton.Click += new System.EventHandler(this.changePrincipalButton_Click);
			// 
			// taskUserAcctLabel
			// 
			resources.ApplyResources(this.taskUserAcctLabel, "taskUserAcctLabel");
			this.taskUserAcctLabel.Name = "taskUserAcctLabel";
			this.helpProvider.SetShowHelp(this.taskUserAcctLabel, ((bool)(resources.GetObject("taskUserAcctLabel.ShowHelp"))));
			// 
			// triggersTab
			// 
			this.triggersTab.Controls.Add(this.triggerDeleteButton);
			this.triggersTab.Controls.Add(this.triggerEditButton);
			this.triggersTab.Controls.Add(this.triggerNewButton);
			this.triggersTab.Controls.Add(this.triggerListView);
			this.triggersTab.Controls.Add(this.taskTriggerIntroLabel);
			this.helpProvider.SetHelpKeyword(this.triggersTab, resources.GetString("triggersTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.triggersTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("triggersTab.HelpNavigator"))));
			resources.ApplyResources(this.triggersTab, "triggersTab");
			this.triggersTab.Name = "triggersTab";
			this.helpProvider.SetShowHelp(this.triggersTab, ((bool)(resources.GetObject("triggersTab.ShowHelp"))));
			this.triggersTab.UseVisualStyleBackColor = true;
			// 
			// triggerDeleteButton
			// 
			resources.ApplyResources(this.triggerDeleteButton, "triggerDeleteButton");
			this.triggerDeleteButton.Name = "triggerDeleteButton";
			this.helpProvider.SetShowHelp(this.triggerDeleteButton, ((bool)(resources.GetObject("triggerDeleteButton.ShowHelp"))));
			this.triggerDeleteButton.UseVisualStyleBackColor = true;
			this.triggerDeleteButton.Click += new System.EventHandler(this.triggerDeleteButton_Click);
			// 
			// triggerEditButton
			// 
			resources.ApplyResources(this.triggerEditButton, "triggerEditButton");
			this.triggerEditButton.Name = "triggerEditButton";
			this.helpProvider.SetShowHelp(this.triggerEditButton, ((bool)(resources.GetObject("triggerEditButton.ShowHelp"))));
			this.triggerEditButton.UseVisualStyleBackColor = true;
			this.triggerEditButton.Click += new System.EventHandler(this.triggerEditButton_Click);
			// 
			// triggerNewButton
			// 
			resources.ApplyResources(this.triggerNewButton, "triggerNewButton");
			this.triggerNewButton.Name = "triggerNewButton";
			this.helpProvider.SetShowHelp(this.triggerNewButton, ((bool)(resources.GetObject("triggerNewButton.ShowHelp"))));
			this.triggerNewButton.UseVisualStyleBackColor = true;
			this.triggerNewButton.Click += new System.EventHandler(this.triggerNewButton_Click);
			// 
			// triggerListView
			// 
			resources.ApplyResources(this.triggerListView, "triggerListView");
			this.triggerListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2,
            this.columnHeader3});
			this.triggerListView.FullRowSelect = true;
			this.triggerListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.triggerListView.HideSelection = false;
			this.triggerListView.Name = "triggerListView";
			this.helpProvider.SetShowHelp(this.triggerListView, ((bool)(resources.GetObject("triggerListView.ShowHelp"))));
			this.triggerListView.UseCompatibleStateImageBehavior = false;
			this.triggerListView.View = System.Windows.Forms.View.Details;
			this.triggerListView.SelectedIndexChanged += new System.EventHandler(this.triggerListView_SelectedIndexChanged);
			this.triggerListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.triggerListView_MouseDoubleClick);
			// 
			// columnHeader1
			// 
			resources.ApplyResources(this.columnHeader1, "columnHeader1");
			// 
			// columnHeader2
			// 
			resources.ApplyResources(this.columnHeader2, "columnHeader2");
			// 
			// columnHeader3
			// 
			resources.ApplyResources(this.columnHeader3, "columnHeader3");
			// 
			// taskTriggerIntroLabel
			// 
			resources.ApplyResources(this.taskTriggerIntroLabel, "taskTriggerIntroLabel");
			this.taskTriggerIntroLabel.Name = "taskTriggerIntroLabel";
			this.helpProvider.SetShowHelp(this.taskTriggerIntroLabel, ((bool)(resources.GetObject("taskTriggerIntroLabel.ShowHelp"))));
			// 
			// actionsTab
			// 
			this.actionsTab.Controls.Add(this.actionDownButton);
			this.actionsTab.Controls.Add(this.actionUpButton);
			this.actionsTab.Controls.Add(this.actionDeleteButton);
			this.actionsTab.Controls.Add(this.actionEditButton);
			this.actionsTab.Controls.Add(this.actionNewButton);
			this.actionsTab.Controls.Add(this.actionListView);
			this.actionsTab.Controls.Add(this.actionIntroLabel);
			this.helpProvider.SetHelpKeyword(this.actionsTab, resources.GetString("actionsTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.actionsTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("actionsTab.HelpNavigator"))));
			resources.ApplyResources(this.actionsTab, "actionsTab");
			this.actionsTab.Name = "actionsTab";
			this.helpProvider.SetShowHelp(this.actionsTab, ((bool)(resources.GetObject("actionsTab.ShowHelp"))));
			this.actionsTab.UseVisualStyleBackColor = true;
			// 
			// actionDownButton
			// 
			resources.ApplyResources(this.actionDownButton, "actionDownButton");
			this.actionDownButton.Name = "actionDownButton";
			this.helpProvider.SetShowHelp(this.actionDownButton, ((bool)(resources.GetObject("actionDownButton.ShowHelp"))));
			this.actionDownButton.UseVisualStyleBackColor = true;
			this.actionDownButton.Click += new System.EventHandler(this.actionDownButton_Click);
			// 
			// actionUpButton
			// 
			resources.ApplyResources(this.actionUpButton, "actionUpButton");
			this.actionUpButton.Name = "actionUpButton";
			this.helpProvider.SetShowHelp(this.actionUpButton, ((bool)(resources.GetObject("actionUpButton.ShowHelp"))));
			this.actionUpButton.UseVisualStyleBackColor = true;
			this.actionUpButton.Click += new System.EventHandler(this.actionUpButton_Click);
			// 
			// actionDeleteButton
			// 
			resources.ApplyResources(this.actionDeleteButton, "actionDeleteButton");
			this.actionDeleteButton.Name = "actionDeleteButton";
			this.helpProvider.SetShowHelp(this.actionDeleteButton, ((bool)(resources.GetObject("actionDeleteButton.ShowHelp"))));
			this.actionDeleteButton.UseVisualStyleBackColor = true;
			this.actionDeleteButton.Click += new System.EventHandler(this.actionDeleteButton_Click);
			// 
			// actionEditButton
			// 
			resources.ApplyResources(this.actionEditButton, "actionEditButton");
			this.actionEditButton.Name = "actionEditButton";
			this.helpProvider.SetShowHelp(this.actionEditButton, ((bool)(resources.GetObject("actionEditButton.ShowHelp"))));
			this.actionEditButton.UseVisualStyleBackColor = true;
			this.actionEditButton.Click += new System.EventHandler(this.actionEditButton_Click);
			// 
			// actionNewButton
			// 
			resources.ApplyResources(this.actionNewButton, "actionNewButton");
			this.actionNewButton.Name = "actionNewButton";
			this.helpProvider.SetShowHelp(this.actionNewButton, ((bool)(resources.GetObject("actionNewButton.ShowHelp"))));
			this.actionNewButton.UseVisualStyleBackColor = true;
			this.actionNewButton.Click += new System.EventHandler(this.actionNewButton_Click);
			// 
			// actionListView
			// 
			resources.ApplyResources(this.actionListView, "actionListView");
			this.actionListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader4,
            this.columnHeader5});
			this.actionListView.FullRowSelect = true;
			this.actionListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.actionListView.HideSelection = false;
			this.actionListView.Name = "actionListView";
			this.helpProvider.SetShowHelp(this.actionListView, ((bool)(resources.GetObject("actionListView.ShowHelp"))));
			this.actionListView.UseCompatibleStateImageBehavior = false;
			this.actionListView.View = System.Windows.Forms.View.Details;
			this.actionListView.SelectedIndexChanged += new System.EventHandler(this.actionListView_SelectedIndexChanged);
			this.actionListView.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.actionListView_MouseDoubleClick);
			// 
			// columnHeader4
			// 
			resources.ApplyResources(this.columnHeader4, "columnHeader4");
			// 
			// columnHeader5
			// 
			resources.ApplyResources(this.columnHeader5, "columnHeader5");
			// 
			// actionIntroLabel
			// 
			resources.ApplyResources(this.actionIntroLabel, "actionIntroLabel");
			this.actionIntroLabel.Name = "actionIntroLabel";
			this.helpProvider.SetShowHelp(this.actionIntroLabel, ((bool)(resources.GetObject("actionIntroLabel.ShowHelp"))));
			// 
			// conditionsTab
			// 
			this.conditionsTab.Controls.Add(this.networkConditionGroupBox);
			this.conditionsTab.Controls.Add(this.powerConditionGroupBox);
			this.conditionsTab.Controls.Add(this.idleConditionGroupBox);
			this.conditionsTab.Controls.Add(this.conditionIntroLabel);
			this.helpProvider.SetHelpKeyword(this.conditionsTab, resources.GetString("conditionsTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.conditionsTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("conditionsTab.HelpNavigator"))));
			resources.ApplyResources(this.conditionsTab, "conditionsTab");
			this.conditionsTab.Name = "conditionsTab";
			this.helpProvider.SetShowHelp(this.conditionsTab, ((bool)(resources.GetObject("conditionsTab.ShowHelp"))));
			this.conditionsTab.UseVisualStyleBackColor = true;
			this.conditionsTab.Enter += new System.EventHandler(this.conditionsTab_Enter);
			// 
			// networkConditionGroupBox
			// 
			resources.ApplyResources(this.networkConditionGroupBox, "networkConditionGroupBox");
			this.networkConditionGroupBox.Controls.Add(this.availableConnectionsCombo);
			this.networkConditionGroupBox.Controls.Add(this.taskStartIfConnectionCheck);
			this.networkConditionGroupBox.Name = "networkConditionGroupBox";
			this.helpProvider.SetShowHelp(this.networkConditionGroupBox, ((bool)(resources.GetObject("networkConditionGroupBox.ShowHelp"))));
			this.networkConditionGroupBox.TabStop = false;
			// 
			// availableConnectionsCombo
			// 
			resources.ApplyResources(this.availableConnectionsCombo, "availableConnectionsCombo");
			this.availableConnectionsCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.availableConnectionsCombo.FormattingEnabled = true;
			this.availableConnectionsCombo.Name = "availableConnectionsCombo";
			this.helpProvider.SetShowHelp(this.availableConnectionsCombo, ((bool)(resources.GetObject("availableConnectionsCombo.ShowHelp"))));
			this.availableConnectionsCombo.SelectedIndexChanged += new System.EventHandler(this.availableConnectionsCombo_SelectedIndexChanged);
			// 
			// taskStartIfConnectionCheck
			// 
			resources.ApplyResources(this.taskStartIfConnectionCheck, "taskStartIfConnectionCheck");
			this.taskStartIfConnectionCheck.Name = "taskStartIfConnectionCheck";
			this.helpProvider.SetShowHelp(this.taskStartIfConnectionCheck, ((bool)(resources.GetObject("taskStartIfConnectionCheck.ShowHelp"))));
			this.taskStartIfConnectionCheck.UseVisualStyleBackColor = true;
			this.taskStartIfConnectionCheck.CheckedChanged += new System.EventHandler(this.taskStartIfConnectionCheck_CheckedChanged);
			// 
			// powerConditionGroupBox
			// 
			resources.ApplyResources(this.powerConditionGroupBox, "powerConditionGroupBox");
			this.powerConditionGroupBox.Controls.Add(this.taskStopIfGoingOnBatteriesCheck);
			this.powerConditionGroupBox.Controls.Add(this.taskWakeToRunCheck);
			this.powerConditionGroupBox.Controls.Add(this.taskDisallowStartIfOnBatteriesCheck);
			this.powerConditionGroupBox.Name = "powerConditionGroupBox";
			this.helpProvider.SetShowHelp(this.powerConditionGroupBox, ((bool)(resources.GetObject("powerConditionGroupBox.ShowHelp"))));
			this.powerConditionGroupBox.TabStop = false;
			// 
			// taskStopIfGoingOnBatteriesCheck
			// 
			resources.ApplyResources(this.taskStopIfGoingOnBatteriesCheck, "taskStopIfGoingOnBatteriesCheck");
			this.taskStopIfGoingOnBatteriesCheck.Name = "taskStopIfGoingOnBatteriesCheck";
			this.helpProvider.SetShowHelp(this.taskStopIfGoingOnBatteriesCheck, ((bool)(resources.GetObject("taskStopIfGoingOnBatteriesCheck.ShowHelp"))));
			this.taskStopIfGoingOnBatteriesCheck.UseVisualStyleBackColor = true;
			this.taskStopIfGoingOnBatteriesCheck.CheckedChanged += new System.EventHandler(this.taskStopIfGoingOnBatteriesCheck_CheckedChanged);
			// 
			// taskWakeToRunCheck
			// 
			resources.ApplyResources(this.taskWakeToRunCheck, "taskWakeToRunCheck");
			this.taskWakeToRunCheck.Name = "taskWakeToRunCheck";
			this.helpProvider.SetShowHelp(this.taskWakeToRunCheck, ((bool)(resources.GetObject("taskWakeToRunCheck.ShowHelp"))));
			this.taskWakeToRunCheck.UseVisualStyleBackColor = true;
			this.taskWakeToRunCheck.CheckedChanged += new System.EventHandler(this.taskWakeToRunCheck_CheckedChanged);
			// 
			// taskDisallowStartIfOnBatteriesCheck
			// 
			resources.ApplyResources(this.taskDisallowStartIfOnBatteriesCheck, "taskDisallowStartIfOnBatteriesCheck");
			this.taskDisallowStartIfOnBatteriesCheck.Name = "taskDisallowStartIfOnBatteriesCheck";
			this.helpProvider.SetShowHelp(this.taskDisallowStartIfOnBatteriesCheck, ((bool)(resources.GetObject("taskDisallowStartIfOnBatteriesCheck.ShowHelp"))));
			this.taskDisallowStartIfOnBatteriesCheck.UseVisualStyleBackColor = true;
			this.taskDisallowStartIfOnBatteriesCheck.CheckedChanged += new System.EventHandler(this.taskDisallowStartIfOnBatteriesCheck_CheckedChanged);
			// 
			// idleConditionGroupBox
			// 
			resources.ApplyResources(this.idleConditionGroupBox, "idleConditionGroupBox");
			this.idleConditionGroupBox.Controls.Add(this.taskIdleWaitTimeoutCombo);
			this.idleConditionGroupBox.Controls.Add(this.taskIdleDurationCombo);
			this.idleConditionGroupBox.Controls.Add(this.taskRestartOnIdleCheck);
			this.idleConditionGroupBox.Controls.Add(this.taskStopOnIdleEndCheck);
			this.idleConditionGroupBox.Controls.Add(this.taskIdleWaitTimeoutLabel);
			this.idleConditionGroupBox.Controls.Add(this.taskIdleDurationCheck);
			this.idleConditionGroupBox.Name = "idleConditionGroupBox";
			this.helpProvider.SetShowHelp(this.idleConditionGroupBox, ((bool)(resources.GetObject("idleConditionGroupBox.ShowHelp"))));
			this.idleConditionGroupBox.TabStop = false;
			// 
			// taskIdleWaitTimeoutCombo
			// 
			resources.ApplyResources(this.taskIdleWaitTimeoutCombo, "taskIdleWaitTimeoutCombo");
			this.taskIdleWaitTimeoutCombo.Name = "taskIdleWaitTimeoutCombo";
			this.helpProvider.SetShowHelp(this.taskIdleWaitTimeoutCombo, ((bool)(resources.GetObject("taskIdleWaitTimeoutCombo.ShowHelp"))));
			this.taskIdleWaitTimeoutCombo.ValueChanged += new System.EventHandler(this.taskIdleWaitTimeoutCombo_ValueChanged);
			// 
			// taskIdleDurationCombo
			// 
			resources.ApplyResources(this.taskIdleDurationCombo, "taskIdleDurationCombo");
			this.taskIdleDurationCombo.Name = "taskIdleDurationCombo";
			this.helpProvider.SetShowHelp(this.taskIdleDurationCombo, ((bool)(resources.GetObject("taskIdleDurationCombo.ShowHelp"))));
			this.taskIdleDurationCombo.ValueChanged += new System.EventHandler(this.taskIdleDurationCombo_ValueChanged);
			// 
			// taskRestartOnIdleCheck
			// 
			resources.ApplyResources(this.taskRestartOnIdleCheck, "taskRestartOnIdleCheck");
			this.taskRestartOnIdleCheck.Name = "taskRestartOnIdleCheck";
			this.helpProvider.SetShowHelp(this.taskRestartOnIdleCheck, ((bool)(resources.GetObject("taskRestartOnIdleCheck.ShowHelp"))));
			this.taskRestartOnIdleCheck.UseVisualStyleBackColor = true;
			this.taskRestartOnIdleCheck.CheckedChanged += new System.EventHandler(this.taskRestartOnIdleCheck_CheckedChanged);
			// 
			// taskStopOnIdleEndCheck
			// 
			resources.ApplyResources(this.taskStopOnIdleEndCheck, "taskStopOnIdleEndCheck");
			this.taskStopOnIdleEndCheck.Checked = true;
			this.taskStopOnIdleEndCheck.CheckState = System.Windows.Forms.CheckState.Checked;
			this.taskStopOnIdleEndCheck.Name = "taskStopOnIdleEndCheck";
			this.helpProvider.SetShowHelp(this.taskStopOnIdleEndCheck, ((bool)(resources.GetObject("taskStopOnIdleEndCheck.ShowHelp"))));
			this.taskStopOnIdleEndCheck.UseVisualStyleBackColor = true;
			this.taskStopOnIdleEndCheck.CheckedChanged += new System.EventHandler(this.taskStopOnIdleEndCheck_CheckedChanged);
			// 
			// taskIdleWaitTimeoutLabel
			// 
			resources.ApplyResources(this.taskIdleWaitTimeoutLabel, "taskIdleWaitTimeoutLabel");
			this.taskIdleWaitTimeoutLabel.Name = "taskIdleWaitTimeoutLabel";
			this.helpProvider.SetShowHelp(this.taskIdleWaitTimeoutLabel, ((bool)(resources.GetObject("taskIdleWaitTimeoutLabel.ShowHelp"))));
			// 
			// taskIdleDurationCheck
			// 
			resources.ApplyResources(this.taskIdleDurationCheck, "taskIdleDurationCheck");
			this.taskIdleDurationCheck.Name = "taskIdleDurationCheck";
			this.helpProvider.SetShowHelp(this.taskIdleDurationCheck, ((bool)(resources.GetObject("taskIdleDurationCheck.ShowHelp"))));
			this.taskIdleDurationCheck.UseVisualStyleBackColor = true;
			this.taskIdleDurationCheck.CheckedChanged += new System.EventHandler(this.taskIdleDurationCheck_CheckedChanged);
			// 
			// conditionIntroLabel
			// 
			resources.ApplyResources(this.conditionIntroLabel, "conditionIntroLabel");
			this.conditionIntroLabel.Name = "conditionIntroLabel";
			this.helpProvider.SetShowHelp(this.conditionIntroLabel, ((bool)(resources.GetObject("conditionIntroLabel.ShowHelp"))));
			// 
			// settingsTab
			// 
			this.settingsTab.Controls.Add(this.taskRestartCountText);
			this.settingsTab.Controls.Add(this.taskMultInstCombo);
			this.settingsTab.Controls.Add(this.taskRunningRuleLabel);
			this.settingsTab.Controls.Add(this.taskRestartAttemptTimesLabel);
			this.settingsTab.Controls.Add(this.taskRestartCountLabel);
			this.settingsTab.Controls.Add(this.taskDeleteAfterCheck);
			this.settingsTab.Controls.Add(this.taskAllowHardTerminateCheck);
			this.settingsTab.Controls.Add(this.taskExecutionTimeLimitCheck);
			this.settingsTab.Controls.Add(this.taskRestartIntervalCheck);
			this.settingsTab.Controls.Add(this.taskStartWhenAvailableCheck);
			this.settingsTab.Controls.Add(this.taskAllowDemandStartCheck);
			this.settingsTab.Controls.Add(this.settingsIntroLabel);
			this.settingsTab.Controls.Add(this.taskDeleteAfterCombo);
			this.settingsTab.Controls.Add(this.taskExecutionTimeLimitCombo);
			this.settingsTab.Controls.Add(this.taskRestartIntervalCombo);
			this.helpProvider.SetHelpKeyword(this.settingsTab, resources.GetString("settingsTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.settingsTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("settingsTab.HelpNavigator"))));
			resources.ApplyResources(this.settingsTab, "settingsTab");
			this.settingsTab.Name = "settingsTab";
			this.helpProvider.SetShowHelp(this.settingsTab, ((bool)(resources.GetObject("settingsTab.ShowHelp"))));
			this.settingsTab.UseVisualStyleBackColor = true;
			// 
			// taskRestartCountText
			// 
			resources.ApplyResources(this.taskRestartCountText, "taskRestartCountText");
			this.taskRestartCountText.Name = "taskRestartCountText";
			this.helpProvider.SetShowHelp(this.taskRestartCountText, ((bool)(resources.GetObject("taskRestartCountText.ShowHelp"))));
			this.taskRestartCountText.ValueChanged += new System.EventHandler(this.taskRestartCountText_ValueChanged);
			// 
			// taskMultInstCombo
			// 
			this.taskMultInstCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.taskMultInstCombo.FormattingEnabled = true;
			resources.ApplyResources(this.taskMultInstCombo, "taskMultInstCombo");
			this.taskMultInstCombo.Name = "taskMultInstCombo";
			this.helpProvider.SetShowHelp(this.taskMultInstCombo, ((bool)(resources.GetObject("taskMultInstCombo.ShowHelp"))));
			this.taskMultInstCombo.SelectedIndexChanged += new System.EventHandler(this.taskMultInstCombo_SelectedIndexChanged);
			// 
			// taskRunningRuleLabel
			// 
			resources.ApplyResources(this.taskRunningRuleLabel, "taskRunningRuleLabel");
			this.taskRunningRuleLabel.Name = "taskRunningRuleLabel";
			this.helpProvider.SetShowHelp(this.taskRunningRuleLabel, ((bool)(resources.GetObject("taskRunningRuleLabel.ShowHelp"))));
			// 
			// taskRestartAttemptTimesLabel
			// 
			resources.ApplyResources(this.taskRestartAttemptTimesLabel, "taskRestartAttemptTimesLabel");
			this.taskRestartAttemptTimesLabel.Name = "taskRestartAttemptTimesLabel";
			this.helpProvider.SetShowHelp(this.taskRestartAttemptTimesLabel, ((bool)(resources.GetObject("taskRestartAttemptTimesLabel.ShowHelp"))));
			// 
			// taskRestartCountLabel
			// 
			resources.ApplyResources(this.taskRestartCountLabel, "taskRestartCountLabel");
			this.taskRestartCountLabel.Name = "taskRestartCountLabel";
			this.helpProvider.SetShowHelp(this.taskRestartCountLabel, ((bool)(resources.GetObject("taskRestartCountLabel.ShowHelp"))));
			// 
			// taskDeleteAfterCheck
			// 
			resources.ApplyResources(this.taskDeleteAfterCheck, "taskDeleteAfterCheck");
			this.taskDeleteAfterCheck.Name = "taskDeleteAfterCheck";
			this.helpProvider.SetShowHelp(this.taskDeleteAfterCheck, ((bool)(resources.GetObject("taskDeleteAfterCheck.ShowHelp"))));
			this.taskDeleteAfterCheck.UseVisualStyleBackColor = true;
			this.taskDeleteAfterCheck.CheckedChanged += new System.EventHandler(this.taskDeleteAfterCheck_CheckedChanged);
			// 
			// taskAllowHardTerminateCheck
			// 
			resources.ApplyResources(this.taskAllowHardTerminateCheck, "taskAllowHardTerminateCheck");
			this.taskAllowHardTerminateCheck.Name = "taskAllowHardTerminateCheck";
			this.helpProvider.SetShowHelp(this.taskAllowHardTerminateCheck, ((bool)(resources.GetObject("taskAllowHardTerminateCheck.ShowHelp"))));
			this.taskAllowHardTerminateCheck.UseVisualStyleBackColor = true;
			this.taskAllowHardTerminateCheck.CheckedChanged += new System.EventHandler(this.taskAllowHardTerminateCheck_CheckedChanged);
			// 
			// taskExecutionTimeLimitCheck
			// 
			resources.ApplyResources(this.taskExecutionTimeLimitCheck, "taskExecutionTimeLimitCheck");
			this.taskExecutionTimeLimitCheck.Name = "taskExecutionTimeLimitCheck";
			this.helpProvider.SetShowHelp(this.taskExecutionTimeLimitCheck, ((bool)(resources.GetObject("taskExecutionTimeLimitCheck.ShowHelp"))));
			this.taskExecutionTimeLimitCheck.UseVisualStyleBackColor = true;
			this.taskExecutionTimeLimitCheck.CheckedChanged += new System.EventHandler(this.taskExecutionTimeLimitCheck_CheckedChanged);
			// 
			// taskRestartIntervalCheck
			// 
			resources.ApplyResources(this.taskRestartIntervalCheck, "taskRestartIntervalCheck");
			this.taskRestartIntervalCheck.Name = "taskRestartIntervalCheck";
			this.helpProvider.SetShowHelp(this.taskRestartIntervalCheck, ((bool)(resources.GetObject("taskRestartIntervalCheck.ShowHelp"))));
			this.taskRestartIntervalCheck.UseVisualStyleBackColor = true;
			this.taskRestartIntervalCheck.CheckedChanged += new System.EventHandler(this.taskRestartIntervalCheck_CheckedChanged);
			// 
			// taskStartWhenAvailableCheck
			// 
			resources.ApplyResources(this.taskStartWhenAvailableCheck, "taskStartWhenAvailableCheck");
			this.taskStartWhenAvailableCheck.Name = "taskStartWhenAvailableCheck";
			this.helpProvider.SetShowHelp(this.taskStartWhenAvailableCheck, ((bool)(resources.GetObject("taskStartWhenAvailableCheck.ShowHelp"))));
			this.taskStartWhenAvailableCheck.UseVisualStyleBackColor = true;
			this.taskStartWhenAvailableCheck.CheckedChanged += new System.EventHandler(this.taskStartWhenAvailableCheck_CheckedChanged);
			// 
			// taskAllowDemandStartCheck
			// 
			resources.ApplyResources(this.taskAllowDemandStartCheck, "taskAllowDemandStartCheck");
			this.taskAllowDemandStartCheck.Name = "taskAllowDemandStartCheck";
			this.helpProvider.SetShowHelp(this.taskAllowDemandStartCheck, ((bool)(resources.GetObject("taskAllowDemandStartCheck.ShowHelp"))));
			this.taskAllowDemandStartCheck.UseVisualStyleBackColor = true;
			this.taskAllowDemandStartCheck.CheckedChanged += new System.EventHandler(this.taskAllowDemandStartCheck_CheckedChanged);
			// 
			// settingsIntroLabel
			// 
			resources.ApplyResources(this.settingsIntroLabel, "settingsIntroLabel");
			this.settingsIntroLabel.Name = "settingsIntroLabel";
			this.helpProvider.SetShowHelp(this.settingsIntroLabel, ((bool)(resources.GetObject("settingsIntroLabel.ShowHelp"))));
			// 
			// taskDeleteAfterCombo
			// 
			resources.ApplyResources(this.taskDeleteAfterCombo, "taskDeleteAfterCombo");
			this.taskDeleteAfterCombo.Name = "taskDeleteAfterCombo";
			this.helpProvider.SetShowHelp(this.taskDeleteAfterCombo, ((bool)(resources.GetObject("taskDeleteAfterCombo.ShowHelp"))));
			this.taskDeleteAfterCombo.ValueChanged += new System.EventHandler(this.taskDeleteAfterCombo_ValueChanged);
			// 
			// taskExecutionTimeLimitCombo
			// 
			resources.ApplyResources(this.taskExecutionTimeLimitCombo, "taskExecutionTimeLimitCombo");
			this.taskExecutionTimeLimitCombo.Name = "taskExecutionTimeLimitCombo";
			this.helpProvider.SetShowHelp(this.taskExecutionTimeLimitCombo, ((bool)(resources.GetObject("taskExecutionTimeLimitCombo.ShowHelp"))));
			this.taskExecutionTimeLimitCombo.ValueChanged += new System.EventHandler(this.taskExecutionTimeLimitCombo_ValueChanged);
			// 
			// taskRestartIntervalCombo
			// 
			resources.ApplyResources(this.taskRestartIntervalCombo, "taskRestartIntervalCombo");
			this.taskRestartIntervalCombo.Name = "taskRestartIntervalCombo";
			this.helpProvider.SetShowHelp(this.taskRestartIntervalCombo, ((bool)(resources.GetObject("taskRestartIntervalCombo.ShowHelp"))));
			this.taskRestartIntervalCombo.ValueChanged += new System.EventHandler(this.taskRestartIntervalCombo_ValueChanged);
			// 
			// regInfoTab
			// 
			this.regInfoTab.Controls.Add(this.label5);
			this.regInfoTab.Controls.Add(this.taskRegSDDLLabel);
			this.regInfoTab.Controls.Add(this.taskRegVersionLabel);
			this.regInfoTab.Controls.Add(this.taskRegURILabel);
			this.regInfoTab.Controls.Add(this.taskRegSourceLabel);
			this.regInfoTab.Controls.Add(this.taskRegDocLabel);
			this.regInfoTab.Controls.Add(this.taskRegSDDLText);
			this.regInfoTab.Controls.Add(this.taskRegVersionText);
			this.regInfoTab.Controls.Add(this.taskRegURIText);
			this.regInfoTab.Controls.Add(this.taskRegSourceText);
			this.regInfoTab.Controls.Add(this.taskRegDocText);
			this.regInfoTab.Controls.Add(this.taskRegSDDLBtn);
			resources.ApplyResources(this.regInfoTab, "regInfoTab");
			this.regInfoTab.Name = "regInfoTab";
			this.helpProvider.SetShowHelp(this.regInfoTab, ((bool)(resources.GetObject("regInfoTab.ShowHelp"))));
			this.regInfoTab.UseVisualStyleBackColor = true;
			// 
			// label5
			// 
			resources.ApplyResources(this.label5, "label5");
			this.label5.Name = "label5";
			this.helpProvider.SetShowHelp(this.label5, ((bool)(resources.GetObject("label5.ShowHelp"))));
			// 
			// taskRegSDDLLabel
			// 
			resources.ApplyResources(this.taskRegSDDLLabel, "taskRegSDDLLabel");
			this.taskRegSDDLLabel.Name = "taskRegSDDLLabel";
			this.helpProvider.SetShowHelp(this.taskRegSDDLLabel, ((bool)(resources.GetObject("taskRegSDDLLabel.ShowHelp"))));
			// 
			// taskRegVersionLabel
			// 
			resources.ApplyResources(this.taskRegVersionLabel, "taskRegVersionLabel");
			this.taskRegVersionLabel.Name = "taskRegVersionLabel";
			this.helpProvider.SetShowHelp(this.taskRegVersionLabel, ((bool)(resources.GetObject("taskRegVersionLabel.ShowHelp"))));
			// 
			// taskRegURILabel
			// 
			resources.ApplyResources(this.taskRegURILabel, "taskRegURILabel");
			this.taskRegURILabel.Name = "taskRegURILabel";
			this.helpProvider.SetShowHelp(this.taskRegURILabel, ((bool)(resources.GetObject("taskRegURILabel.ShowHelp"))));
			// 
			// taskRegSourceLabel
			// 
			resources.ApplyResources(this.taskRegSourceLabel, "taskRegSourceLabel");
			this.taskRegSourceLabel.Name = "taskRegSourceLabel";
			this.helpProvider.SetShowHelp(this.taskRegSourceLabel, ((bool)(resources.GetObject("taskRegSourceLabel.ShowHelp"))));
			// 
			// taskRegDocLabel
			// 
			resources.ApplyResources(this.taskRegDocLabel, "taskRegDocLabel");
			this.taskRegDocLabel.Name = "taskRegDocLabel";
			this.helpProvider.SetShowHelp(this.taskRegDocLabel, ((bool)(resources.GetObject("taskRegDocLabel.ShowHelp"))));
			// 
			// taskRegSDDLText
			// 
			resources.ApplyResources(this.taskRegSDDLText, "taskRegSDDLText");
			this.errorProvider.SetIconPadding(this.taskRegSDDLText, ((int)(resources.GetObject("taskRegSDDLText.IconPadding"))));
			this.taskRegSDDLText.Name = "taskRegSDDLText";
			this.helpProvider.SetShowHelp(this.taskRegSDDLText, ((bool)(resources.GetObject("taskRegSDDLText.ShowHelp"))));
			this.taskRegSDDLText.Validating += new System.ComponentModel.CancelEventHandler(this.taskRegSDDLText_Validating);
			this.taskRegSDDLText.Validated += new System.EventHandler(this.taskRegSDDLText_Validated);
			// 
			// taskRegVersionText
			// 
			resources.ApplyResources(this.taskRegVersionText, "taskRegVersionText");
			this.errorProvider.SetIconPadding(this.taskRegVersionText, ((int)(resources.GetObject("taskRegVersionText.IconPadding"))));
			this.taskRegVersionText.Name = "taskRegVersionText";
			this.helpProvider.SetShowHelp(this.taskRegVersionText, ((bool)(resources.GetObject("taskRegVersionText.ShowHelp"))));
			this.taskRegVersionText.Validating += new System.ComponentModel.CancelEventHandler(this.taskRegVersionText_Validating);
			this.taskRegVersionText.Validated += new System.EventHandler(this.taskRegVersionText_Validated);
			// 
			// taskRegURIText
			// 
			resources.ApplyResources(this.taskRegURIText, "taskRegURIText");
			this.errorProvider.SetIconPadding(this.taskRegURIText, ((int)(resources.GetObject("taskRegURIText.IconPadding"))));
			this.taskRegURIText.Name = "taskRegURIText";
			this.helpProvider.SetShowHelp(this.taskRegURIText, ((bool)(resources.GetObject("taskRegURIText.ShowHelp"))));
			this.taskRegURIText.Validating += new System.ComponentModel.CancelEventHandler(this.taskRegURIText_Validating);
			this.taskRegURIText.Validated += new System.EventHandler(this.taskRegURIText_Validated);
			// 
			// taskRegSourceText
			// 
			resources.ApplyResources(this.taskRegSourceText, "taskRegSourceText");
			this.taskRegSourceText.Name = "taskRegSourceText";
			this.helpProvider.SetShowHelp(this.taskRegSourceText, ((bool)(resources.GetObject("taskRegSourceText.ShowHelp"))));
			this.taskRegSourceText.Leave += new System.EventHandler(this.taskRegSourceText_Leave);
			// 
			// taskRegDocText
			// 
			resources.ApplyResources(this.taskRegDocText, "taskRegDocText");
			this.taskRegDocText.Name = "taskRegDocText";
			this.helpProvider.SetShowHelp(this.taskRegDocText, ((bool)(resources.GetObject("taskRegDocText.ShowHelp"))));
			this.taskRegDocText.Leave += new System.EventHandler(this.taskRegDocText_Leave);
			// 
			// taskRegSDDLBtn
			// 
			resources.ApplyResources(this.taskRegSDDLBtn, "taskRegSDDLBtn");
			this.taskRegSDDLBtn.Name = "taskRegSDDLBtn";
			this.helpProvider.SetShowHelp(this.taskRegSDDLBtn, ((bool)(resources.GetObject("taskRegSDDLBtn.ShowHelp"))));
			this.taskRegSDDLBtn.UseVisualStyleBackColor = true;
			this.taskRegSDDLBtn.Click += new System.EventHandler(this.taskRegSDDLBtn_Click);
			// 
			// addPropTab
			// 
			this.addPropTab.Controls.Add(this.secHardGroup);
			this.addPropTab.Controls.Add(this.autoMaintGroup);
			this.addPropTab.Controls.Add(this.taskPriorityCombo);
			this.addPropTab.Controls.Add(this.label8);
			this.addPropTab.Controls.Add(this.taskVolatileCheck);
			this.addPropTab.Controls.Add(this.taskUseUnifiedSchedulingEngineCheck);
			this.addPropTab.Controls.Add(this.taskDisallowStartOnRemoteAppSessionCheck);
			this.addPropTab.Controls.Add(this.taskEnabledCheck);
			this.addPropTab.Controls.Add(this.label4);
			resources.ApplyResources(this.addPropTab, "addPropTab");
			this.addPropTab.Name = "addPropTab";
			this.helpProvider.SetShowHelp(this.addPropTab, ((bool)(resources.GetObject("addPropTab.ShowHelp"))));
			this.addPropTab.UseVisualStyleBackColor = true;
			// 
			// secHardGroup
			// 
			resources.ApplyResources(this.secHardGroup, "secHardGroup");
			this.secHardGroup.Controls.Add(this.principalSIDTypeLabel);
			this.secHardGroup.Controls.Add(this.principalSIDTypeCombo);
			this.secHardGroup.Controls.Add(this.principalReqPrivilegesLabel);
			this.secHardGroup.Controls.Add(this.principalReqPrivilegesDropDown);
			this.secHardGroup.Name = "secHardGroup";
			this.helpProvider.SetShowHelp(this.secHardGroup, ((bool)(resources.GetObject("secHardGroup.ShowHelp"))));
			this.secHardGroup.TabStop = false;
			// 
			// principalSIDTypeLabel
			// 
			resources.ApplyResources(this.principalSIDTypeLabel, "principalSIDTypeLabel");
			this.principalSIDTypeLabel.Name = "principalSIDTypeLabel";
			this.helpProvider.SetShowHelp(this.principalSIDTypeLabel, ((bool)(resources.GetObject("principalSIDTypeLabel.ShowHelp"))));
			// 
			// principalSIDTypeCombo
			// 
			this.principalSIDTypeCombo.DisplayMember = "Text";
			this.principalSIDTypeCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.principalSIDTypeCombo, "principalSIDTypeCombo");
			this.principalSIDTypeCombo.Name = "principalSIDTypeCombo";
			this.helpProvider.SetShowHelp(this.principalSIDTypeCombo, ((bool)(resources.GetObject("principalSIDTypeCombo.ShowHelp"))));
			this.principalSIDTypeCombo.ValueMember = "Value";
			this.principalSIDTypeCombo.SelectedIndexChanged += new System.EventHandler(this.principalSIDTypeCombo_SelectedIndexChanged);
			// 
			// principalReqPrivilegesLabel
			// 
			resources.ApplyResources(this.principalReqPrivilegesLabel, "principalReqPrivilegesLabel");
			this.principalReqPrivilegesLabel.Name = "principalReqPrivilegesLabel";
			this.helpProvider.SetShowHelp(this.principalReqPrivilegesLabel, ((bool)(resources.GetObject("principalReqPrivilegesLabel.ShowHelp"))));
			// 
			// principalReqPrivilegesDropDown
			// 
			this.principalReqPrivilegesDropDown.BackColor = System.Drawing.Color.White;
			this.principalReqPrivilegesDropDown.ControlSize = new System.Drawing.Size(187, 105);
			this.principalReqPrivilegesDropDown.DropSize = new System.Drawing.Size(121, 106);
			resources.ApplyResources(this.principalReqPrivilegesDropDown, "principalReqPrivilegesDropDown");
			this.principalReqPrivilegesDropDown.Name = "principalReqPrivilegesDropDown";
			this.helpProvider.SetShowHelp(this.principalReqPrivilegesDropDown, ((bool)(resources.GetObject("principalReqPrivilegesDropDown.ShowHelp"))));
			this.principalReqPrivilegesDropDown.SelectedIndexChanged += new System.EventHandler(this.principalReqPrivilegesDropDown_SelectedIndexChanged);
			// 
			// autoMaintGroup
			// 
			resources.ApplyResources(this.autoMaintGroup, "autoMaintGroup");
			this.autoMaintGroup.Controls.Add(this.taskMaintenanceDeadlineCombo);
			this.autoMaintGroup.Controls.Add(this.taskMaintenanceExclusiveCheck);
			this.autoMaintGroup.Controls.Add(this.taskMaintenanceDeadlineLabel);
			this.autoMaintGroup.Controls.Add(this.taskMaintenancePeriodLabel);
			this.autoMaintGroup.Controls.Add(this.taskMaintenancePeriodCombo);
			this.autoMaintGroup.Name = "autoMaintGroup";
			this.helpProvider.SetShowHelp(this.autoMaintGroup, ((bool)(resources.GetObject("autoMaintGroup.ShowHelp"))));
			this.autoMaintGroup.TabStop = false;
			// 
			// taskMaintenanceDeadlineCombo
			// 
			resources.ApplyResources(this.taskMaintenanceDeadlineCombo, "taskMaintenanceDeadlineCombo");
			this.taskMaintenanceDeadlineCombo.Name = "taskMaintenanceDeadlineCombo";
			this.helpProvider.SetShowHelp(this.taskMaintenanceDeadlineCombo, ((bool)(resources.GetObject("taskMaintenanceDeadlineCombo.ShowHelp"))));
			this.taskMaintenanceDeadlineCombo.ValueChanged += new System.EventHandler(this.taskMaintenanceDeadlineCombo_ValueChanged);
			// 
			// taskMaintenanceExclusiveCheck
			// 
			resources.ApplyResources(this.taskMaintenanceExclusiveCheck, "taskMaintenanceExclusiveCheck");
			this.taskMaintenanceExclusiveCheck.Name = "taskMaintenanceExclusiveCheck";
			this.helpProvider.SetShowHelp(this.taskMaintenanceExclusiveCheck, ((bool)(resources.GetObject("taskMaintenanceExclusiveCheck.ShowHelp"))));
			this.taskMaintenanceExclusiveCheck.UseVisualStyleBackColor = true;
			this.taskMaintenanceExclusiveCheck.CheckedChanged += new System.EventHandler(this.taskMaintenanceExclusiveCheck_CheckedChanged);
			// 
			// taskMaintenanceDeadlineLabel
			// 
			resources.ApplyResources(this.taskMaintenanceDeadlineLabel, "taskMaintenanceDeadlineLabel");
			this.taskMaintenanceDeadlineLabel.Name = "taskMaintenanceDeadlineLabel";
			this.helpProvider.SetShowHelp(this.taskMaintenanceDeadlineLabel, ((bool)(resources.GetObject("taskMaintenanceDeadlineLabel.ShowHelp"))));
			// 
			// taskMaintenancePeriodLabel
			// 
			resources.ApplyResources(this.taskMaintenancePeriodLabel, "taskMaintenancePeriodLabel");
			this.taskMaintenancePeriodLabel.Name = "taskMaintenancePeriodLabel";
			this.helpProvider.SetShowHelp(this.taskMaintenancePeriodLabel, ((bool)(resources.GetObject("taskMaintenancePeriodLabel.ShowHelp"))));
			// 
			// taskMaintenancePeriodCombo
			// 
			resources.ApplyResources(this.taskMaintenancePeriodCombo, "taskMaintenancePeriodCombo");
			this.taskMaintenancePeriodCombo.Name = "taskMaintenancePeriodCombo";
			this.helpProvider.SetShowHelp(this.taskMaintenancePeriodCombo, ((bool)(resources.GetObject("taskMaintenancePeriodCombo.ShowHelp"))));
			this.taskMaintenancePeriodCombo.ValueChanged += new System.EventHandler(this.taskMaintenancePeriodCombo_ValueChanged);
			// 
			// taskPriorityCombo
			// 
			this.taskPriorityCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			resources.ApplyResources(this.taskPriorityCombo, "taskPriorityCombo");
			this.taskPriorityCombo.Name = "taskPriorityCombo";
			this.helpProvider.SetShowHelp(this.taskPriorityCombo, ((bool)(resources.GetObject("taskPriorityCombo.ShowHelp"))));
			this.taskPriorityCombo.SelectedIndexChanged += new System.EventHandler(this.taskPriorityCombo_SelectedIndexChanged);
			// 
			// label8
			// 
			resources.ApplyResources(this.label8, "label8");
			this.label8.Name = "label8";
			this.helpProvider.SetShowHelp(this.label8, ((bool)(resources.GetObject("label8.ShowHelp"))));
			// 
			// taskVolatileCheck
			// 
			resources.ApplyResources(this.taskVolatileCheck, "taskVolatileCheck");
			this.taskVolatileCheck.Name = "taskVolatileCheck";
			this.helpProvider.SetShowHelp(this.taskVolatileCheck, ((bool)(resources.GetObject("taskVolatileCheck.ShowHelp"))));
			this.taskVolatileCheck.UseVisualStyleBackColor = true;
			this.taskVolatileCheck.CheckedChanged += new System.EventHandler(this.taskVolatileCheck_CheckedChanged);
			// 
			// taskUseUnifiedSchedulingEngineCheck
			// 
			resources.ApplyResources(this.taskUseUnifiedSchedulingEngineCheck, "taskUseUnifiedSchedulingEngineCheck");
			this.taskUseUnifiedSchedulingEngineCheck.Name = "taskUseUnifiedSchedulingEngineCheck";
			this.helpProvider.SetShowHelp(this.taskUseUnifiedSchedulingEngineCheck, ((bool)(resources.GetObject("taskUseUnifiedSchedulingEngineCheck.ShowHelp"))));
			this.taskUseUnifiedSchedulingEngineCheck.UseVisualStyleBackColor = true;
			this.taskUseUnifiedSchedulingEngineCheck.CheckedChanged += new System.EventHandler(this.taskUseUnifiedSchedulingEngineCheck_CheckedChanged);
			// 
			// taskDisallowStartOnRemoteAppSessionCheck
			// 
			resources.ApplyResources(this.taskDisallowStartOnRemoteAppSessionCheck, "taskDisallowStartOnRemoteAppSessionCheck");
			this.taskDisallowStartOnRemoteAppSessionCheck.Name = "taskDisallowStartOnRemoteAppSessionCheck";
			this.helpProvider.SetShowHelp(this.taskDisallowStartOnRemoteAppSessionCheck, ((bool)(resources.GetObject("taskDisallowStartOnRemoteAppSessionCheck.ShowHelp"))));
			this.taskDisallowStartOnRemoteAppSessionCheck.UseVisualStyleBackColor = true;
			this.taskDisallowStartOnRemoteAppSessionCheck.CheckedChanged += new System.EventHandler(this.taskDisallowStartOnRemoteAppSessionCheck_CheckedChanged);
			// 
			// taskEnabledCheck
			// 
			resources.ApplyResources(this.taskEnabledCheck, "taskEnabledCheck");
			this.taskEnabledCheck.Name = "taskEnabledCheck";
			this.helpProvider.SetShowHelp(this.taskEnabledCheck, ((bool)(resources.GetObject("taskEnabledCheck.ShowHelp"))));
			this.taskEnabledCheck.UseVisualStyleBackColor = true;
			this.taskEnabledCheck.CheckedChanged += new System.EventHandler(this.taskEnabledCheck_CheckedChanged);
			// 
			// label4
			// 
			resources.ApplyResources(this.label4, "label4");
			this.label4.Name = "label4";
			this.helpProvider.SetShowHelp(this.label4, ((bool)(resources.GetObject("label4.ShowHelp"))));
			// 
			// runTimesTab
			// 
			this.runTimesTab.Controls.Add(this.taskRunTimesControl1);
			this.runTimesTab.Controls.Add(this.runTimesErrorLabel);
			this.runTimesTab.Controls.Add(this.label3);
			this.runTimesTab.Controls.Add(this.label1);
			resources.ApplyResources(this.runTimesTab, "runTimesTab");
			this.runTimesTab.Name = "runTimesTab";
			this.helpProvider.SetShowHelp(this.runTimesTab, ((bool)(resources.GetObject("runTimesTab.ShowHelp"))));
			this.runTimesTab.UseVisualStyleBackColor = true;
			this.runTimesTab.Enter += new System.EventHandler(this.runTimesTab_Enter);
			this.runTimesTab.Leave += new System.EventHandler(this.runTimesTab_Leave);
			// 
			// taskRunTimesControl1
			// 
			resources.ApplyResources(this.taskRunTimesControl1, "taskRunTimesControl1");
			this.taskRunTimesControl1.Name = "taskRunTimesControl1";
			this.helpProvider.SetShowHelp(this.taskRunTimesControl1, ((bool)(resources.GetObject("taskRunTimesControl1.ShowHelp"))));
			// 
			// runTimesErrorLabel
			// 
			resources.ApplyResources(this.runTimesErrorLabel, "runTimesErrorLabel");
			this.runTimesErrorLabel.Name = "runTimesErrorLabel";
			this.helpProvider.SetShowHelp(this.runTimesErrorLabel, ((bool)(resources.GetObject("runTimesErrorLabel.ShowHelp"))));
			// 
			// label3
			// 
			resources.ApplyResources(this.label3, "label3");
			this.label3.Name = "label3";
			this.helpProvider.SetShowHelp(this.label3, ((bool)(resources.GetObject("label3.ShowHelp"))));
			// 
			// label1
			// 
			resources.ApplyResources(this.label1, "label1");
			this.label1.Name = "label1";
			this.helpProvider.SetShowHelp(this.label1, ((bool)(resources.GetObject("label1.ShowHelp"))));
			// 
			// historyTab
			// 
			this.historyTab.Controls.Add(this.historyListView);
			this.helpProvider.SetHelpKeyword(this.historyTab, resources.GetString("historyTab.HelpKeyword"));
			this.helpProvider.SetHelpNavigator(this.historyTab, ((System.Windows.Forms.HelpNavigator)(resources.GetObject("historyTab.HelpNavigator"))));
			resources.ApplyResources(this.historyTab, "historyTab");
			this.historyTab.Name = "historyTab";
			this.helpProvider.SetShowHelp(this.historyTab, ((bool)(resources.GetObject("historyTab.ShowHelp"))));
			this.historyTab.UseVisualStyleBackColor = true;
			this.historyTab.Enter += new System.EventHandler(this.historyTab_Enter);
			// 
			// historyListView
			// 
			resources.ApplyResources(this.historyListView, "historyListView");
			this.historyListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader6,
            this.columnHeader7,
            this.columnHeader8,
            this.columnHeader9,
            this.columnHeader10,
            this.columnHeader11});
			this.historyListView.FullRowSelect = true;
			this.historyListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.historyListView.HideSelection = false;
			this.historyListView.Name = "historyListView";
			this.helpProvider.SetShowHelp(this.historyListView, ((bool)(resources.GetObject("historyListView.ShowHelp"))));
			this.historyListView.UseCompatibleStateImageBehavior = false;
			this.historyListView.View = System.Windows.Forms.View.Details;
			// 
			// columnHeader6
			// 
			resources.ApplyResources(this.columnHeader6, "columnHeader6");
			// 
			// columnHeader7
			// 
			resources.ApplyResources(this.columnHeader7, "columnHeader7");
			// 
			// columnHeader8
			// 
			resources.ApplyResources(this.columnHeader8, "columnHeader8");
			// 
			// columnHeader9
			// 
			resources.ApplyResources(this.columnHeader9, "columnHeader9");
			// 
			// columnHeader10
			// 
			resources.ApplyResources(this.columnHeader10, "columnHeader10");
			// 
			// columnHeader11
			// 
			resources.ApplyResources(this.columnHeader11, "columnHeader11");
			// 
			// historyBackgroundWorker
			// 
			this.historyBackgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.historyBackgroundWorker_DoWork);
			this.historyBackgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.historyBackgroundWorker_RunWorkerCompleted);
			// 
			// errorProvider
			// 
			this.errorProvider.ContainerControl = this;
			// 
			// TaskPropertiesControl
			// 
			resources.ApplyResources(this, "$this");
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.Controls.Add(this.tabControl);
			this.MinimumSize = new System.Drawing.Size(622, 400);
			this.Name = "TaskPropertiesControl";
			this.helpProvider.SetShowHelp(this, ((bool)(resources.GetObject("$this.ShowHelp"))));
			this.tabControl.ResumeLayout(false);
			this.generalTab.ResumeLayout(false);
			this.generalTab.PerformLayout();
			this.taskSecurityGroupBox.ResumeLayout(false);
			this.taskSecurityGroupBox.PerformLayout();
			this.triggersTab.ResumeLayout(false);
			this.actionsTab.ResumeLayout(false);
			this.conditionsTab.ResumeLayout(false);
			this.networkConditionGroupBox.ResumeLayout(false);
			this.networkConditionGroupBox.PerformLayout();
			this.powerConditionGroupBox.ResumeLayout(false);
			this.powerConditionGroupBox.PerformLayout();
			this.idleConditionGroupBox.ResumeLayout(false);
			this.idleConditionGroupBox.PerformLayout();
			this.settingsTab.ResumeLayout(false);
			this.settingsTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.taskRestartCountText)).EndInit();
			this.regInfoTab.ResumeLayout(false);
			this.regInfoTab.PerformLayout();
			this.addPropTab.ResumeLayout(false);
			this.addPropTab.PerformLayout();
			this.secHardGroup.ResumeLayout(false);
			this.secHardGroup.PerformLayout();
			this.autoMaintGroup.ResumeLayout(false);
			this.autoMaintGroup.PerformLayout();
			this.runTimesTab.ResumeLayout(false);
			this.runTimesTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.taskRunTimesControl1)).EndInit();
			this.historyTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.errorProvider)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage generalTab;
		private System.Windows.Forms.GroupBox taskSecurityGroupBox;
		private System.Windows.Forms.TabPage triggersTab;
		private System.Windows.Forms.TabPage actionsTab;
		private System.Windows.Forms.TabPage conditionsTab;
		private System.Windows.Forms.TabPage settingsTab;
		private System.Windows.Forms.TabPage historyTab;
		private System.Windows.Forms.Label taskUserAcctLabel;
		private System.Windows.Forms.TextBox taskPrincipalText;
		private System.Windows.Forms.Button changePrincipalButton;
		private System.Windows.Forms.CheckBox taskLocalOnlyCheck;
		private System.Windows.Forms.RadioButton taskLoggedOptionalRadio;
		private System.Windows.Forms.RadioButton taskLoggedOnRadio;
		private System.Windows.Forms.ComboBox taskVersionCombo;
		private System.Windows.Forms.Label taskVersionLabel;
		private System.Windows.Forms.CheckBox taskHiddenCheck;
		private System.Windows.Forms.CheckBox taskRunLevelCheck;
		private System.Windows.Forms.Button triggerDeleteButton;
		private System.Windows.Forms.Button triggerEditButton;
		private System.Windows.Forms.Button triggerNewButton;
		private System.Windows.Forms.ListView triggerListView;
		private System.Windows.Forms.ColumnHeader columnHeader1;
		private System.Windows.Forms.ColumnHeader columnHeader2;
		private System.Windows.Forms.ColumnHeader columnHeader3;
		private System.Windows.Forms.Label taskTriggerIntroLabel;
		private System.Windows.Forms.Button actionDeleteButton;
		private System.Windows.Forms.Button actionEditButton;
		private System.Windows.Forms.Button actionNewButton;
		private System.Windows.Forms.ListView actionListView;
		private System.Windows.Forms.ColumnHeader columnHeader4;
		private System.Windows.Forms.ColumnHeader columnHeader5;
		private System.Windows.Forms.Label actionIntroLabel;
		private System.Windows.Forms.Button actionUpButton;
		private System.Windows.Forms.Button actionDownButton;
		private System.Windows.Forms.Label conditionIntroLabel;
		private System.Windows.Forms.Label taskNameLabel;
		private System.Windows.Forms.Label taskAuthorLabel;
		private System.Windows.Forms.Label taskDescLabel;
		private System.Windows.Forms.TextBox taskNameText;
		private System.Windows.Forms.Label taskAuthorText;
		private System.Windows.Forms.TextBox taskDescText;
		private System.Windows.Forms.GroupBox powerConditionGroupBox;
		private System.Windows.Forms.GroupBox idleConditionGroupBox;
		private System.Windows.Forms.TimeSpanPicker taskIdleWaitTimeoutCombo;
		private System.Windows.Forms.TimeSpanPicker taskIdleDurationCombo;
		private System.Windows.Forms.CheckBox taskRestartOnIdleCheck;
		private System.Windows.Forms.CheckBox taskStopOnIdleEndCheck;
		private System.Windows.Forms.Label taskIdleWaitTimeoutLabel;
		private System.Windows.Forms.CheckBox taskIdleDurationCheck;
		private System.Windows.Forms.CheckBox taskStopIfGoingOnBatteriesCheck;
		private System.Windows.Forms.CheckBox taskWakeToRunCheck;
		private System.Windows.Forms.CheckBox taskDisallowStartIfOnBatteriesCheck;
		private System.Windows.Forms.GroupBox networkConditionGroupBox;
		private System.Windows.Forms.ComboBox availableConnectionsCombo;
		private System.Windows.Forms.CheckBox taskStartIfConnectionCheck;
		private System.Windows.Forms.ComboBox taskMultInstCombo;
		private System.Windows.Forms.Label taskRunningRuleLabel;
		private System.Windows.Forms.Label taskRestartCountLabel;
		private System.Windows.Forms.CheckBox taskDeleteAfterCheck;
		private System.Windows.Forms.CheckBox taskAllowHardTerminateCheck;
		private System.Windows.Forms.CheckBox taskExecutionTimeLimitCheck;
		private System.Windows.Forms.CheckBox taskRestartIntervalCheck;
		private System.Windows.Forms.CheckBox taskStartWhenAvailableCheck;
		private System.Windows.Forms.CheckBox taskAllowDemandStartCheck;
		private System.Windows.Forms.Label settingsIntroLabel;
		private System.Windows.Forms.TimeSpanPicker taskDeleteAfterCombo;
		private System.Windows.Forms.TimeSpanPicker taskExecutionTimeLimitCombo;
		private System.Windows.Forms.TimeSpanPicker taskRestartIntervalCombo;
		private System.Windows.Forms.NumericUpDown taskRestartCountText;
		private System.Windows.Forms.Label taskRestartAttemptTimesLabel;
		private System.Windows.Forms.ListView historyListView;
		private System.Windows.Forms.ColumnHeader columnHeader6;
		private System.Windows.Forms.ColumnHeader columnHeader7;
		private System.Windows.Forms.ColumnHeader columnHeader8;
		private System.Windows.Forms.ColumnHeader columnHeader9;
		private System.Windows.Forms.ColumnHeader columnHeader10;
		private System.Windows.Forms.ColumnHeader columnHeader11;
		private System.ComponentModel.BackgroundWorker historyBackgroundWorker;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label taskLocationText;
		private System.Windows.Forms.TabPage runTimesTab;
		private System.Windows.Forms.Label runTimesErrorLabel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label1;
		private TaskRunTimesControl taskRunTimesControl1;
		private System.Windows.Forms.TabPage addPropTab;
		private System.Windows.Forms.ComboBox principalSIDTypeCombo;
		private System.Windows.Forms.Label principalSIDTypeLabel;
		private System.Windows.Forms.TimeSpanPicker taskMaintenancePeriodCombo;
		private System.Windows.Forms.Label taskMaintenancePeriodLabel;
		private System.Windows.Forms.TimeSpanPicker taskMaintenanceDeadlineCombo;
		private System.Windows.Forms.Label taskMaintenanceDeadlineLabel;
		private System.Windows.Forms.CheckBox taskMaintenanceExclusiveCheck;
		private System.Windows.Forms.CheckBox taskVolatileCheck;
		private System.Windows.Forms.CheckBox taskUseUnifiedSchedulingEngineCheck;
		private System.Windows.Forms.CheckBox taskDisallowStartOnRemoteAppSessionCheck;
		private System.Windows.Forms.CheckBox taskEnabledCheck;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label principalReqPrivilegesLabel;
		private DropDownCheckList principalReqPrivilegesDropDown;
		private System.Windows.Forms.ComboBox taskPriorityCombo;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.GroupBox secHardGroup;
		private System.Windows.Forms.GroupBox autoMaintGroup;
		private System.Windows.Forms.TabPage regInfoTab;
		private System.Windows.Forms.Label taskRegURILabel;
		private System.Windows.Forms.Label taskRegSourceLabel;
		private System.Windows.Forms.Label taskRegDocLabel;
		private System.Windows.Forms.TextBox taskRegURIText;
		private System.Windows.Forms.TextBox taskRegSourceText;
		private System.Windows.Forms.TextBox taskRegDocText;
		private System.Windows.Forms.Label taskRegSDDLLabel;
		private System.Windows.Forms.Label taskRegVersionLabel;
		private System.Windows.Forms.TextBox taskRegSDDLText;
		private System.Windows.Forms.TextBox taskRegVersionText;
		private System.Windows.Forms.Button taskRegSDDLBtn;
		private System.Windows.Forms.ErrorProvider errorProvider;
		private System.Windows.Forms.HelpProvider helpProvider;
		private System.Windows.Forms.Label label5;
	}
}
