using System;
using Dwares.Dwarf;


namespace RouteOptimizer.Models
{
	public class RouteLeg
	{
		//static ClassRef @class = new ClassRef(typeof(RouteLeg));

		public RouteLeg()
		{
			//Debug.EnableTracing(@class);
		}

		public string Id {
			get {
				var startId = StartPoint?.Id ?? string.Empty;
				var endId = EndPoint?.Id ?? string.Empty;
				return $"{startId}:{endId}";
			}
		}

		public RouteStop StartPoint { get; set; }
		public RouteStop EndPoint { get; set; }
		public TimeSpan? Duration { get; set; }
		public bool DurationRequested { get; set; }
	}
}
