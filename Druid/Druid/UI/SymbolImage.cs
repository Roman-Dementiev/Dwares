using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace Dwares.Druid.UI
{
	public class SymbolImage : Image, ICommandHolder
	{
		public SymbolImage()
		{
			writ = new WritMixin(this);
		}

		public SymbolImage(SymbolEx symbol) :
			this()
		{
			Symbol = symbol;
		}

		SymbolEx symbol;
		public SymbolEx Symbol {
			get => symbol;
			set {			writ = new WritMixin(this);
				if (value != symbol) {
					symbol = value;
					Source = value.ImageSource();
				}
			}
		}

		TapGestureRecognizer tapped;
		ICommand command;
		public ICommand Command {
			get => command;
			set {
				if (value == command)
					return;

				if (tapped == null) {
					tapped = new TapGestureRecognizer();
					GestureRecognizers.Add(tapped);
				}
				tapped.Command = command = value;
			}
		}

		WritMixin writ;
		public string Writ {
			get => writ.Writ;
			set => writ.Writ = value;
		}
	}
}
