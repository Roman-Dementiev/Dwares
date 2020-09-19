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

	public class ContentPageEx : ContentPage, IThemeAware
	{
		public ContentPageEx()
		{
			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
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

			if (BindingContext is IActivatable activatable) {
				activatable.Activate();
			}
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();

			if (BindingContext is IActivatable activatable) {
				activatable.Deactivate();
			}
		}

		protected virtual View GetContentView() => Content;
		protected virtual void ChangeContentView(View newContentView) => Content = newContentView;

	}
}
