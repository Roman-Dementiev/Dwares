using System;
using System.Collections;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Passket.ViewModels
{
	public class EntityListViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(EntityListViewModel));

		public EntityListViewModel(string title, IList items)
		{
			//Debug.EnableTracing(@class);

			Title = title;
			Items = items;
		}

		public IList Items { get; protected set; }
		public object CurrentItem { get; protected set; }

		public void OnItemSelected(object item)
		{
			if (item == CurrentItem) {
				OpenItem(CurrentItem);
			} else {
				CurrentItem = item;
			}
		}

		protected virtual void OpenItem(object item)
		{

		}
	}
}
