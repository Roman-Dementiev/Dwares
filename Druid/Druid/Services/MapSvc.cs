using System;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface IMapSvc
	{
		Task OpenAddress(string address);
		Task OpenDirections(string from, string dest);
	}

	public static class MapSvc
	{
		static DependencyService<IMapSvc> instance;
		public static IMapSvc Instance {
			get => DependencyService<IMapSvc>.GetInstance(ref instance);
			set => DependencyService<IMapSvc>.SetInstance(ref instance, value);
		}

		public static Task OpenAddress(string address) => Instance.OpenAddress(address);
		public static Task OpenDirections(string from, string dest) => Instance.OpenDirections(from, dest);
	}
}
