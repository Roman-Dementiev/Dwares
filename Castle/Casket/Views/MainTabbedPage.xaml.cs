using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf.Runtime;
using Casket.ViewModels;

namespace Casket.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainTabbedPage : TabbedPage
	{
		public MainTabbedPage(MainPageViewModel viewModel)
		{
			//ViewModel = viewModel;
			this.BindingContext = viewModel;

			InitializeComponent();

			var viewModels = viewModel.ViewModels;
			foreach (var vm in viewModels) {
				var page = vm.CreatePage();
				var navigationPage = new NavigationPage(page) {
					Title = page.Title
				};
				Children.Add(navigationPage);
			}
		}

		//MainPageViewModel ViewModel { get; }
	}
}