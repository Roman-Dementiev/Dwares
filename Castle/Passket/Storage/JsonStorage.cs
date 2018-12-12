using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Json;
using Dwares.Druid.Services;
using Dwares.Dwarf;


namespace Passket.Storage
{
	public class JsonStorage : SerialStorage
	{
		//static ClassRef @class = new ClassRef(typeof(JsonStorage));

		public JsonStorage(string path) :
			base(path)
		{
			//Debug.EnableTracing(@class);
		}

		protected override Task Serialize(Stream output)
		{
			var data = new SData(SData.cVersion1);
			var serializer = new DataContractJsonSerializer(typeof(SData));
			serializer.WriteObject(output, data);
			return Task.CompletedTask;
		}

		protected override Task Deserialize(Stream input)
		{
			var serializer = new DataContractJsonSerializer(typeof(SData));
			var data = (SData)serializer.ReadObject(input);
			
			foreach (var spat in data.Patterns) {
				var pattern = spat;
				AppData.Patterns.Add(pattern);
			}

			return Task.CompletedTask;
		}
	}

}
