using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Satchel;


namespace Dwares.Druid
{
	public static partial class Extensions
	{
		public static void AddDefaultViewLocators()
		{
			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "PageViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Page",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "FormViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Form",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<View> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "View",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Page",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<View> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Form",
				TargetNamespaceSuffix = "Views"
			});

			//ClassLocator.AddLocator(new DefaultClassLocator<Page> {
			//	ReferenceClassNameSuffix = "EditModel",
			//	ReferenceNamespaceSuffix = "ViewModels",
			//	TargetClassNameSuffix = "EditPage",
			//	TargetNamespaceSuffix = "Views"
			//});

			//ClassLocator.AddLocator(new DefaultClassLocator<Page> {
			//	ReferenceClassNameSuffix = "EditModel",
			//	ReferenceNamespaceSuffix = "ViewModels",
			//	TargetClassNameSuffix = "EditForm",
			//	TargetNamespaceSuffix = "Views"
			//});
		}

		public static void AddDefaultViewLocators(this Application app)
		{
			AddDefaultViewLocators();
		}

		public static void InitMainPageWithNavigation(this Application app, Page mainPage)
		{

			// TODO: use Theme
			//
			//app.MainPage = new NavigationPage(mainPage) {
			//	BarBackgroundColor = Color.DodgerBlue,
			//	BarTextColor = Color.White
			//};

			app.MainPage = new NavigationPage(mainPage);
			Navigator.Initialize();
		}

		public static Page InitMainPageWithNavigation(this Application app, Type viewModelType)
		{
			var mainPage = Forge.CreatePage(viewModelType);
			app.InitMainPageWithNavigation(mainPage);
			return mainPage;
		}

		public static void SetToolbarItems(this Page page, IList<ToolbarItem> toolbarItems)
		{
			page.ToolbarItems.Clear();

			if (toolbarItems != null) {
				foreach (var item in toolbarItems) {
					page.ToolbarItems.Add(item);
				}
			}
		}

		public static void SetPageTitle(this Page page, string title)
		{
			if (page is NavigationPage navPage) {
				page = navPage.CurrentPage;
			}

			if (page != null) {
				page.Title = title ?? string.Empty;
			}
		}
	}
}
