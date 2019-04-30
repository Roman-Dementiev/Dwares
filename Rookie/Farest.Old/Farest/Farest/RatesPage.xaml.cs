using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Farest
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RatesPage : ContentPage
	{
		public RatesPage(MainViewModel mainViewModel, bool modal)
		{
			InitializeComponent();

			Modal = modal;
			BindingContext = ViewModel = new RatesViewModel(mainViewModel);
		}

		bool Modal { get; }
		RatesViewModel ViewModel { get; }

		private async void OkButton_Clicked(object sender, EventArgs e)
		{
			ViewModel.OnAccept();
			await Dismiss();
		}

		private async void CancelButton_Clicked(object sender, EventArgs e)
		{
			await Dismiss();
		}

		public Task Dismiss()
		{
			if (Modal) {
				return Navigation.PopModalAsync();
			} else {
				return Navigation.PopAsync();
			}
		}

		public static Task Show(MainViewModel mainViewModel, bool modal = false)
		{
			var page = new RatesPage(mainViewModel, modal);
			if (modal) {
				return Application.Current.MainPage.Navigation.PushModalAsync(page);
			} else {
				return Application.Current.MainPage.Navigation.PushAsync(page);
			}
		}
	}
}