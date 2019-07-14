using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.Resources
{
	public class MaterualDarkColorScheme : MaterialColorScheme
	{
		public MaterualDarkColorScheme() : base(new MaterialDarkResources()) { }
	}


	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MaterialDarkResources : ResourceDictionary
	{
		public MaterialDarkResources()
		{
			InitializeComponent();
		}
	}
}