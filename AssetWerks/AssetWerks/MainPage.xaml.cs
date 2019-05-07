using System;
using System.ComponentModel;
using System.IO;
using System.Windows.Input;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Controls;
using SkiaSharp;
using System.Threading.Tasks;
using Windows.UI.Popups;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace AssetWerks
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class MainPage : Page
	{
		//const string Platform_Android = "Android";
		//const string Platform_iOS = "iOS";
		//const string Platform_UWP = "UWP";

		public ICommand ChooseOutputFolderCommand { get; }
		public ICommand ChooseSourceImageCommand { get; }
		public ICommand ChooseBadgeImageCommand { get; }
		public ICommand PreviewCommand { get; }
		public ICommand SaveCommand { get; }

		public double MinLeftWidth { get; }
		public double MinRightWidth { get; }

		public MainPage()
		{
			this.InitializeComponent();
			
			DataContext = ViewModel = new MainViewModel();
			ViewModel.PropertyChanged += ViewModel_PropertyChanged;

			ChooseOutputFolderCommand = new Command(ChooseOutputFolder);
			ChooseSourceImageCommand = new Command(ChooseSourceImage);
			ChooseBadgeImageCommand = new Command(ChooseBadgeImage);
			PreviewCommand = new Command(Preview);
			SaveCommand = new Command(Save);

			MinLeftWidth = 180;
			MinRightWidth = 100;

			TargetPlatform = ViewModel.SelectedPlatform;
		}

		private void ViewModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(MainViewModel.SelectedPlatform)) {
				TargetPlatform = ViewModel.SelectedPlatform;
			}
		}

		MainViewModel ViewModel { get; }

		IconsViewModel IconsViewModel {
			get => TargetPlatform.GetIconsViewModel();
		}

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

		async void ChooseSourceImage() => await ChooseImage(ViewModel.SetSourceImage);
		async void ChooseBadgeImage() => await ChooseImage(ViewModel.SetBadgeImage);

		Task ChooseImage(Action<string, SKImage> action)
			=> ChooseImage((path, image, isMask) => action(path, image), false);

		async Task ChooseImage(Action<string, SKImage, bool> action, bool checkForMask = true)
		{
			var openPicker = new FileOpenPicker();
			openPicker.ViewMode = PickerViewMode.Thumbnail;
			//openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
			openPicker.FileTypeFilter.Add(".jpg");
			openPicker.FileTypeFilter.Add(".jpeg");
			openPicker.FileTypeFilter.Add(".png");

			var file = await openPicker.PickSingleFileAsync();
			if (file != null) {
				action(string.Empty, null, false);
				try {
					using (var stream = await file.OpenStreamForReadAsync()) 
					using (var bitmap = SKBitmap.Decode(stream)) {
						var image = SKImage.FromBitmap(bitmap);
						if (checkForMask) {
							action(file.Path, image, Skia.IsGrayscale(bitmap));
						} else {
							action(file.Path, image, false);
						}
					}
				}
				catch (Exception exc) {
					Debug.Print($"Can not load file {file.Path}: {exc.Message}");
				}
			}
		}

		async void ChooseOutputFolder() => await AskToChooseOutputFolder();
	
		async Task<bool> AskToChooseOutputFolder()
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

				ViewModel.OutputFolder = folder;
				Settings.OutputFolder = folder;
				return true;
			} else {
				return false;
			}
		}

		void Preview()
		{
			var platform = TargetPlatform;
			IconsViewModel.CreateImages(ViewModel.SelectedBadge, ViewModel.SourceImage,
				badgeColor.GetColor(), iconColor.GetColor());

			this.platform = null;
			TargetPlatform = platform;
		}

		async void Save()
		{
			if (ViewModel.OutputFolder == null) {
				if (! await AskToChooseOutputFolder())
					return;
			}

			try {
				await IconsViewModel.Save(ViewModel.OutputFolder);
			}
			catch (Exception exc) {
				var messageDialog = new MessageDialog(exc.Message, "Error");
				await messageDialog.ShowAsync();
			}
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
