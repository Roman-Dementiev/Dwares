using System;
using Dwares.Dwarf;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface ISecureStorage
	{
		string StorageId { get; }
		Task Initialize(string storageId);

		Task<string> GetAsync(string key);
		Task SetAsync(string key, string value);
		Task<bool> RemoveAsync(string key);
	}

	public static class SecureStorage
	{
		static DependencyService<ISecureStorage> instance;
		public static ISecureStorage Instance => DependencyService<ISecureStorage>.GetInstance(ref instance);

		public static Task Initialize(string storageId)
		{
			Guard.ArgumentNotNull(storageId, nameof(storageId));
			return Instance.Initialize(storageId);
		}

		public static Task<string> GetAsync(string key)
		{
			Guard.ArgumentNotNull(key, nameof(key));
			return Instance.GetAsync(key);
		}

		public static Task SetAsync(string key, string value)
		{
			Guard.ArgumentNotNull(key, nameof(key));
			return Instance.SetAsync(key, value);
		}

		public static Task<bool> RemoveAsync(string key)
		{
			Guard.ArgumentNotNull(key, nameof(key));
			return Instance.RemoveAsync(key);
		}
	}
}
