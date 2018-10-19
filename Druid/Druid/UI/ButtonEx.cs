using System;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Druid.Support;
using Xamarin.Forms;


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
