using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactDetailPage : ContentPage
	{
		ContactDetailViewModel viewModel;

		public ContactDetailPage(Contact contact)
		{
			BindingContext = viewModel = new ContactDetailViewModel(Navigation, contact);

			InitializeComponent();
		}

		public ContactDetailPage(ContactType type)
		{
			BindingContext = viewModel = new ContactDetailViewModel(Navigation, type);

			InitializeComponent();
		}
	}
}