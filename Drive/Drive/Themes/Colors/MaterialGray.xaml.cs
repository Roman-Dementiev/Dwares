using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Drive.Themes.Colors
{
	public class MaterialGray : MaterialColorScheme
	{
		public MaterialGray() : base(new MaterialGrayResources(), "Material.Gray") { }
	}


	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialGrayResources : ResourceDictionary
	{
		public MaterialGrayResources()
		{
			InitializeComponent();
		}
	}
}