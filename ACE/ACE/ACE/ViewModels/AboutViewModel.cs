using System;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace ACE.ViewModels
{
	public class AboutViewModel : BindingScope
	{
		public AboutViewModel()
		{
			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
		}

		public ICommand OpenWebCommand { get; }
	}
}