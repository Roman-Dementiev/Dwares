using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class TextField : Field<string>
	{
		public TextField(string name) : base(name) { }

		protected override void ConvertFromText(string text)
		{
			if (text != Value) {
				Value = text;
			}
		}
	}
}
