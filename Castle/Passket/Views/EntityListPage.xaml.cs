using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Passket.Models;
using Passket.ViewModels;


namespace Passket.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EntityListPage : ContentPageEx
	{
		public EntityListPage()
		{
			InitializeComponent();
		}

		private void ListView_ItemTapped(object sender, ItemTappedEventArgs e)
		{
			var tapped = e.Item as IEntity;
			var selected = listView.SelectedItem as IEntity;

			Debug.Print("EntityListPage.ListView_ItemTapped(): tapped={0} selected={1}", tapped?.Name, selected?.Name);
		}

		bool skipNullItemSelected = false;

		private void ListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (skipNullItemSelected) {
				skipNullItemSelected = false;
				if (e.SelectedItem == null)
					return;
			}

			if (BindingContext is EntityListViewModel viewModel) {
				viewModel.OnItemSelected(e.SelectedItem);
				skipNullItemSelected = true;
				listView.SelectedItem = null;
			}
		}
	}
}