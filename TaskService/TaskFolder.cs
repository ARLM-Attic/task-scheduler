﻿using System;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Provides the methods that are used to register (create) tasks in the folder, remove tasks from the folder, and create or remove subfolders from the folder.
	/// </summary>
	public sealed class TaskFolder : IDisposable
	{
		V1Interop.ITaskScheduler v1List = null;
		V2Interop.ITaskFolder v2Folder = null;

		internal TaskFolder(V1Interop.ITaskScheduler ts)
		{
			v1List = ts;
		}

		internal TaskFolder(V2Interop.ITaskFolder iFldr)
		{
			v2Folder = iFldr;
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			if (v2Folder != null)
				System.Runtime.InteropServices.Marshal.ReleaseComObject(v2Folder);
			v1List = null;
		}

		/// <summary>
		/// Gets the name that is used to identify the folder that contains a task.
		/// </summary>
		public string Name
		{
			get { return (v2Folder == null) ? @"\" : v2Folder.Name; }
		}

		/// <summary>
		/// Gets the path to where the folder is stored.
		/// </summary>
		public string Path
		{
			get { return (v2Folder == null) ? @"\" : v2Folder.Path; }
		}

		/*public TaskFolder GetFolder(string Path)
		{
			if (v2Folder != null)
				return new TaskFolder(v2Folder.GetFolder(Path));
			throw new NotSupportedException();
		}*/

		/// <summary>
		/// Gets all the subfolders in the folder.
		/// </summary>
		public TaskFolderCollection SubFolders
		{
			get
			{
				if (v2Folder != null)
					return new TaskFolderCollection(v2Folder.GetFolders(0));
				return new TaskFolderCollection();
			}
		}

		/// <summary>
		/// Creates a folder for related tasks. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="subFolderName">The name used to identify the folder. If "FolderName\SubFolder1\SubFolder2" is specified, the entire folder tree will be created if the folders do not exist. This parameter can be a relative path to the current <see cref="TaskFolder"/> instance. The root task folder is specified with a backslash (\). An example of a task folder path, under the root task folder, is \MyTaskFolder. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		/// <param name="sddlForm">The name used to identify the folder. If "FolderName\SubFolder1\SubFolder2" is specified, the entire folder tree will be created if the folders do not exist. This parameter can be a relative path to the current ITaskFolder instance. The root task folder is specified with a backslash (\). An example of a task folder path, under the root task folder, is \MyTaskFolder. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		/// <returns>A <see cref="TaskFolder"/> instance that represents the new subfolder.</returns>
		public TaskFolder CreateFolder(string subFolderName, string sddlForm)
		{
			if (v2Folder != null)
				return new TaskFolder(v2Folder.CreateFolder(subFolderName, sddlForm));
			throw new NotSupportedException();
		}

		/// <summary>
		/// Deletes a subfolder from the parent folder. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="subFolderName">The name of the subfolder to be removed. The root task folder is specified with a backslash (\). This parameter can be a relative path to the folder you want to delete. An example of a task folder path, under the root task folder, is \MyTaskFolder. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		public void DeleteFolder(string subFolderName)
		{
			if (v2Folder != null)
				v2Folder.DeleteFolder(subFolderName, 0);
			throw new NotSupportedException();
		}

		/*public Task GetTask(string Path)
		{
			if (v2Folder != null)
				return new Task(v2Folder.GetTask(Path));
			throw new NotImplementedException();
		}*/

		/// <summary>
		/// Gets a collection of all the tasks in the folder.
		/// </summary>
		public TaskCollection Tasks
		{
			get
			{
				if (v2Folder != null)
					return new TaskCollection(v2Folder.GetTasks(1));
				return new TaskCollection(v1List);
			}
		}

		/// <summary>
		/// Deletes a task from the folder.
		/// </summary>
		/// <param name="Name">The name of the task that is specified when the task was registered. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		public void DeleteTask(string Name)
		{
			if (v2Folder != null)
				v2Folder.DeleteTask(Name, 0);
			else
			{
				if (!Name.EndsWith(".job", StringComparison.CurrentCultureIgnoreCase))
					Name += ".job";
				v1List.Delete(Name);
			}
		}

		/// <summary>
		/// Registers (creates) a new task in the folder using XML to define the task. Not available for Task Scheduler 1.0.
		/// </summary>
		/// <param name="Path">The task name. If this value is NULL, the task will be registered in the root task folder and the task name will be a GUID value that is created by the Task Scheduler service. A task name cannot begin or end with a space character. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		/// <param name="XmlText">An XML-formatted definition of the task.</param>
		/// <param name="createType">A union of <see cref="TaskCreation"/> flags.</param>
		/// <param name="UserId">The user credentials used to register the task.</param>
		/// <param name="password">The password for the userId used to register the task.</param>
		/// <param name="LogonType">A <see cref="TaskLogonType"/> value that defines what logon technique is used to run the registered task.</param>
		/// <param name="sddl">The security descriptor associated with the registered task. You can specify the access control list (ACL) in the security descriptor for a task in order to allow or deny certain users and groups access to a task.</param>
		/// <returns>A <see cref="Task"/> instance that represents the new task.</returns>
		public Task RegisterTask(string Path, string XmlText, TaskCreation createType, string UserId, string password, TaskLogonType LogonType, string sddl)
		{
			if (v2Folder != null)
				return new Task(v2Folder.RegisterTask(Path, XmlText, (int)createType, UserId, password, LogonType, sddl));
			throw new NotSupportedException();
		}

		/// <summary>
		/// Registers (creates) a task in a specified location using a <see cref="TaskDefinition"/> instance to define a task.
		/// </summary>
		/// <param name="Path">The task name. If this value is NULL, the task will be registered in the root task folder and the task name will be a GUID value that is created by the Task Scheduler service. A task name cannot begin or end with a space character. The '.' character cannot be used to specify the current task folder and the '..' characters cannot be used to specify the parent task folder in the path.</param>
		/// <param name="definition">The <see cref="TaskDefinition"/> of the registered task.</param>
		/// <param name="createType">A union of <see cref="TaskCreation"/> flags.</param>
		/// <param name="UserId">The user credentials used to register the task.</param>
		/// <param name="password">The password for the userId used to register the task.</param>
		/// <param name="LogonType">A <see cref="TaskLogonType"/> value that defines what logon technique is used to run the registered task.</param>
		/// <param name="sddl">The security descriptor associated with the registered task. You can specify the access control list (ACL) in the security descriptor for a task in order to allow or deny certain users and groups access to a task.</param>
		/// <returns>A <see cref="Task"/> instance that represents the new task.</returns>
		public Task RegisterTaskDefinition(string Path, TaskDefinition definition, TaskCreation createType, string UserId, string password, TaskLogonType LogonType, string sddl)
		{
			if (v2Folder != null)
				return new Task(v2Folder.RegisterTaskDefinition(Path, definition.v2Def, (int)createType, UserId, password, LogonType, sddl));

			definition.V1Save(Path);
			return new Task(definition.v1Task);
		}

		/// <summary>
		/// Gets the security descriptor for the folder. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="includeSections">Section(s) of the security descriptor to return.</param>
		/// <returns>The security descriptor for the folder.</returns>
		public string GetSecurityDescriptorSddlForm(System.Security.AccessControl.AccessControlSections includeSections)
		{
			if (v2Folder != null)
				return v2Folder.GetSecurityDescriptor((int)includeSections);
			throw new NotSupportedException();
		}

		/// <summary>
		/// Sets the security descriptor for the folder. Not available to Task Scheduler 1.0.
		/// </summary>
		/// <param name="sddlForm">The security descriptor for the folder.</param>
		/// <param name="includeSections">Section(s) of the security descriptor to set.</param>
		public void SetSecurityDescriptorSddlForm(string sddlForm, System.Security.AccessControl.AccessControlSections includeSections)
		{
			if (v2Folder != null)
				v2Folder.SetSecurityDescriptor(sddlForm, (int)includeSections);
			else
				throw new NotSupportedException();
		}
	}
}
