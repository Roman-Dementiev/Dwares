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
		public static async Task<string> ReadText(string path, bool async)
		{
			if (async) {
				return await ReadTextAsync(path);
			} else {
				return File.ReadAllText(path);
			}
		}

		public static async Task<string> ReadTextAsync(string path)
		{
			using (var reader = File.OpenText(path)) {
				var text = await reader.ReadToEndAsync();
				return text;
			}
		}

		public static async Task WriteTextAsync(string path, string text)
		{
			using (var writer = File.CreateText(path)) {
				await writer.WriteAsync(text);
				await writer.FlushAsync();
			}
		}


		//public static async Task<bool> FileExists(string path)
		//{
		//	Environment.SpecialFolder folder;
		//	if (IsSpecialPath(ref path, out folder)) {
		//		var deviceFolder = await AsDeviceFolder(folder);
		//		if (deviceFolder != null) {
		//			return await deviceFolder.FileExists(path);
		//		}

		//		var folderPath = Environment.GetFolderPath(folder);
		//		path = Path.Combine(folderPath, path);
		//	}

		//	return File.Exists(path);

		//}

		//public static async Task<byte[]> ReadAllBytes(string path)
		//{
		//	Environment.SpecialFolder folder;
		//	if (IsSpecialPath(ref path, out folder)) {
		//		var deviceFolder = await AsDeviceFolder(folder);
		//		if (deviceFolder != null) {
		//			var text = await deviceFolder.ReadText(path);
		//			return Encoding.UTF8.GetBytes(text);
		//		}

		//		var folderPath = Environment.GetFolderPath(folder);
		//		path = Path.Combine(folderPath, path);
		//	}

		//	return File.ReadAllBytes(path);
		//}

		//public static async Task WriteAllBytes(string path, byte[] bytes)
		//{
		//	Environment.SpecialFolder folder;
		//	if (IsSpecialPath(ref path, out folder)) {
		//		var deviceFolder = await AsDeviceFolder(folder);
		//		if (deviceFolder != null) {
		//			var text = Encoding.UTF8.GetString(bytes);
		//			await deviceFolder.WriteText(path, text);
		//			return;
		//		}

		//		var folderPath = Environment.GetFolderPath(folder);
		//		path = Path.Combine(folderPath, path);
		//	}

		//	File.WriteAllBytes(path, bytes);
		//}

		//static bool IsSpecialPath(ref string path, out Environment.SpecialFolder folder)
		//{
		//	if (String.IsNullOrEmpty(path))
		//		throw new ArgumentException("Path is null or empty");

		//	if (!String.IsNullOrEmpty(path) && path[0] == '@') {
		//		int sep = path.IndexOf('/');
		//		string folderName;
		//		if (sep < 0) {
		//			folderName = path.Substring(1);
		//			path = string.Empty;
		//		} else {
		//			folderName = path.Substring(1, sep - 1);
		//			path = path.Substring(sep + 1);
		//		}

		//		if (Enums.TryParse(folderName, out folder)) {
		//			return true;
		//		} else {
		//			throw new ArgumentException(String.Format("Invalid special folder '{0}", folderName));
		//		}
		//	}

		//	folder = default(Environment.SpecialFolder);
		//	return false;
		//}

		//static async Task<IDeviceFolder> AsDeviceFolder(Environment.SpecialFolder folder)
		//{
		//	if (Device.RuntimePlatform == Device.UWP) {
		//		switch (folder) {
		//		case Environment.SpecialFolder.ApplicationData:
		//		case Environment.SpecialFolder.MyDocuments:
		//		case Environment.SpecialFolder.MyMusic:
		//		case Environment.SpecialFolder.MyPictures:
		//		case Environment.SpecialFolder.MyVideos:
		//			return await DeviceStorage.GetFolder(folder);
		//		}
		//	}

		//	return null;
		//}
	}
}

