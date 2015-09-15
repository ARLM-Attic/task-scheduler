﻿using System;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.Serialization;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Abstract class for throwing a method specific exception.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough, Serializable]
	public abstract class TSNotSupportedException : Exception, ISerializable
	{
		/// <summary>Defines the minimum supported version for the action not allowed by this exception.</summary>
		protected TaskCompatibility min;
		private string myMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="TSNotSupportedException"/> class.
		/// </summary>
		/// <param name="serializationInfo">The serialization information.</param>
		/// <param name="streamingContext">The streaming context.</param>
		protected TSNotSupportedException(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(serializationInfo, streamingContext)
		{
		}

		internal TSNotSupportedException(TaskCompatibility minComp)
		{
			min = minComp;
			StackTrace stackTrace = new StackTrace();
			StackFrame stackFrame = stackTrace.GetFrame(2);
			MethodBase methodBase = stackFrame.GetMethod();
			myMessage = $"{methodBase.DeclaringType.Name}.{methodBase.Name} is not supported on {LibName}";
		}

		internal TSNotSupportedException(string message, TaskCompatibility minComp)
		{
			myMessage = message;
			min = minComp;
		}

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		public override string Message => myMessage;

		/// <summary>
		/// Gets the minimum supported TaskScheduler version required for this method or property.
		/// </summary>
		public TaskCompatibility MinimumSupportedVersion => min;

		internal abstract string LibName { get; }

		/// <summary>
		/// Gets the object data.
		/// </summary>
		/// <param name="info">The information.</param>
		/// <param name="context">The context.</param>
		[SecurityCritical, SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo info, StreamingContext context)
		{
			if (info == null)
				throw new ArgumentNullException(nameof(info));
			info.AddValue("min", min);
			base.GetObjectData(info, context);
		}
	}

	/// <summary>
	/// Thrown when the calling method is not supported by Task Scheduler 1.0.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough, Serializable]
	public class NotV1SupportedException : TSNotSupportedException
	{
		internal NotV1SupportedException() : base(TaskCompatibility.V2) { }
		internal NotV1SupportedException(string message) : base(message, TaskCompatibility.V2) { }
		internal override string LibName => "Task Scheduler 1.0";
	}

	/// <summary>
	/// Thrown when the calling method is not supported by Task Scheduler 2.0.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough, Serializable]
	public class NotV2SupportedException : TSNotSupportedException
	{
		internal NotV2SupportedException() : base(TaskCompatibility.V1) { }
		internal NotV2SupportedException(string message) : base(message, TaskCompatibility.V1) { }
		internal override string LibName => "Task Scheduler 2.0 (1.2)";
	}


	/// <summary>
	/// Thrown when the calling method is not supported by Task Scheduler versions prior to the one specified.
	/// </summary>
	[System.Diagnostics.DebuggerStepThrough, Serializable]
	public class NotSupportedPriorToException : TSNotSupportedException
	{
		internal NotSupportedPriorToException(TaskCompatibility supportedVersion) : base(supportedVersion) { }
		internal override string LibName => $"Task Scheduler versions prior to 2.{((int)min) - 2} (1.{(int)min})";
	}

}
