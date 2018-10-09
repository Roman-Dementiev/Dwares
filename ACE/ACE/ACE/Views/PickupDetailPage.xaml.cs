using System;
using System.Runtime.CompilerServices;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Models;
using ACE.ViewModels;
using Dwares.Dwarf;
using Dwares.Druid.UI;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupDetailPage : ContentPage
	{
		ClassRef @class = new ClassRef(typeof(PickupDetailPage));

		PickupDetailViewModel viewModel;

		public PickupDetailPage(Pickup pickup)
		{
			BindingContext = viewModel = new PickupDetailViewModel(pickup);

			try {
				InitializeComponent();
			} catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				throw;
			}

			//Debug.EnableTracing(@class);

			//clientName.AutoSuggestionSelected += OnAutoSuggestionSelected;
			//officeName.AutoSuggestionSelected += OnAutoSuggestionSelected;
			//clientPhone.AutoSuggestionSelected += OnAutoSuggestionSelected;
			//officePhone.AutoSuggestionSelected += OnAutoSuggestionSelected;
		}

		[System.Diagnostics.Conditional("DEBUG")]
		private void TraceAutoSuggestionSelected(AutoSuggestionSelectedEventArgs e, [CallerMemberName] string method ="")
		{
			Debug.Trace(@class, method, "contact={0}, reason={1}", e.SelectedItem, e.Reason);
		}

		//private void OnAutoSuggestionSelected(object sender, AutoSuggestionSelectedEventArgs e)
		//{
		//	Debug.Print("[{0}] contact={1}, reason={2}", nameof(OnAutoSuggestionSelected), e.SelectedItem, e.Reason);
		//}

		private void OnClientNameSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			TraceAutoSuggestionSelected(e);

			if (e.SelectedItem is Contact contact) {
				viewModel.OnClientSelected(contact);
			}
		}

		private void OnClientPhoneSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			TraceAutoSuggestionSelected(e);

			if (e.SelectedItem is Contact contact) {
				viewModel.OnClientSelected(contact);
			}
		}

		private void OnOfficeNameSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			TraceAutoSuggestionSelected(e);

			if (e.SelectedItem is Contact contact) {
				viewModel.OnOfficeSelected(contact);
			}
		}

		private void OnOfficePhoneSelected(object sender, AutoSuggestionSelectedEventArgs e)
		{
			TraceAutoSuggestionSelected(e);

			if (e.SelectedItem is Contact contact) {
				viewModel.OnOfficeSelected(contact);
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			viewModel.UpdateAutoSuggestions();
		}
	}
}
