using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Drive.Models;


namespace Drive.Storage
{
	public interface IAppStorage
	{
		Task Initialize();

		Task LoadContacts();
		Task SaveContacts();
		Task LoadSchedule();
		Task SaveSchedule();
	}
}
