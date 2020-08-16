using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dwares.Druid.Android
{
	public class SystemInfo
	{
		static int? sdkInt;
		static int SdkInt
			=> sdkInt ??= (int)Build.VERSION.SdkInt;

		public static bool HasApiLevel(BuildVersionCodes versionCode) =>
			SdkInt >= (int)versionCode;

		internal static bool HasApiLevelN =>
#if __ANDROID_24__
			HasApiLevel(BuildVersionCodes.N);
#else
			false;
#endif

		internal static bool HasApiLevelNMr1 =>
#if __ANDROID_25__
			HasApiLevel(BuildVersionCodes.NMr1);
#else
			false;
#endif
	}
}
