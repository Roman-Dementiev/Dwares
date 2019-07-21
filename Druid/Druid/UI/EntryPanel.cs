using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class EntryPanel : StackPanel
	{
		//static ClassRef @class = new ClassRef(typeof(EntryPanel));

		//ArtImage image;
		EntryEx entry;

		public EntryPanel()
		{
			//Debug.EnableTracing(@class);

			//image = new ArtImage {
			//	HeightRequest = 24,
			//	WidthRequest = 24,
			//	IsVisible = false
			//};

			entry = new EntryEx {
				HorizontalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = Color.Transparent
			};

			//Stack.Children.Add(image);
			Stack.Children.Add(entry);
		}

		public static readonly BindableProperty TextColorProperty =
			BindableProperty.Create(
				nameof(TextColor),
				typeof(Color),
				typeof(EntryPanel),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EntryPanel panel && newValue is Color color) {
						panel.entry.TextColor = color;
					}
				});

		public Color TextColor {
			set { SetValue(TextColorProperty, value); }
			get { return (Color)GetValue(TextColorProperty); }
		}

		//public static readonly BindableProperty ArtProperty =
		//	BindableProperty.Create(
		//		nameof(Art),
		//		typeof(string),
		//		typeof(EntryPanel),
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is EntryPanel panel && newValue is string value) {
		//				panel.image.Art = value;
		//				panel.image.IsVisible = panel.image.Source != null;
		//			}
		//		});

		//public string Art {
		//	set { SetValue(ArtProperty, value); }
		//	get { return (string)GetValue(ArtProperty); }
		//}

		//public static readonly BindableProperty ArtColorProperty =
		//	BindableProperty.Create(
		//		nameof(ArtColor),
		//		typeof(Color),
		//		typeof(EntryPanel),
		//		defaultValue: default(Color),
		//		propertyChanged: (bindable, oldValue, newValue) => {
		//			if (bindable is EntryPanel panel && newValue is Color color) {
		//				panel.image.ArtColor = color;
		//			}
		//		});

		//[TypeConverter(typeof(ColorTypeConverter))]
		//public Color ArtColor {
		//	set { SetValue(ArtColorProperty, value); }
		//	get { return (Color)GetValue(ArtColorProperty); }
		//}
	}
}
