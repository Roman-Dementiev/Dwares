using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.Support;
using ACE.Models;


namespace ACE.ViewModels
{
	public class AppoitmentViewModel : RunDetailViewModel
	{
		//ClassRef @class = new ClassRef(typeof(AppoitmentViewModel));

		public AppoitmentViewModel(ScheduleRun source) :
			base(source, SheduleRunType.Appoitment)
		{
			//Debug.EnableTracing(@class);

			DropoffNamePlaceholder = "Enter Office Name";

			if (source == null) {
				AppData.Schedule.EstimateNextPickup(out var pickup, out var appoitment);
				PickupTime = pickup.Time;
				DropoffTime = appoitment.Time;
			}
		}

		public override void UpdateAutoSuggestions()
		{
			base.UpdateAutoSuggestions();

			var offices = AppData.Contacts.GetOffices();
			DropoffNameSuggestions = GetNameSuggestions(offices);
		}

		public override void OnClientSelected(Contact client)
		{
			if (Source != null || client == null || client.ContactType != ContactType.Client)
				return;

			base.OnClientSelected(client);

			//PickupStopName = HomeStopName;
			PickupAddress = client.Address;
		}

		//public override void OnDropoffSelected(Contact contact)
		//{
		//	if (Source != null || contact == null)
		//		return;

		//	base.OnDropoffSelected(contact);
		//}

		public override async Task<bool> DoValidate()
		{
			if (!await ValidateClient(ClientName, ClientPhone, PickupAddress))
				return false;

			if (!await ValidateOffice(DropoffStopName, DropoffAddress))
				return false;
		
			return true;
		}

		protected override async Task DoAccept()
		{
			var client = UpdateClient(ClientName, ClientPhone, PickupAddress);
			var office = UpdateOffice(DropoffStopName, DropoffAddress);

			if (Source == null) {
				var newAppoitment = new Appoitment(client, office,
					new ScheduleTime(PickupTime),
					new ScheduleTime(DropoffTime));

				AppData.Schedule.Add(newAppoitment);
			}

			await AppStorage.SaveAsync();
		}
	}
}
