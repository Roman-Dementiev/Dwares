using System;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms;

namespace Dwares.Druid
{
	public static partial class Extensions
	{
		public static void AddDefaultViewLocators()
		{
			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Page",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "EditModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "EditPage",
				TargetNamespaceSuffix = "Views"
			});

			ClassLocator.AddLocator(new DefaultClassLocator<Page> {
				ReferenceClassNameSuffix = "EditModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "EditForm",
				TargetNamespaceSuffix = "Views"
			});
		}

		public static void AddDefaultViewLocators(this Application app)
		{
			AddDefaultViewLocators();
		}

		public static Page InitMainPageWithNavigation(this Application app, Type viewModelType)
		{
			var mainPage = BindingScope.CreatePage(viewModelType);

			var navPage = new NavigationPage(mainPage) {
				BarBackgroundColor = Color.DodgerBlue,
				BarTextColor = Color.White
			};

			app.MainPage = navPage;

			Navigator.Initialize(navPage);;

			return mainPage;
		}
	}
}
