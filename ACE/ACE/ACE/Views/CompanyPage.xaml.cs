using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class CompanyPage : ContentPage
	{
		ContactsViewModel viewModel;

		public CompanyPage ()
		{
			BindingContext = viewModel = new ContactsViewModel(Navigation, ContactType.Company);

			InitializeComponent();
		}

	}
}