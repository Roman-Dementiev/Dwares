using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Xaml
{
	public class UITheme : Asset<UI.UITheme>
	{
		//
		public UITheme() { }

		protected override UI.UITheme CreateValue()
		{
			var obj = AssetLocator.CreateInstance(Class);
			if (obj is UI.UITheme theme)
				return theme;

			if (obj is ResourceDictionary resources)
				return new UI.UITheme(resources);

			Debug.Print($"Invalid type of asset value {obj.GetType()} (expected {typeof(UI.UITheme)})");
			return null;
		}
	}
}
