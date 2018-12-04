using System;
using Xamarin.Forms;
using Dwares.Dwarf;


namespace Dwares.Druid.UI
{
	public class CarouselPageEx : CarouselPage
	{
		public CarouselPageEx()
		{
			this.CurrentPageChanged += OnCurrentPageChanged;
		}

		public MultiPageViewModel ViewModel => BindingContext as MultiPageViewModel;

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			this.OnViewModelChanged(BindingContext as MultiPageViewModel);
		}

		private void OnCurrentPageChanged(object sender, EventArgs e)
		{
			Debug.Print("CarouselPageEx.OnCurrentPageChanged(): {0}", CurrentPage);

			if (BindingContext is MultiPageViewModel viewModel) {
				viewModel.PageTitle = CurrentPage?.Title;
			}
		}
	}
}
