using System;
using Xamarin.Forms;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;


namespace Dwares.Druid
{
	public static class Forge
	{
		public static T CreateBindable<T>(Type contextType) where T : BindableObject
		{
			var obj = ClassLocator.Create<T>(contextType);
			if (obj != null) {
				var context = Activator.CreateInstance(contextType);
				obj.BindingContext = context;
			}
			return obj;
		}

		public static Page CreatePage(Type viewModelType)
		{
			var page = CreateBindable<Page>(viewModelType);
			return page;
		}

		public static View CreateView(Type viewModelType)
		{
			var view = CreateBindable<View>(viewModelType);
			return view;
		}

		public static T CreateContentPage<T>(Type contentViewModelType) where T : ContentPageEx
		{
			var contentView = CreateView(contentViewModelType);
			if (contentView != null) {
				var page = Activator.CreateInstance(typeof(T)) as T;
				if (page != null) {
					page.ContentView = contentView;
					return page;
				}
			}
			return null;
		}

		public static ContentPageEx CreateContentPage(Type contentViewModelType) => CreateContentPage<ContentPageEx>(contentViewModelType);
		
		//public static T CreateFramedPage<T>(Type contentViewModelType) where T: FramedPage => CreateContentPage<T>(contentViewModelType);
		//public static FramedPage CreateFramedPage(Type contentViewModelType) => CreateContentPage<FramedPage>(contentViewModelType);
	}
}
