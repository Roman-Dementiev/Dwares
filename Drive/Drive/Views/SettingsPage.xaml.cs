using Dwares.Druid.UI;
using Dwares.Dwarf;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Drive.ViewModels;


namespace Drive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ShellPageEx
	{
		public SettingsPage()
		{
			InitializeComponent();

			if (BindingContext is SettingsViewModel vm) {
				CanGoBack = vm.CanGoBack;
			}
		}

		//protected override void OnDisappearing()
		//{
		//	base.OnDisappearing();

		//	if (BindingContext is SettingsViewModel vm) {
		//		vm.OnDisappearing();
		//	}
		//}

		//	protected override bool OnBackButtonPressed()
		//	{
		//		return false;
		//	}
	}
}