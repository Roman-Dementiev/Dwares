using System;
using Dwares.Dwarf;
using Xamarin.Essentials;


namespace Drive.Models
{
	public static class Settings
	{
		public const string cShare = "Drive";

		public static void Reset()
		{
			Preferences.Clear(cShare);
		}

		public static string ApiKey {
			get => Preferences.Get(nameof(ApiKey), string.Empty, cShare);
			set => Preferences.Set(nameof(ApiKey), value, cShare);
		}

		public static string BaseId {
			get => Preferences.Get(nameof(BaseId), string.Empty, cShare);
			set => Preferences.Set(nameof(BaseId), value, cShare);
		}
	}
}
