using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using AssetWerks.Model;
using SkiaSharp;


namespace AssetWerks
{
	public class AndroidIVM : IconsViewModel
	{
		public AndroidIVM() : this(true) { }

		protected AndroidIVM(bool init) :
			base("mdi", "hdi", "xhdi", "xxhdi", "xxxhdi")
		{
			if (init) {
				IconGroups.Add(new AndroidIconGroup("ic_launcher"));
			}
		}

		bool createMissingDirectories = true;
		public bool CreateMissingDirectories {
			get => createMissingDirectories;
			set => SetProperty(ref createMissingDirectories, value);
		}

		async Task<StorageFolder> GetSubfolder(StorageFolder parentFolder, string name)
		{
			StorageFolder subFolder = null;
			try {
				subFolder = await parentFolder.GetFolderAsync(name);
			}
			catch (FileNotFoundException exc) {
				if (CreateMissingDirectories) {
					subFolder = await parentFolder.CreateFolderAsync(name);
				} else {
					throw;
				}
			}
			return subFolder;
		}

		public override async Task Save(StorageFolder outputFolder)
		{
			if (outputFolder == null)
				return;

			foreach (var title in Titles) {
				var subFolder = await GetSubfolder(outputFolder, title);

				foreach (var group in IconGroups) {
					var icon = group.GetIcon(title);
					var image = icon?.Image;
					if (image != null) {
						var file = await subFolder.CreateFileAsync(group.Title+".png", CreationCollisionOption.ReplaceExisting);

						using (var stream = await file.OpenStreamForWriteAsync()) {
							SKPixmap pixmap = image.PeekPixels();
							SKData data = pixmap.Encode(SKPngEncoderOptions.Default);

							data.SaveTo(stream);
						}
					} else {
						try {
							var file = await subFolder.GetFileAsync(group.Title);
							await file.DeleteAsync();
						}
						catch {
						}
					}
				}
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
