using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Druid.Forms;
using Dwares.Dwarf;
using Dwares.Rookie.ViewModels;


namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LeaseForm : FormView
{
		public LeaseForm()
		{
			InitializeComponent();

			BindingContext = new LeaseViewModel();
		}

		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();

			Debug.Print("LeaseForm.OnBindingContextChanged(): BindingContext={0}", BindingContext);
		}
	}
}