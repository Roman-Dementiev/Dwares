using System;
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

			Navigating += OnNavigating;
			Navigated += OnNavigated;

			UITheme.OnCurrentThemeChanged(() => this.ApplyFlavor());
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

		protected virtual void OnNavigating(object sender, ShellNavigatingEventArgs e)
		{
			//Cancel any back navigation
			//if (e.Source == ShellNavigationSource.Pop) {
			//	e.Cancel();
			//}
		}

		protected virtual void OnNavigated(object sender, ShellNavigatedEventArgs e)
		{
		}
	}
}
