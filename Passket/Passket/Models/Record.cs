using System;
using System.Collections.Generic;
using Dwares.Dwarf;


namespace Passket.Models
{
	public class Record : Entity
	{
		//static ClassRef @class = new ClassRef(typeof(Record));

		public Record()
		{
			//Debug.EnableTracing(@class);

			Entries = new List<IEntry>();
		}

		public List<IEntry> Entries { get; }
	}
}
