using System;
using Windows.UI.Xaml;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;
using Dwares.Dwarf;


namespace Dwares.Druid.UWP
{
	public class ViewRendererEx<TElement, TNativeElement> : ViewRenderer<TElement, TNativeElement>
		where TElement : View
		where TNativeElement : FrameworkElement, new()
	{
		Windows.UI.Xaml.Style defaultStyle;

		public ViewRendererEx() { }

		protected static Windows.UI.Xaml.Style GetStyle(string styleName)
		{
			try {
				var resources = Windows.UI.Xaml.Application.Current.Resources;
				if (resources.TryGetValue(styleName, out object value)) {
					if (value is Windows.UI.Xaml.Style style) {
						return style;
					} else {
						Debug.Print("Resource \"{0}\" is not Style", styleName);
					}
				} else {
					Debug.Print("ControlTemplate \"{0}\" not found", styleName);
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
			}
			return null;
		}

		protected void SetControlStyle(string styleName)
		{
			if (Control == null)
				return;

			Windows.UI.Xaml.Style style;
			if (String.IsNullOrEmpty(styleName)) {
				style = defaultStyle;
			} else {
				style = GetStyle(styleName);
			}

			if (style != null) {
				Control.Style = style;
			}
		}

		protected bool CreateControl()
		{
			if (Control == null) {
				if (Element != null) {
					var control = new TNativeElement();
					defaultStyle = control.Style;
					SetNativeControl(control);
				} else
					return false;
			}
			return true;
		}
	}
}
