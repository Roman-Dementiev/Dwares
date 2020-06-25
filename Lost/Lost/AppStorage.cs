using System;
using System.Threading;
using Dwares.Dwarf;
using Lost.Storage;
using Lost.Storage.Air;


namespace Lost
{
	public class AppStorage
	{
		static IAppStorage instance;
		public static IAppStorage Instance {
			get => LazyInitializer.EnsureInitialized(ref instance, () => new AirStorage());
			set => instance = value;
		}
	}
}
