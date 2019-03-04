using System;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public enum PageTopAdjustment
	{
		None,
		Standard,
		Custom
	};

	public class ContentPageEx : ContentPage, IContentHolder, IToolbarHolder
	{
		public ContentPageEx() { }

		public ContentPageEx(BindingScope scope)
		{
			Scope = scope;
		}

		public virtual View ContentView {
			get => GetContentView();
			set {
				if (value != ContentView) {
					OnPropertyChanging();

					if (ToolbarSource != null && ContentView == ToolbarSource) {
						ToolbarSource = null;
					}

					ChangeContentView(value);

					var newToolbarSource = value as IToolbarHolder;
					if (newToolbarSource != null) {
						ToolbarSource = newToolbarSource;
					}

					if (value is ITitleHolder titleHolder) {
						Title = titleHolder.Title;
					} else {
						Title = string.Empty;
					}

					OnPropertyChanged();
				}
			}
		}

		IToolbarHolder toolbarSource;
		public virtual IToolbarHolder ToolbarSource {
			get => toolbarSource;
			set {
				if (value != toolbarSource) {
					OnPropertyChanging();
					ToolbarItems.Clear();
					toolbarSource = value;

					var toolbarItems = toolbarSource?.ToolbarItems;
					if (toolbarItems != null) {
						foreach (var item in toolbarSource.ToolbarItems) {
							ToolbarItems.Add(item);
						}
					}
					OnPropertyChanged();
				}
			}
		}

		public BindingScope Scope {
			get => BindingContext as BindingScope;
			set => BindingContext = value;
		}

		public static readonly BindableProperty TopAdjustmentProperty =
			BindableProperty.Create(
				nameof(TopAdjustment),
				typeof(PageTopAdjustment),
				typeof(ContentPageEx),
				PageTopAdjustment.None,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ContentPageEx page && newValue is PageTopAdjustment adjustment) {
						page.SetTopAdjustment(adjustment);
					}
				});
		
		public PageTopAdjustment TopAdjustment {
			set { SetValue(TopAdjustmentProperty, value); }
			get { return (PageTopAdjustment)GetValue(TopAdjustmentProperty); }
		}

		private void SetTopAdjustment(PageTopAdjustment adjustment)
		{
			switch (adjustment)
			{
			case PageTopAdjustment.None:
				Padding = new Thickness(0);
				break;
			
			case PageTopAdjustment.Standard:
				switch (Device.RuntimePlatform)
				{
				case Device.iOS:
				case Device.Android:
					Padding = new Thickness(0,40,0,0);
					break;
				default:
					Padding = new Thickness(0);
					break;
				}
				break;
			}
		}


		protected override void OnAppearing()
		{
			base.OnAppearing();
			Scope?.UpdateCommands();
		}

		protected virtual View GetContentView() => Content;
		protected virtual void ChangeContentView(View newContentView) => Content = newContentView;
	}
}
