using System;
using Dwares.Dwarf;
using Xamarin.Forms;

// https://material.io/design/color/the-color-system.html#


namespace Dwares.Druid.Satchel
{
	public class MaterialColorScheme : ColorScheme
	{
		//static ClassRef @class = new ClassRef(typeof(MaterialColorScheme));

		public MaterialColorScheme(string name) :
			base(name, MaterialDesign.Name)
		{
			//Debug.EnableTracing(@class);
		}

		public MaterialColorScheme(ResourceDictionary dict, string name = null) :
			base(dict, name, MaterialDesign.Name)
		{
			//Debug.EnableTracing(@class);
		}

		public Color PrimaryColor {
			get => this.GetColor(nameof(PrimaryColor));
		}

		public Color SecondaryColor {
			get => this.GetColor(nameof(SecondaryColor));
		}

		public Color PrimaryVariantColor {
			get => this.GetColor(nameof(PrimaryVariantColor));
		}

		public Color SecondaryVarianrColor {
			get => this.GetColor(nameof(SecondaryVarianrColor));
		}

		public Color BackgroundColor {
			get => this.GetColor(nameof(BackgroundColor));
		}

		public Color SurfaceColor {
			get => this.GetColor(nameof(SurfaceColor));
		}

		public Color ErrorColor {
			get => this.GetColor(nameof(ErrorColor));
		}

		public Color OnPrimaryColor {
			get => this.GetColor(nameof(OnPrimaryColor));
		}

		public Color OnSecondaryColor {
			get => this.GetColor(nameof(OnSecondaryColor));
		}

		public Color OnSurfaceColor {
			get => this.GetColor(nameof(OnSurfaceColor));
		}

		public Color OnErrorColor {
			get => this.GetColor(nameof(OnErrorColor));
		}
	}
}
