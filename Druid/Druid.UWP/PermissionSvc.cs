using System;
using Windows.Devices.Geolocation;
using Xamarin.Forms;
using Dwares.Druid.Services;
using System.Threading.Tasks;

[assembly: Dependency(typeof(Dwares.Druid.UWP.PermissionsSvc))]

namespace Dwares.Druid.UWP
{
	class PermissionsSvc : IPermissionSvc
	{
		public Task<PermissionStatus> CheckStatus(PermissionType permission)
		{
			switch (permission) {
			case PermissionType.LocationWhenInUse:
				return CheckLocationAsync();
			default:
				return Task.FromResult(PermissionStatus.Granted);
			}

		}

		static async Task<PermissionStatus> CheckLocationAsync()
		{
			var accessStatus = await Geolocator.RequestAccessAsync();
			switch (accessStatus)
			{
			case GeolocationAccessStatus.Allowed:
				return PermissionStatus.Granted;
			
			case GeolocationAccessStatus.Denied:
				return PermissionStatus.Denied;
			
			default:
				return PermissionStatus.Unknown;
			}
		}

		public Task<PermissionStatus> Request(PermissionType permission)
		{
			return CheckStatus(permission);
		}

		public async Task<PermissionStatus> Require(PermissionType permission)
		{
			var status = await Request(permission);
			if (status != PermissionStatus.Granted) {
				throw new PermissionException(permission, status);
			}
			return status;
		}
	}


}
