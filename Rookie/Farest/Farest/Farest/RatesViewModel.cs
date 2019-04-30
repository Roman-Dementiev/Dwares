using System;
using Xamarin.Forms;


namespace Farest
{
	public class RatesViewModel : ViewModelBase
	{
		public RatesViewModel(MainViewModel mainViewModel)
		{
			MainViewModel = mainViewModel;

			flagfall = new TextField<decimal>(MainViewModel.Flagfall, (value) => value > 0);
			milesRate = new TextField<decimal>(MainViewModel.MilesRate, (value) => value > 0);
			minutesRate = new TextField<decimal>(MainViewModel.MinutesRate, (value) => value > 0);
		}

		MainViewModel MainViewModel { get; }

		TextField<decimal> flagfall;
		public decimal Flagfall {
			get => flagfall.Value;
		}

		public Color FlagfallColor {
			get => FieldColor(flagfall);
		}

		public string FlagfallText {
			get => flagfall.Text;
			set {
				if (flagfall.SetText(value)) {
					PropertiesChanged(nameof(Flagfall), nameof(FlagfallText), nameof(FlagfallColor));
				}
			}
		}

		TextField<decimal> milesRate;
		public decimal MilesRate {
			get => milesRate.Value;
		}

		public Color MilesRateColor {
			get => FieldColor(milesRate);
		}

		public string MilesRateText {
			get => milesRate.Text;
			set {
				if (milesRate.SetText(value)) {
					PropertiesChanged(nameof(MilesRate), nameof(MilesRateText), nameof(MilesRateColor));
				}
			}
		}

		TextField<decimal> minutesRate;
		public decimal MinutesRate {
			get => minutesRate.Value;
		}

		public Color MinutesRateColor {
			get => FieldColor(minutesRate);
		}

		public string MinutesRateText {
			get => minutesRate.Text;
			set {
				if (minutesRate.SetText(value)) {
					PropertiesChanged(nameof(MinutesRate), nameof(MinutesRateText), nameof(MinutesRateColor));
				}
			}
		}

		public void OnAccept()
		{
			MainViewModel.SetRates(Flagfall, MilesRate, MinutesRate, true);
		}
	}
}
