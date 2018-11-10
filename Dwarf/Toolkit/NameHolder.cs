using System;


namespace Dwares.Dwarf.Toolkit
{
	public interface INameHolder
	{
		string Name { get; }
	}

	public class NameHolder
	{
		public NameHolder() { }
		
		public NameHolder(string name)
		{
			Name = name;
		}

		public string Name { get; set; }

		public override string ToString() => Name;
	}
}
