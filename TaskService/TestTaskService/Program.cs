﻿using System;
using Microsoft.Win32.TaskScheduler;

namespace TestTaskService
{
	class Program
	{
		static void Main(string[] args)
		{
			LongTest();
		}

		static void ShortTest()
		{
			// Get the service on the local machine
			TaskService ts = new TaskService();

			// Create a new task definition and assign properties
			TaskDefinition td = ts.NewTask();
			td.RegistrationInfo.Description = "Does something";
			td.Principal.LogonType = TaskLogonType.InteractiveToken;

			// Create a trigger that will fire the task at this time every other day
			td.Triggers.Add(new DailyTrigger { DaysInterval = 2 });

			// Create an action that will launch Notepad whenever the trigger fires
			td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));

			// Register the task in the root folder
			const string taskName = "Test";
			Task t = ts.RootFolder.RegisterTaskDefinition(taskName, td);
			System.Threading.Thread.Sleep(1000);
			Console.WriteLine("LastTime & Result: {0} ({1})" , t.LastRunTime, t.LastTaskResult);

			// Retrieve the task, add a trigger and save it.
			t = ts.GetTask(taskName);
			td = t.Definition;
			td.Triggers.Add(new BootTrigger { Enabled = false });
			ts.RootFolder.RegisterTaskDefinition(taskName, td, TaskCreation.Update, null, null,
				td.Principal.LogonType, null);

			// Remove the task we just created
			Console.ReadKey(false);
			ts.RootFolder.DeleteTask(taskName);
		}

		static void LongTest()
		{
			TaskService ts = new TaskService();
			Version ver = ts.HighestSupportedVersion;
			bool newVer = (ver == new Version(1, 2));
			Console.WriteLine("Highest version: " + ver);
			Console.WriteLine("Server: {0} ({1})", ts.TargetServer, ts.Connected ? "Connected" : "Disconnected");

			Console.WriteLine("Running tasks:");
			foreach (RunningTask rt in ts.GetRunningTasks(true))
			{
				if (rt != null)
				{
					Console.WriteLine("+ {0}, {1} ({2})", rt.Name, rt.Path, rt.State);
					if (ver.Minor > 0)
						Console.WriteLine("  Current Action: " + rt.CurrentAction);
				}
			}

			TaskFolder tf = ts.RootFolder;
			Console.WriteLine("\nRoot folder tasks ({0}):", tf.Tasks.Count);
			foreach (Task t in tf.Tasks)
			{
				try
				{
					Console.WriteLine("+ {0}, {1} ({2})", t.Name, t.Definition.RegistrationInfo.Author, t.State);
					foreach (Trigger trg in t.Definition.Triggers)
						Console.WriteLine(" + {0}", trg);
					foreach (Action act in t.Definition.Actions)
						Console.WriteLine(" = {0}", act);
				}
				catch { }
			}

			TaskFolderCollection tfs = tf.SubFolders;
			if (tfs.Count > 0)
			{
				Console.WriteLine("\nSub folders:");
				foreach (TaskFolder sf in tfs)
					Console.WriteLine("+ {0}", sf.Path);
			}
			try
			{
				TaskFolder sub = tf.SubFolders["Microsoft"];
				Console.WriteLine("\nSubfolder path: " + sub.Path);
			}
			catch (NotSupportedException) { }
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
			
			TaskDefinition td = ts.NewTask();
			td.Data = "Your data";
			td.Principal.UserId = "AMERICAS\\dahall";
			td.Principal.LogonType = TaskLogonType.InteractiveToken;
			td.RegistrationInfo.Author = "dahall";
			td.RegistrationInfo.Description = "Does something";
			td.RegistrationInfo.Documentation = "Don't pretend this is real.";
			td.Settings.DisallowStartIfOnBatteries = true;
			td.Settings.Enabled = true;
			td.Settings.ExecutionTimeLimit = TimeSpan.FromHours(1);
			td.Settings.Hidden = false;
			td.Settings.IdleSettings.IdleDuration = TimeSpan.FromMinutes(20);
			td.Settings.IdleSettings.RestartOnIdle = false;
			td.Settings.IdleSettings.StopOnIdleEnd = false;
			td.Settings.IdleSettings.WaitTimeout = TimeSpan.FromMinutes(10);
			td.Settings.Priority = System.Diagnostics.ProcessPriorityClass.Normal;
			td.Settings.RunOnlyIfIdle = false;
			td.Settings.RunOnlyIfNetworkAvailable = false;
			td.Settings.StopIfGoingOnBatteries = true;
			if (newVer)
			{
				td.Principal.RunLevel = TaskRunLevel.LUA;
				//td.RegistrationInfo.SecurityDescriptorSddlForm = "O:COG:CGD::(A;;RPWPCCDCLCSWRCWDWOGA;;;S-1-0-0)";
				td.RegistrationInfo.Source = "Test App";
				td.RegistrationInfo.URI = new Uri("test://app");
				td.RegistrationInfo.Version = new Version(0, 9);
				td.Settings.AllowDemandStart = true;
				td.Settings.AllowHardTerminate = true;
				td.Settings.Compatibility = TaskCompatibility.V2;
				td.Settings.DeleteExpiredTaskAfter = TimeSpan.FromMinutes(1);
				td.Settings.MultipleInstances = TaskInstancesPolicy.StopExisting;
				td.Settings.StartWhenAvailable = true;
				td.Settings.WakeToRun = false;
				td.Settings.RestartCount = 5;
				td.Settings.RestartInterval = TimeSpan.FromSeconds(100);
			}

			BootTrigger bTrigger = (BootTrigger)td.Triggers.Add(new BootTrigger { Enabled = false }); //(BootTrigger)td.Triggers.AddNew(TaskTriggerType.Boot);
			if (newVer) bTrigger.Delay = TimeSpan.FromMinutes(5);

			DailyTrigger dTrigger = (DailyTrigger)td.Triggers.Add(new DailyTrigger());
			dTrigger.DaysInterval = 2;
			if (newVer) dTrigger.RandomDelay = TimeSpan.FromHours(2);

			if (newVer)
			{
				EventTrigger eTrigger = (EventTrigger)td.Triggers.Add(new EventTrigger());
				eTrigger.Subscription = @"<QueryList><Query Id='1'><Select Path='System'>*[System/Level=2]</Select></Query></QueryList>";
				eTrigger.ValueQueries.Add("Name", "Value");

				td.Triggers.Add(new RegistrationTrigger { Delay = TimeSpan.FromMinutes(5) });

				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.ConsoleConnect, UserId = "AMERICAS\\dahall" });
				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.ConsoleDisconnect });
				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.RemoteConnect });
				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.RemoteDisconnect });
				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.SessionLock, UserId = "AMERICAS\\dahall" });
				td.Triggers.Add(new SessionStateChangeTrigger { StateChange = TaskSessionStateChangeType.SessionUnlock });
			}

			td.Triggers.Add(new IdleTrigger());

			LogonTrigger lTrigger = (LogonTrigger)td.Triggers.Add(new LogonTrigger());
			if (newVer) lTrigger.Delay = TimeSpan.FromMinutes(15);
			if (newVer) lTrigger.UserId = "AMERICAS\\dahall";
			if (newVer) lTrigger.Repetition.Interval = TimeSpan.FromSeconds(1000);

			MonthlyTrigger mTrigger = (MonthlyTrigger)td.Triggers.Add(new MonthlyTrigger());
			mTrigger.DaysOfMonth = new int[] { 3, 6, 10, 18 };
			mTrigger.MonthsOfYear = MonthsOfTheYear.July | MonthsOfTheYear.November;
			if (newVer) mTrigger.RunOnLastDayOfMonth = true;
			mTrigger.EndBoundary = DateTime.Today + TimeSpan.FromDays(90);

			MonthlyDOWTrigger mdTrigger = (MonthlyDOWTrigger)td.Triggers.Add(new MonthlyDOWTrigger());
			mdTrigger.DaysOfWeek = DaysOfTheWeek.AllDays;
			mdTrigger.MonthsOfYear = MonthsOfTheYear.January | MonthsOfTheYear.December;
			if (newVer) mdTrigger.RunOnLastWeekOfMonth = true;
			mdTrigger.WeeksOfMonth = WhichWeek.FirstWeek;
			
			TimeTrigger tTrigger = (TimeTrigger)td.Triggers.Add(new TimeTrigger());
			tTrigger.StartBoundary = DateTime.Now + TimeSpan.FromMinutes(1);
			tTrigger.EndBoundary = DateTime.Today + TimeSpan.FromDays(7);
			if (newVer) tTrigger.ExecutionTimeLimit = TimeSpan.FromSeconds(15);
			if (newVer) tTrigger.Id = "Time test";
			tTrigger.Repetition.Duration = TimeSpan.FromMinutes(20);
			tTrigger.Repetition.Interval = TimeSpan.FromMinutes(15);
			tTrigger.Repetition.StopAtDurationEnd = true;

			WeeklyTrigger wTrigger = (WeeklyTrigger)td.Triggers.Add(new WeeklyTrigger());
			wTrigger.DaysOfWeek = DaysOfTheWeek.Monday;
			wTrigger.WeeksInterval = 3;

			td.Actions.Add(new ExecAction("notepad.exe", "c:\\test.log", null));
			if (newVer)
			{
				td.Actions.Add(new ShowMessageAction("Running Notepad", "Info"));
				td.Actions.Add(new EmailAction("Testing", "dahall@codeplex.com", "user@test.com", "You've got mail.", "mail.myisp.com"));
				td.Actions.Add(new ComHandlerAction(new Guid("CE7D4428-8A77-4c5d-8A13-5CAB5D1EC734"), string.Empty));
			}

			tf.RegisterTaskDefinition("Test", td, TaskCreation.CreateOrUpdate, null, null, TaskLogonType.InteractiveToken, null);

			Task runningTask = tf.Tasks["Test"];
			Console.WriteLine("\nNew task will run at " + runningTask.NextRunTime);
			Console.WriteLine("\nNew task triggers:");
			for (int i = 0; i < runningTask.Definition.Triggers.Count; i++)
				Console.WriteLine("  {0}: {1}", i, runningTask.Definition.Triggers[i]);
			Console.WriteLine("\nNew task actions:");
			for (int i = 0; i < runningTask.Definition.Actions.Count; i++)
				Console.WriteLine("  {0}: {1}", i, runningTask.Definition.Actions[i]);

			Form1 frm = new Form1();
			frm.taskPropertiesControl1.SetTask(runningTask);
			frm.ShowDialog();
			tf.DeleteTask("Test");
		}
	}
}
