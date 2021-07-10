using System;
using Drive.Services;
using Drive.Views;
using Dwares.Druid.UI;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drive
{
	public partial class App : Application
	{

		public App()
		{
			Dwares.Dwarf.Package.Init();
			Dwares.Druid.Package.Init();
			UIThemeManager.Instance = new UIThemes();

			InitializeComponent();


			//MainPage = new MainPage();

			//MainPage = new NavigationPage(new MainPage());

			MainPage = new AppShell();
		}

		protected override async void OnStart()
		{
			await InitData();
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
