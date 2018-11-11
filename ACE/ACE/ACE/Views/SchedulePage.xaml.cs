using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Dwarf;
using ACE.ViewModels;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SchedulePage : ContentPageEx
	{
		public SchedulePage() :
			base(new ScheduleViewModel())
		{
			InitializeComponent();
		}
	}
}