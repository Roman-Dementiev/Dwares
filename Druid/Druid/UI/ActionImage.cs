using System;
using Xamarin.Forms;
using Dwares.Druid.Support;
using Dwares.Druid.Essential;


namespace Dwares.Druid.UI
{
	public class ActionImage : Image
	{
		public ActionImage() { }

		//string action;
		//public string Action {
		//	get => action;
		//	set {
		//		if (value != action) {
		//			action = value;
		//			Source = new ActionImageSource(value);

		//		}
		//	}
		//}

		public static readonly BindableProperty ActionProperty =
			BindableProperty.Create(
				nameof(Action),
				typeof(string),
				typeof(ActionImage),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is Image image && newValue is string action)
					{
						image.Source = new ActionImageSource(action);
					}
				});

		public string Action {
			set { SetValue(ActionProperty, value); }
			get { return (string)GetValue(ActionProperty); }
		}

	}
}
