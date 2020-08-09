using System;
using System.Threading.Tasks;
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

		protected override async Task ReloadItems(CollectionViewReloadMode mode)
		{
			if (mode == CollectionViewReloadMode.Fast) {
				AppScope.Instance.Customers.Clear();
				await AppStorage.Instance.LoadCustomers();
			} else {
				await base.ReloadItems(mode);
			}
		}
	}
}
