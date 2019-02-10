using System;
using Dwares.Dwarf;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface ISecureStorage
	{
		Task Initialize(string storageName);

		Task<string> GetAsync(string key);
		Task SetAsync(string key, string value);
		Task<bool> RemoveAsync(string key);
	}

	public static class SecureStorage
	{
		static DependencyService<ISecureStorage> instance;
		public static ISecureStorage Instance => DependencyService<ISecureStorage>.GetInstance(ref instance);

		public static Task Initialize(string storageName)
		{
			return Instance.Initialize(storageName);
		}

		public static Task<string> GetAsync(string key)
		{
			return Instance.GetAsync(key);
		}

		public static Task SetAsync(string key, string value)
		{
			return Instance.SetAsync(key, value);
		}

		public static Task<bool> RemoveAsync(string key)
		{
			return Instance.RemoveAsync(key);
		}
	}
}
