using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace Dwares.Dwarf.Toolkit
{
	public class NotifyPropertyChanged : INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (PropertyChanged != null) {
				PropertyChanged.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}

		protected void PropertiesChanged(params string[] names)
		{
			var changed = PropertyChanged;
			if (changed != null) {
				foreach (var propertyName in names) {
					changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
				}
			}
		}

		protected bool SetProperty<T>(ref T storage, T value, [CallerMemberName] string propertyName = null)
		{
			if (Object.Equals(storage, value))
				return false;

			storage = value;
			RaisePropertyChanged(propertyName);
			return true;
		}
	}
}
