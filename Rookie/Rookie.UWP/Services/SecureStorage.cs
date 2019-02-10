//using System;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Windows.Storage;
//using Windows.Security.Cryptography.DataProtection;
//using System.Runtime.InteropServices.WindowsRuntime;
//using Xamarin.Forms;
//using Dwares.Rookie.Druid.Services;


//[assembly: Dependency(typeof(Rookie.UWP.Services.SecureStorage))]

//namespace Rookie.UWP.Services
//{
//	public class SecureStorage : ISecureStorage
//	{
//		static readonly string SecureSettings = "Dwares.AllThrees.Secure";

//		public async Task<string> GetAsync(string key)
//		{
//			var settings = GetContainer(SecureSettings);
//			var encBytes = settings.Values[key] as byte[];
//			if (encBytes == null)
//				return null;

//			var provider = new DataProtectionProvider();
//			var buffer = await provider.UnprotectAsync(encBytes.AsBuffer());

//			return Encoding.UTF8.GetString(buffer.ToArray());
//		}

//		public async Task SetAsync(string key, string value)
//		{
//			var settings = GetContainer(SecureSettings);
//			var valBytes = Encoding.UTF8.GetBytes(value);

//			// LOCAL=user and LOCAL=machine do not require enterprise auth capability
//			var provider = new DataProtectionProvider("LOCAL=user");
//			var buffer = await provider.ProtectAsync(valBytes.AsBuffer());

//			var encBytes = buffer.ToArray();
//			settings.Values[key] = encBytes;
//		}

//		public Task<bool> RemoveAsync(string key)
//		{
//			var settings = GetContainer(SecureSettings);
//			var result = false;

//			if (settings.Values.ContainsKey(key)) {
//				result = settings.Values.Remove(key);
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
