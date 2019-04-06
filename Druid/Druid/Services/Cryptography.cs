using System;
using System.Security.Cryptography;
using System.Text;
using Dwares.Dwarf;


namespace Dwares.Druid.Services
{
	public interface ICryptography
	{
		bool HasSymmetricKeys { get; }
	}

	public static class Cryptography
	{
		static DependencyService<ICryptography> instance;
		public static ICryptography Instance => DependencyService<ICryptography>.GetInstance(ref instance);

		public static bool HasSymmetricKeys => Instance.HasSymmetricKeys;

		public static string MD5Hash(string input)
		{
			var hash = new StringBuilder();
			var md5provider = new MD5CryptoServiceProvider();
			var bytes = md5provider.ComputeHash(Encoding.UTF8.GetBytes(input));

			for (var i = 0; i < bytes.Length; i++) {
				hash.Append(bytes[i].ToString("x2"));
			}

			return hash.ToString();
		}
	}
}
