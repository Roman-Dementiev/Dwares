using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;


namespace ACE.Models
{
	public class NotifyPropertyChanged: INotifyPropertyChanged
	{
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}

		protected void OnPropertiesChanged(params string[] names)
		{
			var changed = PropertyChanged;
			if (changed == null)
				return;

			foreach (var propertyName in names) {
				changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
			}
		}
	}
}
