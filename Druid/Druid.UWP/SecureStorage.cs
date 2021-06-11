//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Storage;
//using Windows.Security.Cryptography.DataProtection;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Xamarin.Forms;
//using Dwares.Druid.Services;
//using Dwares.Dwarf;


//[assembly: Dependency(typeof(Dwares.Druid.UWP.SecureStorage))]

//namespace Dwares.Druid.UWP
//{
//	public class SecureStorage : ISecureStorage
//	{
//		//static readonly string SecureSettings = "Dwares.DSecure";
//		private ApplicationDataContainer container = null;

//		public string StorageId { get; private set; }

//		public Task Initialize(string storageId)
//		{
//			Guard.ArgumentNotEmpty(storageId, nameof(storageId));

//			StorageId = storageId;
//			container = GetContainer(storageId);
//			return Task.CompletedTask;
//		}

//		public async Task<string> GetAsync(string key)
//		{
//			if (container.Values.ContainsKey(key))
//			{
//				var encBytes = container.Values[key] as byte[];
//				if (encBytes != null) {
//					var provider = new DataProtectionProvider();
//					var buffer = await provider.UnprotectAsync(encBytes.AsBuffer());

//					return Encoding.UTF8.GetString(buffer.ToArray());
//				}
//			}
//			return null;
//		}

//		public async Task SetAsync(string key, string value)
//		{
//			var valBytes = Encoding.UTF8.GetBytes(value);

//			// LOCAL=user and LOCAL=machine do not require enterprise auth capability
//			var provider = new DataProtectionProvider("LOCAL=user");
//			var buffer = await provider.ProtectAsync(valBytes.AsBuffer());

//			var encBytes = buffer.ToArray();
//			container.Values[key] = encBytes;
//		}

//		public Task<bool> RemoveAsync(string key)
//		{
//			var result = false;

//			if (container.Values.ContainsKey(key)) {
//				result = container.Values.Remove(key);
//			}

//			return Task.FromResult(result);
//		}

//		static ApplicationDataContainer GetContainer(string name)
//		{
//			var localSettings = ApplicationData.Current.LocalSettings;

//			if (!localSettings.Containers.ContainsKey(name))
//				localSettings.CreateContainer(name, ApplicationDataCreateDisposition.Always);

//			return localSettings.Containers[name];
//		}
//	}
//}
