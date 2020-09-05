using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Drudge.Airtable;
using Beylen.Models;


namespace Beylen.Storage.Air
{
	public partial class AirStorage : AirClient, IAppStorage
	{
		public async Task LoadRoute()
		{
			var route = AppScope.Instance.Route;

			var records = await RouteTable.ListRecords(sortField: RouteRecord._);
			foreach (var rec in records) {
				if (rec.Date < route.Date) {
					try {
						await RouteTable.DeleteRecord(rec.Id);
					}
					catch (Exception ex) {
						Debug.ExceptionCaught(ex);
					}
					continue;
				}

				if (rec.Date == route.Date) {
					var codeName = rec.CodeName.Trim();
					var tag = rec.Tags?.Trim();
					var status = rec.Status?.Trim();

					try {
						RouteStop stop;
						if (tag == Tag.startpoint) {
							stop = new RouteStartStop(codeName);
						} else if (tag == Tag.endpoint) {
							stop = new RouteEndStop(codeName);
						} else if (tag == Tag.midpoint) {
							stop = new RouteMidStop(codeName);
						} else {
							stop = new CustomerStop(codeName);
						}

						stop.RecordId = rec.Id;

						if (status == RouteRecord.Enroute) {
							stop.Status = RoutеStopStatus.Enroute;
						} else if (status == RouteRecord.Arrived) {
							stop.Status = RoutеStopStatus.Arrived;
						} else if (status == RouteRecord.Departed) {
							stop.Status = RoutеStopStatus.Departed;
						}

						await route.Add(stop, false);
					}
					catch (Exception ex) {
						Debug.ExceptionCaught(ex);
					}
				}
			}
		}

		public async Task AddRouteStop(RouteStop stop)
		{
			var rec = new RouteRecord {
				Date = AppScope.Instance.Route.Date,
				Ord = stop.Ordinal,
				CodeName = stop.CodeName
			};

			switch (stop.Kind)
			{
			case RouteStopKind.StartPoint:
				rec.Tags = Tag.startpoint;
				break;
			case RouteStopKind.EndPoint:
				rec.Tags = Tag.endpoint;
				break;
			case RouteStopKind.MidPoint:
				rec.Tags = Tag.midpoint;
				break;
			}

			switch (stop.Status)
			{
			case RoutеStopStatus.Enroute:
				rec.Status = RouteRecord.Enroute;
				break;
			case RoutеStopStatus.Arrived:
				rec.Status = RouteRecord.Arrived;
				break;
			case RoutеStopStatus.Departed:
				rec.Status = RouteRecord.Departed;
				break;
			}

			rec = await RouteTable.CreateRecord(rec);
			stop.RecordId = rec.Id;
		}

		public async Task DeleteRouteStop(RouteStop stop)
		{
			await RouteTable.DeleteRecord(stop.RecordId);
		}

		public async Task ChangeRouteStopOrdinal(RouteStop stop)
		{
			var rec = new RouteRecord {
				Id = stop.RecordId,
				Ord = stop.Ordinal
			};

			await RouteTable.UpdateRecord(rec, RouteRecord.ORD);
		}

		public async Task ChangeRouteStopStatus(RouteStop stop)
		{
			var rec = new RouteRecord {
				Id = stop.RecordId
			};

			switch (stop.Status)
			{
			case RoutеStopStatus.Enroute:
				rec.Status = RouteRecord.Enroute;
				break;
			case RoutеStopStatus.Arrived:
				rec.Status = RouteRecord.Arrived;
				break;
			case RoutеStopStatus.Departed:
				rec.Status = RouteRecord.Departed;
				break;
			}

			await RouteTable.UpdateRecord(rec, RouteRecord.STATUS);
		}

	}
}
