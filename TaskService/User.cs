﻿using System;
using System.Security.Principal;

namespace Microsoft.Win32.TaskScheduler
{
	internal class User : IEquatable<User>, IDisposable
	{
		private WindowsIdentity acct;
		private SecurityIdentifier sid;

		public User(string userName = null)
		{
			var cur = WindowsIdentity.GetCurrent();
			if (userName == null || cur.Name.Equals(userName, StringComparison.InvariantCultureIgnoreCase))
			{
				acct = cur;
				sid = acct.User;
			}
			else if (userName != null && (userName.Contains("\\") || !userName.Contains("@")))
			{
				using (var ds = new Microsoft.Win32.NativeMethods.DsHandle())
				{
					try { acct = new WindowsIdentity(ds.CrackName(userName)); sid = acct.User; }
					catch (System.Security.SecurityException)
					{
						NTAccount ntacct = new NTAccount(userName);
						sid = (SecurityIdentifier)ntacct.Translate(typeof(SecurityIdentifier));
						acct = null;
					}
				}
			}
			else
			{
				acct = new WindowsIdentity(userName);
			}
		}

		internal User(WindowsIdentity wid)
		{
			acct = wid;
		}

		public static User Current => new User(WindowsIdentity.GetCurrent());

		public bool IsAdmin => acct != null ? new WindowsPrincipal(acct).IsInRole(WindowsBuiltInRole.Administrator) : false;

		public bool IsCurrent => acct?.User.Equals(WindowsIdentity.GetCurrent().User) ?? false;

		public bool IsServiceAccount
		{
			get
			{
				try
				{
					return (sid.IsWellKnown(WellKnownSidType.LocalSystemSid) || sid.IsWellKnown(WellKnownSidType.NetworkServiceSid) || sid.IsWellKnown(WellKnownSidType.LocalServiceSid));
				}
				catch { }
				return false;
			}
		}

		public bool IsSystem => acct?.IsSystem ?? false;

		public string Name => acct != null ? acct.Name : ((NTAccount)sid.Translate(typeof(NTAccount))).Value;

		public string SidString => sid.ToString();

		public static User FromSidString(string sid)
		{
			SecurityIdentifier si = new SecurityIdentifier(sid);
			NTAccount acct = (NTAccount)si.Translate(typeof(NTAccount));
			return new User(acct.Value);
		}

		public void Dispose()
		{
			if (acct != null)
				acct.Dispose();
		}

		public override bool Equals(object obj)
		{
			User user = obj as User;
			if (user != null)
				return this.Equals(user);
			WindowsIdentity wid = obj as WindowsIdentity;
			if (wid != null)
				return sid.Equals(wid.User);
			try
			{
				string un = obj as string;
				if (un != null)
					return this.Equals(new User(un));
			}
			catch { }
			return base.Equals(obj);
		}

		public bool Equals(User other) => (other != null) ? sid.Equals(other.sid) : false;

		public override int GetHashCode() => sid.GetHashCode();
	}
}