using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;
using Xam.Plugin.TabView;


namespace Dwares.Druid.UI
{
	public class TabViewEx : TabViewControl, ITargeting
	{
		//static ClassRef @class = new ClassRef(typeof(TabViewEx));
		public static string kHeaderBackgroundColor => UIScheme.DruidColorKey(nameof(TabViewEx), nameof(HeaderBackgroundColor));
		public static string kHeaderTabTextColor => UIScheme.DruidColorKey(nameof(TabViewEx), nameof(HeaderTabTextColor));
		public static string kHeaderSelectionUnderlineColor => UIScheme.DruidColorKey(nameof(TabViewEx), nameof(HeaderSelectionUnderlineColor));
		public static string kHeaderSelectionUnderlineThickness => UIScheme.DruidColorKey(nameof(TabViewEx), nameof(HeaderSelectionUnderlineThickness));
		public static string kHeaderSpacing => UIScheme.DruidColorKey(nameof(TabViewEx), nameof(HeaderSpacing));

		public TabViewEx()
		{
			//Debug.EnableTracing(@class);, 

			HeaderBackgroundColor = UIScheme.Color(kHeaderBackgroundColor);
			HeaderTabTextColor = UIScheme.Color(kHeaderTabTextColor);
			HeaderSelectionUnderlineColor = UIScheme.Color(kHeaderSelectionUnderlineColor);
			HeaderSelectionUnderlineThickness = UIScheme.Value<double>(kHeaderSelectionUnderlineThickness, 1.0);
			HeaderSpacing = UIScheme.Value<double>(kHeaderSpacing);
		}

		double headerSpacing = 0;
		public double HeaderSpacing {
			get => headerSpacing;
			set {
				if (value != headerSpacing) {
					headerSpacing = value;
					var mainContainer = Content as StackLayout;
					var headContainer = mainContainer.Children[0];
					headContainer.Margin = new Thickness(0, 0, 0, headerSpacing);
					OnPropertyChanged();
				}
			}
		}

		public Element GetTargetElement()
		{
			return ContentView;
		}

		public TabItem CurrentItem
		{
			get {
				if (ItemSource != null && SelectedTabIndex >= 0 && SelectedTabIndex < ItemSource.Count) {
					return ItemSource[SelectedTabIndex];
				} else {
					return null;
				}
			}
		}

		public View ContentView => CurrentItem?.Content;

		protected override void OnBindingContextChanged()
		{
			Debug.Print("TabViewEx.OnBindingContextChanged(): BindingContext={0}", BindingContext);

			if (BindingContext != null) {
				var savedCtx = new Dictionary<object, object>();
				foreach (var tab in ItemSource) {
					if (tab.Content != null) {
						savedCtx[tab.Content] = tab.Content.BindingContext;
					}
				}

				base.OnBindingContextChanged();

				foreach (var tab in ItemSource) {
					if (tab.Content != null && savedCtx.ContainsKey(tab.Content)) {
						tab.Content.BindingContext = savedCtx[tab.Content];
					}
				}
			}
			else {
				base.OnBindingContextChanged();

			}

		}
	}

	public class TabViewItem : TabItem
	{

	}
}
