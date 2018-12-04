using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Passket.Models;


namespace Passket.ViewModels
{
	public class RecordEditModel : FormViewModel<Record>
	{
		//static ClassRef @class = new ClassRef(typeof(RecordEditModel));

		public RecordEditModel(Record source) :
			base(source)
		{
			//Debug.EnableTracing(@class);

			Title = source?.Name;
		}

	}
}
