using Dwares.Druid;
using Dwares.Druid.UI;
using System;
using System.Threading.Tasks;
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
		public Command GoBackCommand => ShellEx.GoToMainCommand;
	}
}