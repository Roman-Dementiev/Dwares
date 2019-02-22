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
		}

		private void Center_Clicked(object sender, EventArgs e)
		{
			Mode = FramedPageMode.Center;
		}

		private void Extend_Clicked(object sender, EventArgs e)
		{
			Mode = FramedPageMode.Extend;
		}

		private void FullScreen_Clicked(object sender, EventArgs e)
		{
			Mode = FramedPageMode.FullScreen;
		}
	}
}