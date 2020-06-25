using System;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Lost
{
	public class AppScope : BindingScope
	{
		//static ClassRef @class = new ClassRef(typeof(AppScope));

		static AppScope instance;
		public static AppScope Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		public AppScope() : base(null)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
