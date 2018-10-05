using System;
using Xamarin.Forms;


namespace ACE
{
	public class ToolButton : Button
	{
		public ToolButton()
		{
			this.BackgroundColor = Color.Transparent;
			this.HorizontalOptions = LayoutOptions.Center;
			this.VerticalOptions = LayoutOptions.Center;

			//Image = new FileImageSource();
		}
	}
}
