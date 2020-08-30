using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class SpanEx : Span, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(SpanEx));

		public SpanEx()
		{
			//Debug.EnableTracing(@class);
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor(Flavor));
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(SpanEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is SpanEx span) {
						span.ApplyFlavor(span.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}
	}
}
