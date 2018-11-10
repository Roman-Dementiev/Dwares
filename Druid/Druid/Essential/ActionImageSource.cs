using System;
using System.Threading;
using Xamarin.Forms;


namespace Dwares.Druid.Essential
{
	public abstract class ActionImageSourceBase
	{
		public ActionImageSourceBase(string filenameFormat = null)
		{
			FilenameFormat = filenameFormat;
		}

		//public abstract string Icon { get; }
		public string FilenameFormat { get; set; }
		public abstract string Filename { get; }

		FileImageSource imageSource;
		public FileImageSource ImageSource => LazyInitializer.EnsureInitialized(ref imageSource,
			() => new FileImageSource { File = Filename });

		public static implicit operator FileImageSource(ActionImageSourceBase source) => source?.ImageSource;
	}

	public class ActionImageSource : ActionImageSourceBase
	{
		public static readonly OnPlatform<string> DefaultFilenameFormat = new OnPlatform<string> {
			Default = "{0}.png",
			Android = "ic_action_{0}.png",
			iOS = "{0}.png",
			UWP = "Images/{0}.png"
		};

		//public ActionImageSource() { }

		//public ActionImageSource(string icon) : this(icon, null) { }

		public ActionImageSource(string icon, string filenameFormat = null) :
			base(filenameFormat)
		{
			Icon = icon;
		}

		public string Icon { get; }

		public override string Filename {
			get {
				var format = FilenameFormat ?? DefaultFilenameFormat;
				return String.Format(format, Icon);
			}
		}

		public static ActionImageSource ForIcon(string icon, string filenameFormat = null)
		{
			if (String.IsNullOrEmpty(icon))
				return null;
			return new ActionImageSource(icon, filenameFormat);
		}
	}
}
