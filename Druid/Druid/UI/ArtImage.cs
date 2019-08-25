using System;
using Dwares.Druid.Painting;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ArtImage : Image
	{
		//static ClassRef @class = new ClassRef(typeof(ArtImage));

		public ArtImage()
		{
			//Debug.EnableTracing(@class);

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor(Flavor));
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ArtImage),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtImage image) {
						image.ApplyFlavor(image.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		public static readonly BindableProperty ArtProperty =
			BindableProperty.Create(
				nameof(Art),
				typeof(string),
				typeof(ArtImage),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtImage image && newValue is string value) {
						image.SelectImageSource(value, image.ArtColor);
					}
				});

		public string Art {
			set { SetValue(ArtProperty, value); }
			get { return (string)GetValue(ArtProperty); }
		}

		public static readonly BindableProperty ArtColorProperty =
			BindableProperty.Create(
				nameof(ArtColor),
				typeof(Color),
				typeof(ArtImage),
				defaultValue: default(Color),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtImage image && newValue is Color color) {
						image.SelectImageSource(image.Art, color);
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color ArtColor {
			set { SetValue(ArtColorProperty, value); }
			get { return (Color)GetValue(ArtColorProperty); }
		}

		protected virtual void SelectImageSource(string name, Color color)
		{
			if (color == default) {
				Source = ArtBroker.Instance.GetImageSource(name);
			} else {
				Source = ArtBroker.Instance.GetImageSource(name, null, color);
			}
		}
	}
}
