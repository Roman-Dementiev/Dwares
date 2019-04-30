using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using Xamarin.Forms;


namespace Farest
{
	public class MainViewModel : ViewModelBase
	{
		public MainViewModel()
		{
			miles = new TextField<decimal>((value) => value >= 0);
			minutes = new TextField<decimal>((value) => value >= 0);

			//SetRates(7M, 0.595M, 0.255M);
			SetRates(Settings.Flagfall, Settings.MilesRate, Settings.MinutesRate, false);

			SavePresetCommand = new Command(OnSavePreset);
			RatesCommand = new Command(OnRates);
		}

		public Command SavePresetCommand { get; }
		public Command RatesCommand { get; }

		public void SetRates(IRates rates, bool save)
			=> SetRates(rates.Flagfall, rates.MilesRate, rates.MinutesRate, save);

		public void SetRates(decimal flagfall, decimal milesRate, decimal minutesRate, bool save)
		{
			Flagfall = flagfall;
			MilesRate = milesRate;
			MinutesRate = minutesRate;

			if (save) {
				Settings.Flagfall = flagfall;
				Settings.MilesRate = milesRate;
				Settings.MinutesRate = minutesRate;

			}
			Calculate();
		}


		decimal flagfall;
		public decimal Flagfall {
			get => flagfall;
			set {
				if (SetProperty(ref flagfall, value)) {
					FlagfallText = flagfall.ToString("C");
				}
			}
		}

		string flagfallText;
		public string FlagfallText {
			get => flagfallText;
			set => SetProperty(ref flagfallText, value);
		}

		decimal milesRate;
		public decimal MilesRate {
			get => milesRate;
			set {
				if (SetProperty(ref milesRate, value)) {
					MilesRateText = string.Format("x {0:C}", milesRate);
				}
			}
		}

		string milesRateText;
		public string MilesRateText {
			get => milesRateText;
			set => SetProperty(ref milesRateText, value);
		}

		decimal minutesRate;
		public decimal MinutesRate {
			get => minutesRate;
			set {
				if (SetProperty(ref minutesRate, value)) {
					MinutesRateText = string.Format("x {0:C}", minutesRate);
				}
			}
		}

		string minutesRateText;
		public string MinutesRateText {
			get => minutesRateText;
			set => SetProperty(ref minutesRateText, value);
		}

		TextField<decimal> miles;
		public decimal Miles {
			get => miles.Value;
		}

		public Color MilesColor {
			get => FieldColor(miles);
		}

		public string MilesText {
			get => miles.Text;
			set {
				if (miles.SetText(value)) {
					Calculate();
					PropertiesChanged(nameof(Miles), nameof(MilesText), nameof(MilesColor));
				}
			}
		}

		TextField<decimal> minutes;
		public decimal Minutes {
			get => minutes.Value;
		}

		public Color MinutesColor {
			get => FieldColor(miles);
		}

		public string MinutesText {
			get => minutes.Text;
			set {
				if (minutes.SetText(value)) {
					Calculate();
					PropertiesChanged(nameof(Minutes), nameof(MinutesText), nameof(MinutesColor));
				}
			}
		}

		decimal total = -1M;
		public decimal Total {
			get => total;
			set {
				if (SetProperty(ref total, value)) {
					TotalText = total >= 0 ? total.ToString("C") : string.Empty;
				}
			}
		}

		string totalText;
		public string TotalText {
			get => totalText;
			set => SetProperty(ref totalText, value);
		}

		public void Calculate()
		{
			if (miles.IsValid && minutes.IsValid) {
				Total = Flagfall + Miles*MilesRate + Minutes*MinutesRate;
			} else {
				Total = -1M;
			}
		}

		public ObservableCollection<Preset> Presets {
			get => Preset.List;
		}

		Preset selectedPreset;
		public Preset SelectedPreset {
			get => selectedPreset;
			set {
				if (value != selectedPreset) {
					selectedPreset = value;
					if (selectedPreset != null) {
						SetRates(selectedPreset, true);
					}
					FirePropertyChanged();
				}
			}
		}

		async void OnRates()
		{
			await RatesPage.Show(this);
		}

		async void OnSavePreset()
		{
		}
	}
}
