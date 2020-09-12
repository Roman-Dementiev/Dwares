using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;


namespace DraggableListView.Models
{
	public class FireTeam : OrderableCollection<Marine>
	{
		//static ClassRef @class = new ClassRef(typeof(FireTeam));

		//public FireTeam()
		//{
		//	//Debug.EnableTracing(@class);
		//}

		public FireTeam(string teamName, Marine teamLeader)
		{
			//Debug.EnableTracing(@class);

			TeamName = teamName ?? throw new ArgumentNullException(nameof(teamName));
			TeamLeader = teamLeader ?? throw new ArgumentNullException(nameof(teamLeader));
		}

		public string TeamName { get; }
		public Marine TeamLeader { get; }
	}
}
