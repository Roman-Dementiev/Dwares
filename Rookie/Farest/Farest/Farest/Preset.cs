using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace Farest
{
	public interface IRates
	{
		decimal Flagfall { get; set; }
		decimal MilesRate { get; set; }
		decimal MinutesRate { get; set; }
	}

	public class Preset : IRates
	{
		public string Name { get; set; }
		public decimal Flagfall { get; set; }
		public decimal MilesRate { get; set; }
		public decimal MinutesRate { get; set; }

		public override string ToString() => Name ?? base.ToString();

		static ObservableCollection<Preset> list;
		public static ObservableCollection<Preset> List {
			get => LazyInitializer.EnsureInitialized(ref list);
		}

		public static void InitPresets()
		{
			var list = List;
			
			var str = Settings.GetString("Presets");
			ParsePresets(str);

			if (list.Count == 0) {
				list.Add(new Preset { Name = "Default", Flagfall = 7M, MilesRate = 2.5M, MinutesRate = 0.2M });
				list.Add(new Preset { Name = "Distance only", Flagfall = 7M, MilesRate = 3M, MinutesRate = 0M });
			}
		}

		public static void ParsePresets(string str)
		{
			if (string.IsNullOrEmpty(str))
				return;

			var list = List;
			var split = str.Split(kPresetSeparator);
			foreach (var item in split) {
				var values = item.Split(kValuesSeparator);

				try {
					if (values?.Length != 4)
						throw new Exception("Invalid number of values");

					var name = values[0];
					var flagfall = decimal.Parse(values[1]);
					var milesRate = decimal.Parse(values[2]);
					var minutesRate = decimal.Parse(values[3]);

					var preset = new Preset {
						Name = values[0],
						Flagfall = decimal.Parse(values[1]),
						MilesRate = decimal.Parse(values[2]),
						MinutesRate = decimal.Parse(values[3])
					};
					list.Add(preset);
				}
				catch (Exception exc) {
					System.Diagnostics.Debug.WriteLine($"Can not parse preset values '{item}': {exc}");
				}
			}
		}

		static char[] kPresetSeparator = new char[] { ';' };
		static char[] kValuesSeparator = new char[] { ',' };
	}
}
