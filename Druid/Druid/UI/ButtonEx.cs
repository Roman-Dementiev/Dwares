using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.UI
{
	public class ButtonEx : Button, ICommandHolder
	{
		WritMixin wmix;

		public ButtonEx()
		{
			wmix = new WritMixin(this);
		}

		public WritCommand WritCommand {
			get => wmix.WritCommand;
			set => wmix.WritCommand = value;
		}

		public string Writ {
			get => wmix.Writ;
			set => wmix.Writ = value;
		}
	}
}
