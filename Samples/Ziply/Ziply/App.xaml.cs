using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Ziply
{
	public partial class App : Application
	{
		public App()
		{
			InitializeComponent();

			MainPage = new MainPage();
			ViewModel = new ViewModel();
			MainPage.BindingContext = ViewModel;
		}

		public ViewModel ViewModel { get; }

		protected override async void OnStart()
		{
			await ViewModel.Refresh();
		}

		protected override void OnSleep()
		{
		}

		protected override async void OnResume()
		{
			await ViewModel.RefreshIfExpired();
		}
	}
}
