using Buffy.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Buffy.Views
{
	public partial class ItemDetailPage : ContentPage
	{
		public ItemDetailPage()
		{
			InitializeComponent();
			BindingContext = new ItemDetailViewModel();
		}
	}
}