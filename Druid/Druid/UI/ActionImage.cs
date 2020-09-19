using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using System.Windows.Input;
using Dwares.Druid.Painting;

namespace Dwares.Druid.UI
{
	public class ActionImage : Image
	{
		TapGestureRecognizer tapRecognizer;

		public ActionImage()
		{
			tapRecognizer = new TapGestureRecognizer();
			this.GestureRecognizers.Add(tapRecognizer);
		}

		public static readonly BindableProperty IconProperty =
			BindableProperty.Create(
				nameof(Icon),
				typeof(string),
				typeof(ActionImage),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ActionImage image && newValue is string name) {
						image.Source = ArtBroker.Instance.GetImageSource(name);
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
			get => writ;
			set {
				if (value != writ) {
					OnPropertyChanging();
					writ = value;
					Command = new WritCommand(writ);
					OnPropertyChanged();
				}
			}
		}
		string writ;
	}
}
