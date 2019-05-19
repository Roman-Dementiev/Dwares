using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drive.Models;


namespace Drive.Storage
{
	public interface IAppStorage
	{
		Task LoadContacts(IList<IContact> contacts);
		Task SaveContacts(IList<IContact> contacts);
		Task LoadSchedule(IList<Ride> schedule);
		Task SaveSchedule(IList<Ride> schedule);
	}
}
