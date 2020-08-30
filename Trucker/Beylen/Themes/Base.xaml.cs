using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Beylen.Themes
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Base : ResourceDictionary
	{
		public Base()
		{
			InitializeComponent();
		}
	}
}