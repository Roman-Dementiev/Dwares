using System;
using System.ComponentModel;
using Dwares.Druid.Forms;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Beylen.Models
{
	public enum RouteStopKind
	{
		Customer,
		StartPoint,
		EndPoint,
		MidPoint
	}

	public enum RoutеStopStatus
	{
		Pending,
		Enroute,
		Arrived,
		Departed
	}


	public class RouteStop : Place
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStop));

		protected RouteStop(Route route)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(route, nameof(route));
			Route = route;
		}

		protected RouteStop(Route route, Place place, RouteStopKind kind) :
			this(route)
		{
			Guard.ArgumentNotNull(place, nameof(place));

			Kind = kind;
			Place = place;
		}

		protected RouteStop(Route route, RouteStopKind kind, string codeName, Place defaultPlace = null) :
			this(route)
		{
			Kind = kind;
			
			var place = AppScope.GetPlace(codeName) ?? defaultPlace;
			Place = place ?? throw new ProgramError($"Unknown Place CodeName=\"{codeName ?? string.Empty}\"");
		}

		public Route Route { get; }

		public Place Place {
			get => source;
			set {
				if (value != source) {
					if (source != null) {
						source.PropertyChanged -= OnSourcePropertyChanged;
					}

					SetProperty(ref source, value);
					FromSource();

					if (source != null) {
						source.PropertyChanged += OnSourcePropertyChanged;
					}
				}
			}
		}
		Place source;

		public RouteStopKind Kind {
			get => kind;
			protected set => SetProperty(ref kind, value);
			//protected set => SetPropertyEx(ref kind, value, nameof(Kind),
			//	nameof(IsStartPoint), nameof(IsEndPoint), nameof(IsCustomer));
		}
		RouteStopKind kind;

		public bool IsStartPoint => Kind == RouteStopKind.StartPoint;
		public bool IsEndPoint => Kind == RouteStopKind.EndPoint;
		public bool IsCustomer => Kind == RouteStopKind.Customer;

		public RoutеStopStatus Status {
			get => status;
			set => SetStatus(value, false);
		}
		RoutеStopStatus status;

		public void SetStatus(RoutеStopStatus value, bool forceNotification)
		{
			if (SetProperty(ref status, value, forceNotification, nameof(Status))) {
				UpdateInfo();
			}
		}

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		public string Info {
			get => info;
			set => SetProperty(ref info, value);
		}
		string info;

		public RouteLeg Leg {
			get => leg;
			set => SetProperty(ref leg, value);
		}
		RouteLeg leg;

		//public TimeSpan? LegDuration {
		//	get => legDuration;
		//	set {
		//		if (SetProperty(ref legDuration, value)) {
		//			UpdateInfo();
		//		}
		//	}
		//}
		//TimeSpan? legDuration = null;

		public TimeSpan? LegDuration {
			get => Leg?.Duration;
		}

		public TimeSpan StopDuration {
			// TODO
			get => TimeSpan.FromMinutes(10);
		}

		public TimeSpan? ETA {
			get => eta;
			set => SetProperty(ref eta, value);
		}
		TimeSpan? eta = null;

		public DateTime? ArrivalTime {
			get => arrivalTime;
			set => SetProperty(ref arrivalTime, value);
		}
		DateTime? arrivalTime;

		public DateTime? DepartureTime {
			get => departureTime;
			set => SetProperty(ref departureTime, value);
		}
		DateTime? departureTime;

		protected virtual void FromSource()
		{
			if (Place != null) {
				CodeName = Place.CodeName;
				RealName = Place.RealName;
				Address = Place.Address;
			} else {
				CodeName = RealName = Address = Info = string.Empty;
			}
		}

		static string DurationString(TimeSpan duration)
		{
			int mins = (int)Math.Round(duration.TotalMinutes);
			if (mins < 60) {
				return $"{mins} min";
			}
			int hours = mins / 60;
			mins -= hours * 60;
			return $"{hours} h {mins} min";
		}

		string TimeString(TimeSpan time)
		{
			if (Route.IsStarted) {
				DateTime dt = DateTime.Now.Add(time);
				return dt.ToShortTimeString();
			} else {
				return "in " + DurationString(time);
			}
		}

		public void UpdateInfo()
		{
			string info = string.Empty;
			if (Status < RoutеStopStatus.Arrived) {
				if (Leg?.DurationRequested == true) {
					info = $"{StdGlyph.BlackHourglass} Requesting ETA...";
				} 
				else if (LegDuration != null) {
					info = DurationString((TimeSpan)LegDuration);

					if (ETA != null) {
						var eta = TimeString((TimeSpan)ETA);
						info += $"    ETA: {eta}";
					}
				} else {
					//info = $"{StdGlyph.BlackHourglass} Requesting ETA...";
				}
			}
			else if (Status == RoutеStopStatus.Arrived) {
				var arrived = ArrivalTime?.ToShortTimeString();
				if (arrived != null) {
					info = $"Arrived: {arrived}";
				}
			}
			else if (Status == RoutеStopStatus.Departed) {
				var departed = DepartureTime?.ToShortTimeString();
				if (departed != null) {
					info = $"Departed: {departed}";
				}
			}

			Info = info;
		}

		void OnSourcePropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (sender == Place) {
				FromSource();
			}
		}
	}

	public class CustomerStop : RouteStop
	{
		public CustomerStop(Route route, Customer customer) :
			base(route)
		{
			Guard.ArgumentNotNull(customer, nameof(customer));

			Place = customer;
		}

		public CustomerStop(Route route, string codeName) :
			base(route)
		{
			var customer = AppScope.GetCustomer(codeName);
			Place = customer ?? throw new ProgramError($"Unknown Customer CodeName=\"{codeName ?? string.Empty}\"");
		}

		protected override void FromSource()
		{
			Kind = RouteStopKind.Customer;
			Customer = Place as Customer;

			base.FromSource();
		}

		public Customer Customer { get; private set; }
	}

	public class RouteStartStop : RouteStop
	{
		public RouteStartStop(Route route, Place place = null) :
			base(route, place ?? route?.StartPoint, RouteStopKind.StartPoint)
		{
			Status = RoutеStopStatus.Arrived;
		}

		public RouteStartStop(Route route, string codeName) : 
			base(route, RouteStopKind.StartPoint, codeName, route?.StartPoint)
		{
			Status = RoutеStopStatus.Arrived;
		}
	}

	public class RouteEndStop : RouteStop
	{
		public RouteEndStop(Route route, Place place = null) :
			base(route, place ?? route?.EndPoint, RouteStopKind.EndPoint)
		{
		}

		public RouteEndStop(Route route, string codeName) :
			base(route, RouteStopKind.EndPoint, codeName, route?.EndPoint)
		{
		}
	}

	public class RouteMidStop : RouteStop
	{
		public RouteMidStop(Route route, Place place) :
			base(route, place, RouteStopKind.MidPoint)
		{
		}

		public RouteMidStop(Route route, string codeName) :
			base(route, RouteStopKind.MidPoint, codeName)
		{
		}
	}
}
