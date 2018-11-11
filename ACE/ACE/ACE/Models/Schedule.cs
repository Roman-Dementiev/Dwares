using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;


namespace ACE.Models
{
	public class Schedule : ObservableCollection<Pickup>
	{
		ScheduleTime latestPickupTime = new ScheduleTime();

		public Schedule()
		{
		}

		Route route;
		public Route Route {
			get => LazyInitializer.EnsureInitialized(ref route, () => {
				//TODO
				var ACE = new Contact {
					ContactType = ContactType.ACE,
					Name = "ACE",
					Address = "10162 Bustleton Ave\nPhiladelphia PA 19116"
				};
				return new Route(ACE);
			});
		}

		public void NewSchedule(DateTime startDate, TimeSpan startTime)
		{
			Clear();
			Route.NewRoute(startDate.Add(startTime));
		}

		public new void Clear()
		{
			base.Clear();
			Route.Clear();
			latestPickupTime.Unset();
		}

		public new void Add(Pickup pickup)
		{
			base.Add(pickup);
			Route.AddRun(pickup);

			if (!latestPickupTime.IsSet || pickup.PickupTime.IsAfter(latestPickupTime.DateTime)) {
				latestPickupTime = pickup.PickupTime;
			}

		}

		public new bool Remove(Pickup pickup)
		{
			if (base.Remove(pickup)) {
				Route.Remove(pickup.PickupStop);
				Route.Remove(pickup.DropoffStop);
				return true;
			} else {
				return false;
			}
		}

		public bool EstimateNextPickup(out ScheduleTime pickupTime, out ScheduleTime appoitmentTime)
		{
			if (!latestPickupTime.IsSet) {
				pickupTime = new ScheduleTime(DateTime.Now.Hour, 30);
			}
			else if (latestPickupTime.Minute < 30) {
				pickupTime = new ScheduleTime(latestPickupTime.Hour, 30);
			}
			else if (latestPickupTime.Hour < 23) {
				pickupTime = new ScheduleTime(latestPickupTime.Hour + 1, 0);
			}
			else {
				pickupTime = appoitmentTime = ScheduleTime.Tomorrow;
				return false;
			}

			appoitmentTime = new ScheduleTime(pickupTime, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
			return true;
		}

		bool ContactIsEngaged(Contact contact, Func<Pickup, bool> isEngaged)
		{
			if (contact.ContactType != ContactType.Client && contact.ContactType != ContactType.Office)
				return false;

			foreach (var pickup in this) {
				if (isEngaged(pickup))
					return true;
			}
			return false;
		}

		public bool IsEngaged(Contact contact)
		{
			return ContactIsEngaged(contact, (pickup) => pickup.Client == contact || pickup.Office == contact);
		}

		public bool HasPickup(Contact contact)
		{
			return ContactIsEngaged(contact, (pickup) => pickup.Client == contact);
		}

		public bool HasAppoitment(Contact contact)
		{
			return ContactIsEngaged(contact, (pickup) => pickup.Office == contact);
		}

	}
}
