using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Drive.ViewModels;


namespace Drive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class RootView : ContentViewEx
	{
		public RootView()
		{
			InitializeComponent();

			ApplyTheme();
			UITheme.CurrentThemeChanged += (s,e) => ApplyTheme();
		}

		private void ApplyTheme()
		{
			this.ApplyFlavor("RootView");
		}

		//public RootViewModel ViewModel { 
		//	get => BindingContext as RootViewModel;
		//}
	}
}
