using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Ziply.ViewModels;


namespace Ziply.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ZipPage : ShellPageEx
	{
		public ZipPage()
		{
			InitializeComponent();

			ViewModel = new ZipViewModel();
			BindingContext = ViewModel;
		}

		ZipViewModel ViewModel { get; }

		private async void recipient_DoubleTapped(object sender, EventArgs e)
		{
			await ViewModel.ClearRecipient(sender == recipient2);
		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await ViewModel.Activate();
		}
	}
}
