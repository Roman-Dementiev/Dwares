using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;


namespace Drive.Themes
{
	public class LightTheme : UITheme
	{
		public LightTheme() : base(new LightThemeResources()) { }
	}


	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class LightThemeResources : ResourceDictionary
	{
		public LightThemeResources()
		{
			InitializeComponent();
		}
	}
}