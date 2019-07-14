using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;


namespace Drive.Themes
{
	public class DarkTheme : UITheme
	{
		public DarkTheme() : base(new DarkThemeResources()) { }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DarkThemeResources : ResourceDictionary
	{
		public DarkThemeResources()
		{
			InitializeComponent();
		}
	}
}