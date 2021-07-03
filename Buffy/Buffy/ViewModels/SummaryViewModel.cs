using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Druid.ViewModels;
using Dwares.Dwarf;
using Buffy.Models;
using Xamarin.Forms;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.ComponentModel;

namespace Buffy.ViewModels
{
	public class SummaryViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryViewModel));

		public ObservableCollection<SummaryGroup> Groups { get; }
		//public ObservableCollection<SummaryCell> Items => Groups[0];

		public Command SyncCommand { get; }



		public SummaryViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Summary";
			SyncCommand = new Command(async () => await ExecuteSyncCommand());
			Groups = new ObservableCollection<SummaryGroup>();

			CreateCells(false);

			App.Summary.PropertyChanged += Summary_PropertyChanged;
		}

		public bool IsWeekly {
			get => isWeekly;
			set {
				if (SetProperty(ref isWeekly, value)) {
					CreateCells();
				}
			}
		}
		bool isWeekly;

		public string Total {
			get => App.Summary.TotalStr();
		}

		private void Summary_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(Summary.Total)) {
				FirePropertyChanged(nameof(Total));
			}
		}


		void CreateCells(bool reset = true)
		{
			if (reset) {
				App.Summary.SummaryAdded -= OnSummaryAdded;
				App.Summary.SummaryCleared -= OnSummaryCleared;
				Groups.Clear();
			}

			foreach (var year in App.Summary.YearSummaries) {
				Groups.Add(new SummaryGroup(year));
			}

			if (IsWeekly) {
				CreateCells(App.Summary.WeekSummaries);
			} else {
				CreateCells(App.Summary.MonthSummaries);
			}

			App.Summary.SummaryAdded += OnSummaryAdded;
			App.Summary.SummaryCleared += OnSummaryCleared;
		}

		SummaryGroup GetGroup(int year)
		{
			foreach (var group in Groups) {
				if (group.Year == year)
					return group;
			}
			return null;
		}

		void CreateCells<T>(IList<T> list) where T : SummaryRecord
		{
			foreach (var summary in list) {
				var group = GetGroup(summary.Year);
				if (group != null) {
					group.Add(new SummaryCell(summary));
				} else {
					Debug.Fail($"Summary for year {summary.Year} not found");
				}
			}
		}

		private void OnSummaryAdded(SummaryAddedEventArgs args)
		{
			if (!args.AtEnd) {
				CreateCells();
				return;
			}

			if (args.Summary is YearSummary yearSum) {
				Groups.Add(new SummaryGroup(yearSum));
				return;
			}

			var group = GetGroup(args.Summary.Year);

			if (IsWeekly) {
				if (args.Summary is WeekSummary sum) {
					group.Add(new SummaryCell(sum));
				}
			} else {
				if (args.Summary is MonthSummary sum) {
					group.Add(new SummaryCell(sum));
				}
			}
		}

		private void OnSummaryCleared(EventArgs args)
		{
			CreateCells();
		}


		async Task ExecuteSyncCommand()
		{
			IsBusy = true;

			try {
				//Items.Clear();
				//var items = await DataStore.GetItemsAsync(true);
				//foreach (var item in items) {
				//	Items.Add(item);
				//}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				IsBusy = false;
			}
		}

	}
}
