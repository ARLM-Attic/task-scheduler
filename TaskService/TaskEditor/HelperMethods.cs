﻿using System;
using System.Collections;
using System.Security.Principal;
using CubicOrange.Windows.Forms.ActiveDirectory;

namespace Microsoft.Win32.TaskScheduler
{
	internal static class HelperMethods
	{
		public static bool SelectAccount(System.Windows.Forms.IWin32Window parent, string targetComputerName, ref string acctName, out bool isGroup, out bool isService, out string sid)
		{
			var l = Locations.EnterpriseDomain | Locations.ExternalDomain | Locations.GlobalCatalog | Locations.JoinedDomain | Locations.LocalComputer;
			var dlg = new DirectoryObjectPickerDialog
			{
				TargetComputer = targetComputerName,
				MultiSelect = false,
				SkipDomainControllerCheck = true,
				AllowedLocations = l,
				DefaultLocations = l,
				AllowedObjectTypes = ObjectTypes.Users// | ObjectTypes.WellKnownPrincipals | ObjectTypes.Computers;
			};
			if (NativeMethods.AccountUtils.CurrentUserIsAdmin(targetComputerName)) dlg.AllowedObjectTypes |= ObjectTypes.BuiltInGroups | ObjectTypes.Groups;
			dlg.DefaultObjectTypes = dlg.AllowedObjectTypes;
			dlg.AttributesToFetch.Add("objectSid");
			var res = System.Windows.Forms.DialogResult.None;
			try { res = dlg.ShowDialog(parent); } catch { }
			if (res == System.Windows.Forms.DialogResult.OK)
			{
				if (dlg.SelectedObject != null)
				{
					try
					{
						acctName = !string.IsNullOrEmpty(dlg.SelectedObject.Upn) ? NameTranslator.TranslateUpnToDownLevel(dlg.SelectedObject.Upn) : dlg.SelectedObject.Name;
					}
					catch
					{
						acctName = dlg.SelectedObject.Name;
					}
					sid = AttrToString(dlg.SelectedObject.FetchedAttributes[0]);
					isGroup = dlg.SelectedObject.SchemaClassName.Equals("Group", StringComparison.OrdinalIgnoreCase);
					isService = NativeMethods.AccountUtils.UserIsServiceAccount(acctName);
					return true;
				}
			}
			isGroup = isService = false;
			sid = null;
			return false;
		}

		private static string AttrToString(object attr)
		{
			var multivaluedAttribute = attr;
			if (!(multivaluedAttribute is IEnumerable) || multivaluedAttribute is byte[] || multivaluedAttribute is string)
				multivaluedAttribute = new[] { multivaluedAttribute };

			var list = new System.Collections.Generic.List<string>();

			foreach (var attribute in (IEnumerable)multivaluedAttribute)
			{
				if (attribute == null)
				{
					list.Add(string.Empty);
				}
				else if (attribute is byte[])
				{
					var bytes = (byte[])attribute;
					list.Add(BytesToString(bytes));
				}
				else
				{
					list.Add(attribute.ToString());
				}
			}

			return string.Join("|", list.ToArray());
		}

		private static string BytesToString(byte[] bytes)
		{
			try { return new Guid(bytes).ToString("D"); }
			catch { }

			try { return new SecurityIdentifier(bytes, 0).ToString(); }
			catch { }

			return "0x" + BitConverter.ToString(bytes).Replace('-', ' ');
		}
	}
}
