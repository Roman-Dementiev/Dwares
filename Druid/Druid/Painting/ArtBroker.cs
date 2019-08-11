using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Dwarf;
//using Dwares.Druid.Satchel;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;


namespace Dwares.Druid.Painting
{
	public class ArtBroker : IArtProvider //, IImageProvider
	{
		//static ClassRef @class = new ClassRef(typeof(ArtBroker));

		static Dictionary<string, IArtProvider> providers = new Dictionary<string, IArtProvider>();

		public ArtBroker()
		{
			//Debug.EnableTracing(@class);
		}

		static ArtBroker instance;
		public static ArtBroker Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
			set => instance = value;
		}

		IArtProvider defaultProvider;
		public IArtProvider DefaultProvider {
			get => LazyInitializer.EnsureInitialized(ref defaultProvider, () => new ArtResources());
			set => defaultProvider = value;
		}

		IArtProvider GetProvider(ref string paintingName)
		{
			// TODO
			return DefaultProvider;
		}

		public IPicture GetPicture(string name, Size? desiredSize = null, Color? desiredColor = null)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			var provider = GetProvider(ref name);
			return provider.GetPicture(name, desiredSize, desiredColor);
		}

		public ImageSource GetImageSource(string name, Size? desiredSize = null, Color? desiredColor = null)
		{
			if (string.IsNullOrEmpty(name))
				return null;

			var provider = GetProvider(ref name);
			var imageSource = provider.GetImageSource(name, desiredSize, desiredColor);
			if (imageSource != null)
				return imageSource;

			var picture = provider.GetPicture(name, desiredSize, desiredColor);
			if (picture != null) {
				var bitmap = picture.ToBitmap();
				if (bitmap != null) {
					return new SKBitmapImageSource { Bitmap = bitmap };
				}
			}

			return null;
		}
	}
}
