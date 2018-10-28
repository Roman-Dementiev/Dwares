using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsListPage : ContentPage
	{
		ContactsListViewModel viewModel;

		//public ContactsListPage() : this(ContactType.Member) { }

		public ContactsListPage(int contactType) : this((ContactType)contactType) { }

		public ContactsListPage(ContactType contactType)
		{
			BindingContext = viewModel = new ContactsListViewModel(this, contactType);

			InitializeComponent();

			//descendigCheck.CheckedChanged += OnDescendingChanged;
		}

		private void OnDescendingChanged(object sender, CheckedChangedEventArgs e)
		{
			viewModel.Descending = e.IsChecked;
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
