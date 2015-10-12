﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Microsoft.Win32.TaskScheduler
{
	/// <summary>
	/// Collection that contains the actions that are performed by the task.
	/// </summary>
	/// <remarks>A Task Scheduler 1.0 task can only contain a single <see cref="ExecAction"/>.</remarks>
	[XmlRoot("Actions", Namespace = TaskDefinition.tns, IsNullable = false)]
	public sealed class ActionCollection : IList<Action>, IDisposable, IXmlSerializable, IList
	{
		private V1Interop.ITask v1Task;
		private V2Interop.ITaskDefinition v2Def;
		private V2Interop.IActionCollection v2Coll;

		internal ActionCollection(V1Interop.ITask task)
		{
			v1Task = task;
		}

		internal ActionCollection(V2Interop.ITaskDefinition iTaskDef)
		{
			v2Def = iTaskDef;
			v2Coll = iTaskDef.Actions;
			UnconvertUnsupportedActions();
		}

		/// <summary>
		/// Gets or sets a an action at the specified index.
		/// </summary>
		/// <value>The zero-based index of the action to get or set.</value>
		public Action this[int index]
		{
			get
			{
				if (v2Coll != null)
					return Action.CreateAction(v2Coll[++index]);
				if (index == 0)
					return new ExecAction(v1Task);
				throw new ArgumentOutOfRangeException();
			}
			set
			{
				if (Count <= index)
					throw new ArgumentOutOfRangeException(nameof(index), index, "Index is not a valid index in the ActionCollection");
				RemoveAt(index);
				Insert(index, value);
			}
		}

		/// <summary>
		/// Gets or sets a specified action from the collection.
		/// </summary>
		/// <value>
		/// The <see cref="Action"/>.
		/// </value>
		/// <param name="actionId">The id (<see cref="Action.Id" />) of the action to be retrieved.</param>
		/// <returns>
		/// Specialized <see cref="Action" /> instance.
		/// </returns>
		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentOutOfRangeException"></exception>
		/// <exception cref="System.NullReferenceException"></exception>
		/// <exception cref="System.InvalidOperationException">Mismatching Id for action and lookup.</exception>
		public Action this[string actionId]
		{
			get
			{
				if (string.IsNullOrEmpty(actionId))
					throw new ArgumentNullException(actionId);
				foreach (Action t in this)
					if (string.Equals(t.Id, actionId))
						return t;
				throw new ArgumentOutOfRangeException(actionId);
			}
			set
			{
				if (value == null)
					throw new NullReferenceException();
				if (string.IsNullOrEmpty(actionId))
					throw new ArgumentNullException(actionId);
				if (actionId != value.Id)
					throw new InvalidOperationException("Mismatching Id for action and lookup.");
				int index = IndexOf(actionId);
				if (index >= 0)
				{
					RemoveAt(index);
					Insert(index, value);
				}
				else
					Add(value);
			}
		}

		/// <summary>
		/// Gets or sets the identifier of the principal for the task.
		/// </summary>
		/// <exception cref="NotV1SupportedException">Not supported under Task Scheduler 1.0.</exception>
		[System.Xml.Serialization.XmlAttribute(AttributeName = "Context", DataType = "IDREF")]
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
				return ((string)v1Task.GetApplicationName()).Length == 0 ? 0 : 1;
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
				return XmlSerializationHelper.WriteObjectToXmlText(this);
			}
			set
			{
				if (v2Coll != null)
					v2Coll.XmlText = value;
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
			v2Def = null;
			v2Coll = null;
		}

		/// <summary>
		/// Adds an action to the task.
		/// </summary>
		/// <param name="action">A derived <see cref="Action"/> class.</param>
		/// <returns>The bound <see cref="Action"/> that was added to the collection.</returns>
		public Action Add(Action action)
		{
			if (v2Def != null)
				action.Bind(v2Def);
			else
				action.Bind(v1Task);
			return action;
		}

		/// <summary>
		/// Adds an <see cref="ExecAction"/> to the task.
		/// </summary>
		/// <param name="path">Path to an executable file.</param>
		/// <param name="arguments">Arguments associated with the command-line operation. This value can be null.</param>
		/// <param name="workingDirectory">Directory that contains either the executable file or the files that are used by the executable file. This value can be null.</param>
		/// <returns>The bound <see cref="ExecAction"/> that was added to the collection.</returns>
		public ExecAction Add(string path, string arguments = null, string workingDirectory = null) => (ExecAction)Add(new ExecAction(path, arguments, workingDirectory));

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
		/// Clears all actions from the task.
		/// </summary>
		public void Clear()
		{
			if (v2Coll != null)
				v2Coll.Clear();
			else
				Add(new ExecAction());
		}

		/// <summary>
		/// Determines whether the <see cref="T:System.Collections.Generic.ICollection`1" /> contains a specific value.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		/// true if <paramref name="item" /> is found in the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false.
		/// </returns>
		public bool Contains(Action item) => IndexOf(item) >= 0;

		/// <summary>
		/// Determines whether the specified action type is contained in this collection.
		/// </summary>
		/// <param name="actionType">Type of the action.</param>
		/// <returns>
		///   <c>true</c> if the specified action type is contained in this collection; otherwise, <c>false</c>.
		/// </returns>
		public bool ContainsType(Type actionType)
		{
			foreach (Action a in this)
				if (a.GetType() == actionType)
					return true;
			return false;
		}

		/// <summary>
		/// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1" /> to an <see cref="Array"/>, starting at a particular <see cref="Array"/> index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the elements copied from <see cref="T:System.Collections.Generic.ICollection`1" />. The <see cref="Array"/> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
		/// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null.</exception>
		/// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
		/// <exception cref="System.ArgumentException">The number of elements in the source <see cref="T:System.Collections.Generic.ICollection`1" /> is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination <paramref name="array"/>.</exception>
		public void CopyTo(Action[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			if (Count > (array.Length - arrayIndex))
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));
			for (int i = 0; i < Count; i++)
				array[arrayIndex + i] = (Action)this[i].Clone();
		}

		/// <summary>
		/// Retrieves an enumeration of each of the actions.
		/// </summary>
		/// <returns>Returns an object that implements the <see cref="IEnumerator"/> interface and that can iterate through the <see cref="Action"/> objects within the <see cref="ActionCollection"/>.</returns>
		public IEnumerator<Action> GetEnumerator()
		{
			if (v2Coll != null)
				return new Enumerator(this);
			return new Enumerator(v1Task);
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </summary>
		/// <param name="item">The object to locate in the <see cref="T:System.Collections.Generic.IList`1" />.</param>
		/// <returns>
		/// The index of <paramref name="item" /> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(Action item)
		{
			for (int i = 0; i < Count; i++)
			{
				if (this[i].Equals(item))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Determines the index of a specific item in the <see cref="T:System.Collections.Generic.IList`1" />.
		/// </summary>
		/// <param name="actionId">The id (<see cref="Action.Id"/>) of the action to be retrieved.</param>
		/// <returns>
		/// The index of <paramref name="actionId" /> if found in the list; otherwise, -1.
		/// </returns>
		public int IndexOf(string actionId)
		{
			if (string.IsNullOrEmpty(actionId))
				throw new ArgumentNullException(nameof(actionId));
			for (int i = 0; i < Count; i++)
			{
				if (string.Equals(this[i].Id, actionId))
					return i;
			}
			return -1;
		}

		/// <summary>
		/// Inserts an action at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which action should be inserted.</param>
		/// <param name="action">The action to insert into the list.</param>
		public void Insert(int index, Action action)
		{
			if (v2Coll == null && Count > 0)
				throw new NotV1SupportedException("Only a single action is allowed.");

			Action[] pushItems = new Action[Count - index];
			for (int i = index; i < Count; i++)
				pushItems[i - index] = (Action)this[i].Clone();
			for (int j = Count - 1; j >= index; j--)
				RemoveAt(j);
			Add(action);
			for (int k = 0; k < pushItems.Length; k++)
				Add(pushItems[k]);
		}

		/// <summary>
		/// Removes the first occurrence of a specific object from the <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </summary>
		/// <param name="item">The object to remove from the <see cref="T:System.Collections.Generic.ICollection`1" />.</param>
		/// <returns>
		/// true if <paramref name="item" /> was successfully removed from the <see cref="T:System.Collections.Generic.ICollection`1" />; otherwise, false. This method also returns false if <paramref name="item" /> is not found in the original <see cref="T:System.Collections.Generic.ICollection`1" />.
		/// </returns>
		public bool Remove(Action item)
		{
			int idx = IndexOf(item);
			if (idx != -1)
			{
				try
				{
					RemoveAt(idx);
					return true;
				}
				catch { }
			}
			return false;
		}

		/// <summary>
		/// Removes the action at a specified index.
		/// </summary>
		/// <param name="index">Index of action to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">Index out of range.</exception>
		public void RemoveAt(int index)
		{
			if (index >= Count)
				throw new ArgumentOutOfRangeException(nameof(index), index, "Failed to remove action. Index out of range.");
			if (v2Coll != null)
				v2Coll.Remove(++index);
			else if (index == 0)
				Add(new ExecAction());
			else
				throw new NotV1SupportedException("There can be only a single action and it cannot be removed.");
		}

		/// <summary>
		/// Returns a <see cref="System.String"/> that represents the actions in this collection.
		/// </summary>
		/// <returns>
		/// A <see cref="System.String"/> that represents the actions in this collection.
		/// </returns>
		public override string ToString()
		{
			if (Count == 1)
				return this[0].ToString();
			if (Count > 1)
				return Properties.Resources.MultipleActions;
			return string.Empty;
		}

		internal void ConvertUnsupportedActions()
		{
			for (int i = 0; i < Count; i++)
			{
				Action action = this[i];
				var bindable = action as IBindAsExecAction;
				if (TaskService.LibraryVersion.Minor > 3 && bindable != null)
				{
					string cmd = bindable.GetPowerShellCommand();
					this[i] = ExecAction.AsPowerShellCmd(action.ActionType.ToString(), cmd);
				}
				/*else if (action is IExtendExecAction)
				{
					this[i] = ((IExtendExecAction)action).ToExecAction();
				}*/
			}
		}

		internal void UnconvertUnsupportedActions()
		{
			for (int i = 0; i < Count; i++)
			{
				ExecAction action = this[i] as ExecAction;
				if (action != null)
				{
					if (TaskService.LibraryVersion.Minor > 3)
					{
						var match = action.GetPowerShellCmd();
						if (match != null)
						{
							Action newAction = null;
							if (match[0] == "SendEmail")
								newAction = EmailAction.FromPowerShellCommand(match[1]);
							else if (match[0] == "ShowMessage")
								newAction = ShowMessageAction.FromPowerShellCommand(match[1]);
							if (newAction != null)
							{
								this[i] = newAction;
								continue;
							}
						}
					}
				}
			}
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

			object System.Collections.IEnumerator.Current => Current;

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

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

		System.Xml.Schema.XmlSchema IXmlSerializable.GetSchema() => null;

		void IXmlSerializable.ReadXml(System.Xml.XmlReader reader)
		{
			reader.ReadStartElement("Actions", TaskDefinition.tns);
			while (reader.MoveToContent() == System.Xml.XmlNodeType.Element)
			{
				Action newAction = null;
				switch (reader.LocalName)
				{
					case "Exec":
						newAction = AddNew(TaskActionType.Execute);
						break;
					default:
						reader.Skip();
						break;
				}
				if (newAction != null)
					XmlSerializationHelper.ReadObject(reader, newAction);
			}
			reader.ReadEndElement();
		}

		void IXmlSerializable.WriteXml(System.Xml.XmlWriter writer)
		{
			if (Count > 0)
			{
				XmlSerializationHelper.WriteObject(writer, this[0] as ExecAction);
			}
		}

		bool ICollection<Action>.IsReadOnly => false;

		void ICollection<Action>.Add(Action item)
		{
			Add(item);
		}

		object ICollection.SyncRoot => this;

		bool ICollection.IsSynchronized => false;

		void ICollection.CopyTo(Array array, int index)
		{
			if (array != null && array.Rank != 1)
				throw new RankException("Multi-dimensional arrays are not supported.");
			Action[] src = new Action[Count];
			CopyTo(src, 0);
			Array.Copy(src, 0, array, index, Count);
		}

		bool IList.IsReadOnly => false;

		bool IList.IsFixedSize => false;

		object IList.this[int index]
		{
			get { return this[index]; }
			set { this[index] = (Action)value; }
		}

		int IList.Add(object value)
		{
			Add((Action)value);
			return Count - 1;
		}

		bool IList.Contains(object value) => Contains((Action)value);

		int IList.IndexOf(object value) => IndexOf((Action)value);

		void IList.Insert(int index, object value)
		{
			Insert(index, (Action)value);
		}

		void IList.Remove(object value)
		{
			Remove((Action)value);
		}
	}
}
