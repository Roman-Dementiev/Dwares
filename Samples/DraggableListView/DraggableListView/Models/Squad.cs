using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Druid;


namespace DraggableListView.Models
{
	public class Squad : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Squad));

		public Squad()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name { get; set; }

		public Marine SquadLeader { get; set; }

		public GroupedOrderableCollection<FireTeam, Marine> Teams {
			get => teams ??= new GroupedOrderableCollection<FireTeam, Marine>();
			set => teams = value;
		}
		GroupedOrderableCollection<FireTeam, Marine> teams;
	}
}
