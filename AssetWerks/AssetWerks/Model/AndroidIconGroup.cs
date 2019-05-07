using System;
using SkiaSharp;


namespace AssetWerks.Model
{
	public enum DroidIconDpi
	{
		mdpi ,
		hdpi ,
		xhdpi,
		xxhdpi,
		xxxhdpi
	}

	public class DroidIcon : Icon
	{
		public DroidIcon(DroidIconDpi iconDpi, bool launcher_foreground, bool fullSize = false)
		{
			Title = iconDpi.ToString();
			
			int imageSize;
			switch (iconDpi)
			{
			case DroidIconDpi.mdpi:
				imageSize = launcher_foreground ? 108 : 48;
				break;
			case DroidIconDpi.hdpi:
				imageSize = launcher_foreground ? 162 : 72;
				break;
			case DroidIconDpi.xhdpi:
				imageSize = launcher_foreground ? 216 : 96;
				break;
			case DroidIconDpi.xxhdpi:
				imageSize = launcher_foreground ? 324 : 144;
				break;
			case DroidIconDpi.xxxhdpi:
				imageSize = launcher_foreground ? 432 : 192;
				break;
			default:
				System.Diagnostics.Debug.Fail("Unknown icon dpi");
				imageSize = 0;
				break;
			}

			ImageWidth = ImageHeight = imageSize;

			if (fullSize) {
				IconWidth = imageSize;
				IconHeight = imageSize;
				CellWidth = 440;
				CellHeight = 464;
			}
			else {
				if (launcher_foreground) {
					IconWidth = 2 * imageSize / 3;
					IconHeight = 2 * imageSize / 3;
				} else {
					IconWidth = imageSize;
					IconHeight = imageSize;
				}
				CellWidth = 296;
				CellHeight = 320;
			}
			 
			if (launcher_foreground) {
				int badgeSize = 5 * imageSize / 9;
				int badgeLeft = (imageSize - badgeSize) / 2;
				int badgeTop = (imageSize - badgeSize) / 2;
				int badgeRight = badgeLeft + badgeSize;
				int badgeBottom = badgeTop + badgeSize;
				BadgeRect = new SKRect(badgeLeft, badgeTop, badgeRight, badgeBottom);
			} else {
				BadgeRect = new SKRect(0, 0, imageSize, imageSize);
			}
		}
	}

	public class AndroidIconGroup : IconGroup
	{
		public AndroidIconGroup(string name, bool launcher_foreground = false) :
			base(name)
		{
			Icons.Add(new DroidIcon(DroidIconDpi.mdpi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconDpi.hdpi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconDpi.xhdpi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconDpi.xxhdpi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconDpi.xxxhdpi, launcher_foreground));
		}
	}
}
