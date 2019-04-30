using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.Preferences;

using Xamarin.Forms;


[assembly: Dependency(typeof(Farest.Droid.Settings))]

namespace Farest.Droid
{
	class Settings : SettingsImplementation
	{
		public Settings() : this("Farest") { }

		public Settings(string sharedName)
		{
			SharedName = sharedName;
		}

		string SharedName { get; }

		public override string GetString(string key, string defaultValue)
		{
			if (TryGet(key, out string value)) {
				return value;
			} else {
				return defaultValue;
			}
		}

		public bool TryGet(string key, out string value)
		{
			try {
				using (var preferences = GetSharedPreferences(SharedName)) {
					try {
						value  = preferences.GetString(key, null);
						return value != null;
					}
					catch (Exception exc) {
						System.Diagnostics.Debug.WriteLine($"Can not get Setting value for key='{key}': {exc}");
					}
				}
			}
			catch (Exception exc) {
				System.Diagnostics.Debug.WriteLine($"Can not set Setting value for key='{key}': {exc}");
			}

			value = null;
			return false;
		}

		public override bool TryGet(string key, out object value)
		{
			//try {
			//	using (var preferences = GetSharedPreferences(SharedName)) {
			//		try {
			//			value  = preferences.GetString(key, null);
			//			return value != null;
			//		}
			//		catch (Exception exc) {
			//			System.Diagnostics.Debug.WriteLine($"Can not get Setting value for key='{key}': {exc}");
			//		}
			//	}
			//}
			//catch (Exception exc) {(Try
			//	System.Diagnostics.Debug.WriteLine($"Can not set Setting value for key='{key}': {exc}");
			//}

			//value = null;
			//return false;

			if (TryGet(key, out string str)) {
				value = str;
				return true;
			} else {
				value = null;
				return false;
			}
		}

		public override bool TrySet(string key, object value)
		{
			try {
				using (var preferences = GetSharedPreferences(SharedName))
				using (var editor = preferences.Edit()) {
					var str = value?.ToString();
					if (str != null) {
						editor.PutString(key, str);
					}
					else {
						editor.Remove(key);
					}
					editor.Apply();
					return true;
				}
			}
			catch (Exception exc) {
				System.Diagnostics.Debug.WriteLine($"Can not set Setting value for key='{key}': {exc}");
			}
			return false;
		}

		static ISharedPreferences GetSharedPreferences(string sharedName)
		{
			var context = Android.App.Application.Context;

			return string.IsNullOrWhiteSpace(sharedName) ?
				PreferenceManager.GetDefaultSharedPreferences(context) :
					context.GetSharedPreferences(sharedName, FileCreationMode.Private);
		}
	}
}
