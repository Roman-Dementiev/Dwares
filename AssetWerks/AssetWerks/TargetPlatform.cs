using System;
using System.Collections.Generic;
using System.Threading;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using AssetWerks.Model;


namespace AssetWerks
{

	public abstract class TargetPlatform : Named
	{
		public TargetPlatform(string name)
		{
			Name = name;
		}

		//IconsViewModel iconsViewModel;
		//public IconsViewModel IconsViewModel {
		//	get => LazyInitializer.EnsureInitialized(ref iconsViewModel, CreateIconsViewModel);
		//}

		public abstract IconsViewModel GetIconsViewModel();

		public virtual FrameworkElement CreateAssetsView()
		{
			return new IconsView(GetIconsViewModel());
		}

		public virtual FrameworkElement CreateOptionsView()
		{
			return null;
		}

		static List<TargetPlatform> list;
		public static List<TargetPlatform> List {
			get => LazyInitializer.EnsureInitialized(ref list, () => {
				var list = new List<TargetPlatform>();
				list.Add(new TargetAndroid());
				//list.Add(new TargetPlatform("iOS"));
				//list.Add(new TargetPlatform("UWP"));
				list.Add(new TargetXamarinAndroid());
				return list;
			});
		}

		public static TargetPlatform ByName(string name) => ByName(List, name);
	}

	public class TargetPlatform<IVM> : TargetPlatform where IVM : IconsViewModel, new()
	{
		public TargetPlatform(string name) : base(name) { }

		IVM iconsViewModel;
		public IVM IconsViewModel {
			get => LazyInitializer.EnsureInitialized(ref iconsViewModel);
		}

		public override IconsViewModel GetIconsViewModel()
		{
			return IconsViewModel;
		}
	}

	public class TargetAndroid : TargetPlatform<AndroidIVM>
	{
		public TargetAndroid() : base("Android") { }

		public override FrameworkElement CreateOptionsView()
		{
			return new AndroidIconsOptions(IconsViewModel);
		}
	}

	public class TargetXamarinAndroid : TargetPlatform<XamarinAndroidIVM>
	{
		public TargetXamarinAndroid() : base("Xamarin (Android)") { }

		public override FrameworkElement CreateOptionsView()
		{
			return new AndroidIconsOptions(IconsViewModel);
		}
	}
}
