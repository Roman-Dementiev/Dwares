using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;


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

		SymbolEx symbol;
		public SymbolEx Symbol {
			get => symbol;
			set {
				if (value != symbol) {
					symbol = value;
					Icon = value.ImageSource() as FileImageSource;
				}
			}
		}

		//string uid;
		//public string Uid {
		//	get => uid;
		//	set {
		//		if (value != uid) {
		//			bool changeText, changeWrit, changeSymbol;
		//			if (String.IsNullOrEmpty(uid)) {
		//				changeText = String.IsNullOrEmpty(Text);
		//				changeWrit = String.IsNullOrEmpty(Writ);
		//				changeSymbol = Symbol == SymbolEx.None;
		//			} else {
		//				changeText = Text == uid;
		//				changeWrit = Writ == uid;
		//				changeSymbol = Symbol.Name() == uid;
		//			}

		//			uid = value;

		//			if (changeText) {
		//				Text = uid;
		//			}
		//			if (changeWrit) {
		//				Writ = uid;
		//			}
		//			if (changeSymbol /*&& Order != ToolbarItemOrder.Secondary*/) {
		//				Symbol = Symbols.GetSymbolByName(uid);
		//			}
		//		}
		//	}
		//}
	}
}
