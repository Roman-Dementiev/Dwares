using System;
using System.Threading.Tasks;
using Buffy.Models;


namespace Buffy.Storage
{
	public interface IAppStorage
	{
		Task Initialize();
		Task LoadData();

		Task AddFueling(Fueling fueling);
		Task UpdateFueling(Fueling oldFueling, Fueling newFueling);
		Task DeleteFueling(Fueling fueling);
	}
}
