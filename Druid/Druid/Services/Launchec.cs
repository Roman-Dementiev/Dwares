using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Dwares.Druid.Services
{
	public interface ILauncher
	{
		Task OpenUri(Uri uri, Dictionary<string, object> options);
	}


	public static class Launcher
	{
		static DependencyService<ILauncher> instance;
		public static ILauncher Instance => DependencyService<ILauncher>.GetInstance(ref instance);

		public static Task OpenUri(Uri uri, Dictionary<string, object> options = null)
		{
			return Instance.OpenUri(uri,options);
		}
	}
}
