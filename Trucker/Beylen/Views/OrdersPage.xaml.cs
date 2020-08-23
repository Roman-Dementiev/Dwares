using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using System.Linq;
using Beylen.ViewModels;

namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrdersPage : ShellPageEx
	{
		public OrdersPage()
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