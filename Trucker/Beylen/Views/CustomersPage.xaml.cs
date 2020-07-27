using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;


namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CustomersPage : ContentPageEx
	{
		public CustomersPage()
		{
			BindingContext = new CustomersViewModel();

			InitializeComponent();
		}
	}
}