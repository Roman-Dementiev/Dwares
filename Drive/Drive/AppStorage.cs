using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Drive.Storage;

namespace Drive
{
	public static class AppStorage
	{
		static IAppStorage instance;
		public static IAppStorage Instance { 
			get => instance; 
			set => LazyInitializer.EnsureInitialized(ref instance, () => new MockStorage());
		}
	}
}
