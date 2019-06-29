using System;
using Dwares.Dwarf;
using Dwares.Druid.Satchel;
using Xamarin.Forms;

// https://designguidelines.withgoogle.com/wearos/style/color.html


namespace Dwares.Druid.Satchel
{
	public class WearColorScheme : ColorScheme
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		public const string DesignName = "Wear";

		public WearColorScheme(string name = null) :
			base(name, DesignName)
		{
			//Debug.EnableTracing(@class);
		}

		public WearColorScheme(ResourceDictionary dict) :
			base(dict)
		{
			//Debug.EnableTracing(@class);
		}

		//Color? blackBackground;
		//public Color BlackBackground {
		//	get => Get(ref blackBackground, nameof(BlackBackground));
		//	set => Set(ref blackBackground, value);
		//}

		//Color? lighterBackground1;
		//public Color LighterBackground1 {
		//	get => Get(ref lighterBackground1, nameof(LighterBackground1));
		//	set => Set(ref lighterBackground1, value);
		//}

		//Color? lighterBackground2;
		//public Color LighterBackground2 {
		//	get => Get(ref lighterBackground2, nameof(LighterBackground2));
		//	set => Set(ref lighterBackground2, value);
		//}

		//Color? darkerUIElement;
		//public Color DarkerUIElement {
		//	get => Get(ref darkerUIElement, nameof(DarkerUIElement));
		//	set => Set(ref darkerUIElement, value);
		//}

		//Color? lighterUIElement;
		//public Color LighterUIElement {
		//	get => Get(ref lighterUIElement, nameof(LighterUIElement));
		//	set => Set(ref lighterUIElement, value);
		//}

		//Color? activeUIElement;
		//public Color ActiveUIElement {
		//	get => Get(ref activeUIElement, nameof(ActiveUIElement));
		//	set => Set(ref activeUIElement, value);
		//}

		//Color? accentColor;
		//public Color AccentColor {
		//	get => Get(ref accentColor, nameof(AccentColor));
		//	set => Set(ref accentColor, value);
		//}

		//Color? baseColor;
		//public Color BaseColor {
		//	get => Get(ref baseColor, nameof(BaseColor));
		//	set => Set(ref baseColor, value);
		//}

		//void AssignColor(ref Color? color, string flavor, double h, double s, double v, double a)
		//{
		//	if (color == null && !TryGetColor(ref color, nameof(BaseColor), applyDesign: false)) {
		//		color = Colors.FromHsva(h, s, v, a);
		//	}
		//}

		//protected override void ApplyDesign()
		//{
		//	double h, s, v, a;
		//	if (TryGetColor(ref baseColor, nameof(BaseColor), applyDesign : false)) {
		//		BaseColor.ToHsva(out h, out s, out v, out a);
		//		AssignColor(ref accentColor, nameof(AccentColor), h, s, v, a);
		//	}
		//	else if (TryGetColor(ref accentColor, nameof(AccentColor), applyDesign: false)) {
		//		AccentColor.ToHsva(out h, out s, out v, out a);
		//		v = 1;
		//		baseColor = Colors.FromHsva(h, s, v, a);
		//	}
		//	else {
		//		return;
		//	}

		//	AssignColor(ref blackBackground, nameof(blackBackground), h, s, 0, a);
		//	AssignColor(ref lighterBackground1, nameof(LighterBackground1), h, s, v * 0.2, a);
		//	AssignColor(ref lighterBackground2, nameof(LighterBackground2), h, s, v * 0.3, a);
		//	AssignColor(ref darkerUIElement, nameof(DarkerUIElement), h, s, v * 0.4, a);
		//	AssignColor(ref lighterUIElement, nameof(LighterUIElement), h, s, v * 0.55, a);
		//	AssignColor(ref activeUIElement, nameof(ActiveUIElement), h, s, v * 0.65, a);
		//}
	}
}
