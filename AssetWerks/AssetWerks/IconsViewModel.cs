using System;
using System.Collections.ObjectModel;
using SkiaSharp;
using AssetWerks.Model;

namespace AssetWerks
{
	public class IconsViewModel : ViewModel
	{
		public ObservableCollection<IconGroup> IconGroups { get; }

		protected IconRecord[] Icons { get; set; }

		public IconsViewModel()
		{
			IconGroups = new ObservableCollection<IconGroup>();
		}

		protected SKImage GetImage(int groupIndex, int iconIndex)
		{
			var group = IconGroups[groupIndex];
			if (group != null) {
				return group.Icons[iconIndex]?.Image;
			} else {
				return null;
			}
		}

		protected void SetImage(int groupIndex, int iconIndex, SKImage image)
		{
			var group = IconGroups[groupIndex];
			if (group != null) {
				group.Icons[iconIndex].Image = image;
			}
		}

		protected SKImage GetImage(int index)
		{
			return Icons[index]?.Image;
		}

		protected void SetImage(int index, SKImage image)
		{
			if (Icons[index] != null) {
				Icons[index].Image = image;
				FirePropertyChanged(Icons[index].Property);
			}
		}

		public void CreateImages(Badge badge, SKImage sourceImage)
		{
			for (int groupIndex = 0; groupIndex < IconGroups.Count; groupIndex++) {
				var group = IconGroups[groupIndex];
				if (group == null)
					continue;

				for (int iconIndex = 0; iconIndex < group.Icons.Count; iconIndex++) {
					var icon = group.Icons[iconIndex];
					if (icon == null)
						continue;

					using (var bitmap = new SKBitmap(icon.ImageWidth, icon.ImageHeight))
					using (var canvas = new SKCanvas(bitmap))
					{
						canvas.Clear();

						if (badge != null) {
							badge.Draw(canvas, icon.BadgeRect);
						}
						if (sourceImage != null) {
							canvas.DrawImage(sourceImage, icon.BadgeRect);
						}

						var image = SKImage.FromBitmap(bitmap);
						SetImage(groupIndex, iconIndex, image);
					}
				}
		
			}
		}

		public void CreateImages1(Badge badge)
		{
			for (int i = 0; i < Icons.Length; i++) {
				var icon = Icons[i];
				if (icon == null)
					continue;

				using (var bitmap = new SKBitmap(icon.Size.Width, icon.Size.Height))
				using (var canvas = new SKCanvas(bitmap)) {
					if (badge != null) {
						badge.Draw(canvas, icon.BadgeRect);

						Icons[i].Image = SKImage.FromBitmap(bitmap);
						FirePropertyChanged(icon.Property);
					}
				}
			}
		}

		protected class IconRecord {
			public SKImage Image { get; set; }
			public string Property { get; }
			public SKSizeI Size { get; }
			public SKRectI BadgeRect { get; }
			//public SKRect ImageRect { get; }

			public IconRecord(string property, SKSizeI size, SKRectI? badgeRect)
			{
				Image = null;
				Property = property;
				Size = size;

				if (badgeRect != null) {
					BadgeRect = (SKRectI)badgeRect;
				} else {
					BadgeRect = new SKRectI(0, 0, size.Width, size.Height);
				}
			}

			public IconRecord(string property, int size, int badgeSize) :
				this(property, new SKSizeI(size, size), CenterRect(size, size, badgeSize, badgeSize))
			{
			}

			public IconRecord(string property, int size) :
				this(property, new SKSizeI(size, size), null)
			{
			}

			static SKRectI CenterRect(int width, int height, int rectWidth, int rectHeight)
			{
				int l = (width - rectWidth) / 2;
				int t = (height - rectHeight) / 2;
				int r = l + rectWidth;
				int b = t + rectHeight;

				return new SKRectI(l, t, r, b);
			}

		}
	}
}
