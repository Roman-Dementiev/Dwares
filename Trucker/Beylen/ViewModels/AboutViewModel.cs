using Dwares.Druid;
using Dwares.Druid.UI;
using Dwares.Druid.ViewModels;
using System;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Beylen.ViewModels
{
	public class AboutViewModel : ViewModel
	{
		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://xamarin.com"));

			//GoBackCommand = new Command(async () => {
			//	//await Shell.Current.Navigation.PopAsync();
			//	await Shell.Current.GoToAsync("..");
			//});
		}

		public Command OpenWebCommand { get; }
		//public Command GoBackCommand { get; }
	}
}
