using System;
using Dwares.Druid.Support;
using Dwares.Dwarf;
using ACE.Models;
using ACE.Views;


namespace ACE
{
	class AppScope: BindingScope
	{
		public AppScope() : base(null) { }

		public async void OnAbout()
		{
			var page = new AboutPage();
			await Navigator.PushModal(page);
		}

		public async void OnSettings()
		{
			var page = new SettingsPage();
			await Navigator.PushModal(page);
		}

		public async void OnBackup()
		{
			Debug.Print("OnBackup()");
			await AppData.BackupAsync();
		}

		public async void OnRestore()
		{
			Debug.Print("OnRestore()");
			await AppData.RestoreAsync();
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void OnSelectDevice()
		{
			Debug.Print("AppScope.OnSelectDevice()");
		}
	}
}
