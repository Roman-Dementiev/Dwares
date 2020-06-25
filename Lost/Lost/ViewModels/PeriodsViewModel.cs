using System;
using System.Collections.ObjectModel;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Druid;
using Lost.Models;


namespace Lost.ViewModels
{
	public class PeriodsViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(PeriodsViewModel));

		public static PeriodsViewModel Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}
		static PeriodsViewModel instance;

		public PeriodsViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Periods";
			Periods = new ObservableCollection<PeriodInfo>();
		}

		public ObservableCollection<PeriodInfo> Periods { get; }

		public bool FullShift {
			get => PeriodInfo.UseFullTime;
			set {
				if (value != PeriodInfo.UseFullTime) {
					var periods = new PeriodInfo[Periods.Count];
					Periods.CopyTo(periods, 0);
					Periods.Clear();

					PeriodInfo.UseFullTime = value;
					foreach (var period in periods) {
						Periods.Add(period);
					}

				}
			}
		}
	}
}
