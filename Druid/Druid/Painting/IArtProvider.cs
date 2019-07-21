using System;
using SkiaSharp;


namespace Dwares.Druid.Painting
{
	public interface IArtProvider
	{
		IPainting GetPainting(string name, SKSize? size, SKColor? color);
	}
}
