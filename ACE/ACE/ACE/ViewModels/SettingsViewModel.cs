using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Druid.Support;
using Xamarin.Forms;


namespace ACE.ViewModels
{
	public class SettingsViewModel: BindingScope
	{
		public SettingsViewModel() :
			base(AppScope)
		{
			Title = "Settings";
		}

		public async void OnDismiss()
		{
			await Navigator.PopPage();
		}
	}
}
