using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWerks.Model
{
	public interface INamed
	{
		string Name { get; }
	}

	public class Named : INamed
	{
		public Named() { }
		public Named(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
		public override string ToString() => Name;

		public static T ByName<T>(IList<T> list, string name) where T : INamed
		{
			foreach (var platform in list) {
				if (platform.Name == name)
					return platform;
			}
			return default(T);
		}
	}
}
