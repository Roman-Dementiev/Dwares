using System;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using Dwares.Druid.Painting;

namespace Dwares.Druid.UI
{
	public class ToolbarItemEx: ToolbarItem
	{
		public ToolbarItemEx()
		{
			UITheme.OnCurrentThemeChanged(() => UpdateIcon(IconArt));
		}

		public ToolbarItemEx(string name, string icon, Action activated, ToolbarItemOrder order = ToolbarItemOrder.Default, int priority = 0) :
			base(name, icon, activated, order, priority)
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

		public string IconArt { 
			get => iconArt;
			set {
				if (value != iconArt) {
					iconArt = value;
					UpdateIcon(value);
				}
			}
		}
		string iconArt;

		protected virtual void UpdateIcon(string art)
		{
			if (!string.IsNullOrEmpty(art)) {
				var imageSource = UITheme.Current?.GetImageSource(art);
				if (imageSource == null)
					imageSource = ArtBroker.Instance.GetImageSource(iconArt);

				IconImageSource = imageSource;
			} else {
				IconImageSource = null;
			}
		}
	}
}
