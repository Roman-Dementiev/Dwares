using System;
using SkiaSharp;


namespace AssetWerks.Model
{
	public enum DroidIconSize
	{
		mdi = 6,
		hdi = 9,
		xhdi = 12,
		xxhdi = 18,
		xxxhdi = 24
	}

	public class DroidIcon : Icon
	{
		public DroidIcon(DroidIconSize iconSize, bool launcher_foreground, bool fullSize = true)
		{
			Title = iconSize.ToString();
			
			int factor = launcher_foreground ? 9 : 4;
			ImageWidth = ImageHeight = factor * (int)iconSize;

			if (fullSize) {
				IconWidth = ImageWidth;
				IconHeight = ImageHeight;
				CellWidth = 222;
				CellHeight = 240;
			}
			else {
				if (launcher_foreground) {
					IconWidth = 2 * ImageWidth / 3;
					IconHeight = 2 * ImageHeight / 3;
				} else {
					IconWidth = ImageWidth;
					IconHeight = ImageHeight;
				}
				CellWidth = 152;
				CellHeight = 180;
			}

			int badgeSize = 4 * (int)iconSize;
			int badgeLeft = (ImageWidth - badgeSize) / 2;
			int badgeTop = (ImageHeight - badgeSize) / 2;
			int badgeRight = badgeLeft + badgeSize;
			int badgeBottom = badgeTop + badgeSize;
			BadgeRect = new SKRect(badgeLeft, badgeTop, badgeRight, badgeBottom);
		}
	}

	public class AndroidIconGroup : IconGroup
	{
		public AndroidIconGroup(string name, bool launcher_foreground = false) :
			base(name)
		{
			Icons.Add(new DroidIcon(DroidIconSize.mdi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconSize.hdi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconSize.xhdi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconSize.xxhdi, launcher_foreground));
			Icons.Add(new DroidIcon(DroidIconSize.xxxhdi, launcher_foreground));
		}
	}
}
