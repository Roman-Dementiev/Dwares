using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using RouteOptimizer.ViewModels;


namespace RouteOptimizer.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlaceEditPage : ShellPageEx
	{
		public PlaceEditPage()
		{
			InitializeComponent();

			if (BindingContext is PlaceEditViewModel vm) {
				CanGoBack = vm.CanGoBack;
			}
		}
	}
}
