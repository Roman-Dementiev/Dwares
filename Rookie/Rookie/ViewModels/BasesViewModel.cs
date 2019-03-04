using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Rookie.Models;

namespace Dwares.Rookie.ViewModels
{
	public class BasesViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(BasesViewModel));

		public BasesViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Bases";
		}

		YearlyTripData selectedYearly;
		public YearlyTripData SelectedYearly {
			get => selectedYearly;
			set {
				//Debug.Print("SelectedAnnual.set value={0}", value?.Year);
				if (value != selectedYearly) {
					selectedYearly = value;
					if (value == null) {
						MonthlyData = null;
					} else {
						MonthlyData = selectedYearly.MonthlyData;
						//Debug.Print("Monthly.Count={0}", Monthly.Count);
					}
				}
			}
		}

		public ObservableCollection<YearlyTripData> YearlyData => AppScope.Instance.TripData;

		ObservableCollection<MonthlyTripData> monthlyData;
		public ObservableCollection<MonthlyTripData> MonthlyData {
			get => monthlyData;
			set {
				//monthly = value;
				SetProperty(ref monthlyData, value);
			}
		}

		public async void OnAdd()
		{
			//Debug.Print("BasesViewModel.OnAdd");

			//var page = Forge.CreatePage(typeof(AddBaseViewModel));
			var page = App.CreateForm<AddBaseViewModel>();
			await Navigator.PushPage(page);
		}
	}
}
