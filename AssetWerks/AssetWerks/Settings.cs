using System;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Windows.Storage;


namespace AssetWerks
{
	public static class Settings
	{
		//static ApplicationDataContainer GetDataContainer(string share)
		//{
		//	var localSettings = ApplicationData.Current.LocalSettings;
		//	if (string.IsNullOrWhiteSpace(share))
		//		return localSettings;

		//	if (!localSettings.Containers.ContainsKey(share))
		//		localSettings.CreateContainer(share, ApplicationDataCreateDisposition.Always);

		//	return localSettings.Containers[share];
		//}

		static ApplicationDataContainer LocalSettings => ApplicationData.Current.LocalSettings;

		public static bool TryGet(string key, out object value)
		{
			var container = LocalSettings;

			if (container.Values.ContainsKey(key)) {
				value = container.Values[key];
				return true;
			}

			value = null;
			return false;
		}

		public static T Get<T>(string key, T defaultValue = default(T))
		{
			try {
				if (TryGet(key, out object value)) {
					return (T)value;
				}
			}  catch (Exception exc) {
				Debug.ExceptionCaught(exc);
			}

			return defaultValue;
		}

		public static void Set<T>(string key, T value)
		{
			var container = LocalSettings;

			if (value == null) {
				if (container.Values.ContainsKey(key)) {
					container.Values.Remove(key);
				}
			}
			else {
				container.Values[key] = value;
			}
		}

		static T GetProperty<T>(T defaultValue = default(T), [CallerMemberName] string propertyName = null)
			=> Get(propertyName, defaultValue);
		
		static void SetProperty<T>(T value, [CallerMemberName] string propertyName = null)
			=> Set(propertyName, value);


		//public static async Task<StorageFolder> GetOutputFolder()
		//{
		//	var path = OutputFolderPath;
		//	if (string.IsNullOrEmpty(path))
		//		return null;

		//	return await StorageFolder.GetFolderFromPathAsync(path);
		//}

		//public static void SetOutputFolder(StorageFolder folder)
		//{
		//	OutputFolderPath = folder?.Path;
		//}

		public static /*async*/ Task Initialize()
		{
			//TODO
			//var path = OutputFolderPath;
			//if (!string.IsNullOrEmpty(path)) {
			//	OutputFolder = await StorageFolder.GetFolderFromPathAsync(path);
			//}
			return Task.CompletedTask;
		}

		static StorageFolder outputFolder;
		public static StorageFolder OutputFolder {
			get => outputFolder;
			set {
				if (value != outputFolder) {
					outputFolder = value;
					OutputFolderPath = outputFolder.Path;
				}
			}
		}

		public static string OutputFolderPath {
			get => GetProperty<string>();
			set => SetProperty(value);
		}

		public static string TargetPlatform {
			get => GetProperty<string>(defaultValue: "Android");
			set => SetProperty(value);
		}

		public static string BadgeName {
			get => GetProperty<string>(defaultValue: "None");
			set => SetProperty(value);
		}

		public static string BadgeColor {
			get => GetProperty<string>();
			set => SetProperty(value);
		}

		public static string IconColor {
			get => GetProperty<string>();
			set => SetProperty(value);
		}
	}
}
