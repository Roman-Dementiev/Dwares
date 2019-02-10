using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class Vocabulary : Dictionary<string, string>
	{
		public Vocabulary() { }

		public string Get(string key)
		{
			string value;
			if (TryGetValue(key, out value)) {
				return value;
			} else {
				return null;
			}
		}

		public void Put(string key, string value)
		{
			this[key] = value;
		}

		// TODO
	}
}
