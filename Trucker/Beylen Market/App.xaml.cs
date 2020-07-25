using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Beylen_Market.Services;
using Beylen_Market.Views;

namespace Beylen_Market
{
	public partial class App : Application
	{

		public App()
		{
			InitializeComponent();

			DependencyService.Register<MockDataStore>();
			MainPage = new AppShell();
		}

		protected override void OnStart()
		{
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
