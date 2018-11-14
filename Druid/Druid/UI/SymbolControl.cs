using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class SymbolControl : ToggleGlyphBase
	{
		public SymbolControl() :this(SymbolEx.None, SymbolEx.None) {}

		public SymbolControl(SymbolEx checkedSymbol, SymbolEx uncheckedSymbol)
		{
			GlyphFontFamily = "Segoe MDL2 Assets";
			CheckedSymbol = checkedSymbol;
			UncheckedSymbol = uncheckedSymbol;
		}

		public static readonly BindableProperty CheckedSymbolProperty =
			BindableProperty.Create(
				nameof(CheckedSymbol),
				typeof(SymbolEx),
				typeof(ToggleGlyphBase),
				SymbolEx.None,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleGlyphBase control && newValue is SymbolEx symbol) {
						if (control.IsChecked) {
							control.Glyph = control.GetGlyph(true);
						}
					}
				});

		public SymbolEx CheckedSymbol {
			set { SetValue(CheckedSymbolProperty, value); }
			get { return (SymbolEx)GetValue(CheckedSymbolProperty); }
		}

		public static readonly BindableProperty UncheckedSymbolProperty =
			BindableProperty.Create(
				nameof(UncheckedSymbol),
				typeof(SymbolEx),
				typeof(ToggleGlyphBase),
				SymbolEx.None,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ToggleGlyphBase control) {
						if (!control.IsChecked) {
							control.Glyph = control.GetGlyph(true);
						}
					}
				});

		public SymbolEx UncheckedSymbol {
			set { SetValue(UncheckedSymbolProperty, value); }
			get { return (SymbolEx)GetValue(UncheckedSymbolProperty); }
		}

		public override string GetGlyph(bool isChecked)
		{
			if (isChecked && CheckedSymbol != SymbolEx.None) {
				return CheckedSymbol.Glyph();
			}
			if (UncheckedSymbol != SymbolEx.None) {
				return UncheckedSymbol.Glyph();
			}
			return Glyph;
		}
	}

	public class UpDownSymbol : SymbolControl
	{
		public UpDownSymbol() : base(SymbolEx.UpArrow, SymbolEx.DownArrow) { }
	}
}
