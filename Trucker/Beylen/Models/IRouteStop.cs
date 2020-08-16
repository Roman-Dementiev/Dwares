using System;
using System.Collections.Generic;
using System.Text;
using Dwares.Druid;


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

	public interface IRouteStop : IModel
	{
		RouteStopKind Kind { get; }
		int Seq { get; set; }
		string CodeName { get; set; }
		string Address { get; set; }
		RouteStatus Status { get; set; }

	}

	public static partial class Extensions
	{
		public static bool IsCustomer(this IRouteStop stop) => stop.Kind == RouteStopKind.Customer;
		public static bool IsStartPoint(this IRouteStop stop) => stop.Kind == RouteStopKind.StartPoint;
		public static bool IsEndPoint(this IRouteStop stop) => stop.Kind == RouteStopKind.EndPoint;
	}
}
