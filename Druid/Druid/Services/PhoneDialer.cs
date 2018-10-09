using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace Dwares.Druid.Services
{
	public interface IPhoneDialer
	{
		Exception TryDial(string phoneNumber, string displayName);
	}


	public static class PhoneDialer
	{
		static IPhoneDialer instance = null;

		public static IPhoneDialer Instance {
			get {
				if (instance == null) {
					instance = DependencyService.Get<IPhoneDialer>();
				}
				return instance;
			}
		}

		public static Exception TryDial(string phoneNumber, string displayName)
		{
			try {
				return Instance.TryDial(phoneNumber, displayName ?? "");
			}
			catch (Exception ex) {
				return ex;
			}

		}

		public static async Task DialAsync(string phoneNumber, string displayName)
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
