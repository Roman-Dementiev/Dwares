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

	public class ContentPageEx : ContentPage, IContentHolder
	{
		public ContentPageEx() { }

		public ContentPageEx(BindingScope scope)
		{
			Scope = scope;
		}

		public virtual View ContentView {
			get => Content;
			set => Content = value;
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
	}
}
