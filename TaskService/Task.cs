﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.ComTypes;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using JetBrains.Annotations;
using Microsoft.Win32.TaskScheduler.V1Interop;
using Microsoft.Win32.TaskScheduler.V2Interop;
using IPrincipal = Microsoft.Win32.TaskScheduler.V2Interop.IPrincipal;
// ReSharper disable InconsistentNaming
// ReSharper disable SuspiciousTypeConversion.Global

namespace Microsoft.Win32.TaskScheduler
{
	#region Enumerations

	/// <summary>Defines what versions of Task Scheduler or the AT command that the task is compatible with.</summary>
	public enum TaskCompatibility
	{
		/// <summary>The task is compatible with the AT command.</summary>
		AT,
		/// <summary>The task is compatible with Task Scheduler 1.0 (Windows Server™ 2003, Windows® XP, or Windows® 2000).</summary>
		V1,
		/// <summary>The task is compatible with Task Scheduler 2.0 (Windows Vista™, Windows Server™ 2008).</summary>
		V2,
		/// <summary>The task is compatible with Task Scheduler 2.1 (Windows® 7, Windows Server™ 2008 R2).</summary>
		V2_1,
		/// <summary>The task is compatible with Task Scheduler 2.2 (Windows® 8.x, Windows Server™ 2012).</summary>
		V2_2,
		// <summary>The task is compatible with Task Scheduler 2.3 (Windows® 10, Windows Server™ 2016).</summary>
		// V2_3
	}

	/// <summary>Defines how the Task Scheduler service creates, updates, or disables the task.</summary>
	public enum TaskCreation
	{
		/// <summary>The Task Scheduler service registers the task as a new task.</summary>
		Create = 2,
		/// <summary>The Task Scheduler service either registers the task as a new task or as an updated version if the task already exists. Equivalent to Create | Update.</summary>
		CreateOrUpdate = 6,
		/// <summary>The Task Scheduler service registers the disabled task. A disabled task cannot run until it is enabled. For more information, see Enabled Property of TaskSettings and Enabled Property of RegisteredTask.</summary>
		Disable = 8,
		/// <summary>The Task Scheduler service is prevented from adding the allow access-control entry (ACE) for the context principal. When the TaskFolder.RegisterTaskDefinition or TaskFolder.RegisterTask functions are called with this flag to update a task, the Task Scheduler service does not add the ACE for the new context principal and does not remove the ACE from the old context principal.</summary>
		DontAddPrincipalAce = 0x10,
		/// <summary>The Task Scheduler service creates the task, but ignores the registration triggers in the task. By ignoring the registration triggers, the task will not execute when it is registered unless a time-based trigger causes it to execute on registration.</summary>
		IgnoreRegistrationTriggers = 0x20,
		/// <summary>The Task Scheduler service registers the task as an updated version of an existing task. When a task with a registration trigger is updated, the task will execute after the update occurs.</summary>
		Update = 4,
		/// <summary>The Task Scheduler service checks the syntax of the XML that describes the task but does not register the task. This constant cannot be combined with the Create, Update, or CreateOrUpdate values.</summary>
		ValidateOnly = 1
	}

	/// <summary>Defines how the Task Scheduler handles existing instances of the task when it starts a new instance of the task.</summary>
	public enum TaskInstancesPolicy
	{
		/// <summary>Starts new instance while an existing instance is running.</summary>
		Parallel,
		/// <summary>Starts a new instance of the task after all other instances of the task are complete.</summary>
		Queue,
		/// <summary>Does not start a new instance if an existing instance of the task is running.</summary>
		IgnoreNew,
		/// <summary>Stops an existing instance of the task before it starts a new instance.</summary>
		StopExisting
	}

	/// <summary>Defines what logon technique is required to run a task.</summary>
	public enum TaskLogonType
	{
		/// <summary>The logon method is not specified. Used for non-NT credentials.</summary>
		None,
		/// <summary>Use a password for logging on the user. The password must be supplied at registration time.</summary>
		Password,
		/// <summary>Use an existing interactive token to run a task. The user must log on using a service for user (S4U) logon. When an S4U logon is used, no password is stored by the system and there is no access to either the network or to encrypted files.</summary>
		S4U,
		/// <summary>User must already be logged on. The task will be run only in an existing interactive session.</summary>
		InteractiveToken,
		/// <summary>Group activation. The groupId field specifies the group.</summary>
		Group,
		/// <summary>Indicates that a Local System, Local Service, or Network Service account is being used as a security context to run the task.</summary>
		ServiceAccount,
		/// <summary>First use the interactive token. If the user is not logged on (no interactive token is available), then the password is used. The password must be specified when a task is registered. This flag is not recommended for new tasks because it is less reliable than Password.</summary>
		InteractiveTokenOrPassword
	}

	/// <summary>Defines which privileges must be required for a secured task.</summary>
	public enum TaskPrincipalPrivilege
	{
		/// <summary>Required to create a primary token. User Right: Create a token object.</summary>
		SeCreateTokenPrivilege = 1,
		/// <summary>Required to assign the primary token of a process. User Right: Replace a process-level token.</summary>
		SeAssignPrimaryTokenPrivilege,
		/// <summary>Required to lock physical pages in memory. User Right: Lock pages in memory. </summary>
		SeLockMemoryPrivilege,
		/// <summary>Required to increase the quota assigned to a process. User Right: Adjust memory quotas for a process.</summary>
		SeIncreaseQuotaPrivilege,
		/// <summary>Required to read unsolicited input from a terminal device. User Right: Not applicable.</summary>
		SeUnsolicitedInputPrivilege,
		/// <summary>Required to create a computer account. User Right: Add workstations to domain. </summary>
		SeMachineAccountPrivilege,
		/// <summary>This privilege identifies its holder as part of the trusted computer base. Some trusted protected subsystems are granted this privilege. User Right: Act as part of the operating system.</summary>
		SeTcbPrivilege,
		/// <summary>Required to perform a number of security-related functions, such as controlling and viewing audit messages. This privilege identifies its holder as a security operator. User Right: Manage auditing and the security log.</summary>
		SeSecurityPrivilege,
		/// <summary>Required to take ownership of an object without being granted discretionary access. This privilege allows the owner value to be set only to those values that the holder may legitimately assign as the owner of an object. User Right: Take ownership of files or other objects.</summary>
		SeTakeOwnershipPrivilege,
		/// <summary>Required to load or unload a device driver. User Right: Load and unload device drivers.</summary>
		SeLoadDriverPrivilege,
		/// <summary>Required to gather profiling information for the entire system. User Right: Profile system performance.</summary>
		SeSystemProfilePrivilege,
		/// <summary>Required to modify the system time. User Right: Change the system time. </summary>
		SeSystemtimePrivilege,
		/// <summary>Required to gather profiling information for a single process. User Right: Profile single process.</summary>
		SeProfileSingleProcessPrivilege,
		/// <summary>Required to increase the base priority of a process. User Right: Increase scheduling priority.</summary>
		SeIncreaseBasePriorityPrivilege,
		/// <summary>Required to create a paging file. User Right: Create a pagefile. </summary>
		SeCreatePagefilePrivilege,
		/// <summary>Required to create a permanent object. User Right: Create permanent shared objects.</summary>
		SeCreatePermanentPrivilege,
		/// <summary>Required to perform backup operations. This privilege causes the system to grant all read access control to any file, regardless of the access control list (ACL) specified for the file. Any access request other than read is still evaluated with the ACL. This privilege is required by the RegSaveKey and RegSaveKeyExfunctions. The following access rights are granted if this privilege is held: READ_CONTROL, ACCESS_SYSTEM_SECURITY, FILE_GENERIC_READ, FILE_TRAVERSE. User Right: Back up files and directories.</summary>
		SeBackupPrivilege,
		/// <summary>Required to perform restore operations. This privilege causes the system to grant all write access control to any file, regardless of the ACL specified for the file. Any access request other than write is still evaluated with the ACL. Additionally, this privilege enables you to set any valid user or group security identifier (SID) as the owner of a file. This privilege is required by the RegLoadKey function. The following access rights are granted if this privilege is held: WRITE_DAC, WRITE_OWNER, ACCESS_SYSTEM_SECURITY, FILE_GENERIC_WRITE, FILE_ADD_FILE, FILE_ADD_SUBDIRECTORY, DELETE. User Right: Restore files and directories.</summary>
		SeRestorePrivilege,
		/// <summary>Required to shut down a local system. User Right: Shut down the system. </summary>
		SeShutdownPrivilege,
		/// <summary>Required to debug and adjust the memory of a process owned by another account. User Right: Debug programs.</summary>
		SeDebugPrivilege,
		/// <summary>Required to generate audit-log entries. Give this privilege to secure servers. User Right: Generate security audits.</summary>
		SeAuditPrivilege,
		/// <summary>Required to modify the nonvolatile RAM of systems that use this type of memory to store configuration information. User Right: Modify firmware environment values.</summary>
		SeSystemEnvironmentPrivilege,
		/// <summary>Required to receive notifications of changes to files or directories. This privilege also causes the system to skip all traversal access checks. It is enabled by default for all users. User Right: Bypass traverse checking.</summary>
		SeChangeNotifyPrivilege,
		/// <summary>Required to shut down a system by using a network request. User Right: Force shutdown from a remote system.</summary>
		SeRemoteShutdownPrivilege,
		/// <summary>Required to undock a laptop. User Right: Remove computer from docking station. </summary>
		SeUndockPrivilege,
		/// <summary>Required for a domain controller to use the LDAP directory synchronization services. This privilege allows the holder to read all objects and properties in the directory, regardless of the protection on the objects and properties. By default, it is assigned to the Administrator and LocalSystem accounts on domain controllers. User Right: Synchronize directory service data.</summary>
		SeSyncAgentPrivilege,
		/// <summary>Required to mark user and computer accounts as trusted for delegation. User Right: Enable computer and user accounts to be trusted for delegation.</summary>
		SeEnableDelegationPrivilege,
		/// <summary>Required to enable volume management privileges. User Right: Manage the files on a volume.</summary>
		SeManageVolumePrivilege,
		/// <summary>Required to impersonate. User Right: Impersonate a client after authentication. Windows XP/2000: This privilege is not supported. Note that this value is supported starting with Windows Server 2003, Windows XP with SP2, and Windows 2000 with SP4.</summary>
		SeImpersonatePrivilege,
		/// <summary>Required to create named file mapping objects in the global namespace during Terminal Services sessions. This privilege is enabled by default for administrators, services, and the local system account. User Right: Create global objects. Windows XP/2000: This privilege is not supported. Note that this value is supported starting with Windows Server 2003, Windows XP with SP2, and Windows 2000 with SP4.</summary>
		SeCreateGlobalPrivilege,
		/// <summary>Required to access Credential Manager as a trusted caller. User Right: Access Credential Manager as a trusted caller.</summary>
		SeTrustedCredManAccessPrivilege,
		/// <summary>Required to modify the mandatory integrity level of an object. User Right: Modify an object label.</summary>
		SeRelabelPrivilege,
		/// <summary>Required to allocate more memory for applications that run in the context of users. User Right: Increase a process working set.</summary>
		SeIncreaseWorkingSetPrivilege,
		/// <summary>Required to adjust the time zone associated with the computer's internal clock. User Right: Change the time zone.</summary>
		SeTimeZonePrivilege,
		/// <summary>Required to create a symbolic link. User Right: Create symbolic links.</summary>
		SeCreateSymbolicLinkPrivilege
	}

	/// <summary>Defines the types of process security identifier (SID) that can be used by tasks. These changes are used to specify the type of process SID in the IPrincipal2 interface.</summary>
	public enum TaskProcessTokenSidType
	{
		/// <summary>No changes will be made to the process token groups list.</summary>
		None = 0,
		/// <summary>A task SID that is derived from the task name will be added to the process token groups list, and the token default discretionary access control list (DACL) will be modified to allow only the task SID and local system full control and the account SID read control.</summary>
		Unrestricted = 1,
		/// <summary>A Task Scheduler will apply default settings to the task process.</summary>
		Default = 2
	}

	/// <summary>Defines how a task is run.</summary>
	[Flags]
	public enum TaskRunFlags
	{
		/// <summary>The task is run with all flags ignored.</summary>
		NoFlags = 0,
		/// <summary>The task is run as the user who is calling the Run method.</summary>
		AsSelf = 1,
		/// <summary>The task is run regardless of constraints such as "do not run on batteries" or "run only if idle".</summary>
		IgnoreConstraints = 2,
		/// <summary>The task is run using a terminal server session identifier.</summary>
		UseSessionId = 4,
		/// <summary>The task is run using a security identifier.</summary>
		UserSID = 8
	}

	/// <summary>Defines LUA elevation flags that specify with what privilege level the task will be run.</summary>
	public enum TaskRunLevel
	{
		/// <summary>Tasks will be run with the least privileges.</summary>
		[XmlEnum("LeastPrivilege")]
		LUA,
		/// <summary>Tasks will be run with the highest privileges.</summary>
		[XmlEnum("HighestAvailable")]
		Highest
	}

	/// <summary>Defines what kind of Terminal Server session state change you can use to trigger a task to start. These changes are used to specify the type of state change in the SessionStateChangeTrigger.</summary>
	public enum TaskSessionStateChangeType
	{
		/// <summary>Terminal Server console connection state change. For example, when you connect to a user session on the local computer by switching users on the computer.</summary>
		ConsoleConnect = 1,
		/// <summary>Terminal Server console disconnection state change. For example, when you disconnect to a user session on the local computer by switching users on the computer.</summary>
		ConsoleDisconnect = 2,
		/// <summary>Terminal Server remote connection state change. For example, when a user connects to a user session by using the Remote Desktop Connection program from a remote computer.</summary>
		RemoteConnect = 3,
		/// <summary>Terminal Server remote disconnection state change. For example, when a user disconnects from a user session while using the Remote Desktop Connection program from a remote computer.</summary>
		RemoteDisconnect = 4,
		/// <summary>Terminal Server session locked state change. For example, this state change causes the task to run when the computer is locked.</summary>
		SessionLock = 7,
		/// <summary>Terminal Server session unlocked state change. For example, this state change causes the task to run when the computer is unlocked.</summary>
		SessionUnlock = 8
	}

	/// <summary>
	/// Options for use when calling the SetSecurityDescriptorSddlForm methods.
	/// </summary>
	[Flags]
	public enum TaskSetSecurityOptions
	{
		/// <summary>No special handling.</summary>
		None = 0,
		/// <summary>The Task Scheduler service is prevented from adding the allow access-control entry (ACE) for the context principal.</summary>
		DontAddPrincipalAce = 0x10
	}

	/***** WAITING TO DETERMINE USE CASE *****
	/// <summary>
	/// Success and error codes that some methods will expose through <see cref="COMExcpetion"/>.
	/// </summary>
	public enum TaskResultCode
	{
		/// <summary>The task is ready to run at its next scheduled time.</summary>
		TaskReady = 0x00041300,
		/// <summary>The task is currently running.</summary>
		TaskRunning = 0x00041301,
		/// <summary>The task will not run at the scheduled times because it has been disabled.</summary>
		TaskDisabled = 0x00041302,
		/// <summary>The task has not yet run.</summary>
		TaskHasNotRun = 0x00041303,
		/// <summary>There are no more runs scheduled for this task.</summary>
		TaskNoMoreRuns = 0x00041304,
		/// <summary>One or more of the properties that are needed to run this task on a schedule have not been set.</summary>
		TaskNotScheduled = 0x00041305,
		/// <summary>The last run of the task was terminated by the user.</summary>
		TaskTerminated = 0x00041306,
		/// <summary>Either the task has no triggers or the existing triggers are disabled or not set.</summary>
		TaskNoValidTriggers = 0x00041307,
		/// <summary>Event triggers do not have set run times.</summary>
		EventTrigger = 0x00041308,
		/// <summary>A task's trigger is not found.</summary>
		TriggerNotFound = 0x80041309,
		/// <summary>One or more of the properties required to run this task have not been set.</summary>
		TaskNotReady = 0x8004130A,
		/// <summary>There is no running instance of the task.</summary>
		TaskNotRunning = 0x8004130B,
		/// <summary>The Task Scheduler service is not installed on this computer.</summary>
		ServiceNotInstalled = 0x8004130C,
		/// <summary>The task object could not be opened.</summary>
		CannotOpenTask = 0x8004130D,
		/// <summary>The object is either an invalid task object or is not a task object.</summary>
		InvalidTask = 0x8004130E,
		/// <summary>No account information could be found in the Task Scheduler security database for the task indicated.</summary>
		AccountInformationNotSet = 0x8004130F,
		/// <summary>Unable to establish existence of the account specified.</summary>
		AccountNameNotFound = 0x80041310,
		/// <summary>Corruption was detected in the Task Scheduler security database; the database has been reset.</summary>
		AccountDbaseCorrupt = 0x80041311,
		/// <summary>Task Scheduler security services are available only on Windows NT.</summary>
		NoSecurityServices = 0x80041312,
		/// <summary>The task object version is either unsupported or invalid.</summary>
		UnknownObjectVersion = 0x80041313,
		/// <summary>The task has been configured with an unsupported combination of account settings and run time options.</summary>
		UnsupportedAccountOption = 0x80041314,
		/// <summary>The Task Scheduler Service is not running.</summary>
		ServiceNotRunning = 0x80041315,
		/// <summary>The task XML contains an unexpected node.</summary>
		UnexpectedNode = 0x80041316,
		/// <summary>The task XML contains an element or attribute from an unexpected namespace.</summary>
		Namespace = 0x80041317,
		/// <summary>The task XML contains a value which is incorrectly formatted or out of range.</summary>
		InvalidValue = 0x80041318,
		/// <summary>The task XML is missing a required element or attribute.</summary>
		MissingNode = 0x80041319,
		/// <summary>The task XML is malformed.</summary>
		MalformedXml = 0x8004131A,
		/// <summary>The task is registered, but not all specified triggers will start the task.</summary>
		SomeTriggersFailed = 0x0004131B,
		/// <summary>The task is registered, but may fail to start. Batch logon privilege needs to be enabled for the task principal.</summary>
		BatchLogonProblem = 0x0004131C,
		/// <summary>The task XML contains too many nodes of the same type.</summary>
		TooManyNodes = 0x8004131D,
		/// <summary>The task cannot be started after the trigger end boundary.</summary>
		PastEndBoundary = 0x8004131E,
		/// <summary>An instance of this task is already running.</summary>
		AlreadyRunning = 0x8004131F,
		/// <summary>The task will not run because the user is not logged on.</summary>
		UserNotLoggedOn = 0x80041320,
		/// <summary>The task image is corrupt or has been tampered with.</summary>
		InvalidTaskHash = 0x80041321,
		/// <summary>The Task Scheduler service is not available.</summary>
		ServiceNotAvailable = 0x80041322,
		/// <summary>The Task Scheduler service is too busy to handle your request. Please try again later.</summary>
		ServiceTooBusy = 0x80041323,
		/// <summary>The Task Scheduler service attempted to run the task, but the task did not run due to one of the constraints in the task definition.</summary>
		TaskAttempted = 0x80041324,
		/// <summary>The Task Scheduler service has asked the task to run.</summary>
		TaskQueued = 0x00041325,
		/// <summary>The task is disabled.</summary>
		TaskDisabled = 0x80041326,
		/// <summary>The task has properties that are not compatible with earlier versions of Windows.</summary>
		TaskNotV1Compatible = 0x80041327,
		/// <summary>The task settings do not allow the task to start on demand.</summary>
		StartOnDemand = 0x80041328,
	}
	*/

	/// <summary>Defines the different states that a registered task can be in.</summary>
	public enum TaskState
	{
		/// <summary>The state of the task is unknown.</summary>
		Unknown,
		/// <summary>The task is registered but is disabled and no instances of the task are queued or running. The task cannot be run until it is enabled.</summary>
		Disabled,
		/// <summary>Instances of the task are queued.</summary>
		Queued,
		/// <summary>The task is ready to be executed, but no instances are queued or running.</summary>
		Ready,
		/// <summary>One or more instances of the task is running.</summary>
		Running
	}

	#endregion Enumerations

	/// <summary>
	/// Specifies how the Task Scheduler performs tasks when the computer is in an idle condition. For information about idle conditions, see Task Idle Conditions.
	/// </summary>
	[PublicAPI]
	public sealed class IdleSettings : IDisposable
	{
		private ITask v1Task;
		private IIdleSettings v2Settings;

		internal IdleSettings([NotNull] IIdleSettings iSettings)
		{
			v2Settings = iSettings;
		}

		internal IdleSettings([NotNull] ITask iTask)
		{
			v1Task = iTask;
		}

		/// <summary>
		/// Gets or sets a value that indicates the amount of time that the computer must be in an idle state before the task is run.
		/// </summary>
		/// <value>
		/// A value that indicates the amount of time that the computer must be in an idle state before the task is run. The minimum value is one minute. If this value is <c>TimeSpan.Zero</c>, then the delay will be set to the default of 10 minutes.
		/// </value>
		[DefaultValue(typeof(TimeSpan), "00:10:00")]
		[XmlElement("Duration")]
		public TimeSpan IdleDuration
		{
			get
			{
				if (v2Settings != null)
					return Task.StringToTimeSpan(v2Settings.IdleDuration);
				ushort idleMin, deadMin;
				v1Task.GetIdleWait(out idleMin, out deadMin);
				return TimeSpan.FromMinutes(deadMin);
			}
			set
			{
				if (v2Settings != null)
				{
					if (value != TimeSpan.Zero && value < TimeSpan.FromMinutes(1))
						throw new ArgumentOutOfRangeException(nameof(IdleDuration));
					v2Settings.IdleDuration = Task.TimeSpanToString(value);
				}
				else
				{
					v1Task.SetIdleWait((ushort)WaitTimeout.TotalMinutes, (ushort)value.TotalMinutes);
				}
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates whether the task is restarted when the computer cycles into an idle condition more than once.
		/// </summary>
		[DefaultValue(false)]
		public bool RestartOnIdle
		{
			get { return v2Settings?.RestartOnIdle ?? v1Task.HasFlags(TaskFlags.RestartOnIdleResume); }
			set
			{
				if (v2Settings != null)
					v2Settings.RestartOnIdle = value;
				else
					v1Task.SetFlags(TaskFlags.RestartOnIdleResume, value);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler will terminate the task if the idle condition ends before the task is completed.
		/// </summary>
		[DefaultValue(true)]
		public bool StopOnIdleEnd
		{
			get { return v2Settings?.StopOnIdleEnd ?? v1Task.HasFlags(TaskFlags.KillOnIdleEnd); }
			set
			{
				if (v2Settings != null)
					v2Settings.StopOnIdleEnd = value;
				else
					v1Task.SetFlags(TaskFlags.KillOnIdleEnd, value);
			}
		}

		/// <summary>
		/// Gets or sets a value that indicates the amount of time that the Task Scheduler will wait for an idle condition to occur. If no value is specified for this property, then the Task Scheduler service will wait indefinitely for an idle condition to occur.
		/// </summary>
		/// <value>
		/// A value that indicates the amount of time that the Task Scheduler will wait for an idle condition to occur. The minimum time allowed is 1 minute. If this value is <c>TimeSpan.Zero</c>, then the delay will be set to the default of 1 hour.
		/// </value>
		[DefaultValue(typeof(TimeSpan), "01:00:00")]
		public TimeSpan WaitTimeout
		{
			get
			{
				if (v2Settings != null)
					return Task.StringToTimeSpan(v2Settings.WaitTimeout);
				ushort idleMin, deadMin;
				v1Task.GetIdleWait(out idleMin, out deadMin);
				return TimeSpan.FromMinutes(idleMin);
			}
			set
			{
				if (v2Settings != null)
				{
					if (value != TimeSpan.Zero && value < TimeSpan.FromMinutes(1))
						throw new ArgumentOutOfRangeException(nameof(WaitTimeout));
					v2Settings.WaitTimeout = Task.TimeSpanToString(value);
				}
				else
				{
					v1Task.SetIdleWait((ushort)value.TotalMinutes, (ushort)IdleDuration.TotalMinutes);
				}
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Settings != null)
				Marshal.ReleaseComObject(v2Settings);
			v1Task = null;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (v2Settings != null || v1Task != null)
				return DebugHelper.GetDebugString(this);
			return base.ToString();
		}
	}

	internal static class DebugHelper
	{
		[SuppressMessage("Language", "CSE0003:Use expression-bodied members", Justification = "<Pending>")]
		public static string GetDebugString(object inst)
		{
#if DEBUG
			var sb = new StringBuilder();
			foreach (var pi in inst.GetType().GetProperties(BindingFlags.Public|BindingFlags.Instance|BindingFlags.DeclaredOnly))
			{
				if (pi.Name.StartsWith("Xml"))
					continue;
				var outval = pi.GetValue(inst, null);
				if (outval != null)
				{
					var defval = XmlSerializationHelper.GetDefaultValue(pi);
					if (!outval.Equals(defval))
					{
						var s = $"{pi.Name}:{outval}";
						if (s.Length > 30) s = s.Remove(30);
						sb.Append(s + "; ");
					}
				}
			}
			return sb.ToString();
#else
			return inst.GetType().ToString();
#endif
		}
	}

	/// <summary>
	/// Specifies the task settings the Task scheduler will use to start task during Automatic maintenance.
	/// </summary>
	[XmlType(IncludeInSchema = false)]
	[PublicAPI]
	public sealed class MaintenanceSettings : IDisposable
	{
		private ITaskSettings3 iSettings;
		private IMaintenanceSettings iMaintSettings;

		internal MaintenanceSettings([CanBeNull] ITaskSettings3 iSettings3)
		{
			iSettings = iSettings3;
			if (iSettings3 != null)
				iMaintSettings = iSettings.MaintenanceSettings;
		}

		/// <summary>
		/// Gets or sets the amount of time after which the Task scheduler attempts to run the task during emergency Automatic maintenance, if the task failed to complete during regular Automatic maintenance. The minimum value is one day. The value of the <see cref="Deadline"/> property should be greater than the value of the <see cref="Period"/> property. If the deadline is not specified the task will not be started during emergency Automatic maintenance. 
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.2.</exception>
		[DefaultValue(typeof(TimeSpan), "00:00:00")]
		public TimeSpan Deadline
		{
			get { return iMaintSettings != null ? Task.StringToTimeSpan(iMaintSettings.Deadline) : TimeSpan.Zero; }
			set
			{
				if (iSettings != null)
				{
					if (iMaintSettings == null && value != TimeSpan.Zero)
						iMaintSettings = iSettings.CreateMaintenanceSettings();
					if (iMaintSettings != null)
						iMaintSettings.Deadline = Task.TimeSpanToString(value);
				}
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_2);
			}
		}

		/// <summary>
		/// Gets or sets a value indicating whether the Task Scheduler must start the task during the Automatic maintenance in exclusive mode. The exclusivity is guaranteed only between other maintenance tasks and doesn't grant any ordering priority of the task. If exclusivity is not specified, the task is started in parallel with other maintenance tasks.
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.2.</exception>
		[DefaultValue(false)]
		public bool Exclusive
		{
			get { return iMaintSettings != null && iMaintSettings.Exclusive; }
			set
			{
				if (iSettings != null)
				{
					if (iMaintSettings == null && value)
						iMaintSettings = iSettings.CreateMaintenanceSettings();
					if (iMaintSettings != null)
						iMaintSettings.Exclusive = value;
				}
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_2);
			}
		}

		/// <summary>
		/// Gets or sets the amount of time the task needs to be started during Automatic maintenance. The minimum value is one minute.
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.2.</exception>
		[DefaultValue(typeof(TimeSpan), "00:00:00")]
		public TimeSpan Period
		{
			get { return iMaintSettings != null ? Task.StringToTimeSpan(iMaintSettings.Period) : TimeSpan.Zero; }
			set
			{
				if (iSettings != null)
				{
					if (iMaintSettings == null && value != TimeSpan.Zero)
						iMaintSettings = iSettings.CreateMaintenanceSettings();
					if (iMaintSettings != null)
						iMaintSettings.Period = Task.TimeSpanToString(value);
				}
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_2);
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (iMaintSettings != null)
				Marshal.ReleaseComObject(iMaintSettings);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString() => iMaintSettings != null ? DebugHelper.GetDebugString(this) : base.ToString();

		internal bool IsSet() => (iMaintSettings != null && (iMaintSettings.Period != null || iMaintSettings.Deadline != null || iMaintSettings.Exclusive));
	}

	/// <summary>
	/// Provides the settings that the Task Scheduler service uses to obtain a network profile.
	/// </summary>
	[XmlType(IncludeInSchema = false)]
	[PublicAPI]
	public sealed class NetworkSettings : IDisposable
	{
		private INetworkSettings v2Settings;

		internal NetworkSettings([CanBeNull] INetworkSettings iSettings)
		{
			v2Settings = iSettings;
		}

		/// <summary>
		/// Gets or sets a GUID value that identifies a network profile.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(Guid), "00000000-0000-0000-0000-000000000000")]
		public Guid Id
		{
			get
			{
				string id = null;
				if (v2Settings != null)
					id = v2Settings.Id;
				return string.IsNullOrEmpty(id) ? Guid.Empty : new Guid(id);
			}
			set
			{
				if (v2Settings != null)
					v2Settings.Id = value == Guid.Empty ? null : value.ToString();
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets the name of a network profile. The name is used for display purposes.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(null)]
		public string Name
		{
			get { return v2Settings?.Name; }
			set
			{
				if (v2Settings != null)
					v2Settings.Name = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Settings != null)
				Marshal.ReleaseComObject(v2Settings);
		}

		internal bool IsSet() => (v2Settings != null && (!string.IsNullOrEmpty(v2Settings.Id) || !string.IsNullOrEmpty(v2Settings.Name)));

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (v2Settings != null)
				return DebugHelper.GetDebugString(this);
			return base.ToString();
		}
	}

	/// <summary>
	/// Provides the methods to get information from and control a running task.
	/// </summary>
	[XmlType(IncludeInSchema = false)]
	[PublicAPI]
	public sealed class RunningTask : Task
	{
		private IRunningTask v2RunningTask;

		internal RunningTask([NotNull] TaskService svc, [NotNull] IRegisteredTask iTask, [NotNull] IRunningTask iRunningTask)
			: base(svc, iTask)
		{
			v2RunningTask = iRunningTask;
		}

		internal RunningTask([NotNull] TaskService svc, [NotNull] ITask iTask)
			: base(svc, iTask)
		{
		}

		/// <summary>
		/// Gets the name of the current action that the running task is performing.
		/// </summary>
		public string CurrentAction => v2RunningTask != null ? v2RunningTask.CurrentAction : v1Task.GetApplicationName();

		/// <summary>
		/// Gets the process ID for the engine (process) which is running the task.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		public uint EnginePID
		{
			get
			{
				if (v2RunningTask != null)
					return v2RunningTask.EnginePID;
				throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets the GUID identifier for this instance of the task.
		/// </summary>
		public Guid InstanceGuid => v2RunningTask != null ? new Guid(v2RunningTask.InstanceGuid) : Guid.Empty;

		/// <summary>
		/// Gets the operational state of the running task.
		/// </summary>
		public override TaskState State => v2RunningTask?.State ?? base.State;

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public new void Dispose()
		{
			base.Dispose();
			if (v2RunningTask != null) Marshal.ReleaseComObject(v2RunningTask);
		}

		/// <summary>
		/// Refreshes all of the local instance variables of the task.
		/// </summary>
		public void Refresh() { v2RunningTask?.Refresh(); }
	}

	/// <summary>
	/// Provides the methods that are used to run the task immediately, get any running instances of the task, get or set the credentials that are used to register the task, and the properties that describe the task.
	/// </summary>
	[XmlType(IncludeInSchema = false)]
	[PublicAPI]
	public class Task : IDisposable
	{
		internal ITask v1Task;

		private static readonly DateTime v2InvalidDate = new DateTime(1899, 12, 30);
		private static readonly int osLibMinorVer = GetOSLibraryMinorVersion();

		internal const AccessControlSections defaultAccessControlSections = AccessControlSections.Owner | AccessControlSections.Group | AccessControlSections.Access;
		internal const SecurityInfos defaultSecurityInfosSections = SecurityInfos.Owner | SecurityInfos.Group | SecurityInfos.DiscretionaryAcl;

		private IRegisteredTask v2Task;
		private TaskDefinition myTD;

		internal Task([NotNull] TaskService svc, [NotNull] ITask iTask)
		{
			TaskService = svc;
			v1Task = iTask;
			ReadOnly = false;
		}

		internal Task([NotNull] TaskService svc, [NotNull] IRegisteredTask iTask, ITaskDefinition iDef = null)
		{
			TaskService = svc;
			v2Task = iTask;
			if (iDef != null)
				myTD = new TaskDefinition(iDef);
			ReadOnly = false;
		}

		/// <summary>
		/// Gets the definition of the task.
		/// </summary>
		[NotNull]
		public TaskDefinition Definition => myTD ?? (myTD = v2Task != null ? new TaskDefinition(GetV2Definition(TaskService, v2Task, true)) : new TaskDefinition(v1Task, Name));

		/// <summary>
		/// Gets or sets a Boolean value that indicates if the registered task is enabled.
		/// </summary>
		/// <remarks>As of version 1.8.1, under V1 systems (prior to Vista), this property will immediately update the Disabled state and re-save the current task. If changes have been made to the <see cref="TaskDefinition"/>, then those changes will be saved.</remarks>
		public bool Enabled
		{
			get { return v2Task?.Enabled ?? Definition.Settings.Enabled; }
			set
			{
				if (v2Task != null)
					v2Task.Enabled = value;
				else
				{
					Definition.Settings.Enabled = value;
					Definition.V1Save(null);
				}
			}
		}

		/// <summary>
		/// Gets an instance of the parent folder.
		/// </summary>
		/// <value>
		/// A <see cref="TaskFolder"/> object representing the parent folder of this task.
		/// </value>
		[NotNull]
		public TaskFolder Folder
		{
			get
			{
				if (v2Task == null)
					return TaskService.RootFolder;

				var path = v2Task.Path;
				var parentPath = System.IO.Path.GetDirectoryName(path);
				if (string.IsNullOrEmpty(parentPath) || parentPath == TaskFolder.rootString)
					return TaskService.RootFolder;
				return TaskService.GetFolder(parentPath);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this task instance is active.
		/// </summary>
		/// <value>
		///   <c>true</c> if this task instance is active; otherwise, <c>false</c>.
		/// </value>
		public bool IsActive
		{
			get
			{
				var now = DateTime.Now;
				if (Definition.Settings.Enabled)
				{
					foreach (var trigger in Definition.Triggers)
					{
						if (trigger.Enabled && (now >= trigger.StartBoundary && now <= trigger.EndBoundary))
						{
							if (!(trigger is ICalendarTrigger))
								return true;
							if (DateTime.MinValue != NextRunTime || (trigger is TimeTrigger))
								return true;
						}
					}
				}
				return false;
			}
		}

		/// <summary>
		/// Gets the time the registered task was last run.
		/// </summary>
		/// <value>Returns <see cref="DateTime.MinValue"/> if there are no prior run times.</value>
		public DateTime LastRunTime
		{
			get
			{
				if (v2Task == null) return v1Task.GetMostRecentRunTime();
				var dt = v2Task.LastRunTime;
				return dt == v2InvalidDate ? DateTime.MinValue : dt;
			}
		}

		/// <summary>
		/// Gets the results that were returned the last time the registered task was run.
		/// </summary>
		public int LastTaskResult
		{
			get
			{
				if (v2Task != null)
					return v2Task.LastTaskResult;
				return (int)v1Task.GetExitCode();
			}
		}

		/// <summary>
		/// Gets the name of the registered task.
		/// </summary>
		[NotNull]
		public string Name => v2Task != null ? v2Task.Name : System.IO.Path.GetFileNameWithoutExtension(GetV1Path(v1Task));

		/// <summary>
		/// Gets the time when the registered task is next scheduled to run.
		/// </summary>
		/// <value>Returns <see cref="DateTime.MinValue"/> if there are no future run times.</value>
		/// <remarks>
		/// Potentially breaking change in release 1.8.2. For Task Scheduler 2.0, the return value prior to 1.8.2 would be Dec 30, 1899
		/// if there were no future run times. For 1.0, that value would have been <c>DateTime.MinValue</c>. In release 1.8.2 and later, all 
		/// versions will return <c>DateTime.MinValue</c> if there are no future run times. While this is different from the native 2.0
		/// library, it was deemed more appropriate to have consistency between the two libraries and with other .NET libraries.
		/// </remarks>
		public DateTime NextRunTime
		{
			get
			{
				if (v2Task == null) return v1Task.GetNextRunTime();
				var ret = v2Task.NextRunTime;
				if (ret == DateTime.MinValue || ret == v2InvalidDate)
				{
					var nrts = GetRunTimes(DateTime.Now, DateTime.MaxValue, 1);
					ret = nrts.Length > 0 ? nrts[0] : DateTime.MinValue;
				}
				return ret == v2InvalidDate ? DateTime.MinValue : ret;
			}
		}

		/// <summary>
		/// Gets the number of times the registered task has missed a scheduled run.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		public int NumberOfMissedRuns
		{
			get
			{
				if (v2Task != null)
					return v2Task.NumberOfMissedRuns;

				throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets the path to where the registered task is stored.
		/// </summary>
		[NotNull]
		public string Path
		{
			get
			{
				if (v2Task != null)
					return v2Task.Path;
				return "\\" + Name; // GetV1Path(v1Task);
			}
		}

		/// <summary>
		/// Gets a value indicating whether this task is read only. Only available if <see cref="Microsoft.Win32.TaskScheduler.TaskService.AllowReadOnlyTasks"/> is <c>true</c>.
		/// </summary>
		/// <value>
		///   <c>true</c> if read only; otherwise, <c>false</c>.
		/// </value>
		public bool ReadOnly { get; internal set; }

		/// <summary>
		/// Gets or sets the security descriptor for the task.
		/// </summary>
		/// <value>The security descriptor.</value>
		[Obsolete("This property will be removed in deference to the GetAccessControl, GetSecurityDescriptorSddlForm, SetAccessControl and SetSecurityDescriptorSddlForm methods.")]
		public GenericSecurityDescriptor SecurityDescriptor
		{
			get
			{
				var sddl = GetSecurityDescriptorSddlForm();
				return new RawSecurityDescriptor(sddl);
			}
			set
			{
				SetSecurityDescriptorSddlForm(value.GetSddlForm(defaultAccessControlSections));
			}
		}

		/// <summary>
		/// Gets the operational state of the registered task.
		/// </summary>
		public virtual TaskState State
		{
			get
			{
				if (v2Task != null)
					return v2Task.State;

				V1Reactivate();
				if (!Enabled)
					return TaskState.Disabled;
				switch (v1Task.GetStatus())
				{
					case TaskStatus.Ready:
					case TaskStatus.NeverRun:
					case TaskStatus.NoMoreRuns:
					case TaskStatus.Terminated:
						return TaskState.Ready;
					case TaskStatus.Running:
						return TaskState.Running;
					case TaskStatus.Disabled:
						return TaskState.Disabled;
					// case TaskStatus.NotScheduled:
					// case TaskStatus.NoTriggers:
					// case TaskStatus.NoTriggerTime:
					default:
						return TaskState.Unknown;
				}
			}
		}

		/// <summary>
		/// Gets or sets the <see cref="TaskService"/> that manages this task.
		/// </summary>
		/// <value>The task service.</value>
		public TaskService TaskService { get; }

		/// <summary>
		/// Gets the XML-formatted registration information for the registered task.
		/// </summary>
		public string Xml => v2Task != null ? v2Task.Xml : Definition.XmlText;

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Task != null)
				Marshal.ReleaseComObject(v2Task);
			v1Task = null;
		}

		/// <summary>
		/// Exports the task to the specified file in XML.
		/// </summary>
		/// <param name="outputFileName">Name of the output file.</param>
		public void Export([NotNull] string outputFileName)
		{
			File.WriteAllText(outputFileName, Xml, Encoding.Unicode);
		}

		/// <summary>
		/// Gets a <see cref="TaskSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the task described by the current <see cref="Task"/> object.
		/// </summary>
		/// <returns>A <see cref="TaskSecurity"/> object that encapsulates the access control rules for the current task.</returns>
		public TaskSecurity GetAccessControl() => GetAccessControl(defaultAccessControlSections);

		/// <summary>
		/// Gets a <see cref="TaskSecurity"/> object that encapsulates the specified type of access control list (ACL) entries for the task described by the current <see cref="Task"/> object.
		/// </summary>
		/// <param name="includeSections">One of the <see cref="System.Security.AccessControl.AccessControlSections"/> values that specifies which group of access control entries to retrieve.</param>
		/// <returns>A <see cref="TaskSecurity"/> object that encapsulates the access control rules for the current task.</returns>
		public TaskSecurity GetAccessControl(AccessControlSections includeSections) => new TaskSecurity(this, includeSections);

		/// <summary>
		/// Gets all instances of the currently running registered task.
		/// </summary>
		/// <returns>A <see cref="RunningTaskCollection"/> with all instances of current task.</returns>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[NotNull, ItemNotNull]
		public RunningTaskCollection GetInstances()
		{
			if (v2Task != null)
				return new RunningTaskCollection(TaskService, v2Task.GetInstances(0));
			throw new NotV1SupportedException();
		}

		/// <summary>
		/// Gets the last registration time, looking first at the <see cref="TaskRegistrationInfo.Date"/> value and
		/// then looking for the most recent registration event in the Event Log.
		/// </summary>
		/// <returns><see cref="DateTime"/> of the last registration or <see cref="DateTime.MinValue"/> if no value can be found.</returns>
		public DateTime GetLastRegistrationTime()
		{
			var ret = Definition.RegistrationInfo.Date;
			if (ret == DateTime.MinValue)
			{
				var log = new TaskEventLog(Path, new[] { (int)StandardTaskEventId.JobRegistered }, null, TaskService.TargetServer, TaskService.UserAccountDomain, TaskService.UserName, TaskService.UserPassword);
				if (log.Enabled)
				{
					foreach (var item in log)
					{
						if (item.TimeCreated.HasValue)
							return item.TimeCreated.Value;
					}
				}
			}
			return ret;
		}

		/// <summary>
		/// Gets the times that the registered task is scheduled to run during a specified time.
		/// </summary>
		/// <param name="start">The starting time for the query.</param>
		/// <param name="end">The ending time for the query.</param>
		/// <param name="count">The requested number of runs. A value of 0 will return all times requested.</param>
		/// <returns>The scheduled times that the task will run.</returns>
		[NotNull]
		public DateTime[] GetRunTimes(DateTime start, DateTime end, uint count = 0)
		{
			const ushort TASK_MAX_RUN_TIMES = 1440;

			NativeMethods.SYSTEMTIME stStart = start;
			NativeMethods.SYSTEMTIME stEnd = end;
			var runTimes = IntPtr.Zero;
			var ret = new DateTime[0];
			try
			{
				if (v2Task != null)
					v2Task.GetRunTimes(ref stStart, ref stEnd, ref count, ref runTimes);
				else
				{
					var count1 = (count > 0 && count <= TASK_MAX_RUN_TIMES) ? (ushort)count : TASK_MAX_RUN_TIMES;
					v1Task.GetRunTimes(ref stStart, ref stEnd, ref count1, ref runTimes);
					count = count1;
				}
				ret = InteropUtil.ToArray<NativeMethods.SYSTEMTIME, DateTime>(runTimes, (int)count);
			}
			catch (Exception ex)
			{
				Debug.WriteLine($"Task.GetRunTimes failed: Error {ex}.");
			}
			finally
			{
				Marshal.FreeCoTaskMem(runTimes);
			}
			Debug.WriteLine($"Task.GetRunTimes ({(v2Task != null ? "V2" : "V1")}): Returned {count} items from {stStart} to {stEnd}.");
			return ret;
		}

		/// <summary>
		/// Gets the security descriptor for the task. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="includeSections">Section(s) of the security descriptor to return.</param>
		/// <returns>The security descriptor for the task.</returns>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		public string GetSecurityDescriptorSddlForm(SecurityInfos includeSections = defaultSecurityInfosSections)
		{
			if (v2Task != null)
				return v2Task.GetSecurityDescriptor((int)includeSections);

			throw new NotV1SupportedException();
		}

		/// <summary>
		/// Updates the task with any changes made to the <see cref="Definition"/> by calling <see cref="TaskFolder.RegisterTaskDefinition(string, TaskDefinition)"/> from the currently registered folder using the currently registered name.
		/// </summary>
		/// <exception cref="System.Security.SecurityException">Thrown if task was previously registered with a password.</exception>
		public void RegisterChanges()
		{
			if (Definition.Principal.LogonType == TaskLogonType.InteractiveTokenOrPassword || Definition.Principal.LogonType == TaskLogonType.Password)
				throw new SecurityException("Tasks which have been registered previously with stored passwords must use the TaskFolder.RegisterTaskDefinition method for updates.");
			if (v2Task != null)
				TaskService.GetFolder(System.IO.Path.GetDirectoryName(Path)).RegisterTaskDefinition(Name, Definition);
			else
				TaskService.RootFolder.RegisterTaskDefinition(Name, Definition);
		}

		/// <summary>
		/// Runs the registered task immediately.
		/// </summary>
		/// <param name="parameters">
		/// <para>The parameters used as values in the task actions. A maximum of 32 parameters can be supplied.</para>
		/// <para>
		/// These values can be used in the task action where the $(ArgX) variables are used in the action properties. For more information see
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa383549(v=vs.85).aspx#USING_VARIABLES_IN_ACTION_PROPERTIES">Task Actions</a>.
		/// </para>
		/// </param>
		/// <returns>A <see cref="RunningTask"/> instance that defines the new instance of the task.</returns>
		public RunningTask Run(params string[] parameters)
		{
			if (v2Task != null)
			{
				if (parameters.Length > 32)
					throw new ArgumentOutOfRangeException(nameof(parameters), "A maximum of 32 values is allowed.");
				var irt = v2Task.Run(parameters.Length == 0 ? null : parameters);
				return irt != null ? new RunningTask(TaskService, v2Task, irt) : null;
			}

			v1Task.Run();
			return new RunningTask(TaskService, v1Task);
		}

		/// <summary>Runs the registered task immediately using specified flags and a session identifier.</summary>
		/// <param name="flags">Defines how the task is run.</param>
		/// <param name="sessionID">
		/// <para>The terminal server session in which you want to start the task.</para>
		/// <para>
		/// If the <see cref="TaskRunFlags.UseSessionId"/> value is not passed into the <paramref name="flags"/> parameter, then the value specified
		/// in this parameter is ignored.If the <see cref="TaskRunFlags.UseSessionId"/> value is passed into the flags parameter and the sessionID
		/// value is less than or equal to 0, then an invalid argument error will be returned.
		/// </para>
		/// <para>
		/// If the <see cref="TaskRunFlags.UseSessionId"/> value is passed into the <paramref name="flags"/> parameter and the sessionID value is a
		/// valid session ID greater than 0 and if no value is specified for the user parameter, then the Task Scheduler service will try to start the
		/// task interactively as the user who is logged on to the specified session.
		/// </para>
		/// <para>
		/// If the <see cref="TaskRunFlags.UseSessionId"/> value is passed into the <paramref name="flags"/> parameter and the sessionID value is a
		/// valid session ID greater than 0 and if a user is specified in the user parameter, then the Task Scheduler service will try to start the
		/// task interactively as the user who is specified in the user parameter.
		/// </para>
		/// </param>
		/// <param name="user">The user for which the task runs.</param>
		/// <param name="parameters">
		/// <para>The parameters used as values in the task actions. A maximum of 32 parameters can be supplied.</para>
		/// <para>
		/// These values can be used in the task action where the $(ArgX) variables are used in the action properties. For more information see
		/// <a href="https://msdn.microsoft.com/en-us/library/windows/desktop/aa383549(v=vs.85).aspx#USING_VARIABLES_IN_ACTION_PROPERTIES">Task Actions</a>.
		/// </para>
		/// </param>
		/// <returns>A <see cref="RunningTask"/> instance that defines the new instance of the task.</returns>
		/// <remarks>
		/// <para>
		/// This method will return without error, but the task will not run if the AllowDemandStart property of ITaskSettings is set to false for the task.
		/// </para>
		/// <para>If RunEx is invoked from a disabled task, it will return <c>null</c> and the task will not be run.</para>
		/// </remarks>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		public RunningTask RunEx(TaskRunFlags flags, int sessionID, string user, params string[] parameters)
		{
			if (v2Task != null)
			{
				if (parameters.Length > 32)
					throw new ArgumentOutOfRangeException(nameof(parameters), "A maximum of 32 parameters can be supplied to RunEx.");
				var irt = v2Task.RunEx((parameters.Length == 0) ? null : parameters, (int)flags, sessionID, user);
				return irt != null ? new RunningTask(TaskService, v2Task, irt) : null;
			}
			throw new NotV1SupportedException();
		}

		/// <summary>
		/// Applies access control list (ACL) entries described by a <see cref="TaskSecurity"/> object to the file described by the current <see cref="Task"/> object.
		/// </summary>
		/// <param name="taskSecurity">A <see cref="TaskSecurity"/> object that describes an access control list (ACL) entry to apply to the current task.</param>
		public void SetAccessControl([NotNull] TaskSecurity taskSecurity)
		{
			taskSecurity.Persist(this);
		}

		/// <summary>
		/// Sets the security descriptor for the task. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="sddlForm">The security descriptor for the task.</param>
		/// <param name="options">Flags that specify how to set the security descriptor.</param>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		public void SetSecurityDescriptorSddlForm([NotNull] string sddlForm, TaskSetSecurityOptions options = TaskSetSecurityOptions.None)
		{
			if (v2Task != null)
				v2Task.SetSecurityDescriptor(sddlForm, (int)options);
			else
				throw new NotV1SupportedException();
		}

		/// <summary>
		/// Dynamically tries to load the assembly for the editor and displays it as editable for this task.
		/// </summary>
		/// <returns><c>true</c> if editor returns with OK response; <c>false</c> otherwise.</returns>
		/// <remarks>The Microsoft.Win32.TaskSchedulerEditor.dll assembly must reside in the same directory as the Microsoft.Win32.TaskScheduler.dll or in the GAC.</remarks>
		public bool ShowEditor()
		{
			try
			{
				var t = ReflectionHelper.LoadType("Microsoft.Win32.TaskScheduler.TaskEditDialog", "Microsoft.Win32.TaskSchedulerEditor.dll");
				if (t != null)
					return ReflectionHelper.InvokeMethod<int>(t, new object[] { this, true, true }, "ShowDialog") == 1;
			}
			catch { }
			return false;
		}

		/// <summary>
		/// Shows the property page for the task (v1.0 only).
		/// </summary>
		public void ShowPropertyPage()
		{
			if (v1Task != null)
				v1Task.EditWorkItem(IntPtr.Zero, 0);
			else
				throw new NotV2SupportedException();
		}

		/// <summary>Stops the registered task immediately.</summary>
		/// <remarks>
		/// <para>The <c>Stop</c> method stops all instances of the task.</para>
		/// <para>
		/// System account users can stop a task, users with Administrator group privileges can stop a task, and if a user has rights to execute and
		/// read a task, then the user can stop the task. A user can stop the task instances that are running under the same credentials as the user
		/// account. In all other cases, the user is denied access to stop the task.
		/// </para>
		/// </remarks>
		public void Stop()
		{
			if (v2Task != null)
				v2Task.Stop(0);
			else
				v1Task.Terminate();
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString() => Name;

		internal void V1Reactivate()
		{
			var iTask = TaskService.GetTask(TaskService.v1TaskScheduler, Name);
			if (iTask != null)
				v1Task = iTask;
		}

		internal static Task CreateTask(TaskService svc, IRegisteredTask iTask, bool throwError = false)
		{
			var iDef = GetV2Definition(svc, iTask, throwError);
			if (iDef == null && svc.AllowReadOnlyTasks)
			{
				iDef = GetV2StrippedDefinition(svc, iTask);
				return new Task(svc, iTask, iDef) { ReadOnly = true };
			}
			return new Task(svc, iTask, iDef);
		}

		internal static int GetOSLibraryMinorVersion()
		{
			if (Environment.OSVersion.Version.Major == 6)
			{
				switch (Environment.OSVersion.Version.Minor)
				{
					case 0:
						return 2;
					case 1:
						return 3;
					default:
						return Convert.ToInt32(Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Windows NT\CurrentVersion", "CurrentMajorVersionNumber", 0)) == 10 ? 5 : 4;
				}
			}
			return 1;
		}

		[NotNull]
		internal static string GetV1Path(ITask v1Task)
		{
			string fileName;
			var iFile = (IPersistFile)v1Task;
			iFile.GetCurFile(out fileName);
			return fileName ?? string.Empty;
		}

		/// <summary>
		/// Gets the ITaskDefinition for a V2 task and prevents the errors that come when connecting remotely to a higher version of the Task Scheduler.
		/// </summary>
		/// <param name="svc">The local task service.</param>
		/// <param name="iTask">The task instance.</param>
		/// <param name="throwError">if set to <c>true</c> this method will throw an exception if unable to get the task definition.</param>
		/// <returns>
		/// A valid ITaskDefinition that should not throw errors on the local instance.
		/// </returns>
		/// <exception cref="System.InvalidOperationException">Unable to get a compatible task definition for this version of the library.</exception>
		internal static ITaskDefinition GetV2Definition(TaskService svc, IRegisteredTask iTask, bool throwError = false)
		{
			var xmlVer = new Version();
			try
			{
				var dd = new DefDoc(iTask.Xml);
				xmlVer = dd.Version;
				if (xmlVer.Minor > osLibMinorVer)
				{
					var newMinor = xmlVer.Minor;
					if (!dd.Contains("Volatile", "false", true) &&
						!dd.Contains("MaintenanceSettings"))
						newMinor = 3;
					if (!dd.Contains("UseUnifiedSchedulingEngine", "false", true) &&
						!dd.Contains("DisallowStartOnRemoteAppSession", "false", true) &&
						!dd.Contains("RequiredPrivileges") &&
						!dd.Contains("ProcessTokenSidType", "Default", true))
						newMinor = 2;
					if (!dd.Contains("DisplayName") &&
						!dd.Contains("GroupId") &&
						!dd.Contains("RunLevel", "LeastPrivilege", true) &&
						!dd.Contains("SecurityDescriptor") &&
						!dd.Contains("Source") &&
						!dd.Contains("URI") &&
						!dd.Contains("AllowStartOnDemand", "true", true) &&
						!dd.Contains("AllowHardTerminate", "true", true) &&
						!dd.Contains("MultipleInstancesPolicy", "IgnoreNew", true) &&
						!dd.Contains("NetworkSettings") &&
						!dd.Contains("StartWhenAvailable", "false", true) &&
						!dd.Contains("SendEmail") &&
						!dd.Contains("ShowMessage") &&
						!dd.Contains("ComHandler") &&
						!dd.Contains("EventTrigger") &&
						!dd.Contains("SessionStateChangeTrigger") &&
						!dd.Contains("RegistrationTrigger") &&
						!dd.Contains("RestartOnFailure") &&
						!dd.Contains("LogonType", "None", true))
						newMinor = 1;

					if (newMinor > osLibMinorVer && throwError)
						throw new InvalidOperationException($"The current version of the native library (1.{osLibMinorVer}) does not support the original or minimum version of the \"{iTask.Name}\" task ({xmlVer}/1.{newMinor})");

					if (newMinor != xmlVer.Minor)
					{
						dd.Version = new Version(1, newMinor);
						var def = svc.v2TaskService.NewTask(0);
						def.XmlText = dd.Xml;
						return def;
					}
				}
				return iTask.Definition;
			}
			catch (COMException comEx)
			{
				if (throwError)
				{
					if ((uint)comEx.ErrorCode == 0x80041318 && xmlVer.Minor != osLibMinorVer) // Incorrect XML value
						throw new InvalidOperationException($"The current version of the native library (1.{osLibMinorVer}) does not support the version of the \"{iTask.Name}\" task ({xmlVer})");
					throw;
				}
			}
			catch
			{
				if (throwError)
					throw;
			}
			return null;
		}

		internal static ITaskDefinition GetV2StrippedDefinition(TaskService svc, IRegisteredTask iTask)
		{
			try
			{
				var dd = new DefDoc(iTask.Xml);
				var xmlVer = dd.Version;
				if (xmlVer.Minor > osLibMinorVer)
				{
					if (osLibMinorVer < 4)
					{
						dd.RemoveTag("Volatile");
						dd.RemoveTag("MaintenanceSettings");
					}
					if (osLibMinorVer < 3)
					{
						dd.RemoveTag("UseUnifiedSchedulingEngine");
						dd.RemoveTag("DisallowStartOnRemoteAppSession");
						dd.RemoveTag("RequiredPrivileges");
						dd.RemoveTag("ProcessTokenSidType");
					}
					if (osLibMinorVer < 2)
					{
						dd.RemoveTag("DisplayName");
						dd.RemoveTag("GroupId");
						dd.RemoveTag("RunLevel");
						dd.RemoveTag("SecurityDescriptor");
						dd.RemoveTag("Source");
						dd.RemoveTag("URI");
						dd.RemoveTag("AllowStartOnDemand");
						dd.RemoveTag("AllowHardTerminate");
						dd.RemoveTag("MultipleInstancesPolicy");
						dd.RemoveTag("NetworkSettings");
						dd.RemoveTag("StartWhenAvailable");
						dd.RemoveTag("SendEmail");
						dd.RemoveTag("ShowMessage");
						dd.RemoveTag("ComHandler");
						dd.RemoveTag("EventTrigger");
						dd.RemoveTag("SessionStateChangeTrigger");
						dd.RemoveTag("RegistrationTrigger");
						dd.RemoveTag("RestartOnFailure");
						dd.RemoveTag("LogonType");
					}
					dd.Version = new Version(1, osLibMinorVer);
					var def = svc.v2TaskService.NewTask(0);
#if DEBUG
					var path = System.IO.Path.Combine(System.IO.Path.GetTempPath(), $"TS_Stripped_Def_{xmlVer.Minor}-{osLibMinorVer}_{iTask.Name}.xml");
					File.WriteAllText(path, dd.Xml, Encoding.Unicode);
#endif
					def.XmlText = dd.Xml;
					return def;
				}
			}
			catch (Exception ex) { Debug.WriteLine($"Error in GetV2StrippedDefinition: {ex}"); }
			return iTask.Definition;
		}

		internal static TimeSpan StringToTimeSpan(string input)
		{
			if (!string.IsNullOrEmpty(input))
				try { return XmlConvert.ToTimeSpan(input); } catch { }
			return TimeSpan.Zero;
		}

		internal static string TimeSpanToString(TimeSpan span)
		{
			if (span != TimeSpan.Zero)
				try { return XmlConvert.ToString(span); } catch { }
			return null;
		}

		private class DefDoc
		{
			private readonly XmlDocument doc;

			public DefDoc(string xml)
			{
				doc = new XmlDocument();
				doc.LoadXml(xml);
			}

			public Version Version
			{
				get
				{
					try
					{
						return new Version(doc["Task"].Attributes["version"].Value);
					}
					catch
					{
						throw new InvalidOperationException("Task definition does not contain a version.");
					}
				}
				set
				{
					var task = doc["Task"];
					if (task != null) task.Attributes["version"].Value = value.ToString(2);
				}
			}

			public string Xml => doc.OuterXml;

			public bool Contains(string tag, string defaultVal = null, bool removeIfFound = false)
			{
				var elems = doc.GetElementsByTagName(tag);
				if (elems.Count > 0)
				{
					var e = elems[0];
					if (e.InnerText != defaultVal || !removeIfFound || e.ParentNode == null)
						return true;
					e.ParentNode.RemoveChild(e);
				}
				return false;
			}

			public void RemoveTag(string tag)
			{
				var elems = doc.GetElementsByTagName(tag);
				if (elems.Count > 0)
				{
					var e = elems[0];
					e.ParentNode?.RemoveChild(e);
				}
			}
		}
	}

	/// <summary>
	/// Contains information about the compatibility of the current configuration with a specified version.
	/// </summary>
	[PublicAPI]
	public class TaskCompatibilityEntry
	{
		internal TaskCompatibilityEntry(TaskCompatibility comp, string prop, string reason)
		{
			CompatibilityLevel = comp;
			Property = prop;
			Reason = reason;
		}

		/// <summary>
		/// Gets the compatibility level.
		/// </summary>
		/// <value>
		/// The compatibility level.
		/// </value>
		public TaskCompatibility CompatibilityLevel { get; }

		/// <summary>
		/// Gets the property name with the incompatibility.
		/// </summary>
		/// <value>
		/// The property name.
		/// </value>
		public string Property { get; }

		/// <summary>
		/// Gets the reason for the incompatibility.
		/// </summary>
		/// <value>
		/// The reason.
		/// </value>
		public string Reason { get; }
	}

	/// <summary>
	/// Defines all the components of a task, such as the task settings, triggers, actions, and registration information.
	/// </summary>
	[XmlRoot("Task", Namespace = tns, IsNullable = false)]
	[XmlSchemaProvider("GetV1SchemaFile")]
	[PublicAPI]
	public sealed class TaskDefinition : IDisposable, IXmlSerializable
	{
		internal const string tns = "http://schemas.microsoft.com/windows/2004/02/mit/task";

		internal string v1Name = string.Empty;
		internal ITask v1Task;
		internal ITaskDefinition v2Def;

		private ActionCollection actions;
		private TaskPrincipal principal;
		private TaskRegistrationInfo regInfo;
		private TaskSettings settings;
		private TriggerCollection triggers;

		internal TaskDefinition([NotNull] ITask iTask, string name)
		{
			v1Task = iTask;
			v1Name = name;
		}

		internal TaskDefinition([NotNull] ITaskDefinition iDef)
		{
			v2Def = iDef;
		}

		/// <summary>
		/// Gets a collection of actions that are performed by the task.
		/// </summary>
		[XmlArrayItem(ElementName = "Exec", IsNullable = true, Type = typeof(ExecAction))]
		[XmlArrayItem(ElementName = "ShowMessage", IsNullable = true, Type = typeof(ShowMessageAction))]
		[XmlArrayItem(ElementName = "ComHandler", IsNullable = true, Type = typeof(ComHandlerAction))]
		[XmlArrayItem(ElementName = "SendEmail", IsNullable = true, Type = typeof(EmailAction))]
		[XmlArray]
		[NotNull, ItemNotNull]
		public ActionCollection Actions => actions ?? (actions = v2Def != null ? new ActionCollection(v2Def) : new ActionCollection(v1Task));

		/// <summary>
		/// Gets or sets the data that is associated with the task. This data is ignored by the Task Scheduler service, but is used by third-parties who wish to extend the task format.
		/// </summary>
		[CanBeNull]
		public string Data
		{
			get { return v2Def != null ? v2Def.Data : v1Task.GetDataItem(nameof(Data)); }
			set
			{
				if (v2Def != null)
					v2Def.Data = value;
				else
					v1Task.SetDataItem(nameof(Data), value);
			}
		}

		/// <summary>
		/// Gets the lowest supported version that supports the settings for this <see cref="TaskDefinition"/>.
		/// </summary>
		[XmlIgnore]
		public TaskCompatibility LowestSupportedVersion => GetLowestSupportedVersion(null);

		/// <summary>
		/// Gets the principal for the task that provides the security credentials for the task.
		/// </summary>
		[NotNull]
		public TaskPrincipal Principal => principal ?? (principal = v2Def != null ? new TaskPrincipal(v2Def.Principal) : new TaskPrincipal(v1Task));

		/// <summary>
		/// Gets a class instance of registration information that is used to describe a task, such as the description of the task, the author of the task, and the date the task is registered.
		/// </summary>
		public TaskRegistrationInfo RegistrationInfo => regInfo ?? (regInfo = v2Def != null ? new TaskRegistrationInfo(v2Def.RegistrationInfo) : new TaskRegistrationInfo(v1Task));

		/// <summary>
		/// Gets the settings that define how the Task Scheduler service performs the task.
		/// </summary>
		[NotNull]
		public TaskSettings Settings => settings ?? (settings = v2Def != null ? new TaskSettings(v2Def.Settings) : new TaskSettings(v1Task));

		/// <summary>
		/// Gets a collection of triggers that are used to start a task.
		/// </summary>
		[XmlArrayItem(ElementName = "BootTrigger", IsNullable = true, Type = typeof(BootTrigger))]
		[XmlArrayItem(ElementName = "CalendarTrigger", IsNullable = true, Type = typeof(CalendarTrigger))]
		[XmlArrayItem(ElementName = "IdleTrigger", IsNullable = true, Type = typeof(IdleTrigger))]
		[XmlArrayItem(ElementName = "LogonTrigger", IsNullable = true, Type = typeof(LogonTrigger))]
		[XmlArrayItem(ElementName = "TimeTrigger", IsNullable = true, Type = typeof(TimeTrigger))]
		[XmlArray]
		[NotNull, ItemNotNull]
		public TriggerCollection Triggers => triggers ?? (triggers = v2Def != null ? new TriggerCollection(v2Def) : new TriggerCollection(v1Task));

		/// <summary>
		/// Gets or sets the XML-formatted definition of the task.
		/// </summary>
		[XmlIgnore]
		public string XmlText
		{
			get
			{
				if (v2Def != null)
					return v2Def.XmlText;
				return XmlSerializationHelper.WriteObjectToXmlText(this);
			}
			set
			{
				if (v2Def != null)
					v2Def.XmlText = value;
				else
					XmlSerializationHelper.ReadObjectFromXmlText(value, this);
			}
		}

		/// <summary>
		/// Determines whether this <see cref="TaskDefinition"/> can use the Unified Scheduling Engine or if it contains unsupported properties.
		/// </summary>
		/// <param name="throwExceptionWithDetails">if set to <c>true</c> throws an <see cref="InvalidOperationException"/> with details about unsupported properties in the Data property of the exception.</param>
		/// <returns><c>true</c> if this <see cref="TaskDefinition"/> can use the Unified Scheduling Engine; otherwise, <c>false</c>.</returns>
		public bool CanUseUnifiedSchedulingEngine(bool throwExceptionWithDetails = false)
		{
			var ex = new InvalidOperationException { HelpLink = "http://msdn.microsoft.com/en-us/library/windows/desktop/aa384138(v=vs.85).aspx" };
			var bad = false;
			if (Principal.LogonType == TaskLogonType.InteractiveTokenOrPassword)
			{
				bad = true;
				if (!throwExceptionWithDetails) return false;
				TryAdd(ex.Data, "Principal.LogonType", "== TaskLogonType.InteractiveTokenOrPassword");
			}
			if (Settings.MultipleInstances == TaskInstancesPolicy.StopExisting)
			{
				bad = true;
				if (!throwExceptionWithDetails) return false;
				TryAdd(ex.Data, "Settings.MultipleInstances", "== TaskInstancesPolicy.StopExisting");
			}
			if (Settings.NetworkSettings.Id != Guid.Empty)
			{
				bad = true;
				if (!throwExceptionWithDetails) return false;
				TryAdd(ex.Data, "Settings.NetworkSettings.Id", "!= Guid.Empty");
			}
			if (!Settings.AllowHardTerminate)
			{
				bad = true;
				if (!throwExceptionWithDetails) return false;
				TryAdd(ex.Data, "Settings.AllowHardTerminate", "== false");
			}
			for (var i = 0; i < Actions.Count; i++)
			{
				var a = Actions[i];
				if (a is EmailAction)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Actions[{i}]", "== typeof(EmailAction)");
				}
				else if (a is ShowMessageAction)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Actions[{i}]", "== typeof(ShowMessageAction)");
				}
			}
			for (var i = 0; i < Triggers.Count; i++)
			{
				Trigger t;
				try { t = Triggers[i]; }
				catch
				{
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}]", "is irretrievable.");
					continue;
				}
				if (t is MonthlyTrigger)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}]", "== typeof(MonthlyTrigger)");
				}
				if (t is MonthlyDOWTrigger)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}]", "== typeof(MonthlyDOWTrigger)");
				}
				if (t.ExecutionTimeLimit != TimeSpan.Zero)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}].ExecutionTimeLimit", "!= TimeSpan.Zero");
				}
				if (t is ICalendarTrigger && t.Repetition.IsSet())
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}].Repetition", "");
				}
				if (t is EventTrigger && ((EventTrigger)t).ValueQueries.Count > 0)
				{
					bad = true;
					if (!throwExceptionWithDetails) return false;
					TryAdd(ex.Data, $"Triggers[{i}].ValueQueries.Count", "!= 0");
				}
			}
			if (throwExceptionWithDetails)
				throw ex;
			return !bad;
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			regInfo = null;
			triggers = null;
			settings = null;
			principal = null;
			actions = null;
			if (v2Def != null) Marshal.ReleaseComObject(v2Def);
			v1Task = null;
		}

		/// <summary>
		/// Gets the XML Schema file for V1 tasks.
		/// </summary>
		/// <param name="xs">The <see cref="System.Xml.Schema.XmlSchemaSet"/> for V1 tasks.</param>
		/// <returns>An object containing the XML Schema for V1 tasks.</returns>
		public static XmlSchemaComplexType GetV1SchemaFile([NotNull] XmlSchemaSet xs)
		{
			XmlSchema schema;
			using (var xsdFile = Assembly.GetAssembly(typeof(TaskDefinition)).GetManifestResourceStream("Microsoft.Win32.TaskScheduler.V1.TaskSchedulerV1Schema.xsd"))
			{
				var schemaSerializer = new XmlSerializer(typeof(XmlSchema));
				schema = (XmlSchema)schemaSerializer.Deserialize(XmlReader.Create(xsdFile));
				xs.Add(schema);
			}

			// target namespace
			var name = new XmlQualifiedName("taskType", tns);
			var productType = (XmlSchemaComplexType)schema.SchemaTypes[name];

			return productType;
		}

		/// <summary>
		/// Validates the current <see cref="TaskDefinition"/>.
		/// </summary>
		/// <param name="throwException">if set to <c>true</c> throw a <see cref="InvalidOperationException"/> with details about invalid properties.</param>
		/// <returns><c>true</c> if current <see cref="TaskDefinition"/> is valid; <c>false</c> if not.</returns>
		public bool Validate(bool throwException = false)
		{
			var ex = new InvalidOperationException();
			if (Settings.UseUnifiedSchedulingEngine)
			{
				try { CanUseUnifiedSchedulingEngine(throwException); }
				catch (InvalidOperationException iox)
				{
					foreach (DictionaryEntry kvp in iox.Data)
						TryAdd(ex.Data, (kvp.Key as ICloneable)?.Clone() ?? kvp.Key, (kvp.Value as ICloneable)?.Clone() ?? kvp.Value);
				}
			}

			if (Settings.Compatibility >= TaskCompatibility.V2_2)
			{
				var PT1D = TimeSpan.FromDays(1);
				if (Settings.MaintenanceSettings.IsSet() && (Settings.MaintenanceSettings.Period < PT1D || Settings.MaintenanceSettings.Deadline < PT1D || Settings.MaintenanceSettings.Deadline <= Settings.MaintenanceSettings.Period))
					TryAdd(ex.Data, "Settings.MaintenanceSettings", "Period or Deadline must be at least 1 day and Deadline must be greater than Period.");
			}

			var list = new List<TaskCompatibilityEntry>();
			if (GetLowestSupportedVersion(list) > Settings.Compatibility)
				foreach (var item in list)
					TryAdd(ex.Data, item.Property, item.Reason);

			var startWhenAvailable = Settings.StartWhenAvailable;
			var delOldTask = Settings.DeleteExpiredTaskAfter != TimeSpan.Zero;
			var v1 = Settings.Compatibility < TaskCompatibility.V2;
			var hasEndBound = false;
			for (var i = 0; i < Triggers.Count; i++)
			{
				Trigger trigger;
				try { trigger = Triggers[i]; }
				catch
				{
					TryAdd(ex.Data, $"Triggers[{i}]", "is irretrievable.");
					continue;
				}
				if (startWhenAvailable && trigger.Repetition.Duration != TimeSpan.Zero && trigger.EndBoundary == DateTime.MaxValue)
					TryAdd(ex.Data, "Settings.StartWhenAvailable", "== true requires time-based tasks with an end boundary or time-based tasks that are set to repeat infinitely.");
				if (v1 && trigger.Repetition.Interval != TimeSpan.Zero && trigger.Repetition.Interval >= trigger.Repetition.Duration)
					TryAdd(ex.Data, "Trigger.Repetition.Interval", ">= Trigger.Repetition.Duration under Task Scheduler 1.0.");
				if (delOldTask && trigger.EndBoundary != DateTime.MaxValue)
					hasEndBound = true;
			}
			if (delOldTask && !hasEndBound)
				TryAdd(ex.Data, "Settings.DeleteExpiredTaskAfter", "!= TimeSpan.Zero requires at least one trigger with an end boundary.");

			if (throwException && ex.Data.Count > 0)
				throw ex;
			return ex.Data.Count == 0;
		}

		private static void TryAdd(IDictionary d, object k, object v)
		{
			if (!d.Contains(k))
				d.Add(k, v);
		}

		internal void V1Save(string newName)
		{
			if (v1Task != null)
			{
				Triggers.Bind();

				var iFile = (IPersistFile)v1Task;
				if (string.IsNullOrEmpty(newName) || newName == v1Name)
				{
					try
					{
						iFile.Save(null, false);
						iFile = null;
						return;
					}
					catch { }
				}

				string path;
				iFile.GetCurFile(out path);
				File.Delete(path);
				path = Path.GetDirectoryName(path) + Path.DirectorySeparatorChar + newName + Path.GetExtension(path);
				File.Delete(path);
				iFile.Save(path, true);
			}
		}

		/// <summary>
		/// Gets the lowest supported version.
		/// </summary>
		/// <param name="outputList">The output list.</param>
		/// <returns></returns>
		private TaskCompatibility GetLowestSupportedVersion(IList<TaskCompatibilityEntry> outputList = null)
		{
			var res = TaskCompatibility.V1;
			var list = new List<TaskCompatibilityEntry>();

			//if (Principal.DisplayName != null)
			//	{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Principal.DisplayName", "cannot have a value.")); }
			if (Principal.GroupId != null)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Principal.GroupId", "cannot have a value.")); }
				//this.Principal.Id != null ||
			if ((Principal.LogonType == TaskLogonType.Group || Principal.LogonType == TaskLogonType.None || Principal.LogonType == TaskLogonType.S4U))
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Principal.LogonType", "cannot be Group, None or S4U.")); }
			if (Principal.RunLevel == TaskRunLevel.Highest)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Principal.RunLevel", "cannot be set to Highest.")); }
			if (RegistrationInfo.SecurityDescriptorSddlForm != null)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "RegistrationInfo.SecurityDescriptorSddlForm", "cannot have a value.")); }
			//if (RegistrationInfo.Source != null)
			//	{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "RegistrationInfo.Source", "cannot have a value.")); }
			//if (RegistrationInfo.URI != null)
			//	{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "RegistrationInfo.URI", "cannot have a value.")); }
			//if (RegistrationInfo.Version != new Version(1, 0))
			//	{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "RegistrationInfo.Version", "cannot be set or equal 1.0.")); }
			if (Settings.AllowDemandStart == false)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.AllowDemandStart", "must be true.")); }
			if (Settings.AllowHardTerminate == false)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.AllowHardTerminate", "must be true.")); }
			if (Settings.MultipleInstances != TaskInstancesPolicy.IgnoreNew)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.MultipleInstances", "must be set to IgnoreNew.")); }
			if (Settings.NetworkSettings.IsSet())
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.NetworkSetting", "cannot have a value.")); }
			if (Settings.RestartCount != 0)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.RestartCount", "must be 0.")); }
			if (Settings.RestartInterval != TimeSpan.Zero)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.RestartInterval", "must be 0 (TimeSpan.Zero).")); }
			if (Settings.StartWhenAvailable)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Settings.StartWhenAvailable", "must be false.")); }

			if ((Actions.PowerShellConversion & PowerShellActionPlatformOption.Version1) != PowerShellActionPlatformOption.Version1 && (Actions.ContainsType(typeof(EmailAction)) || Actions.ContainsType(typeof(ShowMessageAction)) || Actions.ContainsType(typeof(ComHandlerAction))))
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Actions", "may only contain ExecAction types unless Actions.PowerShellConversion includes Version1.")); }
			if ((Actions.PowerShellConversion & PowerShellActionPlatformOption.Version2) != PowerShellActionPlatformOption.Version2 && (Actions.ContainsType(typeof(EmailAction)) || Actions.ContainsType(typeof(ShowMessageAction))))
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_1, "Actions", "may only contain ExecAction and ComHanlderAction types unless Actions.PowerShellConversion includes Version2.")); }

			try
			{
				if (null != Triggers.Find(t => (t is ITriggerDelay && ((ITriggerDelay)t).Delay != TimeSpan.Zero)))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain delays.")); }
				if (null != Triggers.Find(t => t.ExecutionTimeLimit != TimeSpan.Zero || t.Id != null))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain an ExecutionTimeLimit or Id.")); }
				if (null != Triggers.Find(t => ((t as LogonTrigger)?.UserId != null)))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain a LogonTrigger with a UserId.")); }
				if (null != Triggers.Find(t => (t is MonthlyDOWTrigger && ((MonthlyDOWTrigger)t).RunOnLastWeekOfMonth) || (t is MonthlyDOWTrigger && ((((MonthlyDOWTrigger)t).WeeksOfMonth & (((MonthlyDOWTrigger)t).WeeksOfMonth - 1)) != 0))))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain a MonthlyDOWTrigger with RunOnLastWeekOfMonth set or multiple WeeksOfMonth.")); }
				if (null != Triggers.Find(t => (t is MonthlyTrigger && ((MonthlyTrigger)t).RunOnLastDayOfMonth)))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain a MonthlyTrigger with RunOnLastDayOfMonth set.")); }
				if (Triggers.ContainsType(typeof(EventTrigger)) || Triggers.ContainsType(typeof(SessionStateChangeTrigger)) || Triggers.ContainsType(typeof(RegistrationTrigger)))
					{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain EventTrigger, SessionStateChangeTrigger, or RegistrationTrigger types.")); }
			}
			catch
			{
				list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2, "Triggers", "cannot contain Custom triggers."));
			}
			
			if (Principal.ProcessTokenSidType != TaskProcessTokenSidType.Default)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_1, "Principal.ProcessTokenSidType", "must be Default.")); }
			if (Principal.RequiredPrivileges.Count > 0)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_1, "Principal.RequiredPrivileges", "must be empty.")); }
			if (Settings.DisallowStartOnRemoteAppSession)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_1, "Settings.DisallowStartOnRemoteAppSession", "must be false.")); }
			if (Settings.UseUnifiedSchedulingEngine)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_1, "Settings.UseUnifiedSchedulingEngine", "must be false.")); }

			if (Settings.MaintenanceSettings.IsSet())
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_2, "this.Settings.MaintenanceSettings", "must have no values set.")); }
			if (Settings.Volatile)
				{ list.Add(new TaskCompatibilityEntry(TaskCompatibility.V2_2, " this.Settings.Volatile", "must be false.")); }

			foreach (var item in list)
			{
				if (res < item.CompatibilityLevel) res = item.CompatibilityLevel;
				outputList?.Add(item);
			}
			return res;
		}

		internal static Dictionary<string, string> GetV1TaskDataDictionary(ITask v1Task)
		{
			Dictionary<string, string> dict;
			var o = GetV1TaskData(v1Task);
			if (o is string)
				dict = new Dictionary<string, string> {{"Data", o.ToString()}, {"Documentation", o.ToString()}};
			else
				dict = o as Dictionary<string, string>;
			return dict ?? new Dictionary<string, string>();
		}

		private static object GetV1TaskData(ITask v1Task)
		{
			var Data = IntPtr.Zero;
			try
			{
				ushort DataLen;
				v1Task.GetWorkItemData(out DataLen, out Data);
				if (DataLen == 0)
					return null;
				var bytes = new byte[DataLen];
				Marshal.Copy(Data, bytes, 0, DataLen);
				var stream = new MemoryStream(bytes, false);
				var b = new BinaryFormatter();
				return b.Deserialize(stream);
			}
			catch { }
			finally
			{
				if (Data != IntPtr.Zero)
					Marshal.FreeCoTaskMem(Data);
			}
			return null;
		}

		internal static void SetV1TaskData(ITask v1Task, object value)
		{
			if (value == null)
				v1Task.SetWorkItemData(0, null);
			else
			{
				var b = new BinaryFormatter();
				var stream = new MemoryStream();
				b.Serialize(stream, value);
				v1Task.SetWorkItemData((ushort)stream.Length, stream.ToArray());
			}
		}

		XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(XmlSerializationHelper.GetElementName(this), tns);
			XmlSerializationHelper.ReadObjectProperties(reader, this);
			reader.ReadEndElement();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			// TODO:FIX writer.WriteAttributeString("version", "1.1");
			XmlSerializationHelper.WriteObjectProperties(writer, this);
		}
	}

	/// <summary>
	/// Provides the security credentials for a principal. These security credentials define the security context for the tasks that are associated with the principal.
	/// </summary>
	[XmlRoot("Principals", Namespace = TaskDefinition.tns, IsNullable = true)]
	[PublicAPI]
	public sealed class TaskPrincipal : IDisposable, IXmlSerializable
	{
		private const string localSystemAcct = "SYSTEM";
		private string v1CachedAcctInfo;
		private ITask v1Task;
		private IPrincipal v2Principal;
		private IPrincipal2 v2Principal2;
		private TaskPrincipalPrivileges reqPriv;

		internal TaskPrincipal([NotNull] IPrincipal iPrincipal)
		{
			v2Principal = iPrincipal;
			try { v2Principal2 = (IPrincipal2)v2Principal; }
			catch { }
		}

		internal TaskPrincipal([NotNull] ITask iTask)
		{
			v1Task = iTask;
		}

		/// <summary>
		/// Gets or sets the name of the principal that is displayed in the Task Scheduler UI.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(null)]
		public string DisplayName
		{
			get { return v2Principal != null ? v2Principal.DisplayName : v1Task.GetDataItem("PrincipalDisplayName"); }
			set
			{
				if (v2Principal != null)
					v2Principal.DisplayName = value;
				else
					v1Task.SetDataItem("PrincipalDisplayName", value);
			}
		}

		/// <summary>
		/// Gets or sets the identifier of the user group that is required to run the tasks that are associated with the principal. Setting this property to something other than a null or empty string, will set the <see cref="UserId"/> property to NULL and will set the <see cref="LogonType"/> property to TaskLogonType.Group;
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(null)]
		[XmlIgnore]
		public string GroupId
		{
			get { return v2Principal?.GroupId; }
			set
			{

				if (v2Principal != null)
				{
					if (string.IsNullOrEmpty(value))
						value = null;
					else
					{
						v2Principal.UserId = null;
						v2Principal.LogonType = TaskLogonType.Group;
					}
					v2Principal.GroupId = value;
				}
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets the identifier of the principal.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(null)]
		[XmlAttribute(AttributeName = "id", DataType = "ID")]
		public string Id
		{
			get { return v2Principal != null ? v2Principal.Id : v1Task.GetDataItem("PrincipalId"); }
			set
			{
				if (v2Principal != null)
					v2Principal.Id = value;
				else
					v1Task.SetDataItem("PrincipalId", value);
			}
		}

		/// <summary>
		/// Gets or sets the security logon method that is required to run the tasks that are associated with the principal.
		/// </summary>
		/// <exception cref="NotV1SupportedException">TaskLogonType values of Group, None, or S4UNot are not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(TaskLogonType), "None")]
		public TaskLogonType LogonType
		{
			get
			{
				if (v2Principal != null)
					return v2Principal.LogonType;
				if (UserId == localSystemAcct)
					return TaskLogonType.ServiceAccount;
				if (v1Task.HasFlags(TaskFlags.RunOnlyIfLoggedOn))
					return TaskLogonType.InteractiveToken;
				return TaskLogonType.InteractiveTokenOrPassword;
			}
			set
			{
				if (v2Principal != null)
					v2Principal.LogonType = value;
				else
				{
					if (value == TaskLogonType.Group || value == TaskLogonType.None || value == TaskLogonType.S4U)
						throw new NotV1SupportedException();
					v1Task.SetFlags(TaskFlags.RunOnlyIfLoggedOn, value == TaskLogonType.InteractiveToken);
				}
			}
		}

		/// <summary>
		/// Gets or sets the task process security identifier (SID) type.
		/// </summary>
		/// <value>
		/// One of the <see cref="TaskProcessTokenSidType"/> enumeration constants.
		/// </value>
		/// <remarks>Setting this value appears to break the Task Scheduler MMC and does not output in XML. Removed to prevent problems.</remarks>
		/// <exception cref="NotSupportedPriorToException">Not supported under Task Scheduler versions prior to 2.1.</exception>
		[XmlIgnore, DefaultValue(typeof(TaskProcessTokenSidType), "Default")]
		public TaskProcessTokenSidType ProcessTokenSidType
		{
			get { return v2Principal2?.ProcessTokenSidType ?? TaskProcessTokenSidType.Default; }
			set
			{
				if (v2Principal2 != null)
					v2Principal2.ProcessTokenSidType = value;
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_1);
			}
		}

		/// <summary>
		/// Gets the security credentials for a principal. These security credentials define the security context for the tasks that are associated with the principal.
		/// </summary>
		/// <remarks>Setting this value appears to break the Task Scheduler MMC and does not output in XML. Removed to prevent problems.</remarks>
		[XmlIgnore]
		public TaskPrincipalPrivileges RequiredPrivileges => reqPriv ?? (reqPriv = new TaskPrincipalPrivileges(v2Principal2));

		/// <summary>
		/// Gets or sets the identifier that is used to specify the privilege level that is required to run the tasks that are associated with the principal.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(TaskRunLevel), "LUA")]
		[XmlIgnore]
		public TaskRunLevel RunLevel
		{
			get { return v2Principal?.RunLevel ?? TaskRunLevel.LUA; }
			set
			{
				if (v2Principal != null)
					v2Principal.RunLevel = value;
				else if (value != TaskRunLevel.LUA)
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets the user identifier that is required to run the tasks that are associated with the principal. Setting this property to something other than a null or empty string, will set the <see cref="GroupId"/> property to NULL;
		/// </summary>
		[DefaultValue(null)]
		public string UserId
		{
			get
			{
				if (v2Principal != null)
					return v2Principal.UserId;
				if (v1CachedAcctInfo == null)
				{
					try
					{
						string acct = v1Task.GetAccountInformation();
						v1CachedAcctInfo = string.IsNullOrEmpty(acct) ? localSystemAcct : acct;
					}
					catch { v1CachedAcctInfo = string.Empty; }
				}
				return v1CachedAcctInfo == string.Empty ? null : v1CachedAcctInfo;
			}
			set
			{
				if (v2Principal != null)
				{
					if (string.IsNullOrEmpty(value))
						value = null;
					else
					{
						v2Principal.GroupId = null;
						//if (value.Contains(@"\") && !value.Contains(@"\\"))
						//	value = value.Replace(@"\", @"\\");
					}
					v2Principal.UserId = value;
				}
				else
				{
					if (value.Equals(localSystemAcct, StringComparison.CurrentCultureIgnoreCase))
						value = "";
					v1Task.SetAccountInformation(value, IntPtr.Zero);
					v1CachedAcctInfo = null;
				}
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Principal != null)
				Marshal.ReleaseComObject(v2Principal);
			v1Task = null;
		}

		/// <summary>
		/// Gets a value indicating whether current Principal settings require a password to be provided.
		/// </summary>
		/// <value><c>true</c> if settings requires a password to be provided; otherwise, <c>false</c>.</value>
		public bool RequiresPassword() => LogonType == TaskLogonType.InteractiveTokenOrPassword ||
			LogonType == TaskLogonType.Password || (LogonType == TaskLogonType.S4U && UserId != null && string.Compare(UserId, WindowsIdentity.GetCurrent().Name, StringComparison.OrdinalIgnoreCase) != 0);

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents this instance.
		/// </returns>
		public override string ToString() => LogonType == TaskLogonType.Group ? GroupId : UserId;

		/// <summary>
		/// Validates the supplied account against the supplied <see cref="TaskProcessTokenSidType"/>.
		/// </summary>
		/// <param name="acct">The user or group account name.</param>
		/// <param name="sidType">The SID type for the process.</param>
		/// <returns><c>true</c> if supplied account can be used for the supplied SID type.</returns>
		public static bool ValidateAccountForSidType(string acct, TaskProcessTokenSidType sidType)
		{
			string[] validUserIds = { "NETWORK SERVICE", "LOCAL SERVICE", "S-1-5-19", "S-1-5-20" };
			if (sidType != TaskProcessTokenSidType.Default)
			{
				if (Array.Find(validUserIds, id => id.Equals(acct, StringComparison.InvariantCultureIgnoreCase)) == null)
					return false;
			}
			return true;
		}

		XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			reader.ReadStartElement(XmlSerializationHelper.GetElementName(this), TaskDefinition.tns);
			if (reader.HasAttributes)
				Id = reader.GetAttribute("id");
			reader.Read();
			while (reader.MoveToContent() == XmlNodeType.Element)
			{
				switch (reader.LocalName)
				{
					case "Principal":
						reader.Read();
						XmlSerializationHelper.ReadObjectProperties(reader, this);
						reader.ReadEndElement();
						break;
					default:
						reader.Skip();
						break;
				}
			}
			reader.ReadEndElement();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			if (!string.IsNullOrEmpty(ToString()) || LogonType != TaskLogonType.None)
			{
				writer.WriteStartElement("Principal");
				if (!string.IsNullOrEmpty(Id))
					writer.WriteAttributeString("id", Id);
				XmlSerializationHelper.WriteObjectProperties(writer, this);
				writer.WriteEndElement();
			}
		}
	}

	/// <summary>
	/// List of security credentials for a principal under version 1.3 of the Task Scheduler. These security credentials define the security context for the tasks that are associated with the principal.
	/// </summary>
	[PublicAPI]
	public sealed class TaskPrincipalPrivileges : IList<TaskPrincipalPrivilege>
	{
		private IPrincipal2 v2Principal2;

		internal TaskPrincipalPrivileges(IPrincipal2 iPrincipal2 = null)
		{
			v2Principal2 = iPrincipal2;
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1"/>.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <returns>The index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
		public int IndexOf(TaskPrincipalPrivilege item)
		{
			for (var i = 0; i < Count; i++)
			{
				if (item == this[i])
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Inserts an item to the <see cref="T:System.Collections.Generic.IList`1"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
		/// <param name="item">The object to insert into the <see cref="T:System.Collections.Generic.IList`1"/>.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		void IList<TaskPrincipalPrivilege>.Insert(int index, TaskPrincipalPrivilege item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Removes the <see cref="T:System.Collections.Generic.IList`1"/> item at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the item to remove.</param>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		void IList<TaskPrincipalPrivilege>.RemoveAt(int index)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Gets or sets the element at the specified index.
		/// </summary>
		/// <returns>The element at the specified index.</returns>
		/// <exception cref="T:System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in the <see cref="T:System.Collections.Generic.IList`1"/>.</exception>
		/// <exception cref="T:System.NotSupportedException">The property is set and the <see cref="T:System.Collections.Generic.IList`1"/> is read-only.</exception>
		public TaskPrincipalPrivilege this[int index]
		{
			get
			{
				if (v2Principal2 != null)
					return (TaskPrincipalPrivilege)Enum.Parse(typeof(TaskPrincipalPrivilege), v2Principal2[index + 1]);
				throw new IndexOutOfRangeException();
			}
			set { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Adds an item to the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to add to the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		public void Add(TaskPrincipalPrivilege item)
		{
			if (v2Principal2 != null)
				v2Principal2.AddRequiredPrivilege(item.ToString());
			else
				throw new NotSupportedPriorToException(TaskCompatibility.V2_1);
		}

		/// <summary>
		/// Removes all items from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		void ICollection<TaskPrincipalPrivilege>.Clear()
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1"/> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>true if <paramref name="item"/> is found in the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false.</returns>
		public bool Contains(TaskPrincipalPrivilege item) => (IndexOf(item) != -1);

		/// <summary>
		/// Copies to.
		/// </summary>
		/// <param name="array">The array.</param>
		/// <param name="arrayIndex">Index of the array.</param>
		public void CopyTo(TaskPrincipalPrivilege[] array, int arrayIndex)
		{
			using (var pEnum = GetEnumerator())
			{
				for (var i = arrayIndex; i < array.Length; i++)
				{
					if (!pEnum.MoveNext())
						break;
					array[i] = pEnum.Current;
				}
			}
		}

		/// <summary>
		/// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <returns>The number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		public int Count => v2Principal2?.RequiredPrivilegeCount ?? 0;

		/// <summary>
		/// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
		/// </summary>
		/// <returns>true if the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only; otherwise, false.</returns>
		public bool IsReadOnly => false;

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1"/>.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1"/>.</param>
		/// <returns>true if <paramref name="item"/> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1"/>; otherwise, false. This method also returns false if <paramref name="item"/> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1"/>.</returns>
		/// <exception cref="T:System.NotSupportedException">The <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.</exception>
		bool ICollection<TaskPrincipalPrivilege>.Remove(TaskPrincipalPrivilege item)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Returns an enumerator that iterates through the collection.
		/// </summary>
		/// <returns>A <see cref="T:System.Collections.Generic.IEnumerator`1"/> that can be used to iterate through the collection.</returns>
		public IEnumerator<TaskPrincipalPrivilege> GetEnumerator() => new TaskPrincipalPrivilegesEnumerator(v2Principal2);

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

		/// <summary>
		/// Enumerates the privileges set for a principal under version 1.3 of the Task Scheduler.
		/// </summary>
		public sealed class TaskPrincipalPrivilegesEnumerator : IEnumerator<TaskPrincipalPrivilege>
		{
			private int cur;
			private readonly IPrincipal2 v2Principal2;

			internal TaskPrincipalPrivilegesEnumerator(IPrincipal2 iPrincipal2 = null)
			{
				v2Principal2 = iPrincipal2;
				Reset();
			}

			/// <summary>
			/// Gets the element in the collection at the current position of the enumerator.
			/// </summary>
			/// <returns>The element in the collection at the current position of the enumerator.</returns>
			public TaskPrincipalPrivilege Current { get; private set; }

			object IEnumerator.Current => Current;

			/// <summary>
			/// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
			/// </summary>
			public void Dispose() { }

			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns>true if the enumerator was successfully advanced to the next element; false if the enumerator has passed the end of the collection.</returns>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			public bool MoveNext()
			{
				if (v2Principal2 != null && cur < v2Principal2.RequiredPrivilegeCount)
				{
					cur++;
					Current = (TaskPrincipalPrivilege)Enum.Parse(typeof(TaskPrincipalPrivilege), v2Principal2[cur]);
					return true;
				}
				Current = 0;
				return false;
			}

			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			/// <exception cref="T:System.InvalidOperationException">The collection was modified after the enumerator was created.</exception>
			public void Reset()
			{
				cur = 0;
				Current = 0;
			}
		}
	}

	/// <summary>
	/// Provides the administrative information that can be used to describe the task. This information includes details such as a description of the task, the author of the task, the date the task is registered, and the security descriptor of the task.
	/// </summary>
	[XmlRoot("RegistrationInfo", Namespace = TaskDefinition.tns, IsNullable = true)]
	[PublicAPI]
	public sealed class TaskRegistrationInfo : IDisposable, IXmlSerializable
	{
		private ITask v1Task;
		private IRegistrationInfo v2RegInfo;

		internal TaskRegistrationInfo([NotNull] IRegistrationInfo iRegInfo)
		{
			v2RegInfo = iRegInfo;
		}

		internal TaskRegistrationInfo([NotNull] ITask iTask)
		{
			v1Task = iTask;
		}

		/// <summary>
		/// Gets or sets the author of the task.
		/// </summary>
		[DefaultValue(null)]
		public string Author
		{
			get { return v2RegInfo != null ? v2RegInfo.Author : v1Task.GetCreator(); }
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Author = value;
				else
					v1Task.SetCreator(value);
			}
		}

		/// <summary>
		/// Gets or sets the date and time when the task is registered.
		/// </summary>
		[DefaultValue(typeof(DateTime), "0001-01-01T00:00:00")]
		public DateTime Date
		{
			get
			{
				if (v2RegInfo != null)
				{
					DateTime ret;
					if (DateTime.TryParse(v2RegInfo.Date, Trigger.DefaultDateCulture, DateTimeStyles.AssumeLocal, out ret))
						return ret;
				}
				else
				{
					var v1Path = Task.GetV1Path(v1Task);
					if (!string.IsNullOrEmpty(v1Path) && File.Exists(v1Path))
						return File.GetLastWriteTime(v1Path);
				}
				return DateTime.MinValue;
			}
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Date = value == DateTime.MinValue ? null : value.ToString(Trigger.V2BoundaryDateFormat, Trigger.DefaultDateCulture);
				else
				{
					var v1Path = Task.GetV1Path(v1Task);
					if (!string.IsNullOrEmpty(v1Path) && File.Exists(v1Path))
						File.SetLastWriteTime(v1Path, value);
				}
			}
		}

		/// <summary>
		/// Gets or sets the description of the task.
		/// </summary>
		[DefaultValue(null)]
		public string Description
		{
			get { return v2RegInfo != null ? FixCrLf(v2RegInfo.Description) : v1Task.GetComment(); }
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Description = value;
				else
					v1Task.SetComment(value);
			}
		}

		/// <summary>
		/// Gets or sets any additional documentation for the task.
		/// </summary>
		[DefaultValue(null)]
		public string Documentation
		{
			get { return v2RegInfo != null ? FixCrLf(v2RegInfo.Documentation) : v1Task.GetDataItem(nameof(Documentation)); }
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Documentation = value;
				else
					v1Task.SetDataItem(nameof(Documentation), value);
			}
		}

		/// <summary>
		/// Gets or sets the security descriptor of the task.
		/// </summary>
		/// <value>The security descriptor.</value>
		[XmlIgnore]
		public GenericSecurityDescriptor SecurityDescriptor
		{
			get { return new RawSecurityDescriptor(SecurityDescriptorSddlForm); }
			set { SecurityDescriptorSddlForm = value?.GetSddlForm(Task.defaultAccessControlSections); }
		}

		/// <summary>
		/// Gets or sets the security descriptor of the task.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(null)]
		[XmlIgnore]
		public string SecurityDescriptorSddlForm
		{
			get
			{
				object sddl = null;
				if (v2RegInfo != null)
					sddl = v2RegInfo.SecurityDescriptor;
				return sddl?.ToString();
			}
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.SecurityDescriptor = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets where the task originated from. For example, a task may originate from a component, service, application, or user.
		/// </summary>
		[DefaultValue(null)]
		public string Source
		{
			get { return v2RegInfo != null ? v2RegInfo.Source : v1Task.GetDataItem(nameof(Source)); }
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Source = value;
				else
					v1Task.SetDataItem(nameof(Source), value);
			}
		}

		/// <summary>
		/// Gets or sets the URI of the task.
		/// </summary>
		/// <remarks><c>Note: </c>Breaking change in version 2.0. This property was previously of type <see cref="Uri"/>. It was found that in Windows 8,
		/// many of the native tasks use this property in a string format rather than in a URI format.</remarks>
		[DefaultValue(null)]
		public string URI
		{
			get
			{
				var uri = v2RegInfo != null ? v2RegInfo.URI : v1Task.GetDataItem(nameof(URI));
				return string.IsNullOrEmpty(uri) ? null : uri;
			}
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.URI = value;
				else
					v1Task.SetDataItem(nameof(URI), value);
			}
		}

		/// <summary>
		/// Gets or sets the version number of the task.
		/// </summary>
		[DefaultValueEx(typeof(Version), "1.0")]
		public Version Version
		{
			get
			{
				var sver = v2RegInfo != null ? v2RegInfo.Version : v1Task.GetDataItem(nameof(Version));
				if (sver != null) try { return new Version(sver); } catch { }
				return new Version(1, 0);
			}
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.Version = value?.ToString();
				else
					v1Task.SetDataItem(nameof(Version), value.ToString());
			}
		}

		/// <summary>
		/// Gets or sets an XML-formatted version of the registration information for the task.
		/// </summary>
		[XmlIgnore]
		public string XmlText
		{
			get
			{
				return v2RegInfo != null ? v2RegInfo.XmlText : XmlSerializationHelper.WriteObjectToXmlText(this);
			}
			set
			{
				if (v2RegInfo != null)
					v2RegInfo.XmlText = value;
				else
					XmlSerializationHelper.ReadObjectFromXmlText(value, this);
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			v1Task = null;
			if (v2RegInfo != null)
				Marshal.ReleaseComObject(v2RegInfo);
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (v2RegInfo != null || v1Task != null)
				return DebugHelper.GetDebugString(this);
			return base.ToString();
		}

		internal static string FixCrLf(string text) => text == null ? null : Regex.Replace(text, "\r?\n", "\r\n");

		XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement(XmlSerializationHelper.GetElementName(this), TaskDefinition.tns);
				XmlSerializationHelper.ReadObjectProperties(reader, this, ProcessVersionXml);
				reader.ReadEndElement();
			}
			else
				reader.Skip();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			XmlSerializationHelper.WriteObjectProperties(writer, this, ProcessVersionXml);
		}

		private bool ProcessVersionXml(PropertyInfo pi, object obj, ref object value)
		{
			if (pi.Name == "Version" && value != null)
			{
				if (value is Version)
					value = value.ToString();
				else if (value is string)
					value = new Version(value.ToString());
				return true;
			}
			return false;
		}
	}

	internal class DefaultValueExAttribute : DefaultValueAttribute
	{
		public DefaultValueExAttribute(Type type, string value) : base(null)
		{
			try
			{
				if (type == typeof(Version))
				{
					SetValue(new Version(value));
					return;
				}
				SetValue(TypeDescriptor.GetConverter(type).ConvertFromInvariantString(value));
			}
			catch
			{
				Debug.Fail("Default value attribute of type " + type.FullName + " threw converting from the string '" + value + "'.");
			}
		}
	}

	/// <summary>
	/// Provides the settings that the Task Scheduler service uses to perform the task.
	/// </summary>
	[XmlRoot("Settings", Namespace = TaskDefinition.tns, IsNullable = true)]
	[PublicAPI]
	public sealed class TaskSettings : IDisposable, IXmlSerializable
	{
		private const uint InfiniteRunTimeV1 = 0xFFFFFFFF;

		private IdleSettings idleSettings;
		private MaintenanceSettings maintenanceSettings;
		private NetworkSettings networkSettings;
		private ITask v1Task;
		private ITaskSettings v2Settings;
		private ITaskSettings2 v2Settings2;
		private ITaskSettings3 v2Settings3;

		internal TaskSettings([NotNull] ITaskSettings iSettings)
		{
			v2Settings = iSettings;
			try { v2Settings2 = (ITaskSettings2)v2Settings; }
			catch { }
			try { v2Settings3 = (ITaskSettings3)v2Settings; }
			catch { }
		}

		internal TaskSettings([NotNull] ITask iTask)
		{
			v1Task = iTask;
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task can be started by using either the Run command or the Context menu.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(true)]
		[XmlElement("AllowStartOnDemand")]
		[XmlIgnore]
		public bool AllowDemandStart
		{
			get { return v2Settings == null || v2Settings.AllowDemandStart; }
			set
			{
				if (v2Settings != null)
					v2Settings.AllowDemandStart = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task may be terminated by using TerminateProcess.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(true)]
		[XmlIgnore]
		public bool AllowHardTerminate
		{
			get { return v2Settings == null || v2Settings.AllowHardTerminate; }
			set
			{
				if (v2Settings != null)
					v2Settings.AllowHardTerminate = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets an integer value that indicates which version of Task Scheduler a task is compatible with.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[XmlIgnore]
		public TaskCompatibility Compatibility
		{
			get { return v2Settings?.Compatibility ?? TaskCompatibility.V1; }
			set
			{
				if (v2Settings != null)
					v2Settings.Compatibility = value;
				else
					if (value != TaskCompatibility.V1)
						throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets the amount of time that the Task Scheduler will wait before deleting the task after it expires. If no value is specified for this property, then the Task Scheduler service will not delete the task.
		/// </summary>
		/// <value>
		/// Gets and sets the amount of time that the Task Scheduler will wait before deleting the task after it expires. A TimeSpan value of 1 second indicates the task is set to delete when done. A value of TimeSpan.Zero indicates that the task should not be deleted.
		/// </value>
		/// <remarks>
		/// A task expires after the end boundary has been exceeded for all triggers associated with the task. The end boundary for a trigger is specified by the <c>EndBoundary</c> property of all trigger types.
		/// </remarks>
		[DefaultValue(typeof(TimeSpan), "12:00:00")]
		public TimeSpan DeleteExpiredTaskAfter
		{
			get
			{
				if (v2Settings != null)
					return v2Settings.DeleteExpiredTaskAfter == "PT0S" ? TimeSpan.FromSeconds(1) : Task.StringToTimeSpan(v2Settings.DeleteExpiredTaskAfter);
				return v1Task.HasFlags(TaskFlags.DeleteWhenDone) ? TimeSpan.FromSeconds(1) : TimeSpan.Zero;
			}
			set
			{
				if (v2Settings != null)
					v2Settings.DeleteExpiredTaskAfter = value == TimeSpan.FromSeconds(1) ? "PT0S" : Task.TimeSpanToString(value);
				else
					v1Task.SetFlags(TaskFlags.DeleteWhenDone, value >= TimeSpan.FromSeconds(1));
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task will not be started if the computer is running on battery power.
		/// </summary>
		[DefaultValue(true)]
		public bool DisallowStartIfOnBatteries
		{
			get { return v2Settings?.DisallowStartIfOnBatteries ?? v1Task.HasFlags(TaskFlags.DontStartIfOnBatteries); }
			set
			{
				if (v2Settings != null)
					v2Settings.DisallowStartIfOnBatteries = value;
				else
					v1Task.SetFlags(TaskFlags.DontStartIfOnBatteries, value);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task will not be started if the task is triggered to run in a Remote Applications Integrated Locally (RAIL) session.
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.1.</exception>
		[DefaultValue(false)]
		[XmlIgnore]
		public bool DisallowStartOnRemoteAppSession 
		{
			get
			{
				if (v2Settings2 != null)
					return v2Settings2.DisallowStartOnRemoteAppSession;
				if (v2Settings3 != null)
					return v2Settings3.DisallowStartOnRemoteAppSession;
				return false;
			}
			set
			{
				if (v2Settings2 != null)
					v2Settings2.DisallowStartOnRemoteAppSession = value;
				else if (v2Settings3 != null)
					v2Settings3.DisallowStartOnRemoteAppSession = value;
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_1);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task is enabled. The task can be performed only when this setting is TRUE.
		/// </summary>
		[DefaultValue(true)]
		public bool Enabled
		{
			get { return v2Settings?.Enabled ?? !v1Task.HasFlags(TaskFlags.Disabled); }
			set
			{
				if (v2Settings != null)
					v2Settings.Enabled = value;
				else
					v1Task.SetFlags(TaskFlags.Disabled, !value);
			}
		}

		/// <summary>
		/// Gets or sets the amount of time that is allowed to complete the task. By default, a task will be stopped 72 hours after it starts to run.
		/// </summary>
		/// <value>
		/// The amount of time that is allowed to complete the task. When this parameter is set to <see cref="TimeSpan.Zero"/>, the execution time limit is infinite.
		/// </value>
		/// <remarks>
		/// If a task is started on demand, the ExecutionTimeLimit setting is bypassed. Therefore, a task that is started on demand will not be terminated if it exceeds the ExecutionTimeLimit.
		/// </remarks>
		[DefaultValue(typeof(TimeSpan), "3")]
		public TimeSpan ExecutionTimeLimit
		{
			get
			{
				if (v2Settings != null)
					return Task.StringToTimeSpan(v2Settings.ExecutionTimeLimit);
				var ms = v1Task.GetMaxRunTime();
				return ms == InfiniteRunTimeV1 ? TimeSpan.Zero : TimeSpan.FromMilliseconds(ms);
			}
			set
			{
				if (v2Settings != null)
					v2Settings.ExecutionTimeLimit = value == TimeSpan.Zero ? "PT0S" : Task.TimeSpanToString(value);
				else
				{
					// Due to an issue introduced in Vista, and propagated to Windows 7, setting the
					// MaxRunTime to INFINITE results in the task only running for 72 hours. For
					// these operating systems, setting the RunTime to "INFINITE - 1" gets the
					// desired behavior of allowing an "infinite" run of the task.
					var ms = value == TimeSpan.Zero ? InfiniteRunTimeV1 : Convert.ToUInt32(value.TotalMilliseconds);
					v1Task.SetMaxRunTime(ms);
					if (value == TimeSpan.Zero && v1Task.GetMaxRunTime() != InfiniteRunTimeV1)
						v1Task.SetMaxRunTime(InfiniteRunTimeV1 - 1);
				}
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task will not be visible in the UI by default.
		/// </summary>
		[DefaultValue(false)]
		public bool Hidden
		{
			get { return v2Settings?.Hidden ?? v1Task.HasFlags(TaskFlags.Hidden); }
			set
			{
				if (v2Settings != null)
					v2Settings.Hidden = value;
				else
					v1Task.SetFlags(TaskFlags.Hidden, value);
			}
		}

		/// <summary>
		/// Gets or sets the information that specifies how the Task Scheduler performs tasks when the computer is in an idle state.
		/// </summary>
		[NotNull]
		public IdleSettings IdleSettings => idleSettings ?? (idleSettings = v2Settings != null ? new IdleSettings(v2Settings.IdleSettings) : new IdleSettings(v1Task));

		/// <summary>
		/// Gets or sets the information that the Task Scheduler uses during Automatic maintenance.
		/// </summary>
		[XmlIgnore]
		[NotNull]
		public MaintenanceSettings MaintenanceSettings => maintenanceSettings ?? (maintenanceSettings = new MaintenanceSettings(v2Settings3));

		/// <summary>
		/// Gets or sets the policy that defines how the Task Scheduler handles multiple instances of the task.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(TaskInstancesPolicy), "IgnoreNew")]
		[XmlIgnore]
		public TaskInstancesPolicy MultipleInstances
		{
			get { return v2Settings?.MultipleInstances ?? TaskInstancesPolicy.IgnoreNew; }
			set
			{
				if (v2Settings != null)
					v2Settings.MultipleInstances = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets the network settings object that contains a network profile identifier and name. If the RunOnlyIfNetworkAvailable property of ITaskSettings is true and a network profile is specified in the NetworkSettings property, then the task will run only if the specified network profile is available.
		/// </summary>
		[XmlIgnore]
		[NotNull]
		public NetworkSettings NetworkSettings => networkSettings ?? (networkSettings = new NetworkSettings(v2Settings?.NetworkSettings));

		/// <summary>
		/// Gets or sets the priority level of the task.
		/// </summary>
		/// <value>
		/// The priority.
		/// </value>
		/// <exception cref="NotV1SupportedException">Value set to AboveNormal or BelowNormal on Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(ProcessPriorityClass), "Normal")]
		public ProcessPriorityClass Priority
		{
			get
			{
				return v2Settings != null ? GetPriorityFromInt(v2Settings.Priority) : (ProcessPriorityClass)v1Task.GetPriority();
			}
			set
			{
				if (v2Settings != null)
				{
					v2Settings.Priority = GetPriorityAsInt(value);
				}
				else
				{
					if (value == ProcessPriorityClass.AboveNormal || value == ProcessPriorityClass.BelowNormal)
						throw new NotV1SupportedException("Unsupported priority level on Task Scheduler 1.0.");
					v1Task.SetPriority((uint)value);
				}
			}
		}

		private ProcessPriorityClass GetPriorityFromInt(int value)
		{
			switch (value)
			{
				case 0:
					return ProcessPriorityClass.RealTime;
				case 1:
					return ProcessPriorityClass.High;
				case 2:
				case 3:
					return ProcessPriorityClass.AboveNormal;
				case 4:
				case 5:
				case 6:
					return ProcessPriorityClass.Normal;
				// case 7:
				// case 8:
				default:
					return ProcessPriorityClass.BelowNormal;
				case 9:
				case 10:
					return ProcessPriorityClass.Idle;
			}
		}

		private int GetPriorityAsInt(ProcessPriorityClass value)
		{
			var p = 7;
			switch (value)
			{
				case ProcessPriorityClass.AboveNormal:
					p = 3;
					break;
				case ProcessPriorityClass.High:
					p = 1;
					break;
				case ProcessPriorityClass.Idle:
					p = 10;
					break;
				case ProcessPriorityClass.Normal:
					p = 5;
					break;
				case ProcessPriorityClass.RealTime:
					p = 0;
					break;
				// case ProcessPriorityClass.BelowNormal:
				// default:
				// 	break;
			}
			return p;
		}

		/// <summary>
		/// Gets or sets the number of times that the Task Scheduler will attempt to restart the task.
		/// </summary>
		/// <value>
		/// The number of times that the Task Scheduler will attempt to restart the task. If this property is set, the <see cref="RestartInterval"/> property must also be set.
		/// </value>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(0)]
		[XmlIgnore]
		public int RestartCount
		{
			get { return v2Settings?.RestartCount ?? 0; }
			set
			{
				if (v2Settings != null)
					v2Settings.RestartCount = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets a value that specifies how long the Task Scheduler will attempt to restart the task.
		/// </summary>
		/// <value>
		/// A value that specifies how long the Task Scheduler will attempt to restart the task. If this property is set, the <see cref="RestartCount"/> property must also be set. The maximum time allowed is 31 days, and the minimum time allowed is 1 minute.
		/// </value>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(typeof(TimeSpan), "00:00:00")]
		[XmlIgnore]
		public TimeSpan RestartInterval
		{
			get { return v2Settings != null ? Task.StringToTimeSpan(v2Settings.RestartInterval) : TimeSpan.Zero; }
			set
			{
				if (v2Settings != null)
					v2Settings.RestartInterval = Task.TimeSpanToString(value);
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler will run the task only if the computer is in an idle condition.
		/// </summary>
		[DefaultValue(false)]
		public bool RunOnlyIfIdle
		{
			get { return v2Settings?.RunOnlyIfIdle ?? v1Task.HasFlags(TaskFlags.StartOnlyIfIdle); }
			set
			{
				if (v2Settings != null)
					v2Settings.RunOnlyIfIdle = value;
				else
					v1Task.SetFlags(TaskFlags.StartOnlyIfIdle, value);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler will run the task only if the user is logged on (v1.0 only)
		/// </summary>
		/// <exception cref="NotV2SupportedException">Property set for a task on a Task Scheduler version other than 1.0.</exception>
		[XmlIgnore]
		public bool RunOnlyIfLoggedOn
		{
			get { return v2Settings != null || v1Task.HasFlags(TaskFlags.RunOnlyIfLoggedOn); }
			set
			{
				if (v1Task != null)
					v1Task.SetFlags(TaskFlags.RunOnlyIfLoggedOn, value);
				else if (v2Settings != null)
					throw new NotV2SupportedException("Task Scheduler 2.0 (1.2) does not support setting this property. You must use an InteractiveToken in order to have the task run in the current user session.");
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler will run the task only when a network is available.
		/// </summary>
		[DefaultValue(false)]
		public bool RunOnlyIfNetworkAvailable
		{
			get { return v2Settings?.RunOnlyIfNetworkAvailable ?? v1Task.HasFlags(TaskFlags.RunIfConnectedToInternet); }
			set
			{
				if (v2Settings != null)
					v2Settings.RunOnlyIfNetworkAvailable = value;
				else
					v1Task.SetFlags(TaskFlags.RunIfConnectedToInternet, value);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler can start the task at any time after its scheduled time has passed.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[DefaultValue(false)]
		[XmlIgnore]
		public bool StartWhenAvailable
		{
			get { return v2Settings != null && v2Settings.StartWhenAvailable; }
			set
			{
				if (v2Settings != null)
					v2Settings.StartWhenAvailable = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the task will be stopped if the computer switches to battery power.
		/// </summary>
		[DefaultValue(true)]
		public bool StopIfGoingOnBatteries
		{
			get { return v2Settings?.StopIfGoingOnBatteries ?? v1Task.HasFlags(TaskFlags.KillIfGoingOnBatteries); }
			set
			{
				if (v2Settings != null)
					v2Settings.StopIfGoingOnBatteries = value;
				else
					v1Task.SetFlags(TaskFlags.KillIfGoingOnBatteries, value);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Unified Scheduling Engine will be utilized to run this task.
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.1.</exception>
		[DefaultValue(false)]
		[XmlIgnore]
		public bool UseUnifiedSchedulingEngine
		{
			get
			{
				if (v2Settings2 != null)
					return v2Settings2.UseUnifiedSchedulingEngine;
				if (v2Settings3 != null)
					return v2Settings3.UseUnifiedSchedulingEngine;
				return false;
			}
			set
			{
				if (v2Settings2 != null)
					v2Settings2.UseUnifiedSchedulingEngine = value;
				else if (v2Settings3 != null)
					v2Settings3.UseUnifiedSchedulingEngine = value;
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_1);
			}
		}

		/// <summary>
		/// Gets or sets a boolean value that indicates whether the task is automatically disabled every time Windows starts.
		/// </summary>
		/// <exception cref="NotSupportedPriorToException">Property set for a task on a Task Scheduler version prior to 2.2.</exception>
		[DefaultValue(false)]
		[XmlIgnore]
		public bool Volatile
		{
			get { return v2Settings3 != null && v2Settings3.Volatile; }
			set
			{
				if (v2Settings3 != null)
					v2Settings3.Volatile = value;
				else
					throw new NotSupportedPriorToException(TaskCompatibility.V2_2);
			}
		}

		/// <summary>
		/// Gets or sets a Boolean value that indicates that the Task Scheduler will wake the computer when it is time to run the task.
		/// </summary>
		[DefaultValue(false)]
		public bool WakeToRun
		{
			get { return v2Settings?.WakeToRun ?? v1Task.HasFlags(TaskFlags.SystemRequired); }
			set
			{
				if (v2Settings != null)
					v2Settings.WakeToRun = value;
				else
					v1Task.SetFlags(TaskFlags.SystemRequired, value);
			}
		}

		/// <summary>
		/// Gets or sets an XML-formatted definition of the task settings.
		/// </summary>
		[XmlIgnore]
		public string XmlText
		{
			get { return v2Settings != null ? v2Settings.XmlText : XmlSerializationHelper.WriteObjectToXmlText(this); }
			set
			{
				if (v2Settings != null)
					v2Settings.XmlText = value;
				else
					XmlSerializationHelper.ReadObjectFromXmlText(value, this);
			}
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Settings != null)
				Marshal.ReleaseComObject(v2Settings);
			idleSettings = null;
			networkSettings = null;
			v1Task = null;
		}

		/// <summary>
		/// Returns a <see cref="System.String" /> that represents this instance.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String" /> that represents this instance.
		/// </returns>
		public override string ToString()
		{
			if (v2Settings != null || v1Task != null)
				return DebugHelper.GetDebugString(this);
			return base.ToString();
		}

		XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(XmlReader reader)
		{
			if (!reader.IsEmptyElement)
			{
				reader.ReadStartElement(XmlSerializationHelper.GetElementName(this), TaskDefinition.tns);
				XmlSerializationHelper.ReadObjectProperties(reader, this, ConvertXmlProperty);
				reader.ReadEndElement();
			}
			else
				reader.Skip();
		}

		void IXmlSerializable.WriteXml(XmlWriter writer)
		{
			XmlSerializationHelper.WriteObjectProperties(writer, this, ConvertXmlProperty);
		}

		private bool ConvertXmlProperty(PropertyInfo pi, object obj, ref object value)
		{
			if (pi.Name == "Priority" && value != null)
			{
				if (value is int)
					value = GetPriorityFromInt((int)value);
				else if (value is ProcessPriorityClass)
					value = GetPriorityAsInt((ProcessPriorityClass)value);
				return true;
			}
			return false;
		}
	}

	internal static class TSInteropExt
	{
		public static string GetDataItem(this ITask v1Task, string name)
		{
			string ret;
			TaskDefinition.GetV1TaskDataDictionary(v1Task).TryGetValue(name, out ret);
			return ret;
		}

		public static void SetDataItem(this ITask v1Task, string name, string value)
		{
			var d = TaskDefinition.GetV1TaskDataDictionary(v1Task);
			d[name] = value;
			TaskDefinition.SetV1TaskData(v1Task, d);
		}

		public static bool HasFlags(this ITask v1Task, TaskFlags flags) => v1Task.GetFlags().IsFlagSet(flags);

		public static void SetFlags(this ITask v1Task, TaskFlags flags, bool value = true)
		{
			v1Task.SetFlags(v1Task.GetFlags().SetFlags(flags, value));
		}
	}
}