using System;
using Xamarin.Forms;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.UI;


namespace Dwares.Druid
{
	public static class Forge
	{
		public static object GetInstance(object instanceOrType)
		{
			if (instanceOrType is Type type) {
				return Activator.CreateInstance(type);
			} else {
				return instanceOrType;
			}
		}

		public static T CreateBindable<T>(object context) where T : BindableObject
		{
			var obj = ClassLocator.Create<T>(context);
			if (obj != null) {
				obj.BindingContext = GetInstance(context);
			}
			return obj;
		}

		public static Page CreatePage(object viewModel)
		{
			var page = CreateBindable<Page>(viewModel);
			return page;
		}

		public static View CreateView(object viewModel)
		{
			var view = CreateBindable<View>(viewModel);
			return view;
		}

		public static View CreateView(Type viewType, object viewModel)
		{
			if (viewType == null)
				return CreateView(viewModel);

			var view = Activator.CreateInstance(viewType) as View;
			if (view != null) {
				view.BindingContext = GetInstance(viewModel);
			}
			return view;
		}

		public static T CreateContentPage<T>(object contentViewModel) where T : ContentPageEx
		{
			return CreateContentPage<T>(contentViewModel, out var contentView);
		}

		public static T CreateContentPage<T>(object contentViewModel, out View contentView) where T : ContentPageEx
		{
			contentView = CreateView(contentViewModel);
			if (contentView != null) {
				var page = Activator.CreateInstance(typeof(T)) as T;
				if (page != null) {
					page.ContentView = contentView;
					return page;
				}
			}
			return null;
		}

		public static ContentPageEx CreateContentPage(Type contentViewModel)
		{
			return CreateContentPage<ContentPageEx>(contentViewModel);
		}


		public static T CreateContentPageByView<T>(object contentViewOrType) where T : ContentPageEx
		{
			var contentView = GetInstance(contentViewOrType) as View;
			if (contentView != null) {
				var page = Activator.CreateInstance(typeof(T)) as T;
				if (page != null) {
					page.ContentView = contentView;
					return page;
				}
			}
			return null;
		}
	}
}
