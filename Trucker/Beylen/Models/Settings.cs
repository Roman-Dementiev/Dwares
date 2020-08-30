﻿using System;
using Dwares.Dwarf;
using Dwares.Druid.Services;


namespace Beylen.Models
{
	public static class Settings
	{
		//static ClassRef @class = new ClassRef(typeof(Settings));

		public static void Reset()
		{
			Preferences.Clear();
		}

		public static string UITheme {
			get => Preferences.Get(nameof(UITheme), "Light", null);
			set => Preferences.Set(nameof(UITheme), value);
		}

		public static string ApplicationMode {
			get => Preferences.Get(nameof(ApplicationMode), "Market", null);
			set => Preferences.Set(nameof(ApplicationMode), value);
		}

		public static string Car {
			get => Preferences.Get<string>(nameof(Car));
			set => Preferences.Set(nameof(Car), value);
		}

		public static bool UseRealNames {
			get => Preferences.Get<bool>(nameof(UseRealNames));
			set => Preferences.Set(nameof(UseRealNames), value);
		}
	}
}
