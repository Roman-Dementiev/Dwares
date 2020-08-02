using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Beylen.Models;

namespace Beylen
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class UIThemes : UIThemeManager
	{
		public UIThemes()
		{
			InitializeComponent();

			SelectTheme(Settings.UITheme);
		}
	}
}
