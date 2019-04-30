using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SkiaSharp;
using AssetWerks.Model;


namespace AssetWerks
{
	public class MainViewModel : ViewModel
	{
		public MainViewModel()
		{
			SelectedPlatform = TargetPlatform.ByName("Android");
			SelectedBadge = Badge.ByName("None");
		}


		public IList<TargetPlatform> Paltforms {
			get => TargetPlatform.List;
		}

		public IList<Badge> Badges {
			get => Badge.List;
		}

		string outputFolder;
		public string OutputFolder {
			get => outputFolder ?? string.Empty;
			set => SetProperty(ref outputFolder, value);
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
			set =>SetProperty(ref sourceImage, value);
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
				Debug.WriteLine($"SelectedPlatform: {value}");
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
				}
			}
		}

		bool badgeImageEnabled;
		public bool BadgeImageEnabled {
			get => badgeImageEnabled;
			set => SetProperty(ref badgeImageEnabled, value);
		}
	}
}
