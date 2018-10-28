using System;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace Dwares.Druid.UI
{
	public abstract class ToggleGlyphBase: GlyphControl
	{
		public event EventHandler<bool> CheckedChanged;

		public static readonly BindableProperty IsCheckedProperty =
			BindableProperty.Create(
				nameof(IsChecked),
				typeof(bool),
				typeof(ToggleGlyphBase),
				false,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleGlyphBase control && newValue is bool isChecked) {
						// Set the graphic.
						control.Glyph = control.GetGlyph(isChecked);

						// Fire the event.
						control.CheckedChanged?.Invoke(control, isChecked);
					}
				});

		public bool IsChecked {
			set { SetValue(IsCheckedProperty, value); }
			get { return (bool)GetValue(IsCheckedProperty); }
		}

		protected override void OnTapped()
		{
			IsChecked = !IsChecked;
		}

		public abstract string GetGlyph(bool isChecked);
	}


	public class ToggleGlyph : ToggleGlyphBase
	{
		public ToggleGlyph() { }

		public ToggleGlyph(string checkedGlyph, string uncheckedGlyph)
		{
			CheckedGlyph = checkedGlyph;
			UncheckedGlyph = uncheckedGlyph;
		}

		public static readonly BindableProperty CheckedGlyphProperty =
			BindableProperty.Create(
				nameof(CheckedGlyph),
				typeof(string),
				typeof(ToggleGlyph),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleGlyphBase control) {
						if (control.IsChecked) {
							control.Glyph = control.GetGlyph(true);
						}
					}
				});

		public string CheckedGlyph {
			set { SetValue(CheckedGlyphProperty, value); }
			get { return (string)GetValue(CheckedGlyphProperty); }
		}

		public static readonly BindableProperty UncheckedGlyphProperty =
			BindableProperty.Create(
				nameof(UncheckedGlyph),
				typeof(string),
				typeof(ToggleGlyph),
				null,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleGlyphBase control) {
						if (!control.IsChecked) {
							control.Glyph = control.GetGlyph(false);
						}
					}
				});

		public string UncheckedGlyph {
			set { SetValue(UncheckedGlyphProperty, value); }
			get { return (string)GetValue(UncheckedGlyphProperty); }
		}

		public override string GetGlyph(bool isChecked)
		{
			if (isChecked && CheckedGlyph != null) {
				return CheckedGlyph;
			}
			return UncheckedGlyph ?? Glyph;
		}
	}

	public class UpDownGlyph : ToggleGlyph
	{
		public UpDownGlyph() : base(StdGlyph.UpArrow, StdGlyph.DownArrow) { }

		public bool IsUpwards => IsChecked;
	}

	public class FilledUpDownGlyph : ToggleGlyph
	{
		public FilledUpDownGlyph() : base(StdGlyph.FilledUpwardsArrow, StdGlyph.FilledDownwardsArrow) { }

		public bool IsUpwards => IsChecked;
	}
}

