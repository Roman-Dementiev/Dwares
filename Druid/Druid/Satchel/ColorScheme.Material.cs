using System;
using Dwares.Dwarf;
using Xamarin.Forms;

// https://material.io/design/color/the-color-system.html#


namespace Dwares.Druid.Satchel
{
	public class MaterialColorScheme : ColorScheme
	{
		//static ClassRef @class = new ClassRef(typeof(MaterialColorScheme));

		public const string DesignName = "Material";

		public MaterialColorScheme(string name = null) :
			base(name, DesignName)
		{
			//Debug.EnableTracing(@class);
		}

		public MaterialColorScheme(ResourceDictionary dict) :
			base(dict)
		{
			//Debug.EnableTracing(@class);
		}


		//Color? primaryColor;
		//public Color PrimaryColor {
		//	get => Get(ref primaryColor, nameof(PrimaryColor));
		//	set => Set(ref primaryColor, value);
		//}

		//Color? secondaryColor;
		//public Color SecondaryColor {
		//	get => Get(ref secondaryColor, nameof(SecondaryColor));
		//	set => Set(ref secondaryColor, value);
		//}


		//Color? primaryVariantColor;
		//public Color PrimaryVariantColor {
		//	get => Get(ref primaryVariantColor, nameof(PrimaryVariantColor));
		//	set => Set(ref primaryVariantColor, value);
		//}

		//Color? secondaryVarianrColor;
		//public Color SecondaryVarianrColor {
		//	get => Get(ref secondaryVarianrColor, nameof(SecondaryVarianrColor));
		//	set => Set(ref secondaryVarianrColor, value);
		//}

		//Color? backgroundColor;
		//public Color BackgroundColor {
		//	get => Get(ref backgroundColor, nameof(BackgroundColor));
		//	set => Set(ref backgroundColor, value);
		//}

		//Color? surfaceColor;
		//public Color SurfaceColor {
		//	get => Get(ref surfaceColor, nameof(SurfaceColor));
		//	set => Set(ref surfaceColor, value);
		//}

		//Color? errorColor;
		//public Color ErrorColor {
		//	get => Get(ref errorColor, nameof(ErrorColor));
		//	set => Set(ref errorColor, value);
		//}

		//Color? onPrimaryColor;
		//public Color OnPrimaryColor {
		//	get => Get(ref onPrimaryColor, nameof(OnPrimaryColor));
		//	set => Set(ref onPrimaryColor, value);
		//}

		//Color? onSecondaryColor;
		//public Color OnSecondaryColor {
		//	get => Get(ref onSecondaryColor, nameof(OnSecondaryColor));
		//	set => Set(ref onSecondaryColor, value);
		//}

		//Color? onBackgroundColor;
		//public Color OnBackgroundColor {
		//	get => Get(ref onBackgroundColor, nameof(OnBackgroundColor));
		//	set => Set(ref onBackgroundColor, value);
		//}

		//Color? onSurfaceColor;
		//public Color OnSurfaceColor {
		//	get => Get(ref onSurfaceColor, nameof(OnSurfaceColor));
		//	set => Set(ref onSurfaceColor, value);
		//}

		//Color? onErrorColor;
		//public Color OnErrorColor {
		//	get => Get(ref onErrorColor, nameof(OnErrorColor));
		//	set => Set(ref onErrorColor, value);
		//}

		//protected override void ApplyDesign()
		//{ 
		//}
	}
}
