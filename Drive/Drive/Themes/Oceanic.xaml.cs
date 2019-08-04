using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;


namespace Drive.Themes
{
	public class OceanicTheme : UITheme
	{
		public OceanicTheme() : base(new OceanicThemeResources()) { }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OceanicThemeResources : ResourceDictionary
	{
		public OceanicThemeResources()
		{
			InitializeComponent();
		}
	}
}