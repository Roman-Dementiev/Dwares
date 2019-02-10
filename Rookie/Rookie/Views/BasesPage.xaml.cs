using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Dwares.Rookie.ViewModels;

namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BasesPage : ContentPageEx
	{
		public BasesPage()
		{
			BindingContext = new BasesViewModel();
			InitializeComponent();
		}
	}
}