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
	public partial class GasForm : FormView
{
		public GasForm()
		{
			InitializeComponent();

			BindingContext = new GasViewModel();
		}
	}
}