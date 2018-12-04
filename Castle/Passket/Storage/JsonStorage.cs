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

		public JsonStorage(IDeviceFile file) :
			base(file)
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
			SData json = (SData)serializer.ReadObject(input);
			
			return Task.CompletedTask;
		}
	}

}
