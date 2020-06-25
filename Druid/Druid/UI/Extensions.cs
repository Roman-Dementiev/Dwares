using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;


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

		public static bool ApplyFlavor_(this VisualElement element, string flavor, UITheme theme = null)
		{
			if (element == null || string.IsNullOrEmpty(flavor))
				return false;

			if (!GetTheme(ref theme))
				return false;

			var style = theme.GetStyle(flavor);
			if (style != null) {
				element.Style = style;
				return true;
			} else {
				Debug.Print($"Style '{flavor}' not found in UITheme");
				return false;
			}
		}

		public static bool ApplyFlavor<T>(this T element) where T : VisualElement, IThemeAware
		{
			return ApplyFlavor_(element, element?.Flavor);
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

		public static void ResetPages<T>(this MultiPage<T> multiPage, IEnumerable<Type> viewModelTypes, bool contentViewModels) where T : Page
		{
			multiPage.Children.Clear();

			if (viewModelTypes != null) {
				foreach (var type in viewModelTypes) {
					T page;
					if (contentViewModels) {
						page = Forge.CreateContentPage(type) as T;
					}
					else {
						page = Forge.CreatePage(type) as T;
					}
					if (page != null) {
						multiPage.Children.Add(page);
					}
				}
			}
		}

		public static void ResetPages(this MultiPage<Page> multiPage, IEnumerable<Type> viewModelTypes, bool contentViewModels, bool useNavigationPages)
		{
			multiPage.Children.Clear();

			if (viewModelTypes != null) {
				foreach (var type in viewModelTypes) {
					Page page;
					if (contentViewModels) {
						page = Forge.CreateContentPage(type);
					} else {
						page = Forge.CreatePage(type);
					}
					if (useNavigationPages) {
						page = new NavigationPage(page) {
							Title = page.Title
						};
					}
					multiPage.Children.Add(page);
				}
			}
		}

		public static void OnViewModelChanged<T>(this MultiPage<T> multiPage, MultiPageViewModel viewModel) where T : Page
		{
			ResetPages(multiPage, viewModel?.ViewModelTypes, viewModel?.ContentViewModels == true);
		}

		public static void OnViewModelChanged(this MultiPage<Page> multiPage, MultiPageViewModel viewModel, bool useNavigationPages)
		{
			ResetPages(multiPage, viewModel?.ViewModelTypes, viewModel?.ContentViewModels == true, useNavigationPages);
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
	}
}
