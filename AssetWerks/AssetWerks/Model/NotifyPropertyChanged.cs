using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace AssetWerks.Model
{
	public class NotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void FirePropertyChanged(params string[] propertyNames)
		{
			if (PropertyChanged != null) {
				foreach (var propertyName in propertyNames) {
					PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
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
	}
}
