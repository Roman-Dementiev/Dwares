using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Dwares.Druid.Services;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Dwares.Druid.Android
{
	class Cryptography : ICryptography
	{
		public bool HasSymmetricKeys {
			get => true; // TODO: Platform.HasApiLevel(BuildVersionCodes.M)
		}
	}
}
