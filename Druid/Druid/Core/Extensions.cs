using System;
using Xamarin.Forms;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Forms;


namespace Dwares.Druid
{
	public static partial class Extensions
	{
		public static void AddDefaultViewLocators()
		{
			ClassLocator.AddLocator(new DefaultClassLocator<View> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "View",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "PageViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Page",
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
			var navPage = new NavigationPage(mainPage) {
				BarBackgroundColor = Color.DodgerBlue,
				BarTextColor = Color.White
			};

			app.MainPage = navPage;

			Navigator.Initialize(navPage);
		}

		public static Page InitMainPageWithNavigation(this Application app, Type viewModelType)
		{
			var mainPage = Forge.CreatePage(viewModelType);
			app.InitMainPageWithNavigation(mainPage);
			return mainPage;
		}
	}
}
