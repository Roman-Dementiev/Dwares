using System;
using Xamarin.Forms;
using ACE.Services;

[assembly: Dependency(typeof(ACE.UWP.PhoneDialer))]
namespace ACE.UWP
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
