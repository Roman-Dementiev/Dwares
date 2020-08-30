using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using Beylen.ViewModels;


namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class InvoicesPage : ShellPageEx
	{
		public InvoicesPage()
		{
			InitializeComponent();
		}

		async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
		{
			var selected = e.CurrentSelection.FirstOrDefault();
			if (selected is InvoiceCardModel card) {
				var uri = $"invoice?number={card.Number}";
				await Shell.Current.GoToAsync(uri);
			}
		}
	}
}
