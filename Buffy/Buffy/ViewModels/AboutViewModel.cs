using System;
using System.Windows.Input;
using Dwares.Druid.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Buffy.ViewModels
{
	public class AboutViewModel : ViewModel
	{
		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync("https://aka.ms/xamarin-quickstart"));
		}

		public ICommand OpenWebCommand { get; }
	}
}