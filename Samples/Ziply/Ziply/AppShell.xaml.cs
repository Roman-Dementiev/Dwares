using System;
using Xamarin.Forms;
using Dwares.Druid.UI;
using Ziply.ViewModels;
using Ziply.Views;

namespace Ziply
{
	public partial class AppShell : ShellEx
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(AboutPage), typeof(AboutPage));
			Routing.RegisterRoute(nameof(ZipPage), typeof(ZipPage));
			Routing.RegisterRoute(nameof(EtaPage), typeof(EtaPage));
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}
