using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BingMapsRESTToolkitEx;


namespace Ziply.ViewModels
{

	public class TimeZoneInfo
	{
		public string Name { get; set; }
		public string Abbr { get; set; }

		public double UtcOffset { get; set; }
	}


	public partial class EtaViewModel : BaseViewModel
	{

		TimeZoneInfo DestinationTimeZone { get; set; }

		public async Task<TimeZoneInfo> GetTimeZone(double latitude, double longitude)
		{
			if (DestinationTimeZone == null) {
				var destRequest = new FindTimeZoneRequest(Address, DateTime.Now) {
					BingMapsKey = BMK
				};

				var destResponse = await destRequest.Execute();

				DestinationTimeZone = TimeZoneFromResponse(destResponse);
			}

			var request = new FindTimeZoneRequest(new Coordinate(latitude, longitude)) {
				BingMapsKey = BMK
			};
			var response = await request.Execute();

			return TimeZoneFromResponse(response);
		}

		TimeZoneResponse GetTimeZoneResponse(Response response)
		{
			foreach (var rs in response.ResourceSets) {
				int i = 0;
				foreach (var resource in rs.Resources) {
					var tz = resource as RESTTimeZone;
					if (tz != null) {
						if (tz.TimeZone != null)
							return tz.TimeZone;

						if (tz.TimeZoneAtLocation != null && tz.TimeZoneAtLocation.Length > 0) {
							var tzal = tz.TimeZoneAtLocation[0];
							if (tzal != null && tzal.TimeZone != null && tzal.TimeZone.Length > 0)
								return tzal.TimeZone[0];
						}
					}
				}
			}
			return null;
		}

		TimeZoneInfo TimeZoneFromResponse(Response response)
		{
			var tz = GetTimeZoneResponse(response);
			if (tz != null) {
				return new TimeZoneInfo {
					Name = tz.GenericName,
					Abbr = tz.Abbreviation,
					UtcOffset = TimeOffset(tz.UtcOffset)
				};
			};
			return null;
		}

		double TimeOffset(string offset)
		{
			offset = offset.Trim();
			if (offset.Length == 0)
				return 0;

			bool isNegative = false;
			if (offset[0] == '-') {
				isNegative = true;
				offset = offset.Substring(1);
			}
			else if (offset[0] == '+') {
				offset = offset.Substring(1);
			}

			var split = offset.Split(':');
			if (split.Length == 2) {
				int hh, mm;
				if (int.TryParse(split[0], out hh) && int.TryParse(split[1], out mm)) {
					var result = hh + mm / 60.0;
					if (isNegative)
						result = -result;
					return result;
				}
			}

			return 0;
		}

	}
}
