using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Dwares.Druid.Support;

namespace Dwares.Druid.UI
{
	public class ToolbarItemEx: ToolbarItem, ICommandHolder
	{
		public ToolbarItemEx()
		{
			writ = new WritMixin(this);
		}
		
		public ToolbarItemEx(string name, string icon, Action activated, ToolbarItemOrder order = ToolbarItemOrder.Default, int priority = 0) :
			base(name, icon, activated, order, priority)
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
