using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;


namespace ACE.ViewModels
{
	public class BaseViewModel : NotifyPropertyChanged
	{
		public BaseViewModel(INavigation navigation)
		{
			//Navigation = navigation;
		}

		//public INavigation Navigation { get; }

		bool isBusy = false;
		public bool IsBusy {
			get { return isBusy; }
			set { SetProperty(ref isBusy, value); }
		}

		string title = string.Empty;
		public string Title {
			get { return title; }
			set { SetProperty(ref title, value); }
		}

		//protected bool SetProperty<T>(ref T backingStore, T value,
		//	[CallerMemberName]string propertyName = "")
		//{
		//	if (EqualityComparer<T>.Default.Equals(backingStore, value))
		//		return false;

		//	backingStore = value;
		//	RaisePropertyChanged(propertyName);
		//	return true;
		//}

		//#region INotifyPropertyChanged
		//public event PropertyChangedEventHandler PropertyChanged;
		//protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
		//{
		//	var changed = PropertyChanged;
		//	if (changed == null)
		//		return;

		//	changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
		//}

		//protected void OnPropertiesChanged(params string[] names)
		//{
		//	foreach (var propertyName in names) {
		//		OnPropertyChanged(propertyName);
		//	}
		//}
		//#endregion
	}
}
