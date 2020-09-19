using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.ViewModels;
using Dwares.Druid.Satchel;
using RouteOptimizer.Models;
using Xamarin.Forms;


namespace RouteOptimizer.ViewModels
{
	public class RouteViewModel : CardCollectionViewModel<RouteStop, RouteStopCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteViewModel));

		public RouteViewModel() :
			base(App.Current.Route.Stops)
		{
			//Debug.EnableTracing(@class);

			Title = "Route";

			TestCommand = new WritCommand("Test");
		}

		public ObservableCollection<RouteStopCardModel> Stops => Items;

		public WritCommand TestCommand { get; }

		public bool CanExecuteTest()
		{
			return IsNotBusy;
		}
		

		public async Task ExecuteTest()
		{
			if (!CanExecuteTest())
				return;

			TimeSpan delay = TimeSpan.Zero; //TimeSpan.FromSeconds(3);
			TimeSpan duration = TimeSpan.FromSeconds(5);

			ProgressValue = 0;

			await Task.Delay(delay);
			StartBusy("Working...");

			DateTime now = DateTime.Now;

			Device.StartTimer(TimeSpan.FromSeconds(0.1), () => {
				var progress = (DateTime.Now - now).TotalMilliseconds / duration.TotalMilliseconds;
				ProgressValue = progress;

				bool continueTimer = progress < 1;
				if (!continueTimer) {
					IsBusy = false;
				}
				return continueTimer;
			});
		}

		//public bool OverlayIsVisible {
		//	get => overlayIsVisible;
		//	set => SetProperty(ref overlayIsVisible, value);
		//}
		//bool overlayIsVisible;

		public double ProgressValue {
			get => progress;
			set => SetProperty(ref progress, value);
		}
		double progress;
	}
}
