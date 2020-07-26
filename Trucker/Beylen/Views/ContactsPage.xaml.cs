using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;

namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsPage : ContentPageEx
	{
		public ContactsPage()
		{
			BindingContext = new ContactsViewModel();

			InitializeComponent();
		}

		private void Image_Focused(object sender, FocusEventArgs e)
		{

		}
	}
}