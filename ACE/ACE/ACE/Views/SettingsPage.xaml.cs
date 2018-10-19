using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPage
	{
		SettingsViewModel viewModel;

		public SettingsPage ()
		{
			BindingContext = viewModel = new SettingsViewModel();

			InitializeComponent ();
		}
	}
}