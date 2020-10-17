using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Ziply.ViewModels;


namespace Ziply.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EtaPage : ContentPageEx
	{
		public EtaPage()
		{
			InitializeComponent();

			ViewModel = new EtaViewModel();
			BindingContext = ViewModel;
		}

		EtaViewModel ViewModel { get; }

		private async void recipient_DoubleTapped(object sender, EventArgs e)
		{
			await ViewModel.ClearRecipient(sender == recipient2);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.Activate();
		}

		private void DestinationChanged(object sender, TextChangedEventArgs e)
		{
			ViewModel.OnDestinationChanged();
		}
	}
}
