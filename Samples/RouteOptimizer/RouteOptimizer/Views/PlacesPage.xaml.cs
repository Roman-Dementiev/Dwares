using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid.UI;
using RouteOptimizer.ViewModels;


namespace RouteOptimizer.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PlacesPage : ShellPageEx
	{
		public PlacesPage()
		{
			InitializeComponent();

			if (listView.ItemsSource is ObservableCollection<PlaceCardModel> cards) {
				cards.CollectionChanged += Cards_CollectionChanged;
			}
		}

		private void Cards_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (e.Action == NotifyCollectionChangedAction.Add && e.NewItems.Count > 0) {
				var card = e.NewItems[0] as PlaceCardModel;
				if (card?.IsEditing == true) {
					listView.ScrollTo(card, ScrollToPosition.MakeVisible, false);
				}
			}
		}
	}
}
