using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;
using Beylen.ViewModels;


namespace Beylen.ViewModels
{
	public class ContactsViewModel : CollectionViewModel<ContactCardModel>
	{
		//static ClassRef @class = new ClassRef(typeof(ContactsViewModel));

		public ContactsViewModel() :
			base(ContactCardModel.CreateCollection())
		{
			//Debug.EnableTracing(@class);

			Title = "Contacts";
		}

		protected override async Task ReloadData(CollectionViewReloadMode mode)
		{
			if (mode == CollectionViewReloadMode.Fast) {
				AppScope.Instance.Customers.Clear();
				await AppStorage.Instance.LoadContacts();
			} else {
				await base.ReloadData(mode);
			}
		}
	}
}
