using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Resources
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialPalette : ColorPalette
	{
		public const string DefaultName = "MaterialPalette.2014";

		public MaterialPalette()
		{
			InitializeComponent();
		}
	}
}
