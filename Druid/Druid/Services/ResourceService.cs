using System;
using System.Collections.Generic;
using System.Text;


namespace Dwares.Druid.Services
{
	public interface IResourceService
	{
		string GetString(string key, bool useKeyAsDefault = true);
	}

	public static class ResourceService
	{
		public static IResourceService Instance {
			get => DependencyService.GetInstance<IResourceService>(ref instance);
			set => DependencyService.SetInstance(ref instance, value);
		}
		static IResourceService instance;

		public static string GetString(string key, bool useKeyAsDefault = true)
		{
			return Instance.GetString(key, useKeyAsDefault);
		}
	}
}
