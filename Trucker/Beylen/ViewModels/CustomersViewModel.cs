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

			Title = "Contacts";
		}

		protected override async Task ReloadData(CollectionViewReloadMode mode)
		{
			if (mode == CollectionViewReloadMode.Fast) {
				AppScope.Instance.Customers.Clear();
				await AppStorage.Instance.LoadCustomers();
			} else {
				await base.ReloadData(mode);
			}
		}
	}
}
