using System;
using Xamarin.Forms;
using Dwares.Druid;


namespace Casket.ViewModels
{
	public class MainPageViewModel : BindingScope
	{
		public MainPageViewModel() : base(AppScope)
		{
			//Pages = new Page[] {
			//	new GroupsPage(),
			//	new TypesPage()
			//};

			ViewModels = new BindingScope[] {
				new GroupsViewModel(),
				new TypesViewModel()
			};
		}

		//public Page[] Pages { get; }
		public BindingScope[] ViewModels{ get; }
	}
}
