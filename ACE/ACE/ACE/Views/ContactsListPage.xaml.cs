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

		//public ContactsListPage() : this(ContactType.Member) { }

		public ContactsListPage(int contactType) : this((ContactType)contactType) { }

		public ContactsListPage(ContactType contactType)
		{
			BindingContext = viewModel = new ContactsListViewModel(this, contactType);

			InitializeComponent();

			if (contactType == ContactType.Client) {
				sortOrder.ItemsSource = new SortOrder[] {
					new SortOrder("By First Name"),
					new SortOrder("By Last Name"),
					new SortOrder("By Phone Number")
				};
			} else {
				sortOrder.ItemsSource = new SortOrder[] {
					new SortOrder("By Name"),
					new SortOrder("By Phone Number")
				};
//				sortOrder.ItemsSource = new string[] { "By Name", "By Phone" };
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.UpdateCommands();
		}

		Frame ShownPanel { get; set; }

		void ShowPanel(Frame panel)
		{
			if (ShownPanel != null) {
				ShownPanel.IsVisible = false;
			}

			ShownPanel = panel;
			
			if (panel != null) {
				panel.IsVisible = true;
			}
		}

		public void ShowFindPanel() => ShowPanel(findPanel);
		public void ShowSortPanel() => ShowPanel(sortPanel);
		public void HidePanel() => ShowPanel(null);
	}
}
