using System;
using System.Threading.Tasks;
using Dwares.Drudge.Airtable;
using Dwares.Dwarf;


namespace Drive.Storage.Air
{
	public class PhonesTable : ContactsTable<PhoneRecord>
	{
		//static ClassRef @class = new ClassRef(typeof(PhonesYable));

		public PhonesTable(AirBase airBase) :
			base(airBase, "Phones")
		{
			//Debug.EnableTracing(@class);
		}
	}

	public class PhoneRecord : ContactRecord
	{
		public PhoneRecord() { }

	}
}
