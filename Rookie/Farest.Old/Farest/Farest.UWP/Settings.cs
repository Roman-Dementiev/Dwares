using System;
using System.Diagnostics;
using Windows.Storage;
using Xamarin.Forms;

[assembly: Dependency(typeof(Farest.UWP.Settings))]

namespace Farest.UWP
{
	class Settings : SettingsImplementation
	{
		ApplicationDataContainer container = ApplicationData.Current.LocalSettings;

		public override bool TryGet(string key, out object value)
		{
			try {
				if (container.Values.ContainsKey(key)) {
					value = container.Values[key];
					return true;
				}
			}
			catch (Exception exc) {
				Debug.WriteLine($"Can not get Setting value for key='{key}': {exc}");
			}

			value = null;
			return false;
		}

		public override bool TrySet(string key, object value)
		{
			try {
				if (value == null) {
					container.Values.Remove(key);
				} else {
					container.Values[key] = value.ToString();
				}
				return true;
			}
			catch (Exception exc) {
				Debug.WriteLine($"Can not set Setting value for key='{key}': {exc}");
				return false;
			}
		}

		//public decimal GetDecimal(string key, decimal defaultValue)
		//{
		//	object value;
		//	if (TryGet(key, out value)) {
		//		if (value is decimal result)
		//			return result;
		//		if (value is IConvertible convertible)
		//			return Convert.ToDecimal(convertible);
		//	}

		//	return defaultValue;
		//}

		//public void SetDecimal(string key, decimal value)
		//{
		//	container.Values[key] = value;
		//}
	}
}
