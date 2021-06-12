using System;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Buffy.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Buffy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ShellPageEx
	{
		public SettingsPage()
		{
			InitializeComponent();
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			if (BindingContext is SettingsViewModel vm) {
				vm.OnDisappearing();
			}
		}
	}
}