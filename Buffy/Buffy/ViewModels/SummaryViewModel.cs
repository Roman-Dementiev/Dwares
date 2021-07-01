using System;
using System.Collections;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Druid.ViewModels;
using Dwares.Dwarf;
using Buffy.Models;
using Xamarin.Forms;
using System.Collections.Specialized;

namespace Buffy.ViewModels
{
	public class SummaryViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryViewModel));

		public ObservableCollection<SummaryCell> Items { get; }

		public Command SyncCommand { get; }



		public SummaryViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Summary";
			SyncCommand = new Command(async () => await ExecuteSyncCommand());
			Items = new ObservableCollection<SummaryCell>();

			CreateCells();
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

		public bool AllData {
			get => allData;
			set {
				if (SetProperty(ref allData, value)) {
					CreateCells();
				}
			}
		}
		bool allData;

		Action Unsubscribe = null;

		void CreateCells()
		{
			if (Unsubscribe != null) {
				Unsubscribe();
				Items.Clear();
			}

			if (IsWeekly) {
				foreach (var summary in App.Summary.WeekSummaries) {
					Items.Add(new SummaryCell(summary));
				}
				SubscribeWeekly();
				Unsubscribe = UnsubscribeWeekly;
			} else {
				foreach (var summary in App.Summary.MonthSummaries) {
					Items.Add(new SummaryCell(summary));
				}
				SubscribeMonthly();
				Unsubscribe = UnsubscribeMonthly;
			}
		}

		void SubscribeMonthly()
		{
			App.Summary.MonthSummaries.CollectionChanged += Summary_MonthlyChanged;
		}

		void UnsubscribeMonthly()
		{
			App.Summary.MonthSummaries.CollectionChanged -= Summary_MonthlyChanged;
		}

		void SubscribeWeekly()
		{
			App.Summary.WeekSummaries.CollectionChanged += Summary_WeeklyChanged;
		}

		void UnsubscribeWeekly()
		{
			App.Summary.MonthSummaries.CollectionChanged -= Summary_WeeklyChanged;
		}

		private void Summary_MonthlyChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (IsWeekly)
				return;

			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (var item in e.NewItems) {
					if (item is MonthSummary summary) {
						Items.Add(new SummaryCell(summary));
					} else {
						Debug.Print($"Unknown item in summary: {item.GetType()}");
					}
				}
			} else {
				CreateCells();
			}
		}

		private void Summary_WeeklyChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (!IsWeekly)
				return;

			if (e.Action == NotifyCollectionChangedAction.Add) {
				foreach (var item in e.NewItems) {
					if (item is WeekSummary summary) {
						Items.Add(new SummaryCell(summary));
					} else {
						Debug.Print($"Unknown item in summary: {item.GetType()}");
					}
				}
			} else {
				CreateCells();
			}
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
