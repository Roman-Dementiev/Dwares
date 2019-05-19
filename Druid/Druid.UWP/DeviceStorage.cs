using System;
using System.Collections.Generic;
using System.IO;
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
	using StorageLocation = Environment.SpecialFolder;

	//class DeviceFile : IDeviceFile
	//{
	//	StorageFile file;

	//	public DeviceFile(StorageFile file)
	//	{
	//		this.file = file;
	//	}

	//	public string Name => file.Name;
	//	public DateTime DateCreated => file.DateCreated.DateTime;

	//	public async Task WriteBytesAsync(byte[] buffer)
	//	{
	//		try {
	//			await FileIO.WriteBytesAsync(file, buffer);
	//		}
	//		catch (Exception ex) {
	//			Debug.ExceptionCaught(ex);
	//		}

	//	}

	//	public async Task WriteTextAsync(string text)
	//	{
	//		try {
	//			await FileIO.WriteTextAsync(file, text);
	//		}
	//		catch (Exception ex) {
	//			Debug.ExceptionCaught(ex);
	//		}
	//	}

	//	public async Task<string> ReadTextAsync()
	//	{
	//		try {
	//			return await FileIO.ReadTextAsync(file);
	//		}
	//		catch (Exception ex) {
	//			Debug.ExceptionCaught(ex);
	//			return null;
	//		}

	//	}
	//}

	//class DeviceFolder : IDeviceFolder
	//{
	//	StorageFolder folder;

	//	public DeviceFolder(StorageFolder folder)
	//	{
	//		this.folder = folder;
	//	}

	//	string[] SplitPath(string path, bool isFolder)
	//	{
	//		if (path == null)
	//			throw new ArgumentNullException(nameof(path));

	//		var split = path.Split('/', '\\');
			
	//		for (int i = 0; i < split.Length; i++) {
	//			if (split[i].Length == 0) {
	//				if (isFolder && i == split.Length-1)
	//					return split.Slice(0, i);

	//				throw new ArgumentException("Path contains empty element", nameof(path));
	//			}
	//		}

	//		if (split.Length == 0 && !isFolder)
	//			throw new ArgumentException("Empty file path", nameof(path));

	//		return split;
	//	}

	//	async Task<StorageFolder> GetStorageFolder(string path, bool create, bool throwErrors)
	//	{
	//		var names = SplitPath(path, true);

	//		return await GetStorageFolder(names, names.Length, create, throwErrors);
	//	}

	//	async Task<StorageFolder> GetStorageFolder(string[] names, int count, bool create, bool throwErrors)
	//	{
	//		try {
	//			var folder = this.folder;
	//			for (int i = 0; i < count; i++) {
	//				var subFolder = await folder.GetFolderAsync(names[i]);
	//				if (subFolder == null) {
	//					if (create) {
	//						subFolder = await folder.CreateFolderAsync(names[i]);
	//						if (subFolder == null) {
	//							var path = string.Join('/', names, 0, i+1);
	//							throw new FileNotFoundException("Can not create folder", path);
	//						}
	//					}
	//					else {
	//						var path = string.Join('/', names, 0, i+1);
	//						throw new FileNotFoundException("Folder not found", path);
	//					}
	//				}

	//				folder = subFolder;
	//			}

	//			return folder;
	//		}
	//		catch (Exception ex) {
	//			if (throwErrors)
	//				throw;
	//			Debug.ExceptionCaught(ex);
	//			return null;
	//		}
	//	}

	//	//public async Task<StorageFile> CreateStorageFile(string path, bool replace, bool throwErrors)
	//	//{

	//	//}

	//	public async Task<StorageFile> CreateStorageFile(StorageFolder folder, string filename, bool replace, bool throwErrors)
	//	{
	//		try {
	//			var options = replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.FailIfExists;
	//			return await folder.CreateFileAsync(filename, options);
	//		}
	//		catch (Exception ex) {
	//			if (throwErrors)
	//				throw;
	//			Debug.ExceptionCaught(ex);
	//			return null;
	//		}
	//	}

	//	public async Task<StorageFile> GetStorageFile(string path, bool create, bool throwErrors)
	//	{
	//		var names = SplitPath(path, false);
	//		var folder = await GetStorageFolder(names, names.Length-1, create, throwErrors);
	//		var filename = names[names.Length-1];

	//		try {
	//			return await folder.GetFileAsync(filename);
	//		}
	//		catch (Exception ex) {
	//			if (ex is FileNotFoundException && create) {
	//				return await CreateStorageFile(folder, filename, true, throwErrors);
	//			}
	//			if (throwErrors)
	//				throw;
	//			Debug.ExceptionCaught(ex);
	//			return null;
	//		}
	//	}

	//	public async Task<IDeviceFolder> GetFolder(string path, bool create, bool throwErrors)
	//	{
	//		var names = SplitPath(path, true);
	//		var folder = await GetStorageFolder(names, names.Length, create, throwErrors);
	//		if (folder != null) {
	//			return new DeviceFolder(folder);
	//		} else {
	//			return null;
	//		}
	//	}

	//	public async Task<bool> FileExists(string path)
	//	{
	//		var file = await GetStorageFile(path, false, false);
	//		return file != null;
	//	}

	//	public async Task CreateFile(string path, bool replace)
	//	{
	//		var names = SplitPath(path, false);
	//		var folder = await GetStorageFolder(names, names.Length-1, replace, true);
	//		var filename = names[names.Length-1];

	//		await CreateStorageFile(folder, filename, replace, true);
	//	}

	//	public async Task DeleteFile(string path)
	//	{
	//		var file = await GetStorageFile(path, false, false);
	//		if (file != null) {
	//			await file.DeleteAsync();
	//		}
	//	}

	//	public async Task<DateTime> DateCreated(string path)
	//	{
	//		var file = await GetStorageFile(path, false, false);
	//		if (file != null) {
	//			return file.DateCreated.DateTime;
	//		} else {
	//			return new DateTime(0);
	//		}
	//	}

	//	public async Task WriteText(string path, string text)
	//	{
	//		var file = await GetStorageFile(path, true, true);
	//		await FileIO.WriteTextAsync(file, text);
	//	}

	//	public async Task<string> ReadText(string path)
	//	{
	//		var file = await GetStorageFile(path, true, true);
	//		return await FileIO.ReadTextAsync(file);
	//	}

	//	public async Task<byte[]> ReadBytes(string path)
	//	{
	//		var file = await GetStorageFile(path, true, true);
	//		var text = await FileIO.ReadTextAsync(file);
	//		return Encoding.UTF8.GetBytes(text);
	//	}

	//	//public async Task<IEnumerable<string>> ListFileNamesAsync()
	//	//{
	//	//	try {
	//	//		var filenames =
	//	//			from storageFile in await folder.GetFilesAsync()
	//	//			select storageFile.Name;

	//	//		return filenames;
	//	//	}
	//	//	catch (Exception ex) {
	//	//		Debug.ExceptionCaught(ex);
	//	//		return null;
	//	//	}
	//	//}

	//	//public async Task<IEnumerable<IDeviceFile>> ListFilesAsync()
	//	//{
	//	//	try {
	//	//		//IEnumerable<StorageFile>storageFiles =
	//	//		//	from storageFile in await folder.GetFilesAsync() select storageFile;
	//	//		var storageFiles = await folder.GetFilesAsync();

	//	//		var files = new List<DeviceFile>();
	//	//		foreach (var storageFile in storageFiles) {
	//	//			files.Add(new DeviceFile(storageFile));
	//	//		}
	//	//		return files;
	//	//	}
	//	//	catch (Exception ex) {
	//	//		Debug.ExceptionCaught(ex);
	//	//		return null;
	//	//	}
	//	//}

	//	//public async Task<IDeviceFile> CreateFileAsync(string filename, bool replace)
	//	//{
	//	//	try {
	//	//		var option = replace ? CreationCollisionOption.ReplaceExisting : CreationCollisionOption.OpenIfExists;
	//	//		var storageFile = await folder.CreateFileAsync(filename, option);
	//	//		return new DeviceFile(storageFile);
	//	//	}
	//	//	catch (Exception ex) {
	//	//		Debug.ExceptionCaught(ex);
	//	//		return null;
	//	//	}
	//	//}

	//	//public async Task<IDeviceFile> GetFileAsync(string filename)
	//	//{
	//	//	try {
	//	//		var storageFile = await folder.GetFileAsync(filename);
	//	//		return new DeviceFile(storageFile);
	//	//	}
	//	//	catch (Exception ex) {
	//	//		Debug.ExceptionCaught(ex);
	//	//		return null;
	//	//	}
	//	//}
	//}

	public class DeviceStorage : IDeviceStorage
	{
		StorageFolder folder;

		public DeviceStorage()
		{
			folder = ApplicationData.Current.LocalFolder;
		}

		public DeviceStorage(StorageFolder folder)
		{
			this.folder = folder;
		}

		public async Task<bool> FileExists(string filename)
		{
			try {
				var file = await folder.GetFileAsync(filename);
				return file != null;
			}
			catch (Exception exc) {
				if (exc is FileNotFoundException) {
					return false;
				}

				Debug.ExceptionCaught(exc);
				throw;
			}
		}

		public async Task<string> ReadText(string filename)
		{
			try {
				var file = await folder.GetFileAsync(filename);
				return await FileIO.ReadTextAsync(file);
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				throw;
			}

		}

		public async Task<byte[]> ReadData(string filename)
		{
			var text = await ReadText(filename);
			return Encoding.UTF8.GetBytes(text);
		}
	}
}
