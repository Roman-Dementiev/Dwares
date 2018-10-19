using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.System;
using Xamarin.Forms;
using Dwares.Druid.Services;

[assembly: Dependency(typeof(Dwares.Druid.UWP.Launcher))]

namespace Dwares.Druid.UWP
{
	class Launcher : ILauncher
	{
		const string keyTargetApplicationPackageFamilyName = nameof(LauncherOptions.TargetApplicationPackageFamilyName);

		public async Task OpenUri(Uri uri, Dictionary<string, object> options)
		{
			LauncherOptions launcherOptions = new LauncherOptions();
			if (options != null) {
				if (options.ContainsKey(keyTargetApplicationPackageFamilyName)) {
					var targetApplicationPackageFamilyName = options[keyTargetApplicationPackageFamilyName] as string;
					if (targetApplicationPackageFamilyName != null) {
						launcherOptions.TargetApplicationPackageFamilyName = targetApplicationPackageFamilyName;
					}
				}
			}
			await Windows.System.Launcher.LaunchUriAsync(uri, launcherOptions);
		}
	}
}
