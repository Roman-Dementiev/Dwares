using System;
using System.Collections.ObjectModel;
using Dwares.Dwarf;


namespace Passket.Models
{
	public class Group : ObservableCollection<Record>, IEntity
	{
		//static ClassRef @class = new ClassRef(typeof(Group));

		public Group()
		{
			//Debug.EnableTracing(@class);
		}

		public uint Id { get; set; }
		public string Icon { get; set; }
		public string Name { get; set; }
		public string Info {
			get {
				if (Count == 1) {
					return "1 record";
				} else {
					return String.Format("{0} records", Count);
				}
			}
		}
	}
}
