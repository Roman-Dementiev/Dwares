using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;


namespace Dwares.Rookie.ViewModels
{
	public class FormViewModel : Druid.Forms.FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(FormViewModek));

		public const double DefaultWidth = 340;
		public const double DefaultHeight = 500;

		public FormViewModel()
		{
			//Debug.EnableTracing(@class);
		}

		public override double FormWidth { get; set; } = DefaultWidth;
		public override double FormHeight { get; set; } = DefaultHeight;
	}
}
