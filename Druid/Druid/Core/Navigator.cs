using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Dwares.Druid
{
	public static class Navigator
	{
		//public static event EventHandler NavigationPageChanged;
		public static event EventHandler CurrentPageChanged;

		public static bool Animated { get; set; } = true;
		public static Page RootPage { get; private set; }
		public static INavigation Navigation => NavigationPage?.Navigation;

		private static NavigationPage navigationPage;
		public static NavigationPage NavigationPage
		{
			get => navigationPage;
			set {
				if (value == navigationPage)
					return;

				if (navigationPage != null) {
					navigationPage.Popped -= OnNavigationPageChanged;
					navigationPage.Pushed -= OnNavigationPageChanged;
					navigationPage.PoppedToRoot -= OnNavigationPageChanged;
				}

				navigationPage = value;

				if (navigationPage != null) {
					navigationPage.Popped += OnNavigationPageChanged;
					navigationPage.Pushed += OnNavigationPageChanged;
					navigationPage.PoppedToRoot += OnNavigationPageChanged;
				}

				//CurrentPage = navigationPage.CurrentPage;
				//InvokeNavigationPageChanged();
				InvokeCurrentPageChanged();
			}
		}

		public static Page ModalPage => GetCurrentPage(true, true);
		public static Page ContentPage => GetCurrentPage(true);
		public static Page CurrentPage => GetCurrentPage(false);

		public static bool HasModal {
			get => !Collection.IsNullOrEmpty(Navigation?.ModalStack);
		}

		public static Page GetCurrentPage(bool content, bool? modal = null)
		{
			Page currentPage = null;

			if (NavigationPage != null)
			{
				if (modal != false) {
					currentPage = Collection.Last(Navigation?.ModalStack);
				}

				if (modal != true && currentPage == null) {
					currentPage = NavigationPage.CurrentPage;
				}

				if (content && currentPage is NavigationPage navigationPage) {
					currentPage = navigationPage.CurrentPage;
				}
			}

			return currentPage;
		}

		public static void Initialize(Page rootPage = null)
		{
			if (rootPage == null) {
				rootPage = Application.Current.MainPage;
				Debug.AssertNotNull(rootPage);
			}
			RootPage = rootPage;
			navigationPage = null;
			//currentPage = null;

			if (RootPage is NavigationPage navPage) {
				NavigationPage = navPage;
				//InvokeCurrentPageChanged();
			}
			else if (RootPage is MultiPage<Page> multiPage) {
				multiPage.CurrentPageChanged += (sender, e) => MultiPage_CurrentPageChanged();
				MultiPage_CurrentPageChanged();
			} else {
				throw new Exception(String.Format("Can not initialize Navigator for RootPage={0}", RootPage));
			}
		}

		private static void MultiPage_CurrentPageChanged()
		{
			var multiPage = RootPage as MultiPage<Page>;
			var navigationPage = multiPage.CurrentPage as NavigationPage;
			if (navigationPage == NavigationPage)
				return;

			if (NavigationPage != null) {
				NavigationPage.Popped -= OnNavigationPageChanged;
				NavigationPage.Pushed -= OnNavigationPageChanged;
				NavigationPage.PoppedToRoot -= OnNavigationPageChanged;
			}

			NavigationPage = navigationPage;

			if (NavigationPage != null) {
				NavigationPage.Popped += OnNavigationPageChanged;
				NavigationPage.Pushed += OnNavigationPageChanged;
				NavigationPage.PoppedToRoot += OnNavigationPageChanged;
			}

			InvokeCurrentPageChanged();
		}

		private static void OnNavigationPageChanged(object sender, NavigationEventArgs e)
		{
			InvokeCurrentPageChanged();
		}

		public static bool CanNavigate()
		{
			return Navigation != null;
		}

		//public static async Task NavigateTo(Page page)
		//{
		//	await PushPage(page, false);
		//}

		//public static async void NavigateBack()
		//{
		//	await PopPageAsync();
		//}

		public static async Task PushModal(Page page)
		{
			await PushPage(page, true);
		}

		public static async Task PushPage(Page page, bool modal = false)
		{
			if (page == null)
				return;

			if (Navigation == null) {
				Debug.Fail("Navigation Page not initialized");
				return;
			}

			if (!modal && HasModal) {
				Debug.Fail("Can not navigate to modeless page while in modal");
				return;
			}

			if (modal) {
				await Navigation.PushModalAsync(page, Animated);
			} else {
				await Navigation.PushAsync(page, Animated);
				//await NavigationPage.PushAsync(page);
			}
		}

		public static async Task PopPage()
		{
			if (Navigation != null /*&& CurrentPage != null*/) {
				if (HasModal) {
					await Navigation.PopModalAsync(Animated);
				} else {
					var page = await NavigationPage.PopAsync(Animated);
				}
			}
		}

		public static async Task PopToRoot()
		{
			if (Navigation != null) {
				await Navigation.PopToRootAsync(Animated);
			}
		}

		public static void RemovePreviousPage()
		{
			if (Navigation != null && Navigation.NavigationStack.Count > 1) {
				Navigation.RemovePage(Navigation.NavigationStack[Navigation.NavigationStack.Count-2]);
			}
		}

		public static async Task ReplaceTopPage(Page page)
		{
			await PushPage(page);
			RemovePreviousPage();
		}

		//private static void InvokeNavigationPageChanged()
		//{
		//	NavigationPageChanged?.Invoke(typeof(Navigator), EventArgs.Empty);
		//}

		private static void InvokeCurrentPageChanged()
		{
			CurrentPageChanged?.Invoke(typeof(Navigator), EventArgs.Empty);
		}
	}
}
