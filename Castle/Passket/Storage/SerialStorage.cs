using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using System.Runtime.Serialization;


namespace Passket.Storage
{
	public abstract class SerialStorage : AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(SerialStorage));

		public SerialStorage(string path)
		{
			//Debug.EnableTracing(@class);

			Path = path ?? throw new ArgumentNullException(nameof(path));
		}

		public string Path { get; }

		protected virtual Encoding Encoding {
			get => Encoding.UTF8;
	}	

		protected abstract Task Serialize(Stream output);
		protected abstract Task Deserialize(Stream input);

		public override async Task<bool> Exists()
		{
			return await Files.FileExists(Path);
		}

		public override async Task Load()
		{
			var bytes = await Files.ReadAllBytes(Path);
			using (var stream = new MemoryStream(bytes)) {
				await Deserialize(stream);
			}
		}

		public override async Task Save()
		{
			using (var stream = new MemoryStream()) {
				await Serialize(stream);
				await stream.FlushAsync();

				var bytes = stream.ToArray();
				await Files.WriteAllBytes(Path, bytes);
			}
		}
	}
}
