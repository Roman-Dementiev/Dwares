using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dwares.Druid.UI
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CheckBoxView : ContentView
	{
		public event EventHandler<bool> CheckedChanged;

		public CheckBoxView()
		{
			InitializeComponent();
		}

		public static readonly BindableProperty TextProperty =
			BindableProperty.Create(
				nameof(Text),
				typeof(string),
				typeof(CheckBoxView),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is CheckBoxView checkbox && newValue is string text) {
						checkbox.textLabel.Text = text;
					}
				});

		public string Text {
			set { SetValue(TextProperty, value); }
			get { return (string)GetValue(TextProperty); }
		}

		public static readonly BindableProperty FontSizeProperty =
			BindableProperty.Create(
				nameof(FontSize),
				typeof(double),
				typeof(CheckBoxView),
				Device.GetNamedSize(NamedSize.Default, typeof(Label)),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is CheckBoxView checkbox && newValue is double fontSize) {
						checkbox.boxLabel.FontSize = fontSize;
						checkbox.textLabel.FontSize = fontSize;
					}
				}
				);

		[TypeConverter(typeof(FontSizeConverter))]
		public double FontSize {
			set { SetValue(FontSizeProperty, value); }
			get { return (double)GetValue(FontSizeProperty); }
		}

		public static readonly BindableProperty IsCheckedProperty =
			BindableProperty.Create(
				nameof(IsChecked),
				typeof(bool),
				typeof(CheckBoxView),
				false,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is CheckBoxView checkbox && newValue is bool isChecked) {
						// Set the graphic.
						checkbox.boxLabel.Text = isChecked ? "\u2611" : "\u2610";

						// Fire the event.
						checkbox.CheckedChanged?.Invoke(checkbox, isChecked);
					}
				});

		public bool IsChecked {
			set { SetValue(IsCheckedProperty, value); }
			get { return (bool)GetValue(IsCheckedProperty); }
		}

		// TapGestureRecognizer handler.
		void OnCheckBoxTapped(object sender, EventArgs args)
		{
			IsChecked = !IsChecked;
		}
	}
}
