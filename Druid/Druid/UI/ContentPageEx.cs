using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public enum PageTopAdjustment
	{
		None,
		Standard,
		Custom
	};

	public class ContentPageEx : ContentPage, IContentHolder, IToolbarHolder, ITargeting, IThemeAware
	{
		public ContentPageEx()
		{
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
		}

		public ContentPageEx(BindingScope scope) : this()
		{
			Scope = scope;
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ContentPageEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ContentPageEx page) {
						page.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		public virtual View ContentView
		{
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
					toolbarSource = value;

					var toolbarItems = toolbarSource?.ToolbarItems;
					if (toolbarItems != null) {
						this.SetToolbarItems(toolbarItems);
					}
					OnPropertyChanged();
				}
			}
		}

		public BindingScope Scope {
			get => BindingContext as BindingScope;
			set => BindingContext = value;
		}

		public Element GetTargetElement()
		{
			return ContentView;
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
