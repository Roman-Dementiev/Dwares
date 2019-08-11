using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using Dwares.Druid.Painting;

namespace Dwares.Druid.UI
{
	public class ToolbarItemEx: ToolbarItem, ICommandHolder
	{
		public ToolbarItemEx()
		{
			writ = new WritMixin(this);
			UITheme.CurrentThemeChanged += (s, e) => UpdateIcon(IconArt);
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

		string iconArt;
		public string IconArt { 
			get => iconArt;
			set {
				if (value != iconArt) {
					iconArt = value;
					UpdateIcon(value);
				}
			}
		}

		protected virtual void UpdateIcon(string art)
		{
			if (!string.IsNullOrEmpty(art)) {
				var imageSource = UITheme.Current?.GetImageSource(art);
				if (imageSource == null)
					imageSource = ArtBroker.Instance.GetImageSource(iconArt);
				Icon = imageSource as FileImageSource;
			} else {
				Icon = null;
			}
		}
	}
}
