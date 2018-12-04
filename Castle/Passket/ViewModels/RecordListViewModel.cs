using System;
using System.Collections;
using Dwares.Dwarf;
using Dwares.Druid;
using Passket.Models;


namespace Passket.ViewModels
{
	public class RecordListViewModel : EntityListViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RecordListViewModel));

		public RecordListViewModel(string title, IList items) :
			base(title, items)
		{
			//Debug.EnableTracing(@class);
		}

		protected override async void OpenItem(object item)
		{
			if (item is Record record) {
				var viewModel = new RecordViewModel(record);
				var page = viewModel.CreatePage();
				await Navigator.PushPage(page);
			}
		}
	}
}
