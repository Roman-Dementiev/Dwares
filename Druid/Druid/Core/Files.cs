using System;
using System.IO;
using System.Text;
using Dwares.Dwarf;
using Dwares.Druid.Services;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace Dwares.Druid
{
	public static class Files
	{
		public static async Task<bool> FileExists(string path)
		{
			Environment.SpecialFolder folder;
			if (IsSpecialPath(ref path, out folder))
			{
				var deviceFolder = await AsDeviceFolder(folder);
				if (deviceFolder != null) {
					return await deviceFolder.FileExistsAsync(path);
				}

				var folderPath = Environment.GetFolderPath(folder);
				path = Path.Combine(folderPath, path);
			}

			return File.Exists(path);

		}

		public static async Task<byte[]> ReadAllBytes(string path)
		{
			Environment.SpecialFolder folder;
			if (IsSpecialPath(ref path, out folder))
			{
				var deviceFolder = await AsDeviceFolder(folder);
				if (deviceFolder != null) {
					var text =  await deviceFolder.ReadTextAsync(path);
					return Encoding.UTF8.GetBytes(text);
				}

				var folderPath = Environment.GetFolderPath(folder);
				path = Path.Combine(folderPath, path);
			}
			
			return File.ReadAllBytes(path);
		}

		public static async Task WriteAllBytes(string path, byte[] bytes)
		{
			Environment.SpecialFolder folder;
			if (IsSpecialPath(ref path, out folder)) {
				var deviceFolder = await AsDeviceFolder(folder);
				if (deviceFolder != null) {
					var text = Encoding.UTF8.GetString(bytes);
					await deviceFolder.WriteTextAsync(path, text);
					return;
				}

				var folderPath = Environment.GetFolderPath(folder);
				path = Path.Combine(folderPath, path);
			}

			File.WriteAllBytes(path, bytes);
		}

		static bool IsSpecialPath(ref string path, out Environment.SpecialFolder folder)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentException("Path is null or empty");

			if (!String.IsNullOrEmpty(path) && path[0] == '@')
			{
				int sep = path.IndexOf('/');
				string folderName;
				if (sep < 0) {
					folderName = path.Substring(1);
					path = string.Empty;
				} else {
					folderName = path.Substring(1, sep-1);
					path = path.Substring(sep+1);
				}

				if (Enums.TryParse(folderName, out folder)) {
					return true;
				} else {
					throw new ArgumentException(String.Format("Invalid special folder '{0}", folderName));
				}
			}

			folder = default(Environment.SpecialFolder);
			return false;
		}
	
		static async Task<IDeviceFolder> AsDeviceFolder(Environment.SpecialFolder folder)
		{
			if (Device.RuntimePlatform == Device.UWP)
			{
				switch (folder)
				{
				case Environment.SpecialFolder.ApplicationData:
					return await DeviceStorage.GetFolder(StorageLocation.AppData);

				case Environment.SpecialFolder.MyDocuments:
					return await DeviceStorage.GetFolder(StorageLocation.Documents);

				case Environment.SpecialFolder.MyMusic:
					return await DeviceStorage.GetFolder(StorageLocation.Music);

				case Environment.SpecialFolder.MyPictures:
					return await DeviceStorage.GetFolder(StorageLocation.Pictures);

				case Environment.SpecialFolder.MyVideos:
					return await DeviceStorage.GetFolder(StorageLocation.Videos);
				}
			}

			return null;
		}
	}
}

