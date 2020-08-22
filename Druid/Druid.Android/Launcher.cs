using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Druid.Services;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AndroidApp = Android.App.Application;
using AndroidUri = Android.Net.Uri;


[assembly: Dependency(typeof(Dwares.Druid.Android.Launcher))]

namespace Dwares.Druid.Android
{
	class Launcher : ILauncher
	{
		public Task OpenUri(Uri uri, Dictionary<string, object> options)
		{
            var intent = new Intent(Intent.ActionView, AndroidUri.Parse(uri.OriginalString));
            var flags = ActivityFlags.ClearTop | ActivityFlags.NewTask;
            if (SystemInfo.HasApiLevelN)
                flags |= ActivityFlags.LaunchAdjacent;
            intent.SetFlags(flags);

            AndroidApp.Context.StartActivity(intent);
            return Task.CompletedTask;
        }
    }
}
