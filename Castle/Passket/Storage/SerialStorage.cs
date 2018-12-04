using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using System.Runtime.Serialization;


namespace Passket.Storage
{
	public abstract class SerialStorage : AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(SerialStorage));

		public SerialStorage(IDeviceFile file)
		{
			//Debug.EnableTracing(@class);

			File = file ?? throw new ArgumentNullException(nameof(file));
		}

		public IDeviceFile File { get; }

		protected virtual Encoding Encoding {
			get => Encoding.UTF8;
	}	

		protected abstract Task Serialize(Stream output);
		protected abstract Task Deserialize(Stream input);

		public override async Task Load()
		{
			var text = await File.ReadTextAsync();
			using (var stream = new MemoryStream(Encoding.GetBytes(text))) {
				await Deserialize(stream);
			}
		}

		public override async Task Save()
		{
			using (var stream = new MemoryStream()) {
				await Serialize(stream);
				await stream.FlushAsync();

				var bytes = stream.ToArray();
				await File.WriteBytesAsync(bytes);
			}
		}
	}
}
