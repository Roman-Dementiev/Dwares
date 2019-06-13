using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Drive.Models
{
	public class RouteStop : Model
	{
		//static ClassRef @class = new ClassRef(typeof(RouteStop));

		public RouteStop(IPlace place, ScheduleTime time)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(place, nameof(place));

			//Title = place.RouteTitle;
			Place = place;
			Time = time;
		}

		public IPlace Place { get; }
		public ScheduleTime Time { get; set; }
		public long Seq { get; set; }

		public ScheduleTime EstimatedArrival { get; }
		public ScheduleTime EstimatedDeparture { get; }

		public static int Compare(RouteStop stop1, RouteStop stop2)
		{
			var t1 = stop1.Time.Ticks;
			var t2 = stop2.Time.Ticks;
			if (t1 == t2) {
				t1 = stop1.Seq;
				t2 = stop2.Seq;
			}

			if (t1 < t2)
				return -1;
			if (t1 > t2)
				return 1;
			return 0;
		}
	}
}
