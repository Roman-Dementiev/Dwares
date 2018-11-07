using System;
using Dwares.Dwarf.Toolkit;
using Dwares.Drums;


namespace ACE.Models
{
	public abstract class Run : SelectableItem
	{
		public abstract string OriginName { get; }
		public abstract ILocation Origin { get; }
		public abstract ScheduleTime? OriginTime { get; }

		public abstract string DestinationName { get; }
		public abstract ILocation Destination { get; }
		public abstract ScheduleTime? DestinationTime { get; }
	}
}
