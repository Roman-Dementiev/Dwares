using System;
using System.Runtime.Serialization;
using Dwares.Dwarf;


namespace Dwares.Drudge.Airtable
{using Dwares.Dwarf;
	public class Attachment
	{
		//static ClassRef @class = new ClassRef(typeof(AirAttachment));

		public Attachment()
		{
			//Debug.EnableTracing(@class);
		}

		[DataMember(Name = "id", EmitDefaultValue = false)]
		public string Id { get; set; }

		[DataMember(Name = "url", EmitDefaultValue = false)]
		public string Url { get; set; }

		[DataMember(Name = "filename", EmitDefaultValue = false)]
		public string Filename { get; set; }

		[DataMember(Name = "size", EmitDefaultValue = false)]
		public long? Size { get; set; }

		[DataMember(Name = "type", EmitDefaultValue = false)]
		public string Type { get; set; }

		[DataMember(Name = "thumbnails", EmitDefaultValue = false)]
		public Thumbnails Thumbnails { get; set; }
	}

}
