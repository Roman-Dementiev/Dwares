using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace Dwares.Druid.Support
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

				//NavigationPageChanged?.Invoke(typeof(Navigator), EventArgs.Empty);
				CurrentPage = navigationPage.CurrentPage;
			}
		}

		private static Page currentPage;
		public static Page CurrentPage
		{
			get => currentPage;
			set {
				if (value != currentPage) {
					currentPage = value;
					CurrentPageChanged?.Invoke(typeof(Navigator), EventArgs.Empty);
				}
			}
		}


		public static bool HasModal {
			get => !Collection.IsNullOrEmpty(Navigation?.ModalStack);
		}
		public static Page ModalPage {
			get => Collection.LastElement(Navigation?.ModalStack);
		}

		public static void Initialize(Page rootPage = null)
		{
			if (rootPage == null) {
				rootPage = Application.Current.MainPage;
				Debug.AssertNotNull(rootPage);
			}
			RootPage = rootPage;
			navigationPage = null;
			currentPage = null;

			if (RootPage is NavigationPage navPage) {
				NavigationPage = navPage;
				CurrentPage = NavigationPage.CurrentPage;
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

			CurrentPage = NavigationPage.CurrentPage;
		}

		private static void OnNavigationPageChanged(object sender, NavigationEventArgs e)
		{
			CurrentPage = NavigationPage.CurrentPage;
		}

		public static bool CanNavigate()
		{
			return Navigation != null;
		}

		public static async Task NavigateToRoot()
		{
			if (Navigation != null) {
				await Navigation.PopToRootAsync(Animated);
			}
		}

		public static async Task NavigateToModal(Page page)
		{
			await PushPageAsync(new NavigationPage(page), true);
		}

		public static async Task NavigateTo(Page page)
		{
			await PushPageAsync(new NavigationPage(page), false);
		}

		public static async void NavigateBack()
		{
			await PopPageAsync();
		}

		public static async Task PushPageAsync(Page page, bool modal)
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
			}
		}

		public static async Task PopPageAsync()
		{
			if (Navigation != null /*&& CurrentPage != null*/) {
				if (HasModal) {
					await Navigation.PopModalAsync(Animated);
				} else {
					await Navigation.PopAsync(Animated);
				}
			}
		}

		//private static void InvokeCurrentPageChanged()
		//{
		//	CurrentPageChanged?.Invoke(typeof(Navigator), EventArgs.Empty);
		//}
	}
}
