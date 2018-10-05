using System;
using System.Reflection;
using SkiaSharp;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;


namespace Dwares.Druid.Painting
{
	public static class Bitmaps
	{
		public static SKBitmap LoadBitmap(Assembly assembly, string resourceName)
		{
			using (var stream = assembly.GetManifestResourceStream(resourceName)) {
				return SKBitmap.Decode(stream);
			}
		}

		public static SKBitmap LoadBitmap(Type type, string resourceName)
			=> LoadBitmap(type.GetTypeInfo().Assembly, resourceName);

		public static SKBitmap LoadBitmap(PackageUnit package, string resourceName)
			=> LoadBitmap(package.Assembly, resourceName);

		public static SKBitmap LoadBitmap(ResourceId resourceId)
			=> LoadBitmap(resourceId.Assembly, resourceId.Name);

	}
}
