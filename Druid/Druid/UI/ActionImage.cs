using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using System.Windows.Input;

namespace Dwares.Druid.UI
{
	public class ActionImage : Image, ICommandHolder
	{
		TapGestureRecognizer tapRecognizer;
		WritMixin wmix;

		public ActionImage()
		{
			tapRecognizer = new TapGestureRecognizer();
			this.GestureRecognizers.Add(tapRecognizer);
			wmix = new WritMixin(this);
		}

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

		public static readonly BindableProperty IconProperty =
			BindableProperty.Create(
				nameof(Icon),
				typeof(string),
				typeof(ActionImage),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ActionImage image && newValue is string name) {
						image.Source = ActionImageSource.ForName(name);
					}
				});

		public string Icon {
			set { SetValue(IconProperty, value); }
			get { return (string)GetValue(IconProperty); }
		}

		public static readonly BindableProperty CommandProperty =
			BindableProperty.Create(
				nameof(Command),
				typeof(ICommand),
				typeof(ActionImage),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ActionImage image) {
						image.tapRecognizer.Command = newValue as ICommand;
					}
				});

		public ICommand Command {
			set { SetValue(CommandProperty, value); }
			get { return (ICommand)GetValue(CommandProperty); }
		}

		public string Writ {
			get => wmix.Writ;
			set => wmix.Writ = value;
		}
	}
}
