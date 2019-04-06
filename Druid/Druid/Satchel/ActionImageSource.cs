using System;
using System.Threading;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	//public abstract class ActionImageSourceBase
	//{
	//	public ActionImageSourceBase(string filenameFormat = null)
	//	{
	//		FilenameFormat = filenameFormat;
	//	}

	//	//public abstract string Icon { get; }
	//	public string FilenameFormat { get; set; }
	//	public abstract string Filename { get; }

	//	FileImageSource imageSource;
	//	public FileImageSource ImageSource => LazyInitializer.EnsureInitialized(ref imageSource,
	//		() => new FileImageSource { File = Filename });

	//	public static implicit operator FileImageSource(ActionImageSourceBase source) => source?.ImageSource;
	//}

	public class ActionImageSource
	{
		//public static readonly OnPlatform<string> DefaultFilenameFormat = new OnPlatform<string> {
		//	Default = "{0}.png",
		//	Android = "ic_action_{0}.png",
		//	iOS = "{0}.png",
		//	UWP = "Images/{0}.png"
		//};

		//protected ActionImageSource() { }

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
					imageSource = GetImageSource(ImageProvider.Instance);
				}
				return imageSource;
			}
			set {
				imageSource = value;
			}
		}

		protected virtual ImageSource GetImageSource(IImageProvider provider)
		{
			return provider.GetImageSource(Group, Name);
		}

		public static implicit operator ImageSource(ActionImageSource source) => source?.ImageSource;

		public static ImageSource ForName(string name, string group = null)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			var source =  new ActionImageSource(group, name);
			return source;
		}

	}
}
