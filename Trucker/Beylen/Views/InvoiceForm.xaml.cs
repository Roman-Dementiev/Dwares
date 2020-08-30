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

		private void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			if (sender != articles || e.CurrentSelection.Count == 0)
				return;

			var selected = e.CurrentSelection[0] as ArticleCardModel;
			if (selected != null && selected.Source == null) {
				articles.SelectedItem = null;
			}
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			InvoiceFormModel.Current = BindingContext as InvoiceFormModel;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			InvoiceFormModel.Current = null;
		}
	}
}
