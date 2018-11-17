using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Casket.ViewModels
{
	public class GroupsViewModel : BindingScope
	{
		//ClassRef @class = new ClassRef(typeof(GroupsViewModel));

		public GroupsViewModel() :
			base(AppScope)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
