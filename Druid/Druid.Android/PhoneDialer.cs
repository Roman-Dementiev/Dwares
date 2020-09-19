//using System;
//using Xamarin.Forms;
//using Dwares.Druid.Services;

//using Android.App;
//using Android.Content;
//using Android.OS;
//using Android.Telephony;
//using Java.Net;

//using Android.Runtime;
//using Android.Views;
//using Android.Widget;

//using AndroidApp = Android.App.Application;
//using AndroidUri = Android.Net.Uri;

//[assembly: Dependency(typeof(Dwares.Druid.Android.PhoneDialer))]

//namespace Dwares.Druid.Android
//{
//	class PhoneDialer : IPhoneDialer
//	{
//        const string intentCheck = "00000000000";

//        static bool IsSupported {
//            get {
//                var packageManager = AndroidApp.Context.PackageManager;
//                var dialIntent = ResolveDialIntent(intentCheck);
//                return dialIntent.ResolveActivity(packageManager) != null;
//            }
//        }

//        public Exception TryDial(string number, string displayName)
//		{
//            if (string.IsNullOrWhiteSpace(number))
//                return new ArgumentNullException(nameof(number));

//            if (!IsSupported)
//                return new FeatureNotSupportedException();

//            var phoneNumber = string.Empty;
//            if (SystemInfo.HasApiLevelN) {
//                phoneNumber = PhoneNumberUtils.FormatNumber(number, Java.Util.Locale.GetDefault(Java.Util.Locale.Category.Format).Country);
//            }
//            else if (SystemInfo.HasApiLevel(BuildVersionCodes.Lollipop)) {
//                phoneNumber = PhoneNumberUtils.FormatNumber(number, Java.Util.Locale.Default.Country);
//            }  else {
//                phoneNumber = PhoneNumberUtils.FormatNumber(number);
//            }

//            // if we are an extension then we need to encode
//            if (phoneNumber.Contains(',') || phoneNumber.Contains(';'))
//                phoneNumber = URLEncoder.Encode(phoneNumber, "UTF-8");

//            var dialIntent = ResolveDialIntent(phoneNumber);

//            var flags = ActivityFlags.ClearTop | ActivityFlags.NewTask;
//            if (SystemInfo.HasApiLevelN)
//                flags |= ActivityFlags.LaunchAdjacent;
//            dialIntent.SetFlags(flags);

//            AndroidApp.Context.StartActivity(dialIntent);
//            return null;
//        }

//        static Intent ResolveDialIntent(string number)
//        {
//            var telUri = AndroidUri.Parse($"tel:{number}");
//            return new Intent(Intent.ActionDial, telUri);
//        }


//	}
//}