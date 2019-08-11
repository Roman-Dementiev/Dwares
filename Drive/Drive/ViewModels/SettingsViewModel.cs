using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;


namespace Drive.ViewModels
{
	public class SettingsViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";
		}
	}
}
