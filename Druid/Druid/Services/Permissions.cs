using System;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public enum PermissionType
	{
		Unknown,
		Battery,
		Camera,
		Flashlight,
		LocationWhenInUse,
		NetworkState,
		Vibrate,
	}

	public enum PermissionStatus
	{
		// Permission is in an unknown state
		Unknown,

		// Denied by user
		Denied,

		// Feature is disabled on device
		Disabled,

		// Granted by user
		Granted,

		// Restricted (only iOS)
		Restricted
	}

	public interface IPermissionSvc
	{
		Task<PermissionStatus> CheckStatus(PermissionType permission);
		Task<PermissionStatus> Request(PermissionType permission);
		Task<PermissionStatus> Require(PermissionType permission);
	}

	public static class Permissions
	{
		static DependencyService<IPermissionSvc> instance;
		public static IPermissionSvc Instance => DependencyService<IPermissionSvc>.GetInstance(ref instance);

		public static async Task<PermissionStatus> CheckStatus(PermissionType permission)
		{
			return await Instance.CheckStatus(permission);
		}

		public static async Task<PermissionStatus> Request(PermissionType permission)
		{
			return await Instance.Request(permission);
		}

		public static async Task<PermissionStatus> Require(PermissionType permission)
		{
			return await Instance.Require(permission);
		}
	}
}
