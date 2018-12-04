using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Passket.Models;


namespace Passket.Storage
{
	public abstract class AppStorage
	{
		//static ClassRef @class = new ClassRef(typeof(AppStorage));

		public AppStorage()
		{
			//Debug.EnableTracing(@class);
		}

		public abstract Task Load();
		public abstract Task Save();

		public void PatternAdded(Pattern pattern) { }
		public void GroupAdded(Group group) { }
		public void RecordAdded(Record record) { }
	}
}
