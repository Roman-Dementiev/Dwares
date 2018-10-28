using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GlyphControl : ContentView
	{
		public event EventHandler Tapped;

		public GlyphControl ()
		{
			InitializeComponent ();
		}

		public static readonly BindableProperty GlyphProperty =
			BindableProperty.Create(
				nameof(Glyph),
				typeof(string),
				typeof(GlyphControl),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GlyphControl control && newValue is string text) {
						control.glyph.Text = text;
					}
				});

		public string Glyph {
			set { SetValue(GlyphProperty, value); }
			get { return (string)GetValue(GlyphProperty); }
		}

		public static readonly BindableProperty GlyphFontFamilyProperty =
			BindableProperty.Create(
				nameof(GlyphFontFamily),
				typeof(string),
				typeof(GlyphControl),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GlyphControl control && newValue is string fontFamily) {
						control.glyph.FontFamily = fontFamily;
					}
				});

		public string GlyphFontFamily {
			set { SetValue(GlyphFontFamilyProperty, value); }
			get { return (string)GetValue(GlyphFontFamilyProperty); }
		}

		public static readonly BindableProperty GlyphFontSizeProperty =
			BindableProperty.Create(
				nameof(GlyphFontSize),
				typeof(double),
				typeof(GlyphControl),
				Device.GetNamedSize(NamedSize.Default, typeof(Label)),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GlyphControl control && newValue is double fontSize) {
						control.glyph.FontSize = fontSize;
					}
				}
				);

		[TypeConverter(typeof(FontSizeConverter))]
		public double GlyphFontSize {
			set { SetValue(GlyphFontSizeProperty, value); }
			get { return (double)GetValue(GlyphFontSizeProperty); }
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(
				nameof(Text),
				typeof(string),
				typeof(GlyphControl),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GlyphControl control && newValue is string text) {
						control.text.Text = text;
					}
				});

		public string Text {
			set { SetValue(TextProperty, value); }
			get { return (string)GetValue(TextProperty); }
		}

		public static readonly BindableProperty TextFontSizeProperty =
			BindableProperty.Create(
				nameof(TextFontSize),
				typeof(double),
				typeof(GlyphControl),
				Device.GetNamedSize(NamedSize.Default, typeof(Label)),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is GlyphControl control && newValue is double fontSize) {
						control.text.FontSize = fontSize;
					}
				}
				);

		[TypeConverter(typeof(FontSizeConverter))]
		public double TextFontSize {
			set { SetValue(TextFontSizeProperty, value); }
			get { return (double)GetValue(TextFontSizeProperty); }
		}

		// TapGestureRecognizer handler.
		void OnTapped(object sender, EventArgs args)
		{
			if (IsEnabled) {
				OnTapped();

				Tapped?.Invoke(sender, args);
			}
		}

		protected virtual void OnTapped() { }
	}
}