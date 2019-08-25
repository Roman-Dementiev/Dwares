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
			Margin = new Thickness(2,2,6,2);

			DefaultFlavor = "NavButton-default";
			SelectedFlavor = "NavButton-active";

			MessageBroker.Subscribe<ActiveContentMessage>(this, (message) => {
				IsSelected = message.ActiveContent != null && message.ActiveContent == ContentType;
			});
		}

		public RootContentType ContentType {  get; set; }

		public static readonly BindableProperty SideModeProperty =
			BindableProperty.Create(
				nameof(SideMode),
				typeof(bool),
				typeof(NavButton),
				defaultValue: true,
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is NavButton button && newValue is bool value) {
						button.Orientation = value ? StackOrientation.Horizontal : StackOrientation.Vertical;
						button.LabelIsVisible = !value;
					}
				});

		public bool SideMode {
			set { SetValue(SideModeProperty, value); }
			get { return (bool)GetValue(SideModeProperty); }
		}

	}
}
