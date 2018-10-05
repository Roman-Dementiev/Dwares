using System;
using System.Collections.Generic;


namespace Dwares.Druid.Localization
{
	public class Localizables : List<object>, ILocalizable
	{
		public virtual void Localize()
		{
			foreach (var obj in this) {
				Localizer.Localize(obj);
			}
		}
	}
}
