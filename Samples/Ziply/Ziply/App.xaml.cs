using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Ziply.Services;
using Ziply.Views;
using Ziply.ViewModels;


namespace Ziply
{
	public partial class App : Application
	{

		public App()
		{
			Dwares.Dwarf.Package.Init();
			Dwares.Druid.Package.Init();

			InitializeComponent();

			UIThemeManager.Instance = new UIThemes();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
		}

		protected override async void OnStart()
		{
			//await ZipViewModel.Instance.Refresh();
		}

		protected override void OnSleep()
		{
		}

		protected override async void OnResume()
		{
			//await ZipViewModel.Instance.RefreshIfExpired();
		}
	}
}
