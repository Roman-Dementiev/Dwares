using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoutePage : ContentPage
	{
		RouteViewModel viewModel;

		public RoutePage ()
		{
			BindingContext = viewModel = new RouteViewModel();

			InitializeComponent ();
		}
	}
}