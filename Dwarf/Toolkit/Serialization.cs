using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public static class Serialization
	{
		public static void SerializeToJson<T>(Stream stream, object data, DataContractJsonSerializerSettings settings = null)
		{
			Guard.ArgumentNotNull(stream, nameof(stream));

			DataContractJsonSerializer serializer;
			if (settings == null) {
				serializer = new DataContractJsonSerializer(typeof(T));
			} else {
				serializer = new DataContractJsonSerializer(typeof(T), settings);
			}

			serializer.WriteObject(stream, data);
		}

		public static string SerializeToJson<T>(T data, Encoding encoding = null, DataContractJsonSerializerSettings settings = null)
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			using (var stream = new MemoryStream()) {
				SerializeToJson<T>(stream, data, settings);
				stream.Flush();
				
				var bytes = stream.ToArray();
				var json = encoding.GetString(bytes);
				return json;
			}
		}

		public static T DeserializeJson<T>(Stream stream, IEnumerable<Type> knownTypes = null) where T : class
		{
			Guard.ArgumentNotNull(stream, nameof(stream));

			DataContractJsonSerializer serializer;
			if (knownTypes == null) {
				serializer = new DataContractJsonSerializer(typeof(T));
			} else {
				serializer = new DataContractJsonSerializer(typeof(T), knownTypes);
			}

			var result = serializer.ReadObject(stream) as T;
			return result;
		}

		public static T DeserializeJson<T>(string json, Encoding encoding = null, IEnumerable<Type> knownTypes = null) where T : class
		{
			if (encoding == null)
				encoding = Encoding.UTF8;

			var bytes = encoding.GetBytes(json);
			using (var stream = new MemoryStream(bytes)) {
				return DeserializeJson<T>(stream, knownTypes);
			}
 		}
	}
}
