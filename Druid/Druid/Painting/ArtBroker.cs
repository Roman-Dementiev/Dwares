using System;
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

		public IPainting GetPainting(string name, SKSize? size = null, SKColor? color = null)
		{
			var provider = GetProvider(ref name);
			return provider.GetPainting(name, size, color);
		}

		public ImageSource GetImageSource(string name, SKSize? size = null, SKColor? color = null)
		{
			var painting = GetPainting(name, size, color);
			if (painting != null) {
				var bitmap = painting.ToBitmap();
				if (bitmap != null) {
					return new SKBitmapImageSource { Bitmap = bitmap };
				}
			}

			return null;
		}

		//public ImageSource GetImageSource(string group, string name)
		//	=> GetImageSource(name);
	}
}
