using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.Models;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupDetailPage : ContentPage
	{
		PickupDetailViewModel viewModel;

		public PickupDetailPage(Pickup pickup)
		{
			BindingContext = viewModel = new PickupDetailViewModel(Navigation, pickup);

			InitializeComponent();

			//pickupTime.Time
		}
	}
}
