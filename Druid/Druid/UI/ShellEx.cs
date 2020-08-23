using System;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	public class ShellEx : Shell, IThemeAware
	{
		//static ClassRef @class = new ClassRef(typeof(ShellEx));

		public ShellEx()
		{
			//Debug.EnableTracing(@class);

			//FlyoutHeaderBehavior = FlyoutHeaderBehavior.CollapseOnScroll;

			//Navigating += OnNavigating;
			//Navigated += OnNavigated;

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
			ChildAdded += ShellEx_ChildAdded;
		}

		private void ShellEx_ChildAdded(object sender, ElementEventArgs e)
		{
			if (string.IsNullOrEmpty(MainRoute) && e.Element is ShellItem item)
				MainRoute = item.Route;
		}

		public static readonly BindableProperty FlavorProperty =
			BindableProperty.Create(
				nameof(Flavor),
				typeof(string),
				typeof(ShellEx),
				propertyChanged: (bindable, oldValue, newValue) => {
					if (bindable is ShellEx shell) {
						shell.ApplyFlavor();
					}
				});

		public string Flavor {
			set { SetValue(FlavorProperty, value); }
			get { return (string)GetValue(FlavorProperty); }
		}

		//protected virtual void OnNavigating(object sender, ShellNavigatingEventArgs e)
		//{
		//	//Cancel any back navigation
		//	//if (e.Source == ShellNavigationSource.Pop) {
		//	//	e.Cancel();
		//	//}
		//}

		//protected virtual void OnNavigated(object sender, ShellNavigatedEventArgs e)
		//{
		//}

		public string MainRoute {
			get => mainRoute ?? string.Empty;
			set {
				OnPropertyChanging();
				mainRoute = value;
				OnPropertyChanged();
			}
		}
		string mainRoute;


		//public static async Task GoToMainAsync()
		//{
		//	if (Current is ShellEx shell && !string.IsNullOrEmpty(shell.MainRoute)) {
		//		await Shell.Current.GoToAsync($"///{shell.MainRoute}", true);
		//	}
		//}

		//public static Command GoToMainCommand {
		//	get => LazyInitializer.EnsureInitialized(ref goToMainCommand, () => new Command(async () => await GoToMainAsync()));
		//}		
		//static Command goToMainCommand;
	}
}
