﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace Dwares.Drums
{
	public enum TravelMode
	{
		Driving,
		Walking,
		Transit,
		Truck
	}

	public enum Restriction
	{
		None,
		Minimize,
		Avoid
	}

	public enum Optimization
	{
		Default,
		Distance,
		Time,
		TimeWithTraffic,
		TimeAvoidClosure
	}

	public enum WaypointType
	{
		SatrtPoint,
		ViaPoint,
		EndPoint
	}

	public interface ICoordinate
	{
		double Latitude { get; }
		double Longitude { get; }
	}

	public interface ILocation
	{
		//TODO
		//string Landmark { get; }
		string Address { get; }
		ICoordinate Coordinate { get; }
	}

	public interface IWaypoint : ILocation
	{
		WaypointType WaypointType { get; }
	}

	public interface IRouteOptions
	{
		TravelMode TravelMode { get; set; }
		Restriction HighwaysRestriction { get; set; }
		Restriction TollsRestriction { get; set; }
		Optimization Optimization { get; set; }
	}

	public interface IRouteInfo
	{
		double DistanceInMiles { get; }
		TimeSpan TravelTime { get; }
		string TrafficCongestion { get; }
	}

	public interface IRouteLeg
	{
		double DistanceInMiles { get; }
		TimeSpan TravelTime { get; }
		//TODO
		//ILocation StartPoint { get; }
		//ILocation EndPoint { get; }
	}

	public interface IRoute : IRouteInfo
	{
		IEnumerable<IRouteLeg> Legs { get; }
	}

	public interface IMapService
	{
		Task<IRouteInfo> GetRouteInfo(IRouteOptions options, IEnumerable<IWaypoint> waypoints);
	}

	public interface IMapApplication
	{
		Task OpenAddress(string address);
		Task OpenDirections(string from, string dest);
	}


}
