using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;


namespace RouteOptimizer.Models
{
	public enum RouteStopKind
	{
		Default,
		StartPoint,
		EndPoint
	}

	public abstract class RouteStop : Model, IOrdinal
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStop));

		public RouteStop()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract string Id { get; }

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		public RouteStopKind Kind  {
			get => kind;
			set => SetProperty(ref kind, value);
		}
		RouteStopKind kind;

		public RouteLeg Leg {
			get => leg;
			set => SetProperty(ref leg, value);
		}
		RouteLeg leg;
	}

	public class PlaceStop : RouteStop
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStop));
		
		public PlaceStop()
		{
			//Debug.EnableTracing(@class);
		}

		public PlaceStop(Place place)
		{
			Guard.ArgumentNotNull(place, nameof(place));

			Name = place.Name;
			Address = place.Address;
		}

		public override string Id => Ids.PlaceId(Name, Address);

		public string Name {
			get => name;
			set => SetPropertyEx(ref name, value, nameof(Name), nameof(Id));
		}
		string name;

		public string Address {
			get => Address;
			set => SetPropertyEx(ref address, value, nameof(Address), nameof(Id));
		}
		string address;

	}
}
