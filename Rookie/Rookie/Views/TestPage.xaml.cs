using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;


namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class TestPage : FramedPage
	{
		public TestPage()
		{
			InitializeComponent();

			FrameSize = new Size(300, 500);
			FrameMargin = new Thickness(4);
			BorderColor = Color.Red;
		}

		private void Center_Clicked(object sender, EventArgs e)
		{
			BorderIsVisible = true;
			FrameIsCentered = true;
		}

		private void Extend_Clicked(object sender, EventArgs e)
		{
			BorderIsVisible = true;
			FrameIsCentered = false;
		}

		private void FullScreen_Clicked(object sender, EventArgs e)
		{
			BorderIsVisible = false;
			//	FrameIsCentered = false;
		}
	}
}
