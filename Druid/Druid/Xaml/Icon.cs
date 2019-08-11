//using System;
//using Dwares.Dwarf;
//using Dwares.Dwarf.Runtime;
//using Xamarin.Forms;

//namespace Dwares.Druid.Xaml
//{
//	[ContentProperty(nameof(ResourceId))]
//	public class Icon : MarkupExtension<Painting.Icon>
//	{
//		//static ClassRef @class = new ClassRef(typeof(Icon));

//		public Icon()
//		{
//			//Debug.EnableTracing(@class);
//		}

//		public string ResourceId { get; set; }
//		public int Width { get; set; }
//		public int Height { get; set; }
//		public Color? Color { get; set; }


//		public override Painting.Icon ProvideValue(IServiceProvider serviceProvider)
//		{
//			var resourceId = PackageUnit.GetResourceId(ResourceId, Application.Current.GetType().Assembly);
//			var icon = new Painting.Icon(resourceId, Width, Height, Color);
//			return icon;
//		}
//	}
//}
