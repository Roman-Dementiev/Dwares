//using System;
//using System.Threading;
//using System.Threading.Tasks;
//using Xamarin.Forms;
//using Javax.Crypto;
//using Dwares.Druid.Services;
//using Dwares.Dwarf;
//using AndroidApp = Android.App.Application;

//[assembly: Dependency(typeof(Dwares.Druid.Android.SecureStorage))]

//namespace Dwares.Druid.Android
//{
//	class SecureStorage : ISecureStorage
//	{
//		public string StorageId { get; private set; }
//		AndroidKeyStore KeyStore { get; set; }

//		public Task Initialize(string storageId)
//		{
//			Guard.ArgumentNotEmpty(storageId, nameof(storageId));

//			StorageId = storageId;
//			KeyStore = new AndroidKeyStore(AndroidApp.Context, storageId);
//			return Task.CompletedTask;
//		}
		
//		public Task<string> GetAsync(string key)
//		{
//			//var context = AndroidApp.Context;
//			var md5Key = Services.Cryptography.MD5Hash(key);
//			var encStr = Services.Preferences.Get<string>(md5Key, null, StorageId);

//			string decryptedData = null;
//			if (!string.IsNullOrEmpty(encStr)) {
//				var encData = Convert.FromBase64String(encStr);
//				//var ks = new AndroidKeyStore(context, StorageId);
//				try {
//					//decryptedData = ks.Decrypt(encData);
//					decryptedData = KeyStore.Decrypt(encData);
//				}
//				catch (AEADBadTagException) {
//					Debug.Print($"Unable to decrypt key, {key}, which is likely due to an app uninstall. Removing old key and returning null.");
//					Services.Preferences.RemoveKey(md5Key, StorageId);
//				}
//			}

//			return Task.FromResult(decryptedData);
//		}

//		public Task<bool> RemoveAsync(string key)
//		{
//			var md5Key = Services.Cryptography.MD5Hash(key);

//			bool removed = false;
//			if (Services.Preferences.ContainsKey(md5Key, StorageId)) {
//				Services.Preferences.RemoveKey(md5Key, StorageId);
//				removed = true;
//			}
	
//			return Task.FromResult(removed);

//		}

//		public Task SetAsync(string key, string value)
//		{
//			//var context = AndroidApp.Context;
//			var md5Key = Services.Cryptography.MD5Hash(key);

//			//var ks = new AndroidKeyStore(context, StorageId);
//			//var encryptedData = ks.Encrypt(value);
//			if (string.IsNullOrEmpty(value)) {
//				Services.Preferences.Set(md5Key, string.Empty, StorageId);
//			} else {
//				var encryptedData = KeyStore.Encrypt(value);

//				var encStr = Convert.ToBase64String(encryptedData);
//				Services.Preferences.Set(md5Key, encStr, StorageId);
//			}

//			return Task.CompletedTask;
//		}
//	}
//}
