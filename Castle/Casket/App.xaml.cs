using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf.Runtime;
using Dwares.Druid;
using Casket.ViewModels;
using Casket.Views;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace Casket
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			ClassFactory.AddLocator(new ClassLocator<Page> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetNamespaceSuffix = "Views"
			});

			ClassFactory.AddLocator(new ClassLocator<Page> {
				ReferenceClassNameSuffix = "ViewModel",
				ReferenceNamespaceSuffix = "ViewModels",
				TargetClassNameSuffix = "Page",
				TargetNamespaceSuffix = "Views"
			});

			//var test = ClassFactory.Create<Page>(typeof(MainPageViewModel));
			
			var mainPageViewModel = new MainPageViewModel();
			//MainPage = new MainTabbedPage(mainPageViewModel);
			MainPage = new MainTabbedPage(mainPageViewModel);

			Navigator.Initialize();
		}

		protected override void OnStart()
		{
			// Handle when your app starts
		}

		protected override void OnSleep()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume()
		{
			// Handle when your app resumes
		}
	}
}
