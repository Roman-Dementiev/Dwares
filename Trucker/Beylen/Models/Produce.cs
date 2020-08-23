using System;
using Dwares.Dwarf;


namespace Beylen.Models
{
	public class Produce : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Produce));

		public Produce()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name {
			get => name;
			set => SetProperty(ref name, value);
		}
		string name;

		public string Package {
			get => package;
			set => SetProperty(ref package, value);
		}
		string package;

		public int Cpp {
			get => cpp;
			set => SetProperty(ref cpp, value);
		}
		int cpp;
	}
}
