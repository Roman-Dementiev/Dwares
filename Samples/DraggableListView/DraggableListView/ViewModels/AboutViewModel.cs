using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Dwares.Druid;


namespace DraggableListView.ViewModels
{
	public class AboutViewModel : ViewModel
	{
		//const string url = "https://github.com/peterfournier/Xamarin.Forms.DragNDrop";
		const string url = "https://github.com/peterfournier";

		public AboutViewModel()
		{
			Title = "About";
			OpenWebCommand = new Command(async () => await Browser.OpenAsync(url));
		}

		public Command OpenWebCommand { get; }
	}
}