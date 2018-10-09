//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Threading.Tasks;
//using Windows.Storage;
//using Xamarin.Forms;
////using Dwares.Dwarf;
//using Dwares.Druid.Services;


//[assembly: Dependency(typeof(ACE.UWP.DeviceStorage))]
//namespace ACE.UWP
//{
//	class DeviceFile : IDeviceFile
//	{
//		StorageFile file;

//		public DeviceFile(StorageFile file)
//		{
//			this.file = file;
//		}

//		public string Name => file.Name;
//		public DateTime DateCreated => file.DateCreated.DateTime;
//	}

//	class DeviceFolder : IDeviceFolder
//	{
//		StorageFolder folder;

//		public DeviceFolder(StorageFolder folder)
//		{
//			this.folder = folder;
//		}

//		public async Task<bool> ExistsAsync(string filename)
//		{
//			try {
//				var storageFile = await folder.GetFileAsync(filename);
//				return storageFile != null;
//			}
//			catch {
//				return false;
//			}
//		}

//		public async Task<DateTime> DateCreatedAsync(string filename)
//		{
//			try {
//				var storageFile = await folder.GetFileAsync(filename);
//				if (storageFile != null) {
//					return storageFile.DateCreated.DateTime;
//				}
//			}
//			catch {
//			}
//			return new DateTime(0);
//		}

//		public async Task WriteTextAsync(string filename, string text)
//		{
//			try {
//				IStorageFile storageFile = await folder.CreateFileAsync(filename, CreationCollisionOption.ReplaceExisting);
//				await FileIO.WriteTextAsync(storageFile, text);
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//			}
//		}

//		public async Task<string> ReadTextAsync(string filename)
//		{
//			try {
//				IStorageFile storageFile = await folder.GetFileAsync(filename);
//				if (storageFile != null) {
//					return await FileIO.ReadTextAsync(storageFile);
//				} else {
//					return null;
//				}
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//				return null;
//			}
//		}

//		public async Task<IEnumerable<string>> ListFileNamesAsync()
//		{
//			try {
//				IEnumerable<string> filenames =
//					from storageFile in await folder.GetFilesAsync()
//					select storageFile.Name;

//				return filenames;
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//				return null;
//			}
//		}

//		public async Task<IEnumerable<IDeviceFile>> ListFilesAsync()
//		{
//			try {
//				//IEnumerable<StorageFile>storageFiles =
//				//	from storageFile in await folder.GetFilesAsync() select storageFile;
//				var storageFiles = await folder.GetFilesAsync();

//				var files = new List<DeviceFile>();
//				foreach (var storageFile in storageFiles) {
//					files.Add(new DeviceFile(storageFile));
//				}
//				return files;
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//				return null;
//			}
//		}

//		public async Task DeleteAsync(string filename)
//		{
//			try {
//				StorageFile storageFile = await folder.GetFileAsync(filename);
//				await storageFile.DeleteAsync();
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//			}
//		}

//	}

//	public class DeviceStorage : IDeviceStorage
//	{
//		public async Task<IDeviceFolder> GetFolder(string path, StorageLocation location = StorageLocation.AppData, bool create = true)
//		{
//			try {
//				StorageFolder folder;
//				switch (location) {
//				case StorageLocation.Documents:
//#if FIXED_AccessToDocumentsFolder //TODO: Permission/Capabilities to access Documents folders ?
//					folder = KnownFolders.DocumentsLibrary;
//					break;
//#endif
//				case StorageLocation.Pictures:
//					folder = KnownFolders.PicturesLibrary;
//					break;

//				case StorageLocation.Music:
//					folder = KnownFolders.MusicLibrary;
//					break;

//				case StorageLocation.Videos:
//					folder = KnownFolders.VideosLibrary;
//					break;

//				default:
//					folder = ApplicationData.Current.LocalFolder;
//					break;
//				}

//				string subName;
//				string subPath = path;
//				while (folder != null && !String.IsNullOrEmpty(subPath)) {
//					int slash = subPath.IndexOf('/');
//					if (slash > 0) {
//						subName = subPath.Substring(0, slash);
//						subPath = subPath.Substring(slash + 1);
//					} else {
//						subName = subPath;
//						subPath = null;
//					}
//					StorageFolder subFolder;
//					try {
//						subFolder = await folder.GetFolderAsync(subName);
//					}
//					catch {
//						subFolder = null;
//					}

//					if (subFolder == null && create) {
//						subFolder = await folder.CreateFolderAsync(subName);
//					}
//					folder = subFolder;
//				}
//				return new DeviceFolder(folder);
//			}
//			catch (Exception ex) {
//				//Debug.ExceptionCaught(ex);
//				return null;
//			}
//		}
//	}
//}
