using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Rookie.ViewModels;
using Dwares.Druid;


namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LoginPage : ContentPage
	{
		public LoginPage ()
		{
			InitializeComponent();

			//BindingContext = new LoginViewModel();
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();

			var scope = this.GetScope();
			//var scope = BindingContext as LoginViewModel;
			scope?.UpdateCommands();
		}
	}
}