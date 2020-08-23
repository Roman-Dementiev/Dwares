using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;
using System.Threading.Tasks;

namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InvoiceForm : ShellPageEx
	{
		public InvoiceForm()
		{
			InitializeComponent();

			if (BindingContext is InvoiceFormModel vm) {
				CanGoBack = vm.CanGoBack;
			}
		}

	}
}
