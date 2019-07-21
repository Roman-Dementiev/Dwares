using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace Dwares.Druid.UI
{
	public class StackPanel : ContentView
	{
		//static ClassRef @class = new ClassRef(typeof(StackPanel));

		ArtImage icon;

		public StackPanel()
		{
			//Debug.EnableTracing(@class);

			Stack = new StackLayout {
				Orientation = StackOrientation.Horizontal,
				Margin = new Thickness(-19)
			};

			//Content = Stack;
			//BorderColor = Color.Transparent;
			//BackgroundColor = Color.Transparent;

			Content = Frame = new Frame {
				Content = Stack,
				BorderColor = Color.Transparent,
				BackgroundColor = Color.Transparent
			};

			icon = new ArtImage {
				HeightRequest = 32,
				WidthRequest = 32,
				IsVisible = false
			};

			Stack.Children.Add(icon);
		}

		public Frame Frame { get; }
		public StackLayout Stack { get; }

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(StackPanel),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is StackPanel panel) {
						panel.ApplyFlavor(panel.Flavor);
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		public static readonly BindableProperty OrientationProperty =
			BindableProperty.Create(
				nameof(Orientation),
				typeof(StackOrientation),
				typeof(StackPanel),
				defaultValue: StackOrientation.Horizontal,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is StackPanel panel && newValue is StackOrientation orientation) {
						panel.Stack.Orientation = orientation;
					}
				});

		public StackOrientation Orientation {
			set { SetValue(OrientationProperty, value); }
			get { return (StackOrientation)GetValue(OrientationProperty); }
		}

		public static readonly BindableProperty BorderColorProperty =
			BindableProperty.Create(
				nameof(BorderColor),
				typeof(Color),
				typeof(StackPanel),
				defaultValue: Color.Transparent,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is StackPanel panel && newValue is Color color) {
						panel.SetBorderColor(color);
					}
				});

		public Color BorderColor {
			set { SetValue(BorderColorProperty, value); }
			get { return (Color)GetValue(BorderColorProperty); }
		}

		protected void SetBorderColor(Color color) => Frame.BorderColor = color;

		public static readonly BindableProperty IconArtProperty =
			BindableProperty.Create(
				nameof(IconArt),
				typeof(string),
				typeof(EntryPanel),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EntryPanel panel && newValue is string value) {
						panel.icon.Art = value;
						panel.icon.IsVisible = panel.icon.Source != null;
					}
				});

		public string IconArt {
			set { SetValue(IconArtProperty, value); }
			get { return (string)GetValue(IconArtProperty); }
		}

		public static readonly BindableProperty IconColorProperty =
			BindableProperty.Create(
				nameof(IconColor),
				typeof(Color),
				typeof(EntryPanel),
				defaultValue: default(Color),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is EntryPanel panel && newValue is Color color) {
						panel.icon.ArtColor = color;
					}
				});

		[TypeConverter(typeof(ColorTypeConverter))]
		public Color IconColor {
			set { SetValue(IconColorProperty, value); }
			get { return (Color)GetValue(IconColorProperty); }
		}
	}
}
