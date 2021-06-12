using Buffy.ViewModels;
using Dwares.Druid.UI;
using Buffy.Views;
using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Buffy
{
	public partial class AppShell : ShellEx
	{
		public AppShell()
		{
			InitializeComponent();
			Routing.RegisterRoute(nameof(ItemDetailPage), typeof(ItemDetailPage));
			Routing.RegisterRoute(nameof(NewItemPage), typeof(NewItemPage));
			Routing.RegisterRoute(nameof(FuelingForm), typeof(FuelingForm));
			Routing.RegisterRoute(nameof(SettingsPage), typeof(SettingsPage));
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}
	}
}
