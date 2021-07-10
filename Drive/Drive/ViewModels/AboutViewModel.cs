using System;
using Dwares.Druid.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Drive.ViewModels
{
	public class AboutViewModel : ViewModel
	{
		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
		}

		public Command OpenWebCommand { get; }
	}
}