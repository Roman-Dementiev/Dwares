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

		//Dictionary<string, string> bitmaps = new Dictionary<string, string>();

		public ArtResources() : this(Application.Current) { }

		public ArtResources(Application application)
		{
			Guard.ArgumentNotNull(application, nameof(application));

			Assembly = application.GetType().Assembly;
			Resources = application.Resources;
		}

		public ArtResources(Assembly assembly, ResourceDictionary resources = null)
		{
			//Debug.EnableTracing(@class);
			Assembly = assembly;
			Resources = resources;
		}

		public Assembly Assembly { get;}
		public ResourceDictionary Resources { get; }

		public IPicture GetPicture(string name, Size? desiredSize, Color? desiredColor)
		{
			object value;
			if (!Resources.TryGetValue(name, out value) || value == null)
				return null;

			if (value is IBitmap bitmap) {
				if (desiredColor != null && bitmap.DefaultColor != null && desiredColor != bitmap.DefaultColor) {
					bitmap = bitmap.Recolor((Color)bitmap.DefaultColor, (Color)desiredColor, true);
				}

				//if (desiredSize != null) {
				//	bitmap = bitmap.Resize((Size)desiredSize);
				//}

				return bitmap;
			}


			return null;
		}

		public ImageSource GetImageSource(string name, Size? desiredSize, Color? desiredColor)
		{
			object value;
			if (Resources.TryGetValue(name, out value) && value != null)
			{
				if (value is ImageSource imageSource)
					return imageSource;

				// TODO
				//if (value is IBitmap bitmap) {
				//	return bitmap.ToImageSource();
				//}
			}

			return null;
		}
	}
}
