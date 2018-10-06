using System;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Dwares.Druid.Support
{
	public static class Navigator
	{
		public static event EventHandler CurrentPageChanged;

		public static NavigationPage NavigationPage { get; private set; }
		public static INavigation Navigation { get; private set; }
		public static bool Animated { get; set; } = true;

		public static Page CurrentPage => NavigationPage?.CurrentPage;
		public static bool HasModal => Navigation != null && Navigation.ModalStack != null && Navigation.ModalStack.Count > 0;
		public static Page ModalPage => HasModal ? Navigation.ModalStack[Navigation.ModalStack.Count - 1] : null;

		public static void Init(NavigationPage navigationPage)
		{
			NavigationPage = navigationPage;
			Navigation = navigationPage.Navigation;
			navigationPage.Popped += OnNavigationPageChanged;
			navigationPage.Pushed += OnNavigationPageChanged;
			navigationPage.PoppedToRoot += OnNavigationPageChanged;
			InvokeCurrentPageChanged();
		}

		private static void OnNavigationPageChanged(object sender, NavigationEventArgs e)
		{
			InvokeCurrentPageChanged();
		}

		private static void InvokeCurrentPageChanged()
		{
			if (CurrentPageChanged != null) {
				CurrentPageChanged.Invoke(typeof(Navigator), EventArgs.Empty);
			}

		}
		public static async Task NavigateToRoot()
		{
			if (Navigation != null) {
				await Navigation.PopToRootAsync(Animated);
			}
		}

		public static async Task NavigateToModal(Page page)
		{
			await PushPageAsync(page, true);
		}

		public static async Task NavigateTo(Page page)
		{
			await PushPageAsync(page, false);
		}

		public static async void NavigateBack()
		{
			await PopPageAsync();
		}

		public static async Task PushPageAsync(Page page, bool modal)
		{
			if (page != null && Navigation != null && !HasModal) {
				if (modal) {
					await Navigation.PushModalAsync(page, Animated);
				} else {
					await Navigation.PushAsync(page, Animated);
				}

				//InvokeCurrentPageChanged();
			}
		}

		public static async Task PopPageAsync()
		{
			if (Navigation != null && CurrentPage != null) {
				if (HasModal) {
					await Navigation.PopModalAsync(Animated);
				} else {
					await Navigation.PopAsync(Animated);
				}
				//InvokeCurrentPageChanged();
			}
		}

	}
}
