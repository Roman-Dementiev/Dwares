using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Xamarin.Forms;
using Beylen.Views;

namespace Beylen.ViewModels
{
	public class ShellViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ShellViewModel));

		public ShellViewModel()
		{
			//Debug.EnableTracing(@class);

			//Title = "Title";

			AboutCommand = new Command(async () => {
				var page = new AboutPage();
				await Shell.Current.Navigation.PushAsync(page);
				Shell.Current.FlyoutIsPresented = false;
			});
		}
		public Command AboutCommand { get; }
	}
}
