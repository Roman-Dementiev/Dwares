using System;
using Dwares.Dwarf;
using Xamarin.Essentials;


namespace Buffy.Models
{
	public class Settings
	{
		//static ClassRef @class = new ClassRef(typeof(Settings));
		public const string cShare = "Gazel";

		public Settings()
		{
			//Debug.EnableTracing(@class);
		}

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
