using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Druid.Services;


[assembly: Dependency(typeof(Dwares.Druid.UWP.DeviceStorage))]

namespace Dwares.Druid.UWP
{
	class DeviceFile : IDeviceFile
	{
		StorageFile file;

		public DeviceFile(StorageFile file)
		{
			this.file = file;
		}

		public string Name => file.Name;
		public DateTime DateCreated => file.DateCreated.DateTime;

		public async Task WriteBytesAsync(byte[] buffer)
		{
			try {
				await FileIO.WriteBytesAsync(file, buffer);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}

		}

		public async Task WriteTextAsync(string text)
		{
			try {
				await FileIO.WriteTextAsync(file, text);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}

		public async Task<string> ReadTextAsync()
		{
			try {
				return await FileIO.ReadTextAsync(file);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}

		}
	}

	class DeviceFolder : IDeviceFolder
	{
		StorageFolder folder;

		public DeviceFolder(StorageFolder folder)
		{
			this.folder = folder;
		}

		public async Task<bool> FileExistsAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				return storageFile != null;
			}
			catch {
				return false;
			}
		}

		public async Task<DateTime> DateCreatedAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				if (storageFile != null) {
					return storageFile.DateCreated.DateTime;
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			return new DateTime(0);
		}

		public async Task WriteTextAsync(string filename, string text)
		{
			try {
				var storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
				await FileIO.WriteTextAsync(storageFile, text);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}

		public async Task<string> ReadTextAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				if (storageFile != null) {
					return await FileIO.ReadTextAsync(storageFile);
				} else {
					return null;
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public async Task<byte[]> ReadBytesAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				if (storageFile != null) {
					var text = await FileIO.ReadTextAsync(storageFile);
					return Encoding.UTF8.GetBytes(text);
				}
				else {
					return null;
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public async Task<IEnumerable<string>> ListFileNamesAsync()
		{
			try {
				var filenames =
					from storageFile in await folder.GetFilesAsync()
					select storageFile.Name;

				return filenames;
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public async Task<IEnumerable<IDeviceFile>> ListFilesAsync()
		{
			try {
				//IEnumerable<StorageFile>storageFiles =
				//	from storageFile in await folder.GetFilesAsync() select storageFile;
				var storageFiles = await folder.GetFilesAsync();

				var files = new List<DeviceFile>();
				foreach (var storageFile in storageFiles) {
					files.Add(new DeviceFile(storageFile));
				}
				return files;
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public async Task DeleteAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				await storageFile.DeleteAsync();
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
		}

		public async Task<IDeviceFile> CreateFileAsync(string filename, bool replace)
		{
			try {
				var option = replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.OpenIfExists;
				var storageFile = await folder.CreateFileAsync(filename, option);
				return new DeviceFile(storageFile);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}

		public async Task<IDeviceFile> GetFileAsync(string filename)
		{
			try {
				var storageFile = await folder.GetFileAsync(filename);
				return new DeviceFile(storageFile);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}
	}

	public class DeviceStorage : IDeviceStorage
	{
		public async Task<IDeviceFolder> GetFolder(string path, StorageLocation location = StorageLocation.AppData, bool create = true)
		{
			try {
				StorageFolder folder;
				switch (location) {
				case StorageLocation.Documents:
#if FIXED_AccessToDocumentsFolder //TODO: Permission/Capabilities to access Documents folders ?
					folder = KnownFolders.DocumentsLibrary;
					break;
#endif
				case StorageLocation.Pictures:
					folder = KnownFolders.PicturesLibrary;
					break;

				case StorageLocation.Music:
					folder = KnownFolders.MusicLibrary;
					break;

				case StorageLocation.Videos:
					folder = KnownFolders.VideosLibrary;
					break;

				default:
					folder = ApplicationData.Current.LocalFolder;
					break;
				}

				string subName;
				string subPath = path;
				while (folder != null && !String.IsNullOrEmpty(subPath)) {
					int slash = subPath.IndexOf('/');
					if (slash > 0) {
						subName = subPath.Substring(0, slash);
						subPath = subPath.Substring(slash + 1);
					} else {
						subName = subPath;
						subPath = null;
					}
					StorageFolder subFolder;
					try {
						subFolder = await folder.GetFolderAsync(subName);
					}
					catch {
						subFolder = null;
					}

					if (subFolder == null && create) {
						subFolder = await folder.CreateFolderAsync(subName);
					}
					folder = subFolder;
				}
				return new DeviceFolder(folder);
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				return null;
			}
		}
	}
}
