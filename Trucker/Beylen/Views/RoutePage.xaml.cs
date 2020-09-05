using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;

namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RoutePage : ShellPageEx
	{
		public RoutePage()
		{
			//BindingContext = new RouteViewModel();

			InitializeComponent();

		}
	}
}