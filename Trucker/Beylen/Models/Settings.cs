using System;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using Xamarin.Essentials;


namespace Beylen.Models
{
	public static class Settings
	{
		//static ClassRef @class = new ClassRef(typeof(Settings));

		public const string cShare = "Beylyn";

		public static void Reset()
		{
			Preferences.Clear(cShare);
		}

		public static string UITheme {
			get => Preferences.Get(nameof(UITheme), "Light", cShare);
			set => Preferences.Set(nameof(UITheme), value, cShare);
		}

		public static string ApplicationMode {
			get => Preferences.Get(nameof(ApplicationMode), "Market", cShare);
			set => Preferences.Set(nameof(ApplicationMode), value, cShare);
		}

		public static string Car {
			get => Preferences.Get(nameof(Car), string.Empty, cShare);
			set => Preferences.Set(nameof(Car), value, cShare);
		}

		public static bool UseRealNames {
			get => Preferences.Get(nameof(UseRealNames), false, cShare);
			set => Preferences.Set(nameof(UseRealNames), value, cShare);
		}
	}
}
