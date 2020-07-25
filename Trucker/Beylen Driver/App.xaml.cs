using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Beylen_Driver.Services;
using Beylen_Driver.Views;

namespace Beylen_Driver
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
