using Dwares.Druid;
using System;
using System.Windows.Input;
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
		}

		public ICommand OpenWebCommand { get; }
	}
}