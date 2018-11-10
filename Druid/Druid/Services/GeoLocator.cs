using System;
using System.Threading.Tasks;
using Dwares.Druid.Essential;


namespace Dwares.Druid.Services
{
	public interface IGeoLocator
	{
		Task<GeoPosition> GetPosition();
		Task<GeoPosition> GetPosition(GeolocationAccuracy accuracy, TimeSpan timeout);
	}

	public static class GeoLocator
	{
		static DependencyService<IGeoLocator> instance;
		public static IGeoLocator Instance => DependencyService<IGeoLocator>.GetInstance(ref instance);

		public static async Task<GeoPosition> GetPosition()
		{
			return await Instance.GetPosition();
		}

		public static async Task<GeoPosition> GetPosition(GeolocationAccuracy accuracy, TimeSpan? timeout = null)
		{
			return await Instance.GetPosition(accuracy, timeout ?? TimeSpan.Zero);
		}
	}
}
