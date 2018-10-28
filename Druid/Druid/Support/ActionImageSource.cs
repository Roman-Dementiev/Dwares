using System;
using System.Threading;
using Xamarin.Forms;


namespace Dwares.Druid.Support
{
	public abstract class ActionImageSourceBase
	{
		public ActionImageSourceBase(string filenameFormat)
		{
			FilenameFormat = filenameFormat;
		}

		public abstract string Action { get; }
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

		//public ActionImageSource() : base(null) { }

		public ActionImageSource(string action) : 
			base(DefaultFilenameFormat)
		{
			Action = action;
		}

		//public ActionImageSource(string action, string filenameFormat) :
		//	base(filenameFormat)
		//{
		//	Action = action;
		//}

		public override string Action { get; }

		public override string Filename {
			get => String.Format(FilenameFormat, Action);
		}

		//FileImageSource imageSource;
		//public FileImageSource ImageSource => LazyInitializer.EnsureInitialized(ref imageSource,
		//	() => new FileImageSource { File = Filename });

		//public static implicit operator FileImageSource(ActionImageSource source) => source?.ImageSource;
	}
}
