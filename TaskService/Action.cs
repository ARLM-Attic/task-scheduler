﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Collections;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Defines the type of actions a task can perform.
	/// </summary>
	/// <remarks>The action type is defined when the action is created and cannot be changed later. See <see cref="ActionCollection.AddNew"/>.</remarks>
	public enum TaskActionType
	{
		/// <summary>This action fires a handler.</summary>
		ComHandler = 5,
		/// <summary>This action performs a command-line operation. For example, the action can run a script, launch an executable, or, if the name of a document is provided, find its associated application and launch the application with the document.</summary>
		Execute = 0,
		/// <summary>This action sends and e-mail.</summary>
		SendEmail = 6,
		/// <summary>This action shows a message box.</summary>
		ShowMessage = 7
	}

	/// <summary>
	/// Abstract base class that provides the common properties that are inherited by all action objects. An action object is created by the <see cref="ActionCollection.AddNew"/> method.
	/// </summary>
	public abstract class Action : IDisposable
	{
		internal V2Interop.IAction iAction = null;
		/// <summary>In testing and may change. Do not use until officially introduced into library.</summary>
		protected Dictionary<string, object> unboundValues = new Dictionary<string, object>();

		/// <summary>In testing and may change. Do not use until officially introduced into library.</summary>
		internal abstract void Bind(V2Interop.ITaskDefinition iTaskDef);

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public virtual void Dispose()
		{
			if (iAction != null)
				Marshal.ReleaseComObject(iAction);
		}

		/// <summary>
		/// Gets or sets the identifier of the action.
		/// </summary>
		public virtual string Id
		{
			get { return (iAction == null) ? (string)unboundValues["Id"] : this.iAction.Id; }
			set { if (iAction == null) unboundValues["Id"] = value; else this.iAction.Id = value; }
		}

		/// <summary>In testing and may change. Do not use until officially introduced into library.</summary>
		internal virtual bool Bound { get { return this.iAction != null; } }

		/// <summary>
		/// Binds values to the interface that were set before the interface was available.
		/// </summary>
		protected void BindValues()
		{
			foreach (string key in unboundValues.Keys)
			{
				iAction.GetType().InvokeMember(key, System.Reflection.BindingFlags.SetProperty, null, iAction, new object[] { unboundValues[key] });
			}
		}

		/// <summary>
		/// Creates a specialized class from a defined interface.
		/// </summary>
		/// <param name="iAction">Version 2.0 Action interface.</param>
		/// <returns>Specialized action class</returns>
		internal static Action CreateAction(V2Interop.IAction iAction)
		{
			switch (iAction.Type)
			{
				case TaskActionType.ComHandler:
					return new ComHandlerAction((V2Interop.IComHandlerAction)iAction);
				case TaskActionType.SendEmail:
					return new EmailAction((V2Interop.IEmailAction)iAction);
				case TaskActionType.ShowMessage:
					return new ShowMessageAction((V2Interop.IShowMessageAction)iAction);
				case TaskActionType.Execute:
				default:
					return new ExecAction((V2Interop.IExecAction)iAction);
			}
		}
	}

	/// <summary>
	/// Represents an action that fires a handler. Only available on Task Scheduler 2.0.
	/// </summary>
	public class ComHandlerAction : Action
	{
		internal ComHandlerAction(V2Interop.IComHandlerAction action)
		{
			iAction = action;
		}

		internal override void Bind(Microsoft.Win32.TaskScheduler.V2Interop.ITaskDefinition iTaskDef)
		{
			//iAction = iTaskDef.Actions.Create(InternalV2.TaskActionType.ComHandler);
			BindValues();
		}

		/// <summary>
		/// Gets or sets the identifier of the handler class.
		/// </summary>
		public Guid ClassId
		{
			get { return (iAction == null) ? (Guid)unboundValues["ClassId"] : new Guid(((V2Interop.IComHandlerAction)iAction).ClassId); }
			set { if (iAction == null) unboundValues["ClassId"] = value; else ((V2Interop.IComHandlerAction)iAction).ClassId = value.ToString(); }
		}

		/// <summary>
		/// Gets or sets additional data that is associated with the handler.
		/// </summary>
		public string Data
		{
			get { return (iAction == null) ? (string)unboundValues["Data"] : ((V2Interop.IComHandlerAction)iAction).Data; }
			set { if (iAction == null) unboundValues["Data"] = value; else ((V2Interop.IComHandlerAction)iAction).Data = value; }
		}
	}

	/// <summary>
	/// Represents an action that executes a command-line operation.
	/// </summary>
	public class ExecAction : Action
	{
		private V1Interop.ITask v1Task;

		/// <summary>
		/// Creates a new instance of 
		/// </summary>
		/// <param name="path"></param>
		/// <param name="arguments"></param>
		/// <param name="workingDirectory"></param>
		internal ExecAction(string path, string arguments, string workingDirectory)
		{
			this.Path = path;
			this.Arguments = arguments;
			this.WorkingDirectory = workingDirectory;
		}

		internal ExecAction(V1Interop.ITask task)
		{
			v1Task = task;
		}

		internal ExecAction(V2Interop.IExecAction action)
		{
			iAction = action;
		}

		internal override bool Bound
		{
			get
			{
				if (v1Task != null)
					return true;
				return base.Bound;
			}
		}

		internal void Bind(V1Interop.ITask v1Task)
		{
			object o;
			if (unboundValues.TryGetValue("Path", out o))
				v1Task.SetApplicationName((string)o);
			if (unboundValues.TryGetValue("Arguments", out o))
				v1Task.SetParameters((string)o);
			if (unboundValues.TryGetValue("WorkingDirectory", out o))
				v1Task.SetWorkingDirectory((string)o);
		}

		internal override void Bind(V2Interop.ITaskDefinition iTaskDef)
		{
			iAction = iTaskDef.Actions.Create(TaskActionType.Execute);
			BindValues();
		}

		/// <summary>
		/// Gets or sets the identifier of the action.
		/// </summary>
		public override string Id
		{
			get
			{
				if (v1Task != null)
					return System.IO.Path.GetFileNameWithoutExtension(Task.GetV1Path(v1Task)) + "_Action";
				return base.Id;
			}
			set
			{
				if (v1Task != null)
					throw new NotV1SupportedException();
				base.Id = value;
			}
		}

		/// <summary>
		/// Gets or sets the path to an executable file.
		/// </summary>
		public string Path
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetApplicationName();
				if (iAction != null)
					return ((V2Interop.IExecAction)iAction).Path;
				return (string)unboundValues["Path"];
			}
			set
			{
				if (v1Task != null)
					v1Task.SetApplicationName(value);
				else if (iAction != null)
					((V2Interop.IExecAction)iAction).Path = value;
				else
					unboundValues["Path"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the arguments associated with the command-line operation.
		/// </summary>
		public string Arguments
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetParameters();
				if (iAction != null)
					return ((V2Interop.IExecAction)iAction).Arguments;
				return (string)unboundValues["Arguments"];
			}
			set
			{
				if (v1Task != null)
					v1Task.SetParameters(value);
				else if (iAction != null)
					((V2Interop.IExecAction)iAction).Arguments = value;
				else
					unboundValues["Arguments"] = value;
			}
		}

		/// <summary>
		/// Gets or sets the directory that contains either the executable file or the files that are used by the executable file.
		/// </summary>
		public string WorkingDirectory
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetWorkingDirectory();
				if (iAction != null)
					return ((V2Interop.IExecAction)iAction).WorkingDirectory;
				return (string)unboundValues["WorkingDirectory"];
			}
			set
			{
				if (v1Task != null)
					v1Task.SetWorkingDirectory(value);
				else if (iAction != null)
					((V2Interop.IExecAction)iAction).WorkingDirectory = value;
				else
					unboundValues["WorkingDirectory"] = value;
			}
		}
	}

	/// <summary>
	/// Represents an action that sends an e-mail.
	/// </summary>
	public class EmailAction : Action
	{
		internal EmailAction(V2Interop.IEmailAction action)
		{
			iAction = action;
		}

		internal override void Bind(Microsoft.Win32.TaskScheduler.V2Interop.ITaskDefinition iTaskDef)
		{
			//iAction = iTaskDef.Actions.Create(Microsoft.Win32.TaskScheduler.InternalV2.TaskActionType.SendEmail);
			BindValues();
		}

		/// <summary>
		/// Gets or sets the name of the server that you use to send e-mail from.
		/// </summary>
		public string Server
		{
			get { return (iAction == null) ? (string)unboundValues["Server"] : ((V2Interop.IEmailAction)iAction).Server; }
			set { if (iAction == null) unboundValues["Server"] = value; else ((V2Interop.IEmailAction)iAction).Server = value; }
		}

		/// <summary>
		/// Gets or sets the subject of the e-mail.
		/// </summary>
		public string Subject
		{
			get { return (iAction == null) ? (string)unboundValues["Subject"] : ((V2Interop.IEmailAction)iAction).Subject; }
			set { if (iAction == null) unboundValues["Subject"] = value; else ((V2Interop.IEmailAction)iAction).Subject = value; }
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to send the e-mail to.
		/// </summary>
		public string To
		{
			get { return (iAction == null) ? (string)unboundValues["To"] : ((V2Interop.IEmailAction)iAction).To; }
			set { if (iAction == null) unboundValues["To"] = value; else ((V2Interop.IEmailAction)iAction).To = value; }
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to Cc in the e-mail.
		/// </summary>
		public string Cc
		{
			get { return (iAction == null) ? (string)unboundValues["Cc"] : ((V2Interop.IEmailAction)iAction).Cc; }
			set { if (iAction == null) unboundValues["Cc"] = value; else ((V2Interop.IEmailAction)iAction).Cc = value; }
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to Bcc in the e-mail.
		/// </summary>
		public string Bcc
		{
			get { return (iAction == null) ? (string)unboundValues["Bcc"] : ((V2Interop.IEmailAction)iAction).Bcc; }
			set { if (iAction == null) unboundValues["Bcc"] = value; else ((V2Interop.IEmailAction)iAction).Bcc = value; }
		}

		/// <summary>
		/// Gets or sets the e-mail address that you want to reply to.
		/// </summary>
		public string ReplyTo
		{
			get { return (iAction == null) ? (string)unboundValues["ReplyTo"] : ((V2Interop.IEmailAction)iAction).ReplyTo; }
			set { if (iAction == null) unboundValues["ReplyTo"] = value; else ((V2Interop.IEmailAction)iAction).ReplyTo = value; }
		}

		/// <summary>
		/// Gets or sets the e-mail address that you want to send the e-mail from.
		/// </summary>
		public string From
		{
			get { return (iAction == null) ? (string)unboundValues["From"] : ((V2Interop.IEmailAction)iAction).From; }
			set { if (iAction == null) unboundValues["From"] = value; else ((V2Interop.IEmailAction)iAction).From = value; }
		}

		private NamedValueCollection nvc = null;

		/// <summary>
		/// Gets or sets the header information in the e-mail message to send.
		/// </summary>
		public NamedValueCollection HeaderFields
		{
			get
			{
				if (nvc == null)
					nvc = new NamedValueCollection(((V2Interop.IEmailAction)iAction).HeaderFields);
				return nvc;
			}
		}

		/// <summary>
		/// Gets or sets the body of the e-mail that contains the e-mail message.
		/// </summary>
		public string Body
		{
			get { return (iAction == null) ? (string)unboundValues["Body"] : ((V2Interop.IEmailAction)iAction).Body; }
			set { if (iAction == null) unboundValues["Body"] = value; else ((V2Interop.IEmailAction)iAction).Body = value; }
		}

		/// <summary>
		/// Gets or sets an array of attachments that is sent with the e-mail.
		/// </summary>
		public object[] Attachments
		{
			get { return (iAction == null) ? (object[])unboundValues["Attachments"] : ((V2Interop.IEmailAction)iAction).Attachments; }
			set { if (iAction == null) unboundValues["Attachments"] = value; else ((V2Interop.IEmailAction)iAction).Attachments = value; }
		}
	}

	/// <summary>
	/// Represents an action that shows a message box when a task is activated.
	/// </summary>
	public class ShowMessageAction : Action
	{
		internal ShowMessageAction(string messageBody, string title)
		{
			this.MessageBody = messageBody;
			this.Title = title;
		}

		internal ShowMessageAction(V2Interop.IShowMessageAction action)
		{
			iAction = action;
		}

		internal override void Bind(Microsoft.Win32.TaskScheduler.V2Interop.ITaskDefinition iTaskDef)
		{
			//iAction = iTaskDef.Actions.Create(Microsoft.Win32.TaskScheduler.InternalV2.TaskActionType.ShowMessage);
			BindValues();
		}

		/// <summary>
		/// Gets or sets the title of the message box.
		/// </summary>
		public string Title
		{
			get { return (iAction == null) ? (string)unboundValues["Title"] : ((V2Interop.IShowMessageAction)iAction).Title; }
			set { if (iAction == null) unboundValues["Title"] = value; else ((V2Interop.IShowMessageAction)iAction).Title = value; }
		}

		/// <summary>
		/// Gets or sets the message text that is displayed in the body of the message box.
		/// </summary>
		public string MessageBody
		{
			get { return (iAction == null) ? (string)unboundValues["MessageBody"] : ((V2Interop.IShowMessageAction)iAction).MessageBody; }
			set { if (iAction == null) unboundValues["MessageBody"] = value; else ((V2Interop.IShowMessageAction)iAction).MessageBody = value; }
		}
	}

	/// <summary>
	/// Collection that contains the actions that are performed by the task.
	/// </summary>
	/// <remarks>A Task Scheduler 1.0 task can only contain a single <see cref="ExecAction"/>.</remarks>
	public sealed class ActionCollection : IEnumerable<Action>, IDisposable
	{
		private V1Interop.ITask v1Task;
		private V2Interop.IActionCollection v2Coll;

		internal ActionCollection(V1Interop.ITask task)
		{
			v1Task = task;
		}

		internal ActionCollection(V2Interop.ITaskDefinition iTaskDef)
		{
			v2Coll = iTaskDef.Actions;
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public void Dispose()
		{
			v1Task = null;
			v2Coll = null;
		}

		/// <summary>
		/// Adds a new <see cref="Action"/> instance to the task.
		/// </summary>
		/// <param name="actionType">Type of task to be created</param>
		/// <returns>Specialized <see cref="Action"/> instance.</returns>
		public Action AddNew(TaskActionType actionType)
		{
			if (v1Task != null)
				return new ExecAction(v1Task);

			return Action.CreateAction(v2Coll.Create(actionType));
		}

		/// <summary>
		/// Gets a specified action from the collection.
		/// </summary>
		/// <param name="index">The index of the action to be retrieved.</param>
		/// <returns>Specialized <see cref="Action"/> instance.</returns>
		public Action this[int index]
		{
			get { if (v2Coll != null) return Action.CreateAction(v2Coll[index]); throw new NotV1SupportedException(); }
		}

		/// <summary>
		/// Gets or sets the identifier of the principal for the task.
		/// </summary>
		public string Context
		{
			get
			{
				if (v2Coll != null)
					return v2Coll.Context;
				return string.Empty;
			}
			set
			{
				if (v2Coll != null)
					v2Coll.Context = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Gets the number of actions in the collection.
		/// </summary>
		public int Count
		{
			get
			{
				if (v2Coll != null)
					return v2Coll.Count;
				return 1;
			}
		}

		/// <summary>
		/// Gets or sets an XML-formatted version of the collection.
		/// </summary>
		public string XmlText
		{
			get
			{
				if (v2Coll != null)
					return v2Coll.XmlText;
				throw new NotV1SupportedException();
			}
			set
			{
				if (v2Coll != null)
					v2Coll.XmlText = value;
				else
					throw new NotV1SupportedException();
			}
		}

		/// <summary>
		/// Retrieves an enumeration of each of the actions.
		/// </summary>
		/// <returns>Returns an object that implements the <see cref="IEnumerator"/> interface and that can iterate through the <see cref="Action"/> objects within the <see cref="ActionCollection"/>.</returns>
		public IEnumerator<Action> GetEnumerator()
		{
			throw new NotImplementedException();
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
		{
			return this.GetEnumerator();
		}

		internal class Enumerator : IEnumerator<Action>
		{
			private V1Interop.ITask v1Task;
			private int v1Pos = -1;
			private IEnumerator v2Enum;
			private ActionCollection parent;

			internal Enumerator(V1Interop.ITask task)
			{
				v1Task = task;
			}

			internal Enumerator(ActionCollection iColl)
			{
				parent = iColl;
				if (iColl.v2Coll != null)
					v2Enum = iColl.v2Coll.GetEnumerator();
			}

			public Action Current
			{
				get
				{
					if (v2Enum != null)
					{
						V2Interop.IAction iAction = v2Enum.Current as V2Interop.IAction;
						if (iAction != null)
							return Action.CreateAction(iAction);
					}
					if (v1Pos == 0)
						return new ExecAction(v1Task.GetApplicationName(), v1Task.GetParameters(), v1Task.GetWorkingDirectory());
					throw new InvalidOperationException();
				}
			}

			/// <summary>
			/// Releases all resources used by this class.
			/// </summary>
			public void Dispose()
			{
				v1Task = null;
				v2Enum = null;
			}

			object System.Collections.IEnumerator.Current
			{
				get { return this.Current; }
			}

			public bool MoveNext()
			{
				if (v2Enum != null)
					return v2Enum.MoveNext();
				return ++v1Pos == 0;
			}

			public void Reset()
			{
				if (v2Enum != null)
					v2Enum.Reset();
				v1Pos = -1;
			}
		}
	}

}
