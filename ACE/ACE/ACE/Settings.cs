using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Drums;
using Dwares.Druid.Services;


namespace ACE
{
	public static class Settings
	{
		public static void SelectMaps()
		{
			var applicationName = Preferences.Get<string>(nameof(MapApplication));
			var serviceName = Preferences.Get<string>(nameof(MapService));
			Maps.SetApplication(applicationName);
			Maps.SetService(serviceName);
		}

		public static IMapApplication MapApplication {
			get => Maps.MapApplication ?? Maps.DefaultApplication;
			set {
				Maps.MapApplication = value ?? Maps.DefaultApplication;
				Preferences.Set(nameof(MapApplication), Maps.MapApplication?.Name);
			}
		}

		public static IMapService MapService {
			get => Maps.MapService ?? Maps.DefaultService;
			set {
				Maps.MapService = value ?? Maps.DefaultService;
				Preferences.Set(nameof(MapService), Maps.MapService?.Name);
			}
		}

		public static int AddDrivingTime {
			get => Preferences.Get<int>(nameof(AddDrivingTime));
			set => Preferences.Set(nameof(AddDrivingTime), value);
		}

		public static int AddDrivingTimeWithTrafic {
			get => Preferences.Get<int>(nameof(AddDrivingTimeWithTrafic));
			set => Preferences.Set(nameof(AddDrivingTimeWithTrafic), value);
		}

		public static int DefaultStopTime {
			get => Preferences.Get<int>(nameof(DefaultStopTime));
			set => Preferences.Set(nameof(DefaultStopTime), value);
		}

		public static int WheelchairStopTime {
			get => Preferences.Get<int>(nameof(WheelchairStopTime));
			set => Preferences.Set(nameof(WheelchairStopTime), value);
		}


		public static bool CanDeleteCompanyContacts {
			get => Preferences.Get<bool>(nameof(CanDeleteCompanyContacts));
			set => Preferences.Set(nameof(CanDeleteCompanyContacts), value);
		}
	}
}
