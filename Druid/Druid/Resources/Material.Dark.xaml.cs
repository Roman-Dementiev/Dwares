using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Resources
{
	public class MaterialDarkColorScheme : MaterialColorScheme
	{
		public MaterialDarkColorScheme() : base(new MaterialDarkResources()) { }
	}


	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialDarkResources : ResourceDictionary
	{
		public MaterialDarkResources()
		{
			InitializeComponent();
		}
	}
}