using System;
using System.Collections.Specialized;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid;
using DraggableListView.Models;
using DraggableListView.Services;


namespace DraggableListView.ViewModels
{
	public class MarinesViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(MarinesViewModel));

		public MarinesViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Marines";

			LoadMarinesCommand = new Command(async () => {
				await ExecuteLoadMarinesCommand();
			});

			Marines = App.Current.Marines;
		}


		public Command LoadMarinesCommand { get; set; }
		public OrderableCollection<Marine> Marines { get; }

		async Task ExecuteLoadMarinesCommand()
		{
			if (IsBusy)
				return;

			StartBusy("Loading Marines...");

			try {
				Marines.Clear();
				await DataStore.Instance.LoadMarines(Marines);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			finally {
				ClearBusy();
			}
		}
	}
}
