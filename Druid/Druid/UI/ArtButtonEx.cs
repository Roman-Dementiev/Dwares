using System;
using System.Windows.Input;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Xamarin.Forms;

namespace Dwares.Druid.UI
{
	public class ArtButtonEx : ArtButton
	{
		//static ClassRef @class = new ClassRef(typeof(ArtButtonEx));

		public ArtButtonEx()
		{
			//Debug.EnableTracing(@class);

			UITheme.CurrentThemeChanged += UITheme_CurrentThemeChanged;
		}

		private void UITheme_CurrentThemeChanged(object sender, EventArgs e)
		{
			defaultStyle = UITheme.Current.GetStyle(DefaultFlavor);
			pushedStyle = UITheme.Current.GetStyle(PushedFlavor);
			disabledStyle = UITheme.Current.GetStyle(DisabledFlavor);
			ApplyStyle();
		}

		public event EventHandler StateChanged;

		public Style defaultStyle;
		public Style DefaultStyle {
			get {
				if (defaultStyle == null) {
					defaultStyle = new Style(typeof(ArtButtonEx));
					// TODO
				}
				return defaultStyle;
			}
			set {
				if (value != defaultStyle) {
					defaultStyle = value;
					ApplyStyle();
				}
			}
		}

		Style pushedStyle;
		public Style PushedStyle {
			get => pushedStyle ?? DefaultStyle;
			set {
				if (value != pushedStyle) {
					pushedStyle = value;
					ApplyStyle();
				}
			}
		}

		Style disabledStyle;
		public Style DisabledStyle {
			get => disabledStyle ?? DefaultStyle;
			set {
				if (value != disabledStyle) {
					disabledStyle = value;
					ApplyStyle();
				}
			}
		}

		public static readonly BindableProperty DefaultFlavorProperty =
			BindableProperty.Create(
				nameof(DefaultFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.DefaultStyle = UITheme.Current.GetStyle(value);
					}
				});

		public string DefaultFlavor {
			set { SetValue(DefaultFlavorProperty, value); }
			get { return (string)GetValue(DefaultFlavorProperty); }
		}

		public static readonly BindableProperty PushedFlavorProperty =
			BindableProperty.Create(
				nameof(PushedFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.PushedStyle = UITheme.Current.GetStyle(value);
					}
				});

		public string PushedFlavor {
			set { SetValue(PushedFlavorProperty, value); }
			get { return (string)GetValue(PushedFlavorProperty); }
		}

		public static readonly BindableProperty DisabledFlavorProperty =
			BindableProperty.Create(
				nameof(DisabledFlavor),
				typeof(string),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.DisabledStyle = UITheme.Current.GetStyle(value);
					}
				});

		public string DisabledFlavor {
			set { SetValue(DisabledFlavorProperty, value); }
			get { return (string)GetValue(DisabledFlavorProperty); }
		}

		public static readonly BindableProperty IsPushedProperty =
			BindableProperty.Create(
				nameof(IsPushed),
				typeof(bool),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is bool value) {
						button.OnStateChanged();
					}
				});

		public bool IsPushed {
			set { SetValue(IsPushedProperty, value); }
			get { return (bool)GetValue(IsPushedProperty); }
		}

		public static readonly BindableProperty IsDisabledProperty =
			BindableProperty.Create(
				nameof(IsDisabled),
				typeof(bool),
				typeof(ArtButtonEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is bool value) {
						button.OnStateChanged();
					}
				});

		public static readonly BindableProperty DefaultArtProperty =
			BindableProperty.Create(
				nameof(DefaultArt),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.ApplyArt();
					}
				});

		public string DefaultArt {
			set { SetValue(DefaultArtProperty, value); }
			get { return (string)GetValue(DefaultArtProperty); }
		}

		public static readonly BindableProperty PushedArtProperty =
			BindableProperty.Create(
				nameof(PushedArt),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.ApplyArt();
					}
				});

		public string PushedArt {
			set { SetValue(PushedArtProperty, value); }
			get { return (string)GetValue(PushedArtProperty); }
		}

		public static readonly BindableProperty DisabledArtProperty =
			BindableProperty.Create(
				nameof(DisabledArt),
				typeof(string),
				typeof(ArtButton),
				defaultValue: string.Empty,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ArtButtonEx button && newValue is string value) {
						button.ApplyArt();
					}
				});

		public string DisabledArt {
			set { SetValue(DisabledArtProperty, value); }
			get { return (string)GetValue(DisabledArtProperty); }
		}

		public bool IsDisabled {
			set { SetValue(IsDisabledProperty, value); }
			get { return (bool)GetValue(IsDisabledProperty); }
		}

		public new bool IsEnabled {
			get => !IsDisabled;
			set {
				if (value == IsDisabled) {
					IsDisabled = !value;
					base.IsEnabled = value;
				}
			}
		}

		protected virtual void ApplyStyle()
		{
			if (IsDisabled) {
				Style = DisabledStyle;
			} else if (IsPushed) {
				Style = PushedStyle;
			} else {
				Style = DefaultStyle;
			}
		}

		protected virtual void ApplyArt()
		{
			string art;
			if (IsDisabled && !string.IsNullOrEmpty(DisabledArt)) {
				art = DisabledArt;
			} else if (IsPushed && !string.IsNullOrEmpty(PushedArt)) {
				art = PushedArt;
			} else {
				art = DefaultArt;
			}

			if (!string.IsNullOrEmpty(art)) {
				Art = art;
			}
		}

		protected override void SelectImageSource(string name, Color? color)
		{
			ImageSource = UITheme.Current.GetImage(name);
		}

		protected virtual void OnStateChanged()
		{
			ApplyStyle();
			ApplyArt();
			StateChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}
