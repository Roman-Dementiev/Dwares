using System;
using System.Collections;
using Dwares.Dwarf;
using Dwares.Druid;
using Passket.Models;


namespace Passket.ViewModels
{
	public class GroupListViewModel : EntityListViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(GroupListViewModel));

		public GroupListViewModel() :
			base("Groups", AppData.Groups)
		{
			//Debug.EnableTracing(@class);
		}

		protected override async void OpenItem(object item)
		{
			if (item is Group group) {
				var viewModel = new RecordListViewModel(group.Name, group);
				var page = viewModel.CreatePage();
				await Navigator.PushPage(page);
			}
		}
	}
}
