using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	public interface IImageProvider
	{
		ImageSource GetImageSource(string group, string name);
	}

	public static class ImageProvider
	{
		public const string kGroupImages = "Images";
		public const string kGroupSymbol = "Symbol";
		public const string kGroupToolbar = "Toolbar";

		static IImageProvider instance;
		public static IImageProvider Instance {
			get => LazyInitializer.EnsureInitialized(ref instance, () => new DefaultImageProvider());
		}

		public static ImageSource GetImageSource(this IImageProvider provider, string group, string name)
		{
			return provider?.GetImageSource(group, name);
		}

		public static ImageSource GetImageSource(string group, string name)
		{
			return Instance.GetImageSource(group, name);
		}

		public static ImageSource GetImageSource(this IImageProvider provider, string name)
		{
			return provider?.GetImageSource(null, name);
		}

		public static ImageSource GetImageSource(string name)
		{
			return Instance.GetImageSource(null, name);
		}
	}

	public class DefaultImageProvider : IImageProvider
	{
		//static ClassRef @class = new ClassRef(typeof(ImageProvider));
		public static string DefaultExtension { get; set; } = "png";
		public static string DefaultGroup { get; set; } = ImageProvider.kGroupImages;

		Dictionary<string, string> prefixes = new Dictionary<string, string>();

		public DefaultImageProvider() : this(true) { }

		public DefaultImageProvider(bool standard)
		{
			//Debug.EnableTracing(@class);

			if (standard) {
				if (Device.RuntimePlatform == Device.UWP) {
					SetPrefix(Device.UWP, ImageProvider.kGroupImages, "Images/");
					SetPrefix(Device.UWP, ImageProvider.kGroupSymbol, "Images/");
					SetPrefix(Device.UWP, ImageProvider.kGroupToolbar, "Images/");
				}
				else if (Device.RuntimePlatform == Device.Android) {
					LowercaseNames = true;
					SetPrefix(Device.Android, ImageProvider.kGroupToolbar, "ic_tb_");
				}
			}

		}

		public bool LowercaseNames { get; set; }

		public void SetPrefix(string platform, string group, string prefix)
		{
			if (platform == null ||  platform == Device.RuntimePlatform)
				SetPrefix(group, prefix);
		}

		public void SetPrefix(string group, string prefix)
		{
			if (string.IsNullOrEmpty(group))
				group = DefaultGroup ?? string.Empty;

			if (string.IsNullOrEmpty(prefix)) {
				prefixes.Remove(group);
			} else {
				prefixes[group] = prefix;
			}
		}

		public ImageSource GetImageSource(string group, string name)
		{
			if (string.IsNullOrEmpty(group))
				group = DefaultGroup;

			var prefix = string.Empty;
			if (!string.IsNullOrEmpty(group) && prefixes.ContainsKey(group)) {
				prefix = prefixes[group];
			}

			if (LowercaseNames)
				name = name.ToLower();

			if (name.IndexOf('.') < 0 && !string.IsNullOrEmpty(DefaultExtension))
				name = name + '.' + DefaultExtension;

			return new FileImageSource { File = prefix + name };
		}

	}
}
