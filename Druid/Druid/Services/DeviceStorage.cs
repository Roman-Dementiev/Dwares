using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dwares.Druid.Services
{
	using StorageLocation = Environment.SpecialFolder;

	//public enum StorageLocation
	//{
	//	AppData,
	//	Documents,
	//	Pictures,
	//	Music,
	//	Videos,
	//	// ETC
	//}

	//public interface IDeviceFile
	//{
	//	string Name { get; }
	//	DateTime DateCreated { get; }

	//	Task WriteBytesAsync(byte[] buffer);
	//	Task WriteTextAsync(string text);
	//	Task<string> ReadTextAsync();
	//}

	public interface IDeviceFolder
	{
		Task<IDeviceFolder> GetFolder(string path, bool create, bool throwErrors = true);

		Task<bool> FileExists(string path);
		Task CreateFile(string path, bool replace);
		Task DeleteFile(string path);
		Task<DateTime> DateCreated(string path);
		Task WriteText(string path, string text);
		Task<string> ReadText(string path);
		//Task<byte[]> ReadBytesAsync(string filename);
		//Task<IEnumerable<string>> ListFileNamesAsync();
		//Task<IEnumerable<IDeviceFile>> ListFilesAsync();

		//Task<IDeviceFile> CreateFileAsync(string filename, bool replace);
		//Task<IDeviceFile> GetFileAsync(string filename);
	}

	public interface IDeviceStorage
	{
		Task<IDeviceFolder> GetFolder(string path, StorageLocation location, bool create);

	}

	public static class DeviceStorage
	{
		public const StorageLocation DefaultLocation = StorageLocation.ApplicationData;

		static DependencyService<IDeviceStorage> instance;
		public static IDeviceStorage Instance => DependencyService<IDeviceStorage>.GetInstance(ref instance);

		public static async Task<IDeviceFolder> GetFolder(StorageLocation location = DefaultLocation, bool create = true)
		{
			return await GetFolder(null, location, create);
		}

		public static async Task<IDeviceFolder> GetFolder(string path, StorageLocation location = DefaultLocation, bool create = true)
		{
			return await Instance.GetFolder(path, location, create);
		}
	}
}
