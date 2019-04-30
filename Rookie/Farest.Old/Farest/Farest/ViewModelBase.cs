using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using Xamarin.Forms;


namespace Farest
{
	public class ViewModelBase : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void FirePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void PropertiesChanged(params string[] propertyNames)
		{
			if (PropertyChanged != null) {
				foreach (var name in propertyNames) {
					PropertyChanged.Invoke(this, new PropertyChangedEventArgs(name));
				}
			}
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			//if (EqualityComparer<T>.Default.Equals(oldValue,, value))
			//	return false;
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			FirePropertyChanged(propertyName);
			return true;
		}

		public static bool TrtGetValue<T>(string text, out T value)
		{
			var type = typeof(T);
			try {
				var result = Convert.ChangeType(text, type);
				value = (T)result;
				return true;
			}
			catch (Exception exc) {
				Debug.WriteLine($"Can not convert '{text}' to {type}: {exc}");
				value = default(T);
				return false;
			}
		}

		public static Color ValidValueColor => Color.Black;
		public static Color InvalidValueColor => Color.Red;

		public Color FieldColor<T>(TextField<T> field)
			=> field.IsValid ? ValidValueColor : InvalidValueColor;
	}
}
