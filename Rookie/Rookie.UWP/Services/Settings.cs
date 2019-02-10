//using System;
//using Windows.Storage;
//using Dwares.Rookie.Dwarf;


//namespace Rookie.UWP.Services
//{
//	public class Settings
//	{
//		public bool ContainsKey(string key, string share)
//		{
//			lock (this) {
//				var container = GetDataContainer(share);
//				return container.Values.ContainsKey(key);
//			}
//		}

//		public bool RemoveKey(string key, string share)
//		{
//			lock (this) {
//				var container = GetDataContainer(share);
//				if (container.Values.ContainsKey(key)) {
//					return container.Values.Remove(key);
//				}
//			}

//			return false;
//		}

//		public void Clear(string share)
//		{
//			lock (this) {
//				var container = GetDataContainer(share);
//				container.Values.Clear();
//			}
//		}

//		public bool TryGet(string key, out object value, string share)
//		{
//			lock (this) {
//				var container = GetDataContainer(share);

//				if (container.Values.ContainsKey(key)) {
//					value = container.Values[key];
//					return true;
//				}
//			}

//			value = null;
//			return false;
//		}

//		public T Get<T>(string key, T defaultValue, string share)
//		{
//			if (TryGet(key, out object value, share)) {
//				try {
//					return (T)value;
//				}
//				catch (Exception ex) {
//					Debug.ExceptionCaught(ex);
//				}
//			}

//			return defaultValue;
//		}

//		public void Set<T>(string key, T value, string share)
//		{
//			lock (this) {
//				var container = GetDataContainer(share);

//				if (value == null) {
//					if (container.Values.ContainsKey(key)) {
//						container.Values.Remove(key);
//					}
//				}
//				else {
//					container.Values[key] = value;
//				}
//			}
//		}

//		static ApplicationDataContainer GetDataContainer(string share)
//		{
//			var localSettings = ApplicationData.Current.LocalSettings;
//			if (string.IsNullOrWhiteSpace(share))
//				return localSettings;

//			if (!localSettings.Containers.ContainsKey(share))
//				localSettings.CreateContainer(share, ApplicationDataCreateDisposition.Always);

//			return localSettings.Containers[share];
//		}
//	}
//}
