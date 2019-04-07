using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Dwares.Rookie.ViewModels;


namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EarningsQuickView : ContentView
	{
		public EarningsQuickView ()
		{
			InitializeComponent ();
			BindingContext = new ViewModels.EarningsQuickViewModel();
		}

		async void OnTapped(object sender, EventArgs args)
		{
			//if (IsEnabled) {
			//	Tapped?.Invoke(sender, args);
			//}

			var page = App.CreateForm<EarninsDetailViewModel>();
			await Navigator.PushPage(page);
		}
	}

}
