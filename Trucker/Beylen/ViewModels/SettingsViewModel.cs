using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen;
using Beylen.Models;

namespace Beylen.ViewModels
{
	public class SettingsViewModel : CollectionViewModel<SettingsSection>
	{
		//static ClassRef @class = new ClassRef(typeof(SettingsViewModel));

		public SettingsViewModel() :
			base(ApplicationScope)
		{
			//Debug.EnableTracing(@class);

			Title = "Settings";

			if (AppScope.Instance.ConfigMode == null) {
				Items.Add(new SettingsSection {
					//IconSource = "set_app_mode.png",
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

		static string AppModeString(AppMode mode, string car = null)
		{
			if (mode == AppMode.Driver) {
				return $"Driver, {car}";
			} else {
				return "Market";
			}
		}
		static readonly string _Market = AppModeString(AppMode.Market);
		static readonly string _Driver1 = AppModeString(AppMode.Driver, "Car 1");
		static readonly string _Driver2 = AppModeString(AppMode.Driver, "Car 2");

		async Task ChooseAppMode(Page page, SettingsSection section)
		{
			var result = await page.DisplayActionSheet("Application Mode", "Cancel", null,
				_Market, _Driver1, _Driver2
				);

			if (result == _Market) {
				AppScope.Instance.Configure("Market", null);
			} else if (result == _Driver1) {
				AppScope.Instance.Configure("Driver", "Car 1");
			} else if (result == _Driver2) {
				AppScope.Instance.Configure("Driver", "Car 2");
			} else {
				return;
			}
			section.Value = AppModeString();
		}

		async Task ChooseUIThene(Page page, SettingsSection section)
		{
			var result = await page.DisplayActionSheet("UI Theme", "Cancel", null,
				"Dark", "Light");
			if (result == "Cancel")
				return;

			Settings.UITheme = result;
			section.Value = result;
			UIThemes.Instance.SelectTheme(result);
		}
	}
}
