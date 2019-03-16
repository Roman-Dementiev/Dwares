using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class TabbedPageEx : TabbedPage
	{
		public TabbedPageEx() { }

		MultiPageViewModel MultiPageViewModel => BindingContext as MultiPageViewModel;

		public static readonly BindableProperty UseNavigationPagesProperty =
			BindableProperty.Create(
				nameof(UseNavigationPages),
				typeof(bool),
				typeof(TabbedPageEx),
				defaultValue: true,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is TabbedPageEx page) {
						page.ResetTabs();
					}
				}
				);

		public bool UseNavigationPages {
			set { SetValue(UseNavigationPagesProperty, value); }
			get { return (bool)GetValue(UseNavigationPagesProperty); }
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			ResetTabs();
		}

		protected void ResetTabs()
		{
			var viewModel = MultiPageViewModel;
			if (viewModel != null) {
				this.ResetPages(viewModel.ViewModelTypes, viewModel.ContentViewModels == true, UseNavigationPages);
			} else {
				Children.Clear();
			}
		}
	}
}
