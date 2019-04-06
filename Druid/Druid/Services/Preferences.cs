using System;
//using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface IPreferences
	{
		bool ContainsKey(string key, string share);
		void RemoveKey(string key, string share);
		void Clear(string share); 

		//bool TryGet(string key, out object value, string share);
		T Get<T>(string key, T defaultValue, string share);
		void Set<T>(string key, T value, string share);
	}

	public static class Preferences
	{
		static DependencyService<IPreferences> instance;
		public static IPreferences Instance => DependencyService<IPreferences>.GetInstance(ref instance);

		public static string DefaultShare { get; set; }

		public static bool ContainsKey(string key, string share = null)
		{
			return Instance.ContainsKey(key, share ?? DefaultShare);
		}

		public static void RemoveKey(string key, string share = null)
		{
			Instance.RemoveKey(key, share ?? DefaultShare);
		}

		public static void Clear(string share = null)
		{
			Instance.Clear(share ?? DefaultShare);
		}

		public static T Get<T>(string key, string share = null)
		{
			return Instance.Get<T>(key, default(T), share ?? DefaultShare);
		}

		//public static bool TryGet(string key, out object value, string share = null)
		//{
		//	return Instance.TryGet(key, out value, share ?? DefaultShare);
		//}

		public static T Get<T>(string key, T defaultValue, string share = null)
		{
			return Instance.Get<T>(key, defaultValue, share ?? DefaultShare);
		}

		public static void Set<T>(string key, T value, string share = null)
		{
			Instance.Set<T>(key, value, share ?? DefaultShare);
		}
	}	
}
