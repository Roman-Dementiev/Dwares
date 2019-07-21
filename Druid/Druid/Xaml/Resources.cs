using System;
using Dwares.Dwarf;
using Xamarin.Forms;

namespace Dwares.Druid.Xaml
{
	public class ResourceId
	{
		public string Id { get; set; }
		public override string ToString() => Id;
	}

	public class BitmapId: ResourceId { }


	[ContentProperty(nameof(ResourceId))]
	public class Resource<T> : MarkupExtension<T> where T : ResourceId, new()
	{
		//static ClassRef @class = new ClassRef(typeof(Resources));

		public Resource()
		{
			//Debug.EnableTracing(@class);
		}

		public string ResourceId { get; set; }

		public override T ProvideValue(IServiceProvider serviceProvider)
		{
			return new T { Id = ResourceId };
		}
	}

	public class Bitmap : Resource<BitmapId>
	{
		public Bitmap() { }
	}
}
