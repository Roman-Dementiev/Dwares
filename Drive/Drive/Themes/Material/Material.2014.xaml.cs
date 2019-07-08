using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Drive.Themes.Material
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialPalette : ResourceDictionary
	{
		public MaterialPalette()
		{
			InitializeComponent();
		}
	}
}