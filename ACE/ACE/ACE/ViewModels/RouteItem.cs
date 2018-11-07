using System;
using System.ComponentModel;
using Xamarin.Forms;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Essential;
using ACE.Models;

namespace ACE.ViewModels
{
	public class RouteItem : SelectableItem
	{
		public RouteItem(RouteStop routeStop)
		{
			RouteStop = routeStop ?? throw new ArgumentNullException(nameof(routeStop));
			RouteStop.PropertyChanged += RouteStop_PropertyChanged;

			EstimatedDuration = DurationToString(RouteStop.EstimatedDuration);
			//EstimatedStart = ScheduleTimeToString(RouteStop.EstimatedStart);
			EstimatedArrival = ScheduleTimeToString(RouteStop.EstimatedArrival);
			EstimatedDeparture = ScheduleTimeToString(RouteStop.EstimatedDeparture);
		}

		public RouteStop RouteStop { get; }

		public string Icon {
			get {
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

		string estimatedDuration;
		public string EstimatedDuration {
			get => estimatedDuration;
			private set => SetProperty(ref estimatedDuration, value);
		}

		//string estimatedStart;
		//public string EstimatedStart {
		//	get => estimatedStart;
		//	private set => SetProperty(ref estimatedStart, value);
		//}

		string estimatedArrival;
		public string EstimatedArrival {
			get => estimatedArrival;
			private set => SetProperty(ref estimatedArrival, value);
		}

		string estimatedDeparture;
		public string EstimatedDeparture {
			get => estimatedDeparture;
			private set => SetProperty(ref estimatedDeparture, value);
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
			else if (e.PropertyName == nameof(RouteStop.Updating) || e.PropertyName == nameof(RouteStop.EstimatedDuration))
			{
				EstimatedDuration = DurationToString(RouteStop.EstimatedDuration);
			}
			//else if (e.PropertyName == nameof(RouteStop.EstimatedStart))
			//{
			//	EstimatedStart = ScheduleTimeToString(RouteStop.EstimatedStart);
			//}
			else if (e.PropertyName == nameof(RouteStop.EstimatedArrival))
			{
				EstimatedArrival = ScheduleTimeToString(RouteStop.EstimatedArrival);
			}
			else if (e.PropertyName == nameof(RouteStop.EstimatedDeparture))
			{
				EstimatedDeparture = ScheduleTimeToString(RouteStop.EstimatedDeparture);
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

	}
}
