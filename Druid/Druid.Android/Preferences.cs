using System;
using System.Globalization;
using Xamarin.Forms;
using Dwares.Druid.Services;
using Dwares.Dwarf;

using Android.Content;
using Android.Preferences;
using AndroidApp = Android.App.Application;


[assembly: Dependency(typeof(Dwares.Druid.Android.Preferences))]

namespace Dwares.Druid.Android
{
	class Preferences : IPreferences
	{
		public bool ContainsKey(string key, string share)
		{
			lock (this) {
				using (var sharedPreferences = GetSharedPreferences(share)) {
					return sharedPreferences.Contains(key);
				}
			}

		}

		public void Clear(string share)
		{
			lock (this) {
				using (var sharedPreferences = GetSharedPreferences(share))
				using (var editor = sharedPreferences.Edit()) {
					editor.Clear().Commit();
				}
			}
		}

		public void RemoveKey(string key, string share)
		{
			lock (this) {
				using (var sharedPreferences = GetSharedPreferences(share))
				using (var editor = sharedPreferences.Edit()) {
					editor.Remove(key).Commit();
				}
			}
		}

		public T Get<T>(string key, T defaultValue, string share)
		{
			lock (this) {
				object obj = null;
				using (var sharedPreferences = GetSharedPreferences(share)) {
					if (defaultValue == null) {
						obj = sharedPreferences.GetString(key, null);
					}
					else {
						switch (defaultValue)
						{
						case int i:
							obj = sharedPreferences.GetInt(key, i);
							break;

						case bool b:
							obj = sharedPreferences.GetBoolean(key, b);
							break;

						case long l:
							obj = sharedPreferences.GetLong(key, l);
							break;

						case double d:
							var savedDouble = sharedPreferences.GetString(key, null);
							if (string.IsNullOrWhiteSpace(savedDouble)) {
								obj = defaultValue;
							}
							else {
								if (!double.TryParse(savedDouble, NumberStyles.Number | NumberStyles.AllowExponent, CultureInfo.InvariantCulture, out var outDouble)) {
									var maxString = Convert.ToString(double.MaxValue, CultureInfo.InvariantCulture);
									outDouble = savedDouble.Equals(maxString) ? double.MaxValue : double.MinValue;
								}

								obj = outDouble;
							}
							break;

						case float f:
							obj = sharedPreferences.GetFloat(key, f);
							break;

						case string s:
							// the case when the string is not null
							obj = sharedPreferences.GetString(key, s);
							break;
						}
					}
				}

				if (obj is T value) {
					return value;
				}
				if (obj == null) {
					return defaultValue;
				}

				return (T)Convert.ChangeType(obj, typeof(T));
			}
		}


		public void Set<T>(string key, T value, string share)
		{
			lock (this) {
				using (var sharedPreferences = GetSharedPreferences(share))
				using (var editor = sharedPreferences.Edit()) {
					if (value == null) {
						editor.Remove(key);
					}
					else {
						switch (value) {
						case string s:
							editor.PutString(key, s);
							break;
						case int i:
							editor.PutInt(key, i);
							break;
						case bool b:
							editor.PutBoolean(key, b);
							break;
						case long l:
							editor.PutLong(key, l);
							break;
						case double d:
							var valueString = Convert.ToString(value, CultureInfo.InvariantCulture);
							editor.PutString(key, valueString);
							break;
						case float f:
							editor.PutFloat(key, f);
							break;
						default:
							//TODO
							editor.PutString(key, value.ToString());
							break;
						}
					}
					editor.Apply();
				}
			}
		}

		static ISharedPreferences GetSharedPreferences(string sharedName)
		{
			var context = AndroidApp.Context;

			return string.IsNullOrWhiteSpace(sharedName) ?
				PreferenceManager.GetDefaultSharedPreferences(context) :
					context.GetSharedPreferences(sharedName, FileCreationMode.Private);
		}
	}
}