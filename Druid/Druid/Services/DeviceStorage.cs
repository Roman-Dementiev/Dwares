using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Dwares.Druid.Services
{
	public enum StorageLocation
	{
		AppData,
		Documents,
		Pictures,
		Music,
		Videos,
		// ETC
	}

	public interface IDeviceFile
	{
		string Name { get; }
		DateTime DateCreated { get; }

		Task WriteBytesAsync(byte[] buffer);
		Task WriteTextAsync(string text);
		Task<string> ReadTextAsync();
	}

	public interface IDeviceFolder
	{
		Task<bool> FileExistsAsync(string filename);
		Task<DateTime> DateCreatedAsync(string filename);
		Task WriteTextAsync(string filename, string text);
		Task<string> ReadTextAsync(string filename);
		//Task<byte[]> ReadBytesAsync(string filename);
		Task<IEnumerable<string>> ListFileNamesAsync();
		Task<IEnumerable<IDeviceFile>> ListFilesAsync();
		Task DeleteAsync(string filename);

		Task<IDeviceFile> CreateFileAsync(string filename, bool replace);
		Task<IDeviceFile> GetFileAsync(string filename);
	}

	public interface IDeviceStorage
	{
		Task<IDeviceFolder> GetFolder(string path, StorageLocation location = StorageLocation.AppData, bool create = true);

	}

	public static class DeviceStorage
	{
		static DependencyService<IDeviceStorage> instance;
		public static IDeviceStorage Instance => DependencyService<IDeviceStorage>.GetInstance(ref instance);

		public static async Task<IDeviceFolder> GetFolder(StorageLocation location = StorageLocation.AppData, bool create = true)
		{
			return await GetFolder(null, location, create);
		}

		public static async Task<IDeviceFolder> GetFolder(string path, StorageLocation location = StorageLocation.AppData, bool create = true)
		{
			return await Instance.GetFolder(path, location, create);
		}
	}
}
