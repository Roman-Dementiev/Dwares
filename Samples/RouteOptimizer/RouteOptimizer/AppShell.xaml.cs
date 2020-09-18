using System;
using System.Collections.Generic;
using RouteOptimizer.ViewModels;
using RouteOptimizer.Views;
using Xamarin.Forms;

namespace RouteOptimizer
{
	public partial class AppShell : Xamarin.Forms.Shell
	{
		public AppShell()
		{
			InitializeComponent();
		}

		private async void OnMenuItemClicked(object sender, EventArgs e)
		{
			await Shell.Current.GoToAsync("//LoginPage");
		}

		public string LogoText { get; } = "Route optimizer\rDwares";
	}
}
