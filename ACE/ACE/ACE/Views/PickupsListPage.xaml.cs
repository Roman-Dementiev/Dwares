using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PickupsListPage : ContentPage
	{
		//PickupsListViewModel viewModel;

		public PickupsListPage ()
		{
			//BindingContext = viewModel = new PickupsListViewModel();

			InitializeComponent();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			//viewModel.UpdateCommands();
		}

	}
}