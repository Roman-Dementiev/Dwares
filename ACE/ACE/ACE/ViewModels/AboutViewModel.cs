using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace ACE.ViewModels
{
	public class AboutViewModel : BaseViewModel
	{
		public AboutViewModel() :
			base(null)
		{
			OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
		}

		public ICommand OpenWebCommand { get; }
	}
}