using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RunDetailPage : ContentPageEx
	{
		public RunDetailPage()
		{
			InitializeComponent();
		}

		RunDetailViewModel ViewModel => BindingContext as RunDetailViewModel;


		private void OnClientNameSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			if (e.SelectedItem is Contact contact) {
				ViewModel?.OnClientSelected(contact);
			}
		}

		private void OnClientPhoneSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			if (e.SelectedItem is Contact contact) {
				ViewModel?.OnClientSelected(contact);
			}
		}

		private void OnDropoffNameSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			if (e.SelectedItem is Contact contact) {
				ViewModel?.OnDropoffSelected(contact);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			ViewModel?.UpdateAutoSuggestions();
		}
	}
}