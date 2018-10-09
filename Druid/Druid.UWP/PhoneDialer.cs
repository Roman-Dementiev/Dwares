using System;
using Xamarin.Forms;
using Dwares.Druid.Services;

[assembly: Dependency(typeof(Dwares.Druid.UWP.PhoneDialer))]

namespace Dwares.Druid.UWP
{
	class PhoneDialer : IPhoneDialer
	{
		public Exception TryDial(string phoneNumber, string displayName)
		{
			var message = String.Format("TryDial: phoneNumber={0}, displayName={1}", phoneNumber, displayName);
			System.Diagnostics.Debug.WriteLine(message);

			//PhoneCallManager.ShowPhoneCallUI(phoneNumber, displayName);
			return CallingInfo.CallingInfoInstance.TryDialOnCurrentLine(phoneNumber, displayName);
		}
	}
}
