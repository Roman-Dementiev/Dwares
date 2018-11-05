using System;
using Dwares.Dwarf.Toolkit;
using ACE.Models;

namespace ACE.ViewModels
{
	public class RouteItem : SelectableItem
	{
		public RouteItem(RouteStop routeStop)
		{
			RouteStop = routeStop ?? throw new ArgumentNullException(nameof(routeStop));
		}

		public RouteStop RouteStop { get; }

		public bool HasName => !String.IsNullOrEmpty(Name);
		public string Name => RouteStop.Name;
		public string Address => RouteStop.Address;

		protected override void OnSelectedChanged()
		{
			PropertiesChanged(
				nameof(IsSelected)
				//TODOs
				);
		}
	}
}
