using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Resources
{
	public class MaterialLightColorScheme : MaterialColorScheme
	{
		public MaterialLightColorScheme() : base(new MaterialLightResources()) { }
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialLightResources : ResourceDictionary
	{
		public MaterialLightResources()
		{
			InitializeComponent();
		}
	}
}