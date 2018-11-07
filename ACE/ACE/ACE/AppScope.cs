using System;
using Dwares.Druid.Support;
using Dwares.Dwarf;
using ACE.Models;
using ACE.Views;


namespace ACE
{
	class AppScope: BindingScope
	{
		ClassRef @class = new ClassRef(typeof(AppScope));

		public AppScope() : 
			base(null)
		{
			Debug.EnableTracing(@class);
		}

		public async void OnAbout()
		{
			Debug.Trace(@class, nameof(OnAbout));

			var page = new AboutPage();
			await Navigator.PushModal(page);
		}

		public async void OnSettings()
		{
			Debug.Trace(@class, nameof(OnSettings));

			var page = new SettingsPage();
			await Navigator.PushModal(page);
		}

		public async void OnBackup()
		{
			Debug.Trace(@class, nameof(OnBackup));

			await AppData.BackupAsync();
		}

		public async void OnRestore()
		{
			Debug.Trace(@class, nameof(OnRestore));

			await AppData.RestoreAsync();
		}

		public async void OnNewSchedule()
		{
			Debug.Trace(@class, nameof(OnNewSchedule));

			if (AppData.Pickups.Count > 0) {
				bool clear = await Alerts.ConfirmAlert("Current schedule is not empty\n.Do you want to clear it?");
				if (clear) {
					await AppData.ClearSchedule();
				} else
					return;
			}

			var page = new NewSchedulePage();
			await Navigator.PushModal(page);
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void OnSelectDevice()
		{
			Debug.Trace(@class, nameof(OnSelectDevice));
		}
	}
}
