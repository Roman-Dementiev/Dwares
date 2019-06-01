using System;
using Dwares.Dwarf;
using Newtonsoft.Json;


namespace Dwares.Drudge.Airtable
{
	public class Attachment
	{
		//static ClassRef @class = new ClassRef(typeof(AirAttachment));

		public Attachment()
		{
			//Debug.EnableTracing(@class);
		}

		[JsonProperty("id")]
		public string Id { get; set; }

		[JsonProperty("url")]
		public string Url { get; set; }

		[JsonProperty("filename")]
		public string Filename { get; set; }

		[JsonProperty("size")]
		public long? Size { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("thumbnails")]
		public Thumbnails Thumbnails { get; set; }
	}

}
