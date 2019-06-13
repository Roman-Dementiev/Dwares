using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Drive.ViewModels
{
	public class RouteItem : ListViewItem<RouteStop>
	{
		//static ClassRef @class = new ClassRef(typeof(RouteItem));

		public RouteItem(RouteStop stop) :
			base(stop)
		{
			//Debug.EnableTracing(@class);

			UpdateFromSource();
		}

		public RouteStop Stop => Source;

		protected override void UpdateFromSource()
		{
			Title = Stop.Place.RouteTitle;
			Address = Stop.Place.Address;
			PlannedTime = Stop.Time.ToString();
			EstimatedArrival = Stop.EstimatedArrival.ToString();
			EstimatedDeparture = Stop.EstimatedDeparture.ToString();

			PropertiesChanged(
				nameof(Title),
				nameof(Address),
				nameof(PlannedTime), 
				nameof(EstimatedArrival), 
				nameof(EstimatedDeparture)
				);
		}

		string title;
		public string Title {
			get => title;
			set => SetProperty(ref title, value);
		}


		string address;
		public string Address {
			get => address;
			set => SetProperty(ref address, value);
		}

		string plannedTime;
		public string PlannedTime {
			get => plannedTime;
			set => SetProperty(ref plannedTime, value);
		}

		string estimatedArrival;
		public string EstimatedArrival {
			get => estimatedArrival;
			set => SetProperty(ref estimatedArrival, value);
		}

		string estimatedDeparture;
		public string EstimatedDeparture {
			get => estimatedDeparture;
			set => SetProperty(ref estimatedDeparture, value);
		}

		public static ObservableCollection<RouteItem> CreateCollection()
		{
			return new ShadowCollection<RouteItem, RouteStop>(
				AppScope.Instance.Route,
				(stop) => new RouteItem(stop)
				);
		}
	}
}
