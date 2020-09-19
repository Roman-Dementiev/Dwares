using System;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class FrameEx : Frame, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(FrameEx));

		public FrameEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public View ContentView {
			get => Content;
			set => Content = value;
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(FrameEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is FrameEx frame) {
						frame.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
