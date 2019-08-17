using System;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Xaml
{
	[ContentProperty(nameof(ResourceId))]
	public abstract class Resource<T> : MarkupExtension<T>
	{
		public Resource() { }

		public string ResourceId { get; set; }
	}

	public class Bitmap : Resource<Painting.Bitmap>
	{
		public Bitmap() { }

		public override Painting.Bitmap ProvideValue(IServiceProvider serviceProvider)
		{
			var resourceId = AssetLocator.GetResourceId(ResourceId);
			return new Painting.Bitmap(resourceId);
		}
	}

	//public class BitmapAsset : Resource<Painting.BitmapAsset>
	//{
	//	public BitmapAsset() { }

	//	[TypeConverter(typeof(SizeTypeConverter))]
	//	public Size BitmapSize {
	//		get => new Size(BitmapWidth, BitmapHeight);
	//		set {
	//			BitmapWidth = (int)value.Width;
	//			BitmapHeight = (int)value.Height;
	//		}
	//	}

	//	//public int BitmapWidth { get; set; }
	//	//public int BitmapHeight { get; set; }

	//	public double Resolution { get; }
	//	public Color Color { get; set; }

	//	public override Painting.BitmapAsset ProvideValue(IServiceProvider serviceProvider)
	//	{
	//		var resourceId = AssetLocator.GetResourceId(ResourceId);
	//		return new Painting.BitmapAsset(resourceId, BitmapWidth, BitmapHeight, Resolution, Color);
	//	}
	//}
}
