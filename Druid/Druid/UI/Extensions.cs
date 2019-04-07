using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
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

		public static bool ApplyTheme(this VisualElement element, string styleName = null, UITheme theme = null)
		{
			if (!GetTheme(ref theme))
				return false;

			if (string.IsNullOrEmpty(styleName)) {
				return theme.Apply(element);
			} else {
				return theme.Apply(element, styleName);
			}
		}

		public static bool ApplyTheme(this VisualElement element, Type type, UITheme theme = null)
		{
			if (!GetTheme(ref theme))
				return false;
				
			if (type == null) {
				return theme.Apply(element);
			} else {
				return theme.Apply(element, type);
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
	}
}
