using System;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Druid.Forms;
using Dwares.Druid;


namespace ACE.ViewModels
{
	public class AboutViewModel : FormScope
	{
		public AboutViewModel() :
			base(null)
		{
			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
			DismissCommand = new Command(async () => await Navigator.PopPage());
		}

		public ICommand OpenWebCommand { get; }
		public ICommand DismissCommand { get; }
	}
}
