using System;
using System.Threading.Tasks;


namespace Beylen.Storage
{
	public interface IAppStorage
	{
		Task Initialize();

		Task LoadContacts();
		Task LoadCustomers();
	}
}
