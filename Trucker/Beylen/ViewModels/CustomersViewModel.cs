using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;


namespace Beylen.ViewModels
{
	public class CustomersViewModel : CollectionViewModel<Customer>
	{
		//static ClassRef @class = new ClassRef(typeof(CustomersViewModel));

		public CustomersViewModel() :
			base(ApplicationScope, AppScope.Instance.Customers)
		{
			//Debug.EnableTracing(@class);

			Title = "Customers";
		}
	}
}
