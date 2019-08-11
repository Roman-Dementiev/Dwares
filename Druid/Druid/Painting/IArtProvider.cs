using System;
using Dwares.Druid.Satchel;
using SkiaSharp;
using Xamarin.Forms;

namespace Dwares.Druid.Painting
{
	public interface IArtProvider
	{
		IPicture GetPicture(string name, Size? desiredSize, Color? desiredColor);

		ImageSource GetImageSource(string name, Size? desiredSize, Color? desiredColor);
	}
}
