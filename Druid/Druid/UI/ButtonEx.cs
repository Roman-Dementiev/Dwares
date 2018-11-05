using System;
using Xamarin.Forms;
using Dwares.Druid.Support;
using Dwares.Druid.Essential;


namespace Dwares.Druid.UI
{
	public class ButtonEx : Button, ICommandHolder
	{
		public ButtonEx()
		{
			writ = new WritMixin(this);
		}

		WritMixin writ;

		public WritCommand WritCommand {
			get => writ.WritCommand;
			set => writ.WritCommand = value;
		}

		public string Writ {
			get => writ.Writ;
			set => writ.Writ = value;
		}
	}
}
