using System;
using Dwares.Druid.UI;
using Drive.ViewModels;
using Xamarin.Forms;
using Drive.Views;


namespace Drive
{
	public partial class AppShell : ShellEx
	{
		public AppShell()
		{
			InitializeComponent();

			Routing.RegisterRoute(nameof(MainPage), typeof(MainPage));
			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
		}

		//private async void OnMenuItemClicked(object sender, EventArgs e)
		//{
		//	await Shell.Current.GoToAsync("//LoginPage");
		//}
	}
}
