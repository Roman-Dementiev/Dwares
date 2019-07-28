using System;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
using Xamarin.Forms;
using Drive.ViewModels;


namespace Drive.Views
{
	public class NavButton : ArtButtonEx
	{
		//static ClassRef @class = new ClassRef(typeof(NavButton));

		public NavButton()
		{
			//Debug.EnableTracing(@class);

			HorizontalOptions = VerticalOptions = LayoutOptions.FillAndExpand;

			DefaultFlavor = "NavButton-default";
			SelectedFlavor = "NavButton-active";

			MessageBroker.Subscribe<ActiveContentMessage>(this, (message) => {
				IsSelected = message.ActiveContent != null && message.ActiveContent == ContentType;
			});
		}

		public RootContentType ContentType {  get; set; }
	}
}
