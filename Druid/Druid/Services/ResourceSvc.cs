using System;
using System.Collections.Generic;
using System.Text;


namespace Dwares.Druid.Services
{
	public interface IResourceSvc
	{
		string GetString(string key, bool useKeyAsDefault = true);
	}

	public static class ResourceSvc
	{
		static DependencyService<IResourceSvc> instance = new DependencyService<IResourceSvc>(true);
		public static IResourceSvc Instance = instance.Service;

		public static string GetString(string key, bool useKeyAsDefault = true)
		{
			return Instance.GetString(key, useKeyAsDefault);
		}
	}
}
