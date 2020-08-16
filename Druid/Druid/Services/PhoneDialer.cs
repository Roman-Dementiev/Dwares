using System;
using System.Threading.Tasks;


namespace Dwares.Druid.Services
{
	public interface IPhoneDialer
	{
		Exception TryDial(string phoneNumber, string displayName);
	}


	public static class PhoneDialer
	{
		static DependencyService<IPhoneDialer> instance;
		public static IPhoneDialer Instance => DependencyService<IPhoneDialer>.GetInstance(ref instance);

		public static Exception TryDial(string phoneNumber, string displayName = null)
		{
			try {
				return Instance.TryDial(phoneNumber, displayName ?? string.Empty);
			}
			catch (Exception ex) {
				return ex;
			}

		}

		public static async Task DialAsync(string phoneNumber, string displayName = null)
		{
			var ex = TryDial(phoneNumber, displayName);
			if (ex != null) {
				// TODO
				var message = String.Format("Can't dia {0}: {1}", phoneNumber, ex.Message);
				System.Diagnostics.Debug.WriteLine(message);

				await Alerts.ErrorAlert(ex.Message);
			}
		}
	}
}
