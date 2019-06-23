using System;
using System.Threading;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	public class ActionImageSource
	{
		public ActionImageSource(string name, string group = null)
		{
			Group = group;
			Name = name;
		}

		public string Group { get; }
		public string Name { get; }

		ImageSource imageSource;
		public ImageSource ImageSource {
			get {
				if (imageSource == null) {
					imageSource = GetImageSource(ArtProvider.Instance);
				}
				return imageSource;
			}
			set {
				imageSource = value;
			}
		}

		protected virtual ImageSource GetImageSource(IArtProvider provider)
		{
			return provider.GetImageSource(Group, Name);
		}

		public static implicit operator ImageSource(ActionImageSource source) => source?.ImageSource;

		public static ImageSource ForName(string name, string group = null)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			var source =  new ActionImageSource(name, group);
			return source?.ImageSource;
		}

		public static FileImageSource ToobarIconSource(string name)
			=> ForName(name, ArtProvider.kGroupToolbar) as FileImageSource;
	}
}
