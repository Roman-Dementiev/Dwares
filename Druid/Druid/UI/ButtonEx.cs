using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class ButtonEx : Button
	{
		public ButtonEx()
		{
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
