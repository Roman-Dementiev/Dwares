using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWerks.Model
{
	public class IconGroup : List<Icon>, INamed
	{
		public IconGroup(string name)
		{
			Name = name;
		}

		public string Name { get; set; }
	}

}
