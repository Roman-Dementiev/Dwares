using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		public SettingsPage ()
		{
			BindingContext = new SettingsViewModel();

			InitializeComponent ();
		}
	}
}