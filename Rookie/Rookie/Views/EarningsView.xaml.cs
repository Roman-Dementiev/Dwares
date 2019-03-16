using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dwares.Rookie.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class EarningsView : ContentView
	{
		public EarningsView()	
		{
			InitializeComponent();

			BindingContext = new ViewModels.EarningsViewModel();
		}
	}
}