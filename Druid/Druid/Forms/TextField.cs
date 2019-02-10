using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class TextField : Field<string>
	{
		//static ClassRef @class = new ClassRef(typeof(TextField));

		public TextField(bool required = false) :
			base(required)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
