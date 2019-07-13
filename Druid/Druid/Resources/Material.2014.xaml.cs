using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Resources
{

	public class MaterialColorPalette : ColorPalette
	{
		public MaterialColorPalette() : base(new Material_2014()) { }

		public override bool TryGetColor(string name, string variant, out Color color)
		{
			if (base.TryGetColor(name, variant, out color))
				return true;

			if (variant == null && base.TryGetColor(name, MaterialDesign.DefaultColorVariant, out color))
				return true;

			return false;
		}
	}

	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Material_2014 : ResourceDictionary
	{
		public Material_2014()
		{
			InitializeComponent();
		}

	}

}
