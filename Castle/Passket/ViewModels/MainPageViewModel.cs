using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Passket.ViewModels
{
	public class MainPageViewModel : MultiPageViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(MainPageViewModel));

		public MainPageViewModel() :
			base(AppScope)
		{
			//Debug.EnableTracing(@class);
			MainTitle = "Passket";
			TitleMode = MultiPageTitleMode.Combined;

			PageViewModelTypes.Add(typeof(GroupListViewModel));
			PageViewModelTypes.Add(typeof(AssortedListViewModel));
			PageViewModelTypes.Add(typeof(FavoriteListViewModel));
		}
	}
}
