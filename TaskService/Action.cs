﻿using Microsoft.Win32.TaskScheduler.V2Interop;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Defines the type of actions a task can perform.
	/// </summary>
	/// <remarks>
	/// The action type is defined when the action is created and cannot be changed later. See <see cref="ActionCollection.AddNew"/>.
	/// </remarks>
	public enum TaskActionType
	{
		/// <summary>This action fires a handler.</summary>
		ComHandler = 5,

		/// <summary>
		/// This action performs a command-line operation. For example, the action can run a script,
		/// launch an executable, or, if the name of a document is provided, find its associated
		/// application and launch the application with the document.
		/// </summary>
		Execute = 0,

		/// <summary>This action sends and e-mail.</summary>
		SendEmail = 6,

		/// <summary>This action shows a message box.</summary>
		ShowMessage = 7
	}

	/// <summary>
	/// An interface that exposes the ability to convert an actions functionality to a PowerShell script.
	/// </summary>
	internal interface IBindAsExecAction
	{
	}

	/// <summary>
	/// Abstract base class that provides the common properties that are inherited by all action
	/// objects. An action object is created by the <see cref="ActionCollection.AddNew"/> method.
	/// </summary>
	public abstract class Action : IDisposable, ICloneable, IEquatable<Action>, INotifyPropertyChanged
	{
		internal IAction iAction = null;
		internal V1Interop.ITask v1Task;

		/// <summary>List of unbound values when working with Actions not associated with a registered task.</summary>
		protected Dictionary<string, object> unboundValues = new Dictionary<string, object>();

		internal Action() { }

		internal Action(IAction action)
		{
			iAction = action;
		}

		internal Action(V1Interop.ITask iTask)
		{
			v1Task = iTask;
		}

		/// <summary>
		/// Occurs when a property value changes.
		/// </summary>
		public event PropertyChangedEventHandler PropertyChanged;

		/// <summary>
		/// Gets the type of the action.
		/// </summary>
		/// <value>The type of the action.</value>
		[XmlIgnore]
		public TaskActionType ActionType => iAction?.Type ?? InternalActionType;

		/// <summary>
		/// Gets or sets the identifier of the action.
		/// </summary>
		[DefaultValue(null)]
		[XmlAttribute(AttributeName = "id")]
		public virtual string Id
		{
			get { return GetProperty<string, IAction>(nameof(Id)); }
			set { SetProperty<string, IAction>(nameof(Id), value); }
		}

		internal abstract TaskActionType InternalActionType { get; }

		/// <summary>
		/// Creates the specified action.
		/// </summary>
		/// <param name="actionType">Type of the action to instantiate.</param>
		/// <returns><see cref="Action"/> of specified type.</returns>
		public static Action CreateAction(TaskActionType actionType) => Activator.CreateInstance(GetObjectType(actionType)) as Action;

		/// <summary>
		/// Creates a new object that is a copy of the current instance.
		/// </summary>
		/// <returns>
		/// A new object that is a copy of this instance.
		/// </returns>
		public object Clone()
		{
			Action ret = CreateAction(ActionType);
			ret.CopyProperties(this);
			return ret;
		}

		/// <summary>
		/// Releases all resources used by this class.
		/// </summary>
		public virtual void Dispose()
		{
			if (iAction != null)
				Marshal.ReleaseComObject(iAction);
		}

		/// <summary>
		/// Determines whether the specified <see cref="System.Object"/>, is equal to this instance.
		/// </summary>
		/// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
		/// <returns>
		/// <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(object obj)
		{
			if (obj is Action)
				return Equals((Action)obj);
			return base.Equals(obj);
		}

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public virtual bool Equals(Action other) => ActionType == other.ActionType && Id == other.Id;

		/// <summary>
		/// Returns a hash code for this instance.
		/// </summary>
		/// <returns>
		/// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.
		/// </returns>
		public override int GetHashCode() => new { A = ActionType, B = Id }.GetHashCode();

		/// <summary>
		/// Returns the action Id.
		/// </summary>
		/// <returns>String representation of action.</returns>
		public override string ToString() => Id;

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents this action.
		/// </summary>
		/// <param name="culture">The culture.</param>
		/// <returns>String representation of action.</returns>
		public virtual string ToString(System.Globalization.CultureInfo culture)
		{
			using (new CultureSwitcher(culture))
				return ToString();
		}

		internal static Action ActionFromScript(string actionType, string script)
		{
			TaskActionType tat = TryParse(actionType, TaskActionType.Execute);
			Type t = GetObjectType(tat);
			return (Action)t.InvokeMember("FromPowerShellCommand", BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod, null, null, new object[] { script });
		}

		internal static Action ConvertFromPowerShellAction(ExecAction execAction)
		{
			var psi = execAction.ParsePowerShellItems();
			if (psi != null && psi.Length == 2)
			{
				var a = ActionFromScript(psi[0], psi[1]);
				if (a != null)
				{
					a.v1Task = execAction.v1Task;
					a.iAction = execAction.iAction;
					return a;
				}
			}
			return null;
		}

		/// <summary>
		/// Creates a specialized class from a defined interface.
		/// </summary>
		/// <param name="iTask">Version 1.0 interface.</param>
		/// <returns>Specialized action class</returns>
		internal static Action CreateAction(V1Interop.ITask iTask)
		{
			ExecAction tempAction = new ExecAction(iTask);
			Action a = ConvertFromPowerShellAction(tempAction);
			return a ?? tempAction;
		}

		/// <summary>
		/// Creates a specialized class from a defined interface.
		/// </summary>
		/// <param name="iAction">Version 2.0 Action interface.</param>
		/// <returns>Specialized action class</returns>
		internal static Action CreateAction(IAction iAction)
		{
			Type t = GetObjectType(iAction.Type);
			return Activator.CreateInstance(t, BindingFlags.CreateInstance | BindingFlags.NonPublic | BindingFlags.Instance, null, new object[] { iAction }, null) as Action;
		}

		internal static Type GetObjectType(TaskActionType actionType)
		{
			switch (actionType)
			{
				case TaskActionType.ComHandler:
					return typeof(ComHandlerAction);

				case TaskActionType.SendEmail:
					return typeof(EmailAction);

				case TaskActionType.ShowMessage:
					return typeof(ShowMessageAction);

				case TaskActionType.Execute:
				default:
					return typeof(ExecAction);
			}
		}

		internal static T TryParse<T>(string val, T defaultVal)
		{
			T ret = defaultVal;
			try { ret = (T)Enum.Parse(typeof(T), val); } catch { }
			return ret;
		}

		internal virtual void Bind(V1Interop.ITask iTask)
		{
			if (Id != null)
				TaskDefinition.V1SetDataItem(iTask, "ActionId", Id);
			IBindAsExecAction bindable = this as IBindAsExecAction;
			if (bindable != null)
				TaskDefinition.V1SetDataItem(iTask, "ActionType", this.InternalActionType.ToString());
			object o = null;
			unboundValues.TryGetValue("Path", out o);
			iTask.SetApplicationName(bindable != null ? ExecAction.PowerShellPath : o?.ToString() ?? string.Empty);
			o = null;
			unboundValues.TryGetValue("Arguments", out o);
			iTask.SetParameters(bindable != null ? ExecAction.BuildPowerShellCmd(this.ActionType.ToString(), GetPowerShellCommand()) : o?.ToString() ?? string.Empty);
			o = null;
			unboundValues.TryGetValue("WorkingDirectory", out o);
			iTask.SetWorkingDirectory(o?.ToString() ?? string.Empty);
		}

		internal virtual void Bind(V2Interop.ITaskDefinition iTaskDef)
		{
			V2Interop.IActionCollection iActions = iTaskDef.Actions;
			CreateV2Action(iActions);
			Marshal.ReleaseComObject(iActions);
			foreach (string key in unboundValues.Keys)
			{
				try { ReflectionHelper.SetProperty(iAction, key, unboundValues[key]); }
				catch (TargetInvocationException tie) { throw tie.InnerException; }
				catch { }
			}
			unboundValues.Clear();
		}

		internal abstract void CreateV2Action(V2Interop.IActionCollection iActions);

		internal abstract string GetPowerShellCommand();

		internal T GetProperty<T, B>(string propName, T defaultValue = default(T))
		{
			if (iAction == null)
				return (unboundValues.ContainsKey(propName)) ? (T)unboundValues[propName] : defaultValue;
			return ReflectionHelper.GetProperty((B)iAction, propName, defaultValue);
		}

		internal void OnPropertyChanged(string propName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propName));
		}

		internal void SetProperty<T, B>(string propName, T value)
		{
			if (iAction == null)
			{
				if (Equals(value, default(T)))
					unboundValues.Remove(propName);
				else
					unboundValues[propName] = value;
			}
			else
				ReflectionHelper.SetProperty((B)iAction, propName, value);
			OnPropertyChanged(propName);
		}

		/// <summary>
		/// Copies the properties from another <see cref="Action"/> the current instance.
		/// </summary>
		/// <param name="sourceAction">The source <see cref="Action"/>.</param>
		protected virtual void CopyProperties(Action sourceAction)
		{
			Id = sourceAction.Id;
		}
	}

	/// <summary>
	/// Represents an action that fires a handler. Only available on Task Scheduler 2.0.
	/// </summary>
	[XmlType(IncludeInSchema = true)]
	[XmlRoot("ComHandler", Namespace = TaskDefinition.tns, IsNullable = false)]
	public class ComHandlerAction : Action, IBindAsExecAction
	{
		/// <summary>
		/// Creates an unbound instance of <see cref="ComHandlerAction"/>.
		/// </summary>
		public ComHandlerAction() { }

		/// <summary>
		/// Creates an unbound instance of <see cref="ComHandlerAction"/>.
		/// </summary>
		/// <param name="classId">Identifier of the handler class.</param>
		/// <param name="data">Addition data associated with the handler.</param>
		public ComHandlerAction(Guid classId, string data)
		{
			ClassId = classId;
			Data = data;
		}

		internal ComHandlerAction(V1Interop.ITask task) : base(task) { }

		internal ComHandlerAction(IAction action) : base(action) { }

		/// <summary>
		/// Gets or sets the identifier of the handler class.
		/// </summary>
		public Guid ClassId
		{
			get { return new Guid(GetProperty<string, IComHandlerAction>(nameof(ClassId), Guid.Empty.ToString())); }
			set { SetProperty<string, IComHandlerAction>(nameof(ClassId), value.ToString()); }
		}

		/// <summary>
		/// Gets the name of the object referred to by <see cref="ClassId"/>.
		/// </summary>
		public string ClassName => GetNameForCLSID(ClassId);

		/// <summary>
		/// Gets or sets additional data that is associated with the handler.
		/// </summary>
		[DefaultValue(null)]
		public string Data
		{
			get { return GetProperty<string, IComHandlerAction>(nameof(Data)); }
			set { SetProperty<string, IComHandlerAction>(nameof(Data), value); }
		}

		internal override TaskActionType InternalActionType => TaskActionType.ComHandler;

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(Action other) => base.Equals(other) && ClassId == ((ComHandlerAction)other).ClassId && Data == ((ComHandlerAction)other).Data;

		/// <summary>
		/// Gets a string representation of the <see cref="ComHandlerAction"/>.
		/// </summary>
		/// <returns>String representation of this action.</returns>
		public override string ToString() => string.Format(Properties.Resources.ComHandlerAction, ClassId, Data, Id, ClassName);

		internal static Action FromPowerShellCommand(string p)
		{
			var match = System.Text.RegularExpressions.Regex.Match(p, @"^\[Reflection.Assembly\]::LoadFile\('(?:[^']*)'\); \[Microsoft.Win32.TaskScheduler.TaskService\]::RunComHandlerAction\(\[GUID\]\('(?<g>[^']*)'\), '(?<d>[^']*)'\);?\s*$");
			return (match.Success) ? new ComHandlerAction(new Guid(match.Groups["g"].Value), match.Groups["d"].Value.Replace("''", "'")) : null;
		}

		/// <summary>
		/// Gets the name for CLSID.
		/// </summary>
		/// <param name="guid">The unique identifier.</param>
		/// <returns></returns>
		internal static string GetNameForCLSID(Guid guid)
		{
			using (RegistryKey k = Registry.ClassesRoot.OpenSubKey("CLSID", false))
			{
				if (k != null)
				{
					using (RegistryKey k2 = k.OpenSubKey(guid.ToString("B"), false))
						return k2 != null ? k2.GetValue(null) as string : null;
				}
			}
			return null;
		}

		internal override void CreateV2Action(IActionCollection iActions)
		{
			iAction = iActions.Create(TaskActionType.ComHandler);
		}

		internal override string GetPowerShellCommand()
		{
			var sb = new System.Text.StringBuilder();
			sb.Append($"[Reflection.Assembly]::LoadFile('{System.Reflection.Assembly.GetExecutingAssembly().Location}'); ");
			sb.Append($"[Microsoft.Win32.TaskScheduler.TaskService]::RunComHandlerAction([GUID]('{ClassId.ToString("D")}'), '{Data?.Replace("'", "''") ?? string.Empty}'); ");
			return sb.ToString();
		}

		/// <summary>
		/// Copies the properties from another <see cref="Action"/> the current instance.
		/// </summary>
		/// <param name="sourceAction">The source <see cref="Action"/>.</param>
		protected override void CopyProperties(Action sourceAction)
		{
			if (sourceAction.GetType() == GetType())
			{
				base.CopyProperties(sourceAction);
				ClassId = ((ComHandlerAction)sourceAction).ClassId;
				Data = ((ComHandlerAction)sourceAction).Data;
			}
		}
	}

	/// <summary>
	/// Represents an action that sends an e-mail.
	/// </summary>
	[XmlType(IncludeInSchema = true)]
	[XmlRoot("SendEmail", Namespace = TaskDefinition.tns, IsNullable = false)]
	public sealed class EmailAction : Action, IBindAsExecAction
	{
		private const string ImportanceHeader = "Importance";

		private NamedValueCollection nvc = null;

		/// <summary>
		/// Creates an unbound instance of <see cref="EmailAction"/>.
		/// </summary>
		public EmailAction() { }

		/// <summary>
		/// Creates an unbound instance of <see cref="EmailAction"/>.
		/// </summary>
		/// <param name="subject">Subject of the e-mail.</param>
		/// <param name="from">E-mail address that you want to send the e-mail from.</param>
		/// <param name="to">E-mail address or addresses that you want to send the e-mail to.</param>
		/// <param name="body">Body of the e-mail that contains the e-mail message.</param>
		/// <param name="mailServer">Name of the server that you use to send e-mail from.</param>
		public EmailAction(string subject, string from, string to, string body, string mailServer)
		{
			Subject = subject;
			From = from;
			To = to;
			Body = body;
			Server = mailServer;
		}

		internal EmailAction(V1Interop.ITask task) : base(task) { }

		internal EmailAction(IAction action) : base(action) { }

		/// <summary>
		/// Gets or sets an array of file paths to be sent as attachments with the e-mail. Each item must be a <see cref="System.String"/> value containing a path to file.
		/// </summary>
		[XmlArray("Attachments", IsNullable = true)]
		[XmlArrayItem("File", typeof(string))]
		[DefaultValue(null)]
		public object[] Attachments
		{
			get { return GetProperty<object[], IEmailAction>(nameof(Attachments)); }
			set
			{
				if (value != null)
				{
					if (value.Length > 8)
						throw new ArgumentOutOfRangeException("Attachments", "Attachments array cannot contain more than 8 items.");
					foreach (var o in value)
						if (!(o is string) || !System.IO.File.Exists((string)o))
							throw new ArgumentException("Each value of the array must contain a valid file reference.", nameof(Attachments));
				}
				if (iAction == null && (value == null || value.Length == 0))
				{
					unboundValues.Remove(nameof(Attachments));
					OnPropertyChanged(nameof(Attachments));
				}
				else
					SetProperty<object[], IEmailAction>(nameof(Attachments), value);
			}
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to Bcc in the e-mail.
		/// </summary>
		[DefaultValue(null)]
		public string Bcc
		{
			get { return GetProperty<string, IEmailAction>(nameof(Bcc)); }
			set { SetProperty<string, IEmailAction>(nameof(Bcc), value); }
		}

		/// <summary>
		/// Gets or sets the body of the e-mail that contains the e-mail message.
		/// </summary>
		[DefaultValue(null)]
		public string Body
		{
			get { return GetProperty<string, IEmailAction>(nameof(Body)); }
			set { SetProperty<string, IEmailAction>(nameof(Body), value); }
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to Cc in the e-mail.
		/// </summary>
		[DefaultValue(null)]
		public string Cc
		{
			get { return GetProperty<string, IEmailAction>(nameof(Cc)); }
			set { SetProperty<string, IEmailAction>(nameof(Cc), value); }
		}

		/// <summary>
		/// Gets or sets the e-mail address that you want to send the e-mail from.
		/// </summary>
		[DefaultValue(null)]
		public string From
		{
			get { return GetProperty<string, IEmailAction>(nameof(From)); }
			set { SetProperty<string, IEmailAction>(nameof(From), value); }
		}

		/// <summary>
		/// Gets or sets the header information in the e-mail message to send.
		/// </summary>
		[XmlArray]
		[XmlArrayItem("HeaderField", typeof(NameValuePair))]
		public NamedValueCollection HeaderFields
		{
			get
			{
				if (nvc == null)
				{
					if (iAction != null)
						nvc = new NamedValueCollection(((V2Interop.IEmailAction)iAction).HeaderFields);
					else
						nvc = new NamedValueCollection();
					nvc.AttributedXmlFormat = false;
					nvc.CollectionChanged += (o, e) => OnPropertyChanged(nameof(HeaderFields));
				}
				return nvc;
			}
		}

		/// <summary>
		/// Gets or sets the priority of the e-mail message.
		/// </summary>
		/// <value>
		/// A <see cref="System.Net.Mail.MailPriority"/> that contains the priority of this message.
		/// </value>
		[XmlIgnore]
		[DefaultValue(typeof(System.Net.Mail.MailPriority), "Normal")]
		public System.Net.Mail.MailPriority Priority
		{
			get
			{
				string s;
				if (nvc != null && HeaderFields.TryGetValue(ImportanceHeader, out s))
					return TryParse(s, System.Net.Mail.MailPriority.Normal);
				return System.Net.Mail.MailPriority.Normal;
			}
			set
			{
				HeaderFields[ImportanceHeader] = value.ToString();
			}
		}

		/// <summary>
		/// Gets or sets the e-mail address that you want to reply to.
		/// </summary>
		[DefaultValue(null)]
		public string ReplyTo
		{
			get { return GetProperty<string, IEmailAction>(nameof(ReplyTo)); }
			set { SetProperty<string, IEmailAction>(nameof(ReplyTo), value); }
		}

		/// <summary>
		/// Gets or sets the name of the server that you use to send e-mail from.
		/// </summary>
		[DefaultValue(null)]
		public string Server
		{
			get { return GetProperty<string, IEmailAction>(nameof(Server)); }
			set { SetProperty<string, IEmailAction>(nameof(Server), value); }
		}

		/// <summary>
		/// Gets or sets the subject of the e-mail.
		/// </summary>
		[DefaultValue(null)]
		public string Subject
		{
			get { return GetProperty<string, IEmailAction>(nameof(Subject)); }
			set { SetProperty<string, IEmailAction>(nameof(Subject), value); }
		}

		/// <summary>
		/// Gets or sets the e-mail address or addresses that you want to send the e-mail to.
		/// </summary>
		[DefaultValue(null)]
		public string To
		{
			get { return GetProperty<string, IEmailAction>(nameof(To)); }
			set { SetProperty<string, IEmailAction>(nameof(To), value); }
		}

		internal override TaskActionType InternalActionType => TaskActionType.SendEmail;

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(Action other) => base.Equals(other) && GetPowerShellCommand() == GetPowerShellCommand();

		/// <summary>
		/// Gets a string representation of the <see cref="EmailAction"/>.
		/// </summary>
		/// <returns>String representation of this action.</returns>
		public override string ToString() => string.Format(Properties.Resources.EmailAction, Subject, To, Cc, Bcc, From, ReplyTo, Body, Server, Id);

		internal static Action FromPowerShellCommand(string p)
		{
			var match = System.Text.RegularExpressions.Regex.Match(p, @"^Send-MailMessage -From '(?<from>(?:[^']|'')*)' -Subject '(?<subject>(?:[^']|'')*)' -SmtpServer '(?<server>(?:[^']|'')*)'(?: -Encoding UTF8)?(?: -To (?<to>'(?:(?:[^']|'')*)'(?:, '(?:(?:[^']|'')*)')*))?(?: -Cc (?<cc>'(?:(?:[^']|'')*)'(?:, '(?:(?:[^']|'')*)')*))?(?: -Bcc (?<bcc>'(?:(?:[^']|'')*)'(?:, '(?:(?:[^']|'')*)')*))?(?:(?: -BodyAsHtml)? -Body '(?<body>(?:[^']|'')*)')?(?: -Attachments (?<att>'(?:(?:[^']|'')*)'(?:, '(?:(?:[^']|'')*)')*))?(?: -Priority (?<imp>High|Normal|Low))?;?\s*$");
			if (match.Success)
			{
				EmailAction action = new EmailAction(UnPrep(FromUTF8(match.Groups["subject"].Value)), UnPrep(match.Groups["from"].Value), FromPS(match.Groups["to"]), UnPrep(FromUTF8(match.Groups["body"].Value)), UnPrep(match.Groups["server"].Value))
				{ Cc = FromPS(match.Groups["cc"]), Bcc = FromPS(match.Groups["bcc"]) };
				if (match.Groups["att"].Success)
					action.Attachments = Array.ConvertAll<string, object>(FromPS(match.Groups["att"].Value), s => s);
				if (match.Groups["imp"].Success)
					action.HeaderFields[ImportanceHeader] = match.Groups["imp"].Value;
				return action;
			}
			return null;
		}

		internal override void Bind(ITaskDefinition iTaskDef)
		{
			base.Bind(iTaskDef);
			if (nvc != null)
				nvc.Bind(((V2Interop.IEmailAction)iAction).HeaderFields);
		}

		internal override void CreateV2Action(IActionCollection iActions)
		{
			iAction = iActions.Create(TaskActionType.SendEmail);
		}

		internal override string GetPowerShellCommand()
		{
			// Send-MailMessage [-To] <String[]> [-Subject] <String> [[-Body] <String> ] [[-SmtpServer] <String> ] -From <String> [-Attachments <String[]> ]
			//    [-Bcc <String[]> ] [-BodyAsHtml] [-Cc <String[]> ] [-Credential <PSCredential> ] [-DeliveryNotificationOption <DeliveryNotificationOptions> ]
			//    [-Encoding <Encoding> ] [-Port <Int32> ] [-Priority <MailPriority> ] [-UseSsl] [ <CommonParameters>]
			bool bodyIsHtml = Body != null && Body.Trim().StartsWith("<") && Body.Trim().EndsWith(">");
			var sb = new System.Text.StringBuilder();
			sb.AppendFormat("Send-MailMessage -From '{0}' -Subject '{1}' -SmtpServer '{2}' -Encoding UTF8", Prep(From), ToUTF8(Prep(Subject)), Prep(Server));
			if (!string.IsNullOrEmpty(To))
				sb.AppendFormat(" -To {0}", ToPS(To));
			if (!string.IsNullOrEmpty(Cc))
				sb.AppendFormat(" -Cc {0}", ToPS(Cc));
			if (!string.IsNullOrEmpty(Bcc))
				sb.AppendFormat(" -Bcc {0}", ToPS(Bcc));
			if (bodyIsHtml)
				sb.Append(" -BodyAsHtml");
			if (!string.IsNullOrEmpty(Body))
				sb.AppendFormat(" -Body '{0}'", ToUTF8(Prep(Body)));
			if (Attachments != null && Attachments.Length > 0)
				sb.AppendFormat(" -Attachments {0}", ToPS(Array.ConvertAll<object, string>(Attachments, o => Prep(o.ToString()))));
			var hdr = new List<string>(HeaderFields.Names);
			if (hdr.Contains(ImportanceHeader))
			{
				var p = Priority;
				if (p != System.Net.Mail.MailPriority.Normal)
					sb.Append($" -Priority {p.ToString()}");
				hdr.Remove(ImportanceHeader);
			}
			if (hdr.Count > 0)
				throw new InvalidOperationException("Under Windows 8 and later, EmailAction objects are converted to PowerShell. This action contains headers that are not supported.");
			sb.Append("; ");
			return sb.ToString();

			/*var msg = new System.Net.Mail.MailMessage(this.From, this.To, this.Subject, this.Body);
			if (!string.IsNullOrEmpty(this.Bcc))
				msg.Bcc.Add(this.Bcc);
			if (!string.IsNullOrEmpty(this.Cc))
				msg.CC.Add(this.Cc);
			if (!string.IsNullOrEmpty(this.ReplyTo))
				msg.ReplyTo = new System.Net.Mail.MailAddress(this.ReplyTo);
			if (this.Attachments != null && this.Attachments.Length > 0)
				foreach (string s in this.Attachments)
					msg.Attachments.Add(new System.Net.Mail.Attachment(s));
			if (this.nvc != null)
				foreach (var ha in this.HeaderFields)
					msg.Headers.Add(ha.Name, ha.Value);
			var client = new System.Net.Mail.SmtpClient(this.Server);
			client.Send(msg);*/
		}

		/// <summary>
		/// Copies the properties from another <see cref="Action"/> the current instance.
		/// </summary>
		/// <param name="sourceAction">The source <see cref="Action"/>.</param>
		protected override void CopyProperties(Action sourceAction)
		{
			if (sourceAction.GetType() == GetType())
			{
				base.CopyProperties(sourceAction);
				if (((EmailAction)sourceAction).Attachments != null)
					Attachments = (object[])((EmailAction)sourceAction).Attachments.Clone();
				Bcc = ((EmailAction)sourceAction).Bcc;
				Body = ((EmailAction)sourceAction).Body;
				Cc = ((EmailAction)sourceAction).Cc;
				From = ((EmailAction)sourceAction).From;
				if (((EmailAction)sourceAction).nvc != null)
					((EmailAction)sourceAction).HeaderFields.CopyTo(HeaderFields);
				ReplyTo = ((EmailAction)sourceAction).ReplyTo;
				Server = ((EmailAction)sourceAction).Server;
				Subject = ((EmailAction)sourceAction).Subject;
				To = ((EmailAction)sourceAction).To;
			}
		}

		private static string[] FromPS(string p)
		{
			var list = p.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries);
			return Array.ConvertAll<string, string>(list, i => UnPrep(i).Trim('\''));
		}

		private static string FromPS(System.Text.RegularExpressions.Group g, string delimeter = ";")
		{
			if (g.Success)
				return string.Join(delimeter, FromPS(g.Value));
			return null;
		}

		private static string FromUTF8(string s)
		{
			byte[] bytes = System.Text.Encoding.UTF8.GetBytes(s);
			return System.Text.Encoding.Default.GetString(bytes);
		}

		private static string Prep(string s) => s?.Replace("'", "''");

		private static string ToPS(string input, char[] delimeters = null)
		{
			if (delimeters == null)
				delimeters = new char[] { ';', ',' };
			return ToPS(Array.ConvertAll<string, string>(input.Split(delimeters), i => Prep(i.Trim())));
		}

		private static string ToPS(string[] input) => string.Join(", ", Array.ConvertAll<string, string>(input, i => string.Concat("'", i.Trim(), "'")));

		private static string ToUTF8(string s)
		{
			if (s == null) return null;
			byte[] bytes = System.Text.Encoding.Default.GetBytes(s);
			return System.Text.Encoding.UTF8.GetString(bytes);
		}

		private static string UnPrep(string s) => s?.Replace("''", "'");
	}

	/// <summary>
	/// Represents an action that executes a command-line operation.
	/// </summary>
	[XmlRoot("Exec", Namespace = TaskDefinition.tns, IsNullable = false)]
	public class ExecAction : Action
	{
#if DEBUG
		internal const string PowerShellArgFormat = "-NoExit -Command \"& {{<# {0}:{1} #> {2}}}\"";
#else
		internal const string PowerShellArgFormat = "-NoLogo -NonInteractive -WindowStyle Hidden -Command \"& {{<# {0}:{1} #> {2}}}\"";
#endif
		internal const string PowerShellPath = "powershell";
		internal const string ScriptIdentifer = "TSML_20140424";

		/// <summary>
		/// Creates a new instance of an <see cref="ExecAction"/> that can be added to <see cref="TaskDefinition.Actions"/>.
		/// </summary>
		public ExecAction() { }

		/// <summary>
		/// Creates a new instance of an <see cref="ExecAction"/> that can be added to <see cref="TaskDefinition.Actions"/>.
		/// </summary>
		/// <param name="path">Path to an executable file.</param>
		/// <param name="arguments">Arguments associated with the command-line operation. This value can be null.</param>
		/// <param name="workingDirectory">Directory that contains either the executable file or the files that are used by the executable file. This value can be null.</param>
		public ExecAction(string path, string arguments = null, string workingDirectory = null)
		{
			Path = path;
			Arguments = arguments;
			WorkingDirectory = workingDirectory;
		}

		internal ExecAction(V1Interop.ITask task) : base(task) { }

		internal ExecAction(IAction action) : base(action) { }

		/// <summary>
		/// Gets or sets the arguments associated with the command-line operation.
		/// </summary>
		[DefaultValue("")]
		public string Arguments
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetParameters();
				return GetProperty<string, IExecAction>(nameof(Arguments), "");
			}
			set
			{
				if (v1Task != null)
					v1Task.SetParameters(value);
				else
					SetProperty<string, IExecAction>(nameof(Arguments), value);
			}
		}

		/// <summary>
		/// Gets or sets the path to an executable file.
		/// </summary>
		[XmlElement("Command")]
		[DefaultValue("")]
		public string Path
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetApplicationName();
				return GetProperty<string, IExecAction>(nameof(Path), "");
			}
			set
			{
				if (v1Task != null)
					v1Task.SetApplicationName(value);
				else
					SetProperty<string, IExecAction>(nameof(Path), value);
			}
		}

		/// <summary>
		/// Gets or sets the directory that contains either the executable file or the files that are used by the executable file.
		/// </summary>
		[DefaultValue("")]
		public string WorkingDirectory
		{
			get
			{
				if (v1Task != null)
					return v1Task.GetWorkingDirectory();
				return GetProperty<string, IExecAction>(nameof(WorkingDirectory), "");
			}
			set
			{
				if (v1Task != null)
					v1Task.SetWorkingDirectory(value);
				else
					SetProperty<string, IExecAction>(nameof(WorkingDirectory), value);
			}
		}

		internal override TaskActionType InternalActionType => TaskActionType.Execute;

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns><c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.</returns>
		public override bool Equals(Action other) => base.Equals(other) && Path == ((ExecAction)other).Path && Arguments == ((ExecAction)other).Arguments && WorkingDirectory == ((ExecAction)other).WorkingDirectory;

		/// <summary>
		/// Gets a string representation of the <see cref="ExecAction"/>.
		/// </summary>
		/// <returns>String representation of this action.</returns>
		public override string ToString() => string.Format(Properties.Resources.ExecAction, Path, Arguments, WorkingDirectory, Id);

		internal static string BuildPowerShellCmd(string actionType, string cmd) => string.Format(PowerShellArgFormat, ScriptIdentifer, actionType, cmd);

		internal static ExecAction ConvertToPowerShellAction(Action action) => CreatePowerShellAction(action.ActionType.ToString(), action.GetPowerShellCommand());

		internal static ExecAction CreatePowerShellAction(string actionType, string cmd) => new ExecAction(PowerShellPath, BuildPowerShellCmd(actionType, cmd));

		internal static Action FromPowerShellCommand(string p)
		{
			var match = System.Text.RegularExpressions.Regex.Match(p, "^Start-Process -FilePath '(?<p>[^']*)'(?: -ArgumentList '(?<a>[^']*)')?(?: -WorkingDirectory '(?<d>[^']*)')?;?\\s*$");
			return (match.Success) ? new ExecAction(match.Groups["p"].Value, match.Groups["a"].Success ? match.Groups["a"].Value.Replace("''", "'") : null, match.Groups["d"].Success ? match.Groups["d"].Value : null) : null;
		}

		internal override void CreateV2Action(IActionCollection iActions)
		{
			iAction = iActions.Create(TaskActionType.Execute);
		}

		internal override string GetPowerShellCommand()
		{
			var sb = new System.Text.StringBuilder($"Start-Process -FilePath '{Path}'");
			if (!string.IsNullOrEmpty(Arguments))
				sb.Append($" -ArgumentList '{Arguments?.Replace("'", "''")}'");
			if (!string.IsNullOrEmpty(WorkingDirectory))
				sb.Append($" -WorkingDirectory '{WorkingDirectory}'");
			return sb.Append("; ").ToString();
		}

		internal string[] ParsePowerShellItems()
		{
			if ((Path?.EndsWith(PowerShellPath, StringComparison.InvariantCultureIgnoreCase) ?? false) ||
				(Path?.EndsWith(PowerShellPath + ".exe", StringComparison.InvariantCultureIgnoreCase) ?? false) || (Arguments?.Contains(ScriptIdentifer) ?? false))
			{
				var match = System.Text.RegularExpressions.Regex.Match(Arguments, @"<# " + ScriptIdentifer + ":(?<type>\\w+) #> (?<cmd>.+)}\"$");
				if (match.Success)
					return new string[] { match.Groups["type"].Value, match.Groups["cmd"].Value };
			}
			return null;
		}

		/// <summary>
		/// Copies the properties from another <see cref="Action"/> the current instance.
		/// </summary>
		/// <param name="sourceAction">The source <see cref="Action"/>.</param>
		protected override void CopyProperties(Action sourceAction)
		{
			if (sourceAction.GetType() == GetType())
			{
				base.CopyProperties(sourceAction);
				Path = ((ExecAction)sourceAction).Path;
				Arguments = ((ExecAction)sourceAction).Arguments;
				WorkingDirectory = ((ExecAction)sourceAction).WorkingDirectory;
			}
		}
	}

	/// <summary>
	/// Represents an action that shows a message box when a task is activated.
	/// </summary>
	[XmlType(IncludeInSchema = true)]
	[XmlRoot("ShowMessage", Namespace = TaskDefinition.tns, IsNullable = false)]
	public sealed class ShowMessageAction : Action, IBindAsExecAction
	{
		/// <summary>
		/// Creates a new unbound instance of <see cref="ShowMessageAction"/>.
		/// </summary>
		public ShowMessageAction() { }

		/// <summary>
		/// Creates a new unbound instance of <see cref="ShowMessageAction"/>.
		/// </summary>
		/// <param name="messageBody">Message text that is displayed in the body of the message box.</param>
		/// <param name="title">Title of the message box.</param>
		public ShowMessageAction(string messageBody, string title)
		{
			MessageBody = messageBody;
			Title = title;
		}

		internal ShowMessageAction(V1Interop.ITask task) : base(task) { }

		internal ShowMessageAction(IAction action) : base(action) { }

		/// <summary>
		/// Gets or sets the message text that is displayed in the body of the message box.
		/// </summary>
		[XmlElement("Body")]
		[DefaultValue(null)]
		public string MessageBody
		{
			get { return GetProperty<string, IShowMessageAction>(nameof(MessageBody)); }
			set { SetProperty<string, IShowMessageAction>(nameof(MessageBody), value); }
		}

		/// <summary>
		/// Gets or sets the title of the message box.
		/// </summary>
		[DefaultValue(null)]
		public string Title
		{
			get { return GetProperty<string, IShowMessageAction>(nameof(Title)); }
			set { SetProperty<string, IShowMessageAction>(nameof(Title), value); }
		}

		internal override TaskActionType InternalActionType => TaskActionType.ShowMessage;

		/// <summary>
		/// Indicates whether the current object is equal to another object of the same type.
		/// </summary>
		/// <param name="other">An object to compare with this object.</param>
		/// <returns>
		/// <c>true</c> if the current object is equal to the <paramref name="other" /> parameter; otherwise, <c>false</c>.
		/// </returns>
		public override bool Equals(Action other) => base.Equals(other) && string.Equals(this.Title, (other as ShowMessageAction)?.Title) && string.Equals(this.MessageBody, (other as ShowMessageAction)?.MessageBody);

		/// <summary>
		/// Gets a string representation of the <see cref="ShowMessageAction"/>.
		/// </summary>
		/// <returns>String representation of this action.</returns>
		public override string ToString() => string.Format(Properties.Resources.ShowMessageAction, Title, MessageBody, Id);

		internal static Action FromPowerShellCommand(string p)
		{
			var match = System.Text.RegularExpressions.Regex.Match(p, @"^\[System.Reflection.Assembly\]::LoadWithPartialName\('System.Windows.Forms'\); \[System.Windows.Forms.MessageBox\]::Show\('(?<msg>(?:[^']|'')*)'(?:,'(?<t>(?:[^']|'')*)')?\);?\s*$");
			return (match.Success) ? new ShowMessageAction(match.Groups["msg"].Value.Replace("''", "'"), match.Groups["t"].Success ? match.Groups["t"].Value.Replace("''", "'") : null) : null;
		}

		internal override void CreateV2Action(IActionCollection iActions)
		{
			iAction = iActions.Create(TaskActionType.ShowMessage);
		}

		internal override string GetPowerShellCommand()
		{
			// [System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms'); [System.Windows.Forms.MessageBox]::Show('Your_Desired_Message','Your_Desired_Title');
			var sb = new System.Text.StringBuilder("[System.Reflection.Assembly]::LoadWithPartialName('System.Windows.Forms'); [System.Windows.Forms.MessageBox]::Show('");
			sb.Append(MessageBody.Replace("'", "''"));
			if (Title != null)
			{
				sb.Append("','");
				sb.Append(Title.Replace("'", "''"));
			}
			sb.Append("'); ");
			return sb.ToString();
		}

		/// <summary>
		/// Copies the properties from another <see cref="Action"/> the current instance.
		/// </summary>
		/// <param name="sourceAction">The source <see cref="Action"/>.</param>
		protected override void CopyProperties(Action sourceAction)
		{
			if (sourceAction.GetType() == GetType())
			{
				base.CopyProperties(sourceAction);
				Title = ((ShowMessageAction)sourceAction).Title;
				MessageBody = ((ShowMessageAction)sourceAction).MessageBody;
			}
		}
	}
}