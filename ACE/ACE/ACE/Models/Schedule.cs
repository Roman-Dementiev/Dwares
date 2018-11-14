using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;


namespace ACE.Models
{
	public class Schedule : ObservableCollection<ScheduleRun>
	{
		ScheduleTime? latestPickupTime = null;

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
			latestPickupTime = null;
		}

		public new void Add(ScheduleRun run)
		{
			base.Add(run);
			Route.AddRun(run);

			if (latestPickupTime == null || run.PickupTime.IsAfter((ScheduleTime)latestPickupTime)) {
				latestPickupTime = run.PickupTime;
			}
		}

		public new bool Remove(ScheduleRun run)
		{
			if (base.Remove(run)) {
				Route.Remove(run.PickupStop);
				Route.Remove(run.DropoffStop);
				return true;
			} else {
				return false;
			}
		}

		public bool EstimateNextPickup(out ScheduleTime pickupTime, out ScheduleTime appoitmentTime)
		{
			if (latestPickupTime == null) {
				pickupTime = new ScheduleTime(DateTime.Now.Hour, 30);
			} else {
				var latest = (ScheduleTime)latestPickupTime;
				if (latest.Minute < 30) {
					pickupTime = new ScheduleTime(latest.Hour, 30);
				}
				else if (latest.Hour < 23) {
					pickupTime = new ScheduleTime(latest.Hour + 1, 0);
				}
				else {
					pickupTime = appoitmentTime = ScheduleTime.Tomorrow;
					return false;
				}
			}

			appoitmentTime = new ScheduleTime(pickupTime, new TimeSpan(hours: 1, minutes: 0, seconds: 0));
			return true;
		}

		bool ContactIsEngaged(Contact contact, Func<ScheduleRun, bool> isEngaged)
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
