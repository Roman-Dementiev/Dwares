//using System;
//using System.Collections.ObjectModel;
//using System.Collections.Specialized;
//using Xamarin.Forms;
//using CarouselView.FormsPlugin.Abstractions;
//using Dwares.Dwarf;


//namespace Dwares.Druid.UI
//{
//	public class TabbedView : ContentView
//	{
//		//static ClassRef @class = new ClassRef(typeof(PivotView));

//		public TabbedView()
//		{
//			Tabs = new TabbedViewTabs();
//			Tabs.CollectionChanged +=Tabs_CollectionChanged;

//			HeaderContainer = new Grid {
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				VerticalOptions = LayoutOptions.Start,
//				//BackgroundColor = HeaderBackgroundColor,
//				MinimumHeightRequest = 50
//			};

//			CarouselView = new CarouselViewControl {
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				VerticalOptions = LayoutOptions.FillAndExpand,
//				//HeightRequest = ContentHeight,
//				ShowArrows = false,
//				ShowIndicators = false,
//				BindingContext = this
//			};

//			MainContainer = new Grid {
//				HorizontalOptions = LayoutOptions.FillAndExpand,
//				VerticalOptions = LayoutOptions.FillAndExpand,
//				RowDefinitions = new RowDefinitionCollection {
//					new RowDefinition { Height = GridLength.Auto },
//					new RowDefinition { Height = GridLength.Star }
//				},
//				ColumnDefinitions = new ColumnDefinitionCollection {
//					new ColumnDefinition { Width = GridLength.Star }
//				}
//			};
//			MainContainer.Children.Add(HeaderContainer, 0, 0);
//			MainContainer.Children.Add(HeaderContainer, 0, 1);

//			Content = MainContainer;

//			//Debug.EnableTracing(@class);
//		}

//		private void Tabs_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//		{
//			switch (e.Action)
//			{
//			case NotifyCollectionChangedAction.Add:
//				if (e.NewStartingIndex == Tabs.Count) {
//					foreach (var item in e.NewItems) {
//						if (item is TabbedViewItem newTab) {
//							AddTabToView(newTab);
//						}
//					}
//				} else {
//					ResetTabsInView();
//				}
//				break;
			
//			case NotifyCollectionChangedAction.Move:
//			case NotifyCollectionChangedAction.Remove:
//			case NotifyCollectionChangedAction.Replace:
//			case NotifyCollectionChangedAction.Reset:
//				ResetTabsInView();
//				break;
//			}
//		}

//		private void ResetTabsInView()
//		{
//			// TODO
//		}

//		private void AddTabToView(TabbedViewItem tab)
//		{
//			//if (tab.TabText == null)
//			//	return;

//			Debug.Print("Adding tab: {0} - {1}", tab, tab.TabText);

//			//var tabSize = (TabSizeOption.IsAbsolute && TabSizeOption.Value.Equals(0)) ? new GridLength(1, GridUnitType.Star) : TabSizeOption;
//			var tabSize = tab.TabWidth;

//			//HeaderContainer.ColumnDefinitions.Add(new ColumnDefinition { Width = tabSize });

//			//tab.IsCurrent = _headerContainerGrid.ColumnDefinitions.Count - 1 == SelectedTabIndex;

//			//var headerIcon = new Image {
//			//	Margin = new Thickness(0, 10, 0, 0),
//			//	HorizontalOptions = LayoutOptions.CenterAndExpand,
//			//	VerticalOptions = LayoutOptions.Center,
//			//	WidthRequest = tab.HeaderIconSize,
//			//	HeightRequest = tab.HeaderIconSize,
//			//	BindingContext = tab
//			//};
//			//headerIcon.SetBinding(Image.SourceProperty, nameof(TabbedViewItem.HeaderIcon));
//			//headerIcon.SetBinding(IsVisibleProperty, nameof(TabbedViewItem.HeaderIcon), converter: new NullToBoolConverter());

//			//var headerLabel = new Label {
//			//	Margin = new Thickness(5, headerIcon.IsVisible ? 0 : 10, 5, 0),
//			//	VerticalTextAlignment = TextAlignment.Start,
//			//	HorizontalTextAlignment = TextAlignment.Center,
//			//	HorizontalOptions = LayoutOptions.CenterAndExpand,
//			//	VerticalOptions = LayoutOptions.Center,
//			//	BindingContext = tab
//			//};
//			//headerLabel.SetBinding(Label.TextProperty, nameof(TabbedViewItem.HeaderText));
//			//headerLabel.SetBinding(Label.TextColorProperty, nameof(TabbedViewItem.HeaderTextColor));
//			//headerLabel.SetBinding(Label.FontSizeProperty, nameof(TabbedViewItem.HeaderTabTextFontSize));
//			//headerLabel.SetBinding(Label.FontFamilyProperty, nameof(TabbedViewItem.HeaderTabTextFontFamily));
//			//headerLabel.SetBinding(Label.FontAttributesProperty, nameof(TabbedViewItem.HeaderTabTextFontAttributes));
//			//headerLabel.SetBinding(IsVisibleProperty, nameof(TabbedViewItem.HeaderText), converter: new NullToBoolConverter());

//			//var selectionBarBoxView = new BoxView {
//			//	VerticalOptions = LayoutOptions.EndAndExpand,
//			//	HeightRequest = HeaderSelectionUnderlineThickness,
//			//	WidthRequest = HeaderSelectionUnderlineWidth
//			//	BindingContext = tab,
//			//};
//			//selectionBarBoxView.SetBinding(IsVisibleProperty, nameof(TabbedViewItem.IsCurrent));
//			//selectionBarBoxView.SetBinding(BoxView.ColorProperty, nameof(TabbedViewItem.HeaderSelectionUnderlineColor));
//			//selectionBarBoxView.SetBinding(WidthRequestProperty, nameof(TabbedViewItem.HeaderSelectionUnderlineWidth));
//			//selectionBarBoxView.SetBinding(HeightRequestProperty, nameof(TabbedViewItem.HeaderSelectionUnderlineThickness));
//			//selectionBarBoxView.SetBinding(HorizontalOptionsProperty,
//			//							   nameof(TabItem.HeaderSelectionUnderlineWidthProperty),
//			//							   converter: new DoubleToLayoutOptionsConverter());

//			//selectionBarBoxView.PropertyChanged += (object sender, PropertyChangedEventArgs e) => {
//			//	if (e.PropertyName == nameof(TabItem.IsCurrent)) {
//			//		SetPosition(ItemSource.IndexOf((TabItem)((BoxView)sender).BindingContext));
//			//	}
//			//	if (e.PropertyName == nameof(WidthRequest)) {
//			//		selectionBarBoxView.HorizontalOptions = tab.HeaderSelectionUnderlineWidth > 0 ? LayoutOptions.CenterAndExpand : LayoutOptions.FillAndExpand;
//			//	}
//			//};

//			//var headerItemSL = new StackLayout {
//			//	HorizontalOptions = LayoutOptions.Fill,
//			//	VerticalOptions = LayoutOptions.FillAndExpand,
//			//	Children = { headerIcon, headerLabel, selectionBarBoxView }
//			//};
//			//var tapRecognizer = new TapGestureRecognizer();
//			//tapRecognizer.Tapped += (object s, EventArgs e) => {
//			//	_supressCarouselViewPositionChangedEvent = true;
//			//	var capturedIndex = _headerContainerGrid.Children.IndexOf((View)s);
//			//	SetPosition(capturedIndex);
//			//	_supressCarouselViewPositionChangedEvent = false;
//			//};
//			//headerItemSL.GestureRecognizers.Add(tapRecognizer);
//			//_headerContainerGrid.Children.Add(headerItemSL, _headerContainerGrid.ColumnDefinitions.Count - 1, 0);
//			//_carouselView.ItemsSource = ItemSource.Select(t => t.Content);
//		}

//		public static readonly BindableProperty TabsProperty =
//			BindableProperty.Create(
//				nameof(Tabs),
//				typeof(TabbedViewTabs),
//				typeof(TabbedView)
//				);

//		public TabbedViewTabs Tabs {
//			set { SetValue(TabsProperty, value); }
//			get { return (TabbedViewTabs)GetValue(TabsProperty); }
//		}

//		Grid MainContainer { get; }
//		Grid HeaderContainer { get; }
//		CarouselViewControl CarouselView { get; }
//	}


//	[ContentProperty("Content")]
//	public class TabbedViewItem
//	{
//		public GridLength TabWidth { get; set; } = GridLength.Star;
//		public string TabText { get; set; }
//		public ImageSource TabIcon { get; set; }
//		public View Content { get; set; }
//		public object ViewModel { get; set; }

//		public View GetContentView()
//		{
//			if (Content == null && ViewModel != null) {
//				Content = Forge.CreateView(ViewModel);
//			}
//			return Content;
//		}
//	}

//	public class TabbedViewTabs : ObservableCollection<TabbedViewItem>
//	{
//		public TabbedViewTabs() { }
//	}
//}
