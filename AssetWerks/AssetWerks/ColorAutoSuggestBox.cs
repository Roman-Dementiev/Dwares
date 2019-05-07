using System;
using SkiaSharp;
using AssetWerks.Model;
using Windows.UI.Xaml;

namespace AssetWerks
{
	public class ColorAutoSuggestBox : ChoiceAutoSuggestBox
	{
		public ColorAutoSuggestBox()
		{
			Choices = NamedColor.List;
		}


		//public static DependencyProperty ColorNameProperty { get; } = DependencyProperty.Register(
		//	nameof(ColorName), 
		//	typeof(string), 
		//	typeof(ColorAutoSuggestBox), 
		//	new PropertyMetadata(defaultValue: null)
		//);

		//public string ColorName {
		//	get => (string)GetValue(ColorNameProperty);
		//	set => SetValue(ColorNameProperty, value);
		//}

		public SKColor? GetColor()
		{
			var named = Selected as NamedColor ?? NamedColor.ByName(Input);
			if (named  != null) {
				return named.Color;
			} else {
				//TODO: hex
				return null;
			}
		}

		public void SelectColor(string name)
		{
			foreach (var choice in Choices) {
				if (choice is NamedColor named && named.Title == name) {
					Control.Text = name;
					UpdateSuggestions(name);
					break;
				}
			}
		}
	}
}
