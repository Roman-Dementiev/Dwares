using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Beylen
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AppShell_Market : Shell
	{
		public AppShell_Market()
		{
			InitializeComponent();
		}

		void OnNavigating(object sender, ShellNavigatingEventArgs e)
		{
			//Cancel any back navigation
			//if (e.Source == ShellNavigationSource.Pop) {
			//	e.Cancel();
			//}
		}

		void OnNavigated(object sender, ShellNavigatedEventArgs e)
		{
		}
	}
}
