using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Druid.ViewModels;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public interface IThemeAware
	{
		string Flavor { get; set; }
	}

	public static partial class Extensions
	{
		static bool GetTheme(ref UITheme theme)
		{
			if (theme == null) {
				theme = UITheme.Current;
				if (theme == null)
					return false;
			}
			return true;
		}

		static Style? GetFlavorStyle(string flavor, UITheme theme = null)
		{
			if (string.IsNullOrEmpty(flavor))
				return null;

			if (!GetTheme(ref theme))
				return null;

			return theme.GetStyle(flavor);
		}

		static bool ApplyFlavor_(NavigableElement element, string flavor, UITheme theme = null)
		{
			if (element == null || string.IsNullOrEmpty(flavor))
				return false;

			var style = GetFlavorStyle(flavor, theme);
			if (style != null) {
				element.Style = style;
				return true;
			} else {
				Debug.Print($"Style '{flavor}' not found in UITheme");
				return false;
			}
		}

		public static bool ApplyFlavor<T>(this T element) where T : NavigableElement, IThemeAware
		{
			return ApplyFlavor_(element, element?.Flavor);
		}

		public static bool ApplyFlavor(this Span span, string flavor)
		{
			if (span == null || string.IsNullOrEmpty(flavor))
				return false;

			var style = GetFlavorStyle(flavor);
			if (style != null) {
				span.Style = style;
				return true;
			} else {
				Debug.Print($"Style '{flavor}' not found in UITheme");
				return false;
			}
		}

		public static bool ContainsSetter(this Style style, string propertyName)
		{
			var setters = style.Setters;
			foreach (var setter in style.Setters) {
				if (setter.Property.PropertyName == propertyName)
					return true;
			}
			return false;
		}

		public static bool RemoveSetter(this Style style, string propertyName)
		{
			var setters = style.Setters;
			for(int i = 0; i < setters.Count; i++) {
				if (setters[i].Property.PropertyName == propertyName) {
					setters.RemoveAt(i);
					return true;
				}
			}

			return false;
		}


		public static void MergeIn(this Style style, Style mergeStyle)
		{
			if (style == null || mergeStyle == null)
				return;

			foreach (var setter in mergeStyle.Setters) {
				if (!style.ContainsSetter(setter.Property.PropertyName))
					style.Setters.Add(setter);
			}
		}


		public static Thickness Add(this Thickness thickness, Thickness addendum)
		{
			return new Thickness(
				thickness.Left+ addendum.Left, 
				thickness.Top+ addendum.Top, 
				thickness.Right+addendum.Right,
				thickness.Bottom+addendum.Bottom);
		}

		public static Thickness Add(this Thickness thickness, double horizontalSize, double verticalSize)
			=> Add(thickness, new Thickness(horizontalSize, verticalSize));

		public static Thickness Add(this Thickness thickness, double uniformSize)
			=> Add(thickness, new Thickness(uniformSize));

		public static Page TopNavigationPage(this Page page, bool? modal)
		{
			var navigation = page?.Navigation;
			if (navigation == null)
				return null;

			Page topPage = null;

			if (modal != false) {
				topPage = Dwares.Dwarf.Collections.Collection.Last(navigation.ModalStack);
				if (topPage != null)
					return topPage;
			}

			if (modal != true) {
				topPage = Dwares.Dwarf.Collections.Collection.Last(navigation.NavigationStack);
			}

			return topPage;
		}
	}
}
