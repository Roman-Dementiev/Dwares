using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Drive.Models;

namespace Drive.Storage
{
	internal class MockStorage : IAppStorage
	{
		public Task Initialize()
		{
			return Task.CompletedTask;
		}

		public Task LoadContacts()
		{
			var contacts = AppScope.Instance.Contacts;
			
			contacts.Add(new Place {
				Title = "PASSi",
				FullTitle = "Penn Asian Senior Services",
				Address = "6926 Old York Rd\nPhiladelphia, PA 19126"
			});
			contacts.Add(new Place {
				Title = "Dacha",
				//FullTitle = "Dacha ADC",
				Address = "250 Geiger Rd\nPhiladelphia, PA 19115"
			});


			contacts.Add(new Client {
				FullName = "Han Sang",
				PhoneNumber = "(215) 607-4891",
				Address = "1 Potter St. Apt.410\nWarminster, PA 18974",
				RegularPlace = contacts.GetByName("PASSi") as Place,
			});
			contacts.Add(new Client {
				FullName = "Eom Ki Tae",
				PhoneNumber = "(267) 210-0303",
				Address = "770 Welsh Rd\nHuntington Valley, PA 19006",
				RegularPlace = contacts.GetByName("PASSi") as Place,
			});
			contacts.Add(new Client {
				FullName = "Patrakeyeva Nina",
				Address = "9841 Northeast Ave, Apt.5A\nPhiladelphia, PA 19115",
				RegularPlace = contacts.GetByName("Dacha") as Place,
			});

			return Task.CompletedTask;
		}

		public Task LoadSchedule()
		{
			var contacts = AppScope.Instance.Contacts;
			var schedule = AppScope.Instance.Schedule;

			schedule.Add(PickupRide.AtHome(
				contacts.GetByName("Han Sang") as Client,
				new ScheduleTime(8, 20)));

			schedule.Add(PickupRide.AtHome(
				contacts.GetByName("Eom Ki Tae") as Client,
				new ScheduleTime(8, 40)));

			schedule.Add(new DropoffRide(
				contacts.GetByName("Han Sang") as Client,
				contacts.GetByName("PASSi") as Place,
				new ScheduleTime(9, 0)));

			schedule.Add(new DropoffRide(
				contacts.GetByName("Eom Ki Tae") as Client,
				contacts.GetByName("PASSi") as Place,
				new ScheduleTime(9, 0)));

			schedule.Add(CompleteRide.ToHome(
				contacts.GetByName("Patrakeyeva Nina") as Client,
				contacts.GetByName("Dacha") as Place,
				new ScheduleTime(13, 30),
				new ScheduleTime(13, 40)));

			return Task.CompletedTask;
		}

		//public Task LoadContacts(IList<IContact> contacts)
		//{
		//	contacts.Add(new Client {
		//		FullName = "Dolores Gordon",
		//		PhoneNumber = "215-438-4941",
		//		Address = "222 E Cliveden St\nPhiladelphia, PA 19119"
		//	});
		//	contacts.Add(new Client {
		//		FullName = "Elvin Taylor",
		//		PhoneNumber = "215-424-1409",
		//		Address = "6300 Old York Rd\nPhiladelphia, PA 19141"
		//	});
		//	contacts.Add(new Client {
		//		FullName = "Lloyd Carter",
		//		PhoneNumber = "267-438-8442",
		//		Address = "1758 W Pacific St\nPhiladelphia, PA 19140"
		//	});
		//	contacts.Add(new Client {
		//		FullName = "Zora Malone",
		//		PhoneNumber = "215-549-1176",
		//		Address = "6300 Old York Rd\nApt 215\rPhiladelphia PA 18141",
		//		Escort = true
		//	});
		//	contacts.Add(new Client {
		//		FullName = "Maria Aponte Escribano",
		//		PhoneNumber = "267-904-6127",
		//		Address = "1605 W Allegheny Ave\nPhiladelphia, PA 19132"
		//	});
		//	contacts.Add(new Client {
		//		FullName = "Barbara Crockett",
		//		PhoneNumber = "267-978-4582",
		//		Address = "2839 N Judson St\rPhiladelphia, PA 19132",
		//		Escort = true
		//	});



		//	contacts.Add(new Place {
		//		Title = "Einstein Hospital",
		//		PhoneNumber = "(111) 111-1111",
		//		Address = "5401 Old York Rd\nPhiladelphia, PA 19141"
		//	});
		//	contacts.Add(new Place {
		//		Title = "Einstein Moss Rehab",
		//		PhoneNumber = "(222) 222-222",
		//		Address = "1200 W Tabor Rd\nPhiladelphia, PA 19141"
		//	});
		//	contacts.Add(new Place {
		//		Title = "Roxborough Medical Network",
		//		Address = "5735 Ridge Ave\nPhiladelphia, PA 19128"
		//	});
		//	contacts.Add(new Place {
		//		Title = "Temple University Hospital",
		//		PhoneNumber = "(215) 707-2000",
		//		Address = "3509 N Broad St\nPhiladelphia, PA 19140"
		//	});
		//	contacts.Add(new Place {
		//		Title = "Temple University Boyer Pavillion",
		//		PhoneNumber = "(215) 707-6000",
		//		Address = "3401 N Broad St\nPhiladelphia, PA 19140"
		//	});


		//	return Task.CompletedTask;
		//}


		//public Task LoadSchedule(IList<Ride> schedule)
		//{
		//	var contacts = AppScope.Instance.Contacts;

		//	schedule.Add(CompleteRide.FromHome(
		//		contacts.GetByName("Dolores Gordon") as Client,
		//		new ScheduleTime(9, 20),
		//		contacts.GetByName("Einstein Hospital") as Place,
		//		new ScheduleTime(10, 15)));

		//	schedule.Add(CompleteRide.FromHome(
		//		contacts.GetByName("Lloyd Carter") as Client,
		//		new ScheduleTime(11, 0),
		//		contacts.GetByName("Temple University Hospital") as Place,
		//		new ScheduleTime(11, 45)));

		//	schedule.Add(CompleteRide.FromHome(
		//		contacts.GetByName("Barbara Crockett") as Client,
		//		new ScheduleTime(12, 30),
		//		contacts.GetByName("Temple University Hospital") as Place,
		//		new ScheduleTime(13, 30)));

		//	return Task.CompletedTask;
		//}

		public Task SaveContacts() => Task.CompletedTask;
		public Task SaveSchedule() => Task.CompletedTask;
	}
}
