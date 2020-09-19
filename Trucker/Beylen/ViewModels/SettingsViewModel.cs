using Beylen.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Beylen.ViewModels
{
	public class SettingsViewModel : CollectionViewModel<SettingsSection>
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";

			if (AppScope.Instance.ConfigMode == null) {
				Items.Add(new SettingsSection {
					Icon = "ic_settings_app_mode",
					Title = "Application mode",
					Value = AppModeString(),
					Action = ChooseAppMode
					});;
			}
			Items.Add(new SettingsSection {
				//IconSource = "set_ui_theme.png",
				Icon = "ic_settings_ui_theme",
				Title = "UI Theme",
				Value = Settings.UITheme,
				Action = ChooseUIThene
			});
		}


		static string AppModeString()
		{
			var appScope = AppScope.Instance;
			return AppModeString(appScope.CurrentMode, appScope.Car);
		}

		static string AppModeString(AppMode mode, Car car = null)
		{
			if (mode == AppMode.Driver) {
				return $"Driver, {car.Name}";
			} else {
				return "Market";
			}
		}
		static readonly string _Market = AppModeString(AppMode.Market);
		//static readonly string _DriverA = AppModeString(AppMode.Driver, "Car A");
		//static readonly string _DriverB = AppModeString(AppMode.Driver, "Car B");

		async Task ChooseAppMode(Page page, SettingsSection section)
		{
			var modes = new List<string>() {
				_Market
			};
			foreach (var _car in Car.List) {
				modes.Add(AppModeString(AppMode.Driver, _car));
			}

			var result = await page.DisplayActionSheet("Application Mode", "Cancel", null, modes.ToArray());

			AppMode mode;
			Car car = null;

			if (result == _Market) {
				mode = AppMode.Market;
			} else if (result.StartsWith("Driver, ")) {
				mode = AppMode.Driver;
				car = Car.ByName(result.Substring(8));
			} else {
				return;
			}
			section.Value = AppModeString(mode, car);

			AppScope appScope = AppScope.Instance;
			appScope.ClearData();
			appScope.Configure(mode, car);
			await appScope.ReloadData();
		}

		async Task ChooseUIThene(Page page, SettingsSection section)
		{
			var result = await page.DisplayActionSheet("UI Theme", "Cancel", null,
				"Dark", "Light", "Oceanic");
			if (result == "Cancel")
				return;

			Settings.UITheme = result;
			section.Value = result;
			UIThemes.Instance.SelectTheme(result);
		}
	}
}
