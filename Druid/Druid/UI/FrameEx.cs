using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class FrameEx : Frame, IContentHolder, ITargeting
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

		public Element GetTargetElement() => ContentView;

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(FrameEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FrameEx frame) {
						frame.ApplyFlavor(frame.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
