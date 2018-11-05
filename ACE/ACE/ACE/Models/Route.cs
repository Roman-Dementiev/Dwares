using System;
using System.Collections.ObjectModel;


namespace ACE.Models
{
	public class Route : ObservableCollection<RouteStop>
	{
		public Route(Contact startingPoint)
		{
			Start = new RouteStop(RouteStopType.Endpoint, startingPoint.Name, startingPoint, null);
			Add(Start);
		}
		
		public RouteStop Start { get; }
	}
}
