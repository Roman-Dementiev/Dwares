using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Xamarin.Forms;


namespace Dwares.Rookie.ViewModels
{
	public class FramedFormViewModel : FormPageViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(FormViewModel));

		public FramedFormViewModel()
		{
			//Debug.EnableTracing(@class);
		}

		public Size FrameSize => new Size(FrameWidth, FrameHeight);
		public virtual double FrameWidth => 340;
		public virtual double FrameHeight => 480;
	}
}
