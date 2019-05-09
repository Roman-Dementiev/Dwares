using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.UI;
using Drive.ViewModels;


namespace Drive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class HomePage : ContentPageEx
	{
		public HomePage()
		{
			InitializeComponent();
		}

		private async void Button_Clicked(object sender, EventArgs e)
		{
			//var page = Forge.CreatePage(typeof(ScheduleViewModel));
			var page = AppScope.CreatePage(typeof(ScheduleViewModel));
			await Navigator.PushPage(page);
		}
	}
}