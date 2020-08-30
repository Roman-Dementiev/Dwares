using System;
using Dwares.Druid.Forms;
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

	public enum RouteStatus
	{
		Pending,
		Enroute,
		Arrived,
		Departed
	}


	public class RouteStop : Place //, IRouteStop
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStop));

		protected RouteStop()
		{
			//Debug.EnableTracing(@class);
		}

		protected RouteStop(Place place, RouteStopKind kind)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(place, nameof(place));

			Kind = kind;
			FromSource(place);
		}

		protected RouteStop(RouteStopKind kind, string codeName, Place defaultPlace = null)
		{
			Kind = kind;
			
			var place = AppScope.GetPlace(codeName) ?? defaultPlace;
			if (place == null)
				throw new ProgramError($"Unknown Place CodeName=\"{codeName ?? string.Empty}\"");

			FromSource(place);
		}

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

		public RouteStatus Status {
			get => status;
			set => SetProperty(ref status, value);
		}
		RouteStatus status;

		public void SetStatus(RouteStatus value, bool forceNotification) =>
			SetProperty(ref status, value, forceNotification, nameof(Status));

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		protected void FromSource(Place source)
		{
			if (source != null) {
				CodeName = source.CodeName;
				RealName = source.RealName;
				Address = source.Address;
			} else {
				CodeName = RealName = Address = string.Empty;
			}
		}

	}

	public class CustomerStop : RouteStop
	{
		public CustomerStop(Customer customer)
		{
			Guard.ArgumentNotNull(customer, nameof(customer));

			Init(customer);
		}

		public CustomerStop(string codeName)
		{
			var customer = AppScope.GetCustomer(codeName);
			if (customer == null)
				throw new ProgramError($"Unknown Customer CodeName=\"{codeName ?? string.Empty}\"");

			Init(customer);
		}

		void Init(Customer customer)
		{
			Kind = RouteStopKind.Customer;
			Customer = customer;
			FromSource(customer);

			customer.PropertyChanged += (s, e) => FromSource(Customer);
		}

		public Customer Customer { get; private set; }
	}

	public class RouteStartStop : RouteStop
	{
		public RouteStartStop(Place place = null) :
			base(place ?? AppScope.Instance.StartPoint, RouteStopKind.StartPoint)
		{
			Status = RouteStatus.Arrived;
		}

		public RouteStartStop(string codeName) : 
			base(RouteStopKind.StartPoint, codeName, AppScope.Instance.StartPoint)
		{
			Status = RouteStatus.Arrived;
		}
	}

	public class RouteEndStop : RouteStop
	{
		public RouteEndStop(Place place = null) :
			base(place ?? AppScope.Instance.EndPoint, RouteStopKind.EndPoint)
		{
		}

		public RouteEndStop(string codeName) :
			base(RouteStopKind.EndPoint, codeName, AppScope.Instance.EndPoint)
		{
		}
	}

	public class RouteMidStop : RouteStop
	{
		public RouteMidStop(Place place) :
			base(place, RouteStopKind.MidPoint)
		{
		}

		public RouteMidStop(string codeName) :
			base(RouteStopKind.MidPoint, codeName)
		{
		}
	}
}
