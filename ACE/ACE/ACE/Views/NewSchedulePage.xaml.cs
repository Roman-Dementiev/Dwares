using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class NewSchedulePage : ContentPage
	{
		NewScheduleViewModel viewModel;

		public NewSchedulePage ()
		{
			BindingContext = viewModel = new NewScheduleViewModel();

			InitializeComponent();
		}
	}
}