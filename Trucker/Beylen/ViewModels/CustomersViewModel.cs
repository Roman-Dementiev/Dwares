using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;


namespace Beylen.ViewModels
{
	public class CustomersViewModel : CollectionViewModel<CustomerCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(CustomersViewModel));

		public CustomersViewModel() :
			base(ApplicationScope, CustomerCardModel.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Customers";
		}
	}
}
