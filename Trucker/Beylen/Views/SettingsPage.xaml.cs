using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using Beylen.ViewModels;

namespace Beylen.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsPage : ContentPageEx
	{
		public SettingsPage()
		{
			BindingContext = new SettingsViewModel();

			InitializeComponent();
		}

		private async void ListViewEx_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			var section = e.SelectedItem as SettingsSection;

			if (section?.Action != null) {
				await section.Action(this, section);
				listView.SelectedItem = null;
			}
		}
	}
}
