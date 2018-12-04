using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class TabbedPageEx : TabbedPage
	{
		public TabbedPageEx() { }

		public bool UseNavigationPages { get; } = true;

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			this.OnViewModelChanged(BindingContext as MultiPageViewModel, UseNavigationPages);
		}
	}
}
