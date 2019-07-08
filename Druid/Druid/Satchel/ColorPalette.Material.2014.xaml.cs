using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Dwarf;


namespace Dwares.Druid.Satchel
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialColorPalette : ColorPalette
	{
		public MaterialColorPalette() :
			base("Material.2014", MaterialDesign.Name)
		{
			InitializeComponent();
		}

		public override bool TryGetColor(string name, string variant, out Color color)
		{
			if (base.TryGetColor(name, variant, out color))
				return true;

			if (variant == null && base.TryGetColor(name, MaterialDesign.DefaultColorVariant, out color))
				return true;
			
			return false;
		}
	}


	// TODO
	public static class MaterialDesign
	{
		public const string Name = "Material";
		public const string DefaultColorVariant = "500";
	}
}
