using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupsTabPage : ContentPage
	{
		PickupsViewModel viewModel;

		public PickupsTabPage ()
		{
			BindingContext = viewModel = new PickupsViewModel();

			InitializeComponent();
		}
	}
}