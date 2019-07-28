using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Dwarf;
using Dwares.Druid.Xaml;
using SkiaSharp;
using Xamarin.Forms;


namespace Dwares.Druid.Painting
{
	public class ArtResources : IArtProvider
	{
		//static ClassRef @class = new ClassRef(typeof(ArtResources));

		Dictionary<string, string> bitmaps = new Dictionary<string, string>();

		public ArtResources() : this(Application.Current) { }

		public ArtResources(Application application)
		{
			Guard.ArgumentNotNull(application, nameof(application));

			Assembly = application.GetType().Assembly;
			LoadFrom(application.Resources);
		}

		public ArtResources(Assembly assembly, ResourceDictionary resources = null)
		{
			//Debug.EnableTracing(@class);
			Assembly = assembly;

			if (resources != null) {
				LoadFrom(resources);
			}
		}

		public Assembly Assembly { get;}

		public void LoadFrom(ResourceDictionary resources)
		{
			Guard.ArgumentNotNull(resources, nameof(resources));

			foreach (var pair in resources) {
				if (pair.Value is BitmapId bitmap) {
					var id = string.IsNullOrEmpty(bitmap.Id) ? pair.Key : bitmap.Id;
					bitmaps.Add(pair.Key, id);
				}
			}
		}

		public IPainting GetPainting(string name, SKSize? size, SKColor? color)
		{
			string resourceName;

			if (bitmaps.TryGetValue(name, out resourceName)) {
				var bitmap = Bitmaps.LoadBitmap(Assembly, resourceName);

				if (color != null && color != SKColors.Black) {
					bitmap = bitmap.Recolor(SKColors.Black, (SKColor)color);
				}

				if (size != null) {
					int width = (int)Math.Round(((SKSize)size).Width);
					int height = (int)Math.Round(((SKSize)size).Height);
					if (width != bitmap.Width || height != bitmap.Height) {
						bitmap = bitmap.Resize(width, height);
					}
				}

				return new BitmapPainting(bitmap);
			}

			return null;
		}
	}
}
