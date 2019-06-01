using System;
using Newtonsoft.Json;


namespace Dwares.Drudge.Airtable
{
	public class Thumbnails
	{
		[JsonProperty("small")]
		public Thumbnail Small { get; internal set; }

		[JsonProperty("large")]
		public Thumbnail Large { get; internal set; }
	}

	public class Thumbnail
	{
		[JsonProperty("url")]
		public string Url { get; internal set; }

		[JsonProperty("width")]
		public long Width { get; internal set; }

		[JsonProperty("height")]
		public long Height { get; internal set; }
	}
}
