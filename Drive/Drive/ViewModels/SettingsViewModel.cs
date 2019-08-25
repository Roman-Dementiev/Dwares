using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Druid.UI;


namespace Drive.ViewModels
{
	public class SettingsViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";

			Themes = UIThemeManager.Instance.GetThemeNames(out currentIndex);
		}

		public List<string> Themes { get; }
		
		int currentIndex;
		public int CurrentIndex {
			get => currentIndex;
			set {
				if (value != currentIndex) {
					currentIndex = value;
					if (value < 0)
						return;
					
					var themeName = Themes[value];
					Settings.UITheme = themeName;
					UITheme.Select(themeName);
				}
			}
		}
	}
}
