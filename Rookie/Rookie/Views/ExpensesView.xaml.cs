using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Forms;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Dwares.Rookie.ViewModels;


namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ExpensesView : FormView, ITargeting
	{
		public ExpensesView()
		{
			InitializeComponent();
		}

		public Element GetTargetElement() => tabView;

		//protected override void OnBindingContextChanged()
		//{
		//	base.OnBindingContextChanged();

		//	if (BindingContext is ExpensesViewModel viewModel) {
		//		viewModel.Tabs = tabControl.ItemSource;
		//	}
		//}
	}
}

