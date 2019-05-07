using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using SkiaSharp;
using AssetWerks.Model;
using Windows.Storage;

namespace AssetWerks
{
	public abstract class IconsViewModel : ViewModel
	{
		public ObservableCollection<IconGroup> IconGroups { get; }
		public IList<string> Titles { get; }

		List<SKImage> needDispose;

		public IconsViewModel()
		{
			IconGroups = new ObservableCollection<IconGroup>();
			Titles = new List<string>();
		}

		public IconsViewModel(params string[] titles) : this()
		{
			foreach (var title in titles) {
				Titles.Add(title);
			}
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

		void ToDispose(SKImage image)
		{
			if (image == null)
				return;

			if (needDispose == null) {
				needDispose = new List<SKImage>();
			}
			needDispose.Add(image);
		}

		void DisposeOldBitmaps()
		{
			if (needDispose != null) {
				foreach (var bitmap in needDispose) {
					bitmap.Dispose();
				}
				needDispose = null;
			}
		}


		public void CreateImages(Badge badge, SKImage sourceImage, SKColor? badgeColor, SKColor? iconColor)
		{
			if (sourceImage != null && iconColor != null) {
				using (var coloredImage = Skia.ApplyColor(sourceImage, (SKColor)iconColor)) {
					CreateImages(badge, coloredImage, badgeColor, null);
				}
				return;
			}

			for (int groupIndex = 0; groupIndex < IconGroups.Count; groupIndex++) {
				var group = IconGroups[groupIndex];
				if (group == null)
					continue;

				for (int iconIndex = 0; iconIndex < group.Icons.Count; iconIndex++) {
					var icon = group.Icons[iconIndex];
					if (icon == null)
						continue;

					var bitmap = new SKBitmap(icon.ImageWidth, icon.ImageHeight);
					try {
						using (var canvas = new SKCanvas(bitmap))
						using (var paint = new SKPaint { IsAntialias = false, IsDither = false, BlendMode= SKBlendMode.SrcATop })
						{
							canvas.Clear();

							var badgeRect = icon.BadgeRect;
							var iconInset = new SKRect(0,0,0,0);
							if (badge != null) {
								badge.Draw(canvas, badgeRect, badgeColor);
								iconInset = badge.IconInset;
							}

							if (sourceImage != null) {
								// TODO
								//var iconRect = icon.BadgeRect;
								var iconRect = new SKRect(
									badgeRect.Left + badgeRect.Width * iconInset.Left,
									badgeRect.Top + badgeRect.Height * iconInset.Top,
									badgeRect.Right - badgeRect.Width * iconInset.Right,
									badgeRect.Bottom - badgeRect.Height * iconInset.Bottom
									);
								canvas.DrawImage(sourceImage, iconRect/*, paint*/);
							}

							ToDispose(icon.Image);
							icon.Image = SKImage.FromBitmap(bitmap);
						}
					}
					catch {
						bitmap.Dispose();
						throw;
					}
					finally {
						DisposeOldBitmaps();
					}
				}		
			}
		}

		public abstract Task Save(StorageFolder outputFolder);
	}
}
