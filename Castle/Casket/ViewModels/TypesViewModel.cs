using System;
using Dwares.Dwarf;
using Dwares.Druid;


namespace Casket.ViewModels
{
	public class TypesViewModel : BindingScope
	{
		//ClassRef @class = new ClassRef(typeof(TypesViewModel));

		public TypesViewModel() :
			base(AppScope)
		{
			//Debug.EnableTracing(@class);
		}
	}
}
