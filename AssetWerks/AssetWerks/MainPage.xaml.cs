using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Windows.Input;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using SkiaSharp;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AssetWerks
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		const string Platform_Android = "Android";
		const string Platform_iOS = "iOS";
		const string Platform_UWP = "UWP";

		const int AssetsView_Row = 1;
		const int AssetsView_Col = 1;
		const int AssetsView_ColSpan = 2;

		//public event PropertyChangedEventHandler PropertyChanged;
		ICommand ChooseOutputFolderCommand { get; }
		ICommand ChooseSourceImageCommand { get; }
		ICommand ChooseBadgeImageCommand { get; }
		ICommand PreviewCommand { get; }

		public MainPage()
		{
			this.InitializeComponent();
			
			DataContext = ViewModel = new MainViewModel();
			ViewModel.PropertyChanged += ViewModel_PropertyChanged;

			ChooseOutputFolderCommand = new Command(ChooseOutputFolder);
			ChooseSourceImageCommand = new Command(ChooseSourceImage);
			ChooseBadgeImageCommand = new Command(ChooseBadgeImage);
			PreviewCommand = new Command(Preview);

			TargetPlatform = ViewModel.SelectedPlatform;
		}

		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(MainViewModel.SelectedPlatform)) {
				TargetPlatform = ViewModel.SelectedPlatform;
			}
		}

		MainViewModel ViewModel { get; }

		TargetPlatform platform;
		public TargetPlatform TargetPlatform {
			get => platform;
			set {
				if (value != platform) {
					platform = value;
					assetsView.Content = platform?.CreateAssetsView();
					oprionsView.Content = platform.CreateOptionsView();
				}
			}
		}

		async void ChooseSourceImage() => await ChooseImage((image) => { ViewModel.SourceImage = image; });
		async void ChooseBadgeImage() => await ChooseImage((image) => { ViewModel.BadgeImage = image; });

		async Task ChooseImage(Action<SKImage> action)
		{
			var openPicker = new FileOpenPicker();
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			//openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".jpeg");
			openPicker.FileTypeFilter.Add(".png");

			var file = await openPicker.PickSingleFileAsync();
			if (file != null) {
				ViewModel.SourceImagePath = file.Path;
				ViewModel.SourceImage = null;

				try {
					using (var inputStream = await file.OpenStreamForReadAsync())
					using (var memoryStream = new MemoryStream()) {
						await inputStream.CopyToAsync(memoryStream);
						memoryStream.Position = 0;

						using (var bitmap = SKBitmap.Decode(memoryStream)) {
							var image = SKImage.FromBitmap(bitmap);
							action(image);
						}
					}
				}
				catch (Exception exc) {
					Debug.WriteLine($"Can not load file {file.Path}: {exc.Message}");
				}
			}
		}

		async void ChooseOutputFolder()
		{
			var folderPicker = new FolderPicker();
			//folderPicker.SuggestedStartLocation = PickerLocationId.Desktop;
			folderPicker.FileTypeFilter.Add("*");

			var folder = await folderPicker.PickSingleFolderAsync();
			if (folder != null) {
				// Application now has read/write access to all contents in the picked folder
				// (including other sub-folder contents)
				//Windows.Storage.AccessCache.StorageApplicationPermissions.
				//FutureAccessList.AddOrReplace("PickedFolderToken", folder);
				//this.textBlock.Text = "Picked folder: " + folder.Name;

				ViewModel.OutputFolder = folder.Path;
			}
		}

		void Preview()
		{
			var platform = TargetPlatform;
			platform.GetIconsViewModel().CreateImages(ViewModel.SelectedBadge);

			this.platform = null;
			TargetPlatform = platform;
		}
	}

	internal class Command : ICommand
	{
		public Command(Action action)
		{
			Action = action;
		}

		public event EventHandler CanExecuteChanged;
		public Action Action { get; }

		public bool CanExecute(object parameter) => true;
		public void Execute(object parameter) => Action();
	}
}
