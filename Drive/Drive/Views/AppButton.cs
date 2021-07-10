using System;
using Dwares.Dwarf;
using Dwares.Druid.UI;


namespace Drive.Views
{
	public class AppButton : ArtButton
	{
		//static ClassRef @class = new ClassRef(typeof(AppButton));

		public AppButton()
		{
			//Debug.EnableTracing(@class);

			this.PropertyChanged += (s, e) => {
				if (e.PropertyName == nameof(IsEnabled)) {
					UpdateIcon();
				}
			};

			App.Current.PropertyChanged += (s, e) => {
				if (e.PropertyName == nameof(App.StorageIsAvailable)) {
					IsEnabled = App.StorageIsAvailable;
				}
			};

			Large = false;
			Flavor = "App-button-small";
		}

		public bool Large {
			get => large;
			set {
				if (value != large) {
					large = value;
					OnPropertyChanged();
					Flavor = large ? "App-button-large" : "App-button-small";
				}
			}
		}
		bool large;

		public string DefaultIcon {
			get => defaultIcon;
			set {
				if (value != defaultIcon) {
					defaultIcon = value;
					OnPropertyChanged();
					UpdateIcon();
				}
			}
		}
		string defaultIcon;

		public string DisabledIcon {
			get => disabledIcon;
			set {
				if (value != disabledIcon) {
					disabledIcon = value;
					OnPropertyChanged();
					UpdateIcon();
				}
			}
		}
		string disabledIcon;

		void UpdateIcon()
		{
			var icon = IsEnabled || string.IsNullOrEmpty(DisabledIcon) ? DefaultIcon : DisabledIcon;
			if (!string.IsNullOrEmpty(icon))
				IconArt = icon;
		}

	}
}
