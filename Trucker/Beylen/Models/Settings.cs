﻿using System;
using Dwares.Dwarf;
using Dwares.Druid.Services;


namespace Beylen.Models
{
	public static class Settings
	{
		//static ClassRef @class = new ClassRef(typeof(Settings));

		public static string UITheme {
			get => Preferences.Get<string>(nameof(UITheme));
			set => Preferences.Set(nameof(UITheme), value);
		}

		public static string ApplicationMode {
			get => Preferences.Get<string>(nameof(ApplicationMode));
			set => Preferences.Set(nameof(ApplicationMode), value);
		}

		public static string Car {
			get => Preferences.Get<string>(nameof(Car));
			set => Preferences.Set(nameof(Car), value);
		}

	}
}
