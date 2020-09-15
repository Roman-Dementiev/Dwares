using System;
using Dwares.Druid;
using Dwares.Druid.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace RouteOptimizer.ViewModels
{
	public class AboutViewModel : ViewModel
	{
		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
		}

		public Command OpenWebCommand { get; }
	}
}
