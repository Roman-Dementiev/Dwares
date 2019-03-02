using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class FrameEx : Frame, IContentHolder
	{
		//static ClassRef @class = new ClassRef(typeof(FrameEx));

		public FrameEx()
		{
			//Debug.EnableTracing(@class);
		}

		public View ContentView {
			get => Content;
			set => Content = value;
		}
	}
}
