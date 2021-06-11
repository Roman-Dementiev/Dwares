using System;
using Xamarin.Forms;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Buffy.ViewModels;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;

namespace Buffy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class FuelingForm : ShellPageEx
	{
		public FuelingForm()
		{
			InitializeComponent();

			ViewModel = BindingContext as FuelingFormModel;
			CanGoBack = ViewModel.CanGoBack;
		}

		FuelingFormModel ViewModel { get; }

		private async void ChooseVendor_Clicked(object sender, EventArgs e)
		{
			if (vendors == null) {
				var list = new List<string>();
				foreach (var vendor in App.Vendors) {
					list.Add(vendor.Name);
				}
				vendors = list.ToArray();
			}

			var result = await DisplayActionSheet("Choose Vendor", "Cancel", null, vendors);
			if (!string.IsNullOrEmpty(result) && result != "Cancel") {
				ViewModel.Vendor = result;
			}
		}

		static string[] vendors = null;
	}
}
