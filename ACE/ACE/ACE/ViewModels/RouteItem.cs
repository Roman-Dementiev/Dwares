using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Essential;
using Dwares.Drums;
using ACE.Models;


namespace ACE.ViewModels
{
	public class RouteItem : SelectableItem
	{
		public RouteItem(RouteStop routeStop)
		{
			RouteStop = routeStop ?? throw new ArgumentNullException(nameof(routeStop));
			RouteStop.PropertyChanged += RouteStop_PropertyChanged;

			TimeTillArrive = DurationToString(RouteStop.TimeTillArrive);
			//EstimatedStart = ScheduleTimeToString(RouteStop.EstimatedStart);
			ArriveTime = ScheduleTimeToString(RouteStop.ArriveTime);
			DepartTime = ScheduleTimeToString(RouteStop.DepartTime);
				
			ShowDirectionsCommand = new Command(OnShowDirections, CanShowDirections);
			GoCommand = new Command(OnGo, CanGo);
			ArriveCommand = new Command(OnArrive, CanArrive);
		}

		public RouteStop RouteStop { get; }

		public string Icon {
			get {
				if (RouteStop.State == RouteStopState.Arrived)
					return "done32";

				switch (RouteStop.RouteStopType)
				{
					case RouteStopType.HomePickup:
					case RouteStopType.HomeDropoff:
						return "home24";
					case RouteStopType.OfficePickup:
					case RouteStopType.OfficeDropoff:
						return "office24";
					default:
						return "van24";
				}
			}
		}

		//public LayoutOptions IconLayout {
		//	get {
		//		switch (RouteStop.RouteStopType)
		//		{
		//			case RouteStopType.HomePickup:
		//			case RouteStopType.OfficePickup:
		//				return LayoutOptions.Start;
		//			case RouteStopType.HomeDropoff:
		//			case RouteStopType.OfficeDropoff:
		//				return LayoutOptions.End;
		//			default:
		//				return LayoutOptions.Center;
		//		}
		//	}
		//}
		public LayoutOptions IconLayout => LayoutOptions.Center;

		public bool HasName => !String.IsNullOrEmpty(Name);
		public string Name => RouteStop.Name;
		public string Address => String.IsNullOrEmpty(RouteStop.Address) ? "???" : RouteStop.Address;

		public RouteStopState State => RouteStop.State;
		public bool IsReadyToGo => State == RouteStopState.ReadyToGo;
		public bool IsEnroute => State == RouteStopState.EnRoute;
		public bool IsArrived => State == RouteStopState.Arrived;

		public bool IsUpdatable => RouteStop.IsUpdatable;

		string timeTillArrive;
		public string TimeTillArrive {
			get => timeTillArrive;
			private set => SetProperty(ref timeTillArrive, value);
		}

		string arriveTime;
		public string ArriveTime {
			get => arriveTime;
			private set => SetProperty(ref arriveTime, value);
		}

		string departTime;
		public string DepartTime {
			get => departTime;
			private set => SetProperty(ref departTime, value);
		}

		string DurationToString(TimeSpan? duration)
		{
			if (duration == null) {
				if (RouteStop.Origin == null) {
					return String.Empty;
				} else if (RouteStop.Updating) {
					return StdGlyph.BlackHourglass;
				} else {
					return"-";
				}
			}
			else {
				int mins = (int)Math.Ceiling(((TimeSpan)duration).TotalMinutes);
				if (mins > 90) {
					return String.Format("{0} h {1} min", mins/60, mins%60);
				} else {
					return String.Format("{0} min", mins);
				}
			}
		}

		static string ScheduleTimeToString(ScheduleTime? time)
		{
			if (time?.IsSet == true) {
				return ((ScheduleTime)time).ToString();
			} else {
				return String.Empty;
			}

		}

		private void RouteStop_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(RouteStop.Name))
			{
				PropertiesChanged(nameof(Name), nameof(HasName));
			}
			else if (e.PropertyName == nameof(RouteStop.State))
			{
				PropertiesChanged(nameof(State), nameof(Icon), nameof(IsReadyToGo), nameof(IsEnroute), nameof(IsArrived));
			}
			else if (e.PropertyName == nameof(RouteStop.Updating) || e.PropertyName == nameof(RouteStop.TimeTillArrive))
			{
				TimeTillArrive = DurationToString(RouteStop.TimeTillArrive);
			}
			//else if (e.PropertyName == nameof(RouteStop.EstimatedStart))
			//{
			//	EstimatedStart = ScheduleTimeToString(RouteStop.EstimatedStart);
			//}
			else if (e.PropertyName == nameof(RouteStop.ArriveTime))
			{
				ArriveTime = ScheduleTimeToString(RouteStop.ArriveTime);
			}
			else if (e.PropertyName == nameof(RouteStop.DepartTime))
			{
				DepartTime = ScheduleTimeToString(RouteStop.DepartTime);
			}
			else {
				FirePropertyChanged(e.PropertyName);
			}
		}

		//protected override void OnSelectedChanged()
		//{
		//	PropertiesChanged(
		//		nameof(IsSelected)
		//		//TODO
		//		);
		//}

		public Command ShowDirectionsCommand { get; }
		public Command GoCommand { get; }
		public Command ArriveCommand { get; }


		public bool CanShowDirections()
		{
			return RouteStop.Location.IsValidLocation() && RouteStop.State != RouteStopState.Arrived;
		}

		public async void OnShowDirections()
		{
			if (CanShowDirections()) {
				await AppData.Route.ShowDirections(RouteStop);
			}
		}


		public bool CanGo() => IsReadyToGo;

		public async void OnGo()
		{
			if (IsReadyToGo) {
				await AppData.Route.GoTo(RouteStop);
			}
		}

		public bool CanArrive() => IsEnroute;

		public async void OnArrive()
		{
			if (IsEnroute) {
				await AppData.Route.Arrive(RouteStop);
			}
		}

	}
}
