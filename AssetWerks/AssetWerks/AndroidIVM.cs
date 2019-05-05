using System;
using AssetWerks.Model;

namespace AssetWerks
{
	public class AndroidIVM : IconsViewModel
	{
		public AndroidIVM() : this(true) { }

		protected AndroidIVM(bool init)
		{
			if (init) {
				IconGroups.Add(new AndroidIconGroup("ic_launcher"));
			}
		}
	}

	public class XamarinAndroidIVM : AndroidIVM
	{
		public XamarinAndroidIVM() : base(false)
		{
			IconGroups.Add(new AndroidIconGroup("icon", false));
			IconGroups.Add(new AndroidIconGroup("launcher_foreground", true));
		}
	}
}
