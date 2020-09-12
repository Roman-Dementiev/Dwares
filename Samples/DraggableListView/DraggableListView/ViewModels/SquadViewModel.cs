using System;
using System.Collections;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid;
using DraggableListView.Models;
using DraggableListView.Services;


namespace DraggableListView.ViewModels
{
	public class SquadViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(SquadViewModel));

		public SquadViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Squad";

			LoadSquadCommand = new Command(async () => {
				await ExecuteLoadSquadCommand();
			});

			Squad = App.Current.Squad;
			//Squad.Teams.OrderChanged += OnOrderChanged;
		}

		public Command LoadSquadCommand { get; set; }

		public Squad Squad { get; }
		public IList Teams {
			get => Squad.Teams;
		}

		public string SquadName {
			get => Squad.Name;
		}
		public string LeaderName {
			get => Squad.SquadLeader.FullName;
		}

		public Rank LeaderRank {
			get => Squad.SquadLeader.Rank;
		}

		async Task ExecuteLoadSquadCommand()
		{
			if (IsBusy)
				return;

			StartBusy("Loading Squad...");

			try {
				await DataStore.Instance.LoadSquad(Squad);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				ClearBusy();
			}
		}

		//void OnOrderChanged(object sender, GroupedOrderableCollectionChangedEventArgs e)
		//{
		//	int ordinal = 0;
		//	foreach (var item in e.OldGroup) {
		//		if (item is Marine marine) {
		//			marine.TeamOrdinal = ++ordinal;
		//		}
		//	}

		//	if (e.NewGroup != e.OldGroup) {
		//		ordinal = 0;
		//		foreach (var item in e.NewGroup) {
		//			if (item is Marine marine) {
		//				marine.TeamOrdinal = ++ordinal;
		//			}
		//		}
		//	}
		//}
	}
}
