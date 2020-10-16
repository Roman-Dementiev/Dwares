using System;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Ziply.ViewModels
{
	public class AboutViewModel : Dwares.Druid.ViewModels.ViewModel
	{
		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamain-quickstart"));
		}

		public ICommand OpenWebCommand { get; }
	}
}