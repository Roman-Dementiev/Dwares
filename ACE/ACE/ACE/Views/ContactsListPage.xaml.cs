using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsListPage : ContentPage
	{
		ContactsListViewModel viewModel;
		public ContactsListViewModel ViewModel => viewModel;

		public ContactsListPage() : this(ContactType.Company) { }

		public ContactsListPage(int contactType) : this((ContactType)contactType) { }

		public ContactsListPage(ContactType contactType)
		{
			BindingContext = viewModel = new ContactsListViewModel(Navigation, contactType);

			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.UpdateCommands();
		}
	}

}
