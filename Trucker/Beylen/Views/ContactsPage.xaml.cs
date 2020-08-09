using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using Beylen.ViewModels;


namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ContactsPage : ShellPageEx
	{
		public ContactsPage()
		{
			BindingContext = new ContactsViewModel();

			InitializeComponent();
		}
	}
}
