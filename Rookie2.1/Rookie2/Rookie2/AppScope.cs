using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Dwares.Rookie
{
	public class AppScope : BindingScope
	{
		//static ClassRef @class = new ClassRef(typeof(AppScope));

		public static AppScope Instance { get; private set; }

		public AppScope() : base(null)
		{
			Debug.AssertIsNull(Instance);
			Instance = this;
		}
	}
}
