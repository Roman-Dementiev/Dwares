using System;
using System.Runtime.Serialization;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{
	public class Thumbnails
	{
		//static ClassRef @class = new ClassRef(typeof(Thumbnails));

		public Thumbnails()
		{
			//Debug.EnableTracing(@class);
		}

		[DataMember(Name = "small", EmitDefaultValue = false)]
		public Thumbnail Small { get; internal set; }

		[DataMember(Name = "large", EmitDefaultValue = false)]
		public Thumbnail Large { get; internal set; }
	}

	public class Thumbnail
	{
		//static ClassRef @class = new ClassRef(typeof(Thumbnail));

		public Thumbnail()
		{
			//Debug.EnableTracing(@class);
		}

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; internal set; }

		[DataMember(Name = "width", EmitDefaultValue = false)]
		public long Width { get; internal set; }

		[DataMember(Name = "height", EmitDefaultValue = false)]
		public long Height { get; internal set; }
	}
}
