using Beylen.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ArticleCard : ContentView
	{
		public ArticleCard()
		{
			InitializeComponent();
		}


		private void radioPacking_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{

			if (e.Value) {
				var viewModel = BindingContext as ArticleCardModel;
				viewModel.IsPackage = true;
			}
		}

		private void radioCounts_CheckedChanged(object sender, CheckedChangedEventArgs e)
		{
			if (e.Value) {
				var viewModel = BindingContext as ArticleCardModel;
				viewModel.IsCounts = true;
			}
		}

		private void Produce_QuerySubmitted(object sender, dotMorten.Xamarin.Forms.AutoSuggestBoxQuerySubmittedEventArgs e)
		{
			if (e.ChosenSuggestion != null) {
				quantity.Focus();
			}
		}
	}
}