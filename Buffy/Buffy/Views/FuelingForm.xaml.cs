using System;
using Xamarin.Forms;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Buffy.ViewModels;
using Xamarin.Forms.Xaml;


namespace Buffy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FuelingForm : ShellPageEx
	{
		public FuelingForm()
		{
			InitializeComponent();

			if (BindingContext is FuelingFormModel vm) {
				CanGoBack = vm.CanGoBack;
			}
		}
	}
}
