using System;
using Dwares.Druid.Services;


namespace Drive
{
	public static class Settings
	{
		public static string UITheme {
			get => Preferences.Get<string>(nameof(UITheme));
			set => Preferences.Set(nameof(UITheme), value);
		}
	}
}
