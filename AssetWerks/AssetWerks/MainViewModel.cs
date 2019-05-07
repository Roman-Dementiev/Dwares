using System;
using System.Collections;
using System.Collections.Generic;
using SkiaSharp;
using AssetWerks.Model;
using Windows.Storage;
using System.Threading.Tasks;

namespace AssetWerks
{
	public class MainViewModel : ViewModel
	{
		public MainViewModel()
		{
			var targetPatform = Settings.TargetPlatform;
			SelectedPlatform = TargetPlatform.ByName(targetPatform);
			
			var badgeName = Settings.BadgeName;
			SelectedBadge = Badge.ByName(badgeName);

			//var badgeColor = Settings.BadgeColor;
		}

		public IList<TargetPlatform> Paltforms {
			get => TargetPlatform.List;
		}

		public IList<Badge> Badges {
			get => Badge.List;
		}

		public IList Colors {
			get => NamedColor.List;
		}


		StorageFolder outputFolder;
		public StorageFolder OutputFolder {
			get => outputFolder;
			set {
				if (SetProperty(ref outputFolder, value)) {
					OutputFolderPath = outputFolder?.Path;
				}
			}
		}

		string outputFolderPath;
		public string OutputFolderPath {
			get => outputFolderPath ?? string.Empty;
			private set => SetProperty(ref outputFolderPath, value);
		}

		string sourceImagePath;
		public string SourceImagePath { 
			get => sourceImagePath ?? string.Empty;
			set => SetProperty(ref sourceImagePath, value);
		}

		string badgeImagePath;
		public string BadgeImagePath {
			get => badgeImagePath ?? string.Empty;
			set => SetProperty(ref badgeImagePath, value);
		}

		SKImage sourceImage;
		public SKImage SourceImage { 
			get => sourceImage;
			set => SetProperty(ref sourceImage, value);
		}

		SKImage badgeImage;
		public SKImage BadgeImage {
			get => badgeImage;
			set => SetProperty(ref badgeImage, value);
		}

		TargetPlatform selectedPlatform;
		public TargetPlatform SelectedPlatform {
			get => selectedPlatform;
			set {
				Debug.Print($"SelectedPlatform: {value}");
				if (value == selectedPlatform)
					return;

				SetProperty(ref selectedPlatform, value);
				// TODO
			}
		}

		Badge selectedBadge;
		public Badge SelectedBadge {
			get => selectedBadge;
			set {
				if (SetProperty(ref selectedBadge, value)) {
					BadgeImageEnabled = SelectedBadge is ImageBadge;
					Settings.BadgeName = selectedBadge?.Title;
				}
			}
		}

		bool badgeImageEnabled;
		public bool BadgeImageEnabled {
			get => badgeImageEnabled;
			set => SetProperty(ref badgeImageEnabled, value);
		}

		bool iconColorEnabled;
		public bool IconColorEnabled {
			get => iconColorEnabled;
			set => SetProperty(ref iconColorEnabled, value);
		}

		public void SetSourceImage(string path, SKImage image, bool isMask)
		{
			SourceImagePath = path;
			SourceImage = image;
			IconColorEnabled = isMask;
		}

		public void SetBadgeImage(string path, SKImage image)
		{
			BadgeImagePath = path;
			BadgeImage = image;
			if (SelectedBadge is ImageBadge imageBadge) {
				imageBadge.Image = image;
			}
		}
	}
}
