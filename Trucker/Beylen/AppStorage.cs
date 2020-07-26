using System;
using System.Threading;
using Dwares.Dwarf;
using Beylen.Storage;


namespace Beylen
{
	public static class AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AppStorage));

		static IAppStorage instance;
		public static IAppStorage Instance {
			get => LazyInitializer.EnsureInitialized(ref instance, () => new MockStorage());
			set => instance = value;
		}
	}
}
