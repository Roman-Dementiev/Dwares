//using System;
//using System.Threading.Tasks;
//using Dwares.Dwarf;
//using Dwares.Druid;
//using Dwares.Druid.Forms;
//using Drive.Models;

//namespace Drive.ViewModels
//{
//	public class AppoitmentViewModel: FormViewModel<AppoitmentTrip>
//	{
//		//static ClassRef @class = new ClassRef(typeof(AppoitmentViewModel));

//		public AppoitmentViewModel() : this(null, null) { }
//		public AppoitmentViewModel(AppoitmentTrip source) : this(source, null) { }
//		public AppoitmentViewModel(Schedule schedule) : this(null, schedule) { }

//		private AppoitmentViewModel(AppoitmentTrip source, Schedule schedule) :
//			base(source)
//		{
//			//Debug.EnableTracing(@class);

//			Title = "Appoitment";

//			date = new DateField(nameof(Date));
//			clientName = new TextField(nameof(ClientName)) { MsgFieldIsRequired = "Client name is required." };
//			clientPhone = new PhoneField(nameof(ClientPhone)) { MsgFieldIsRequired = "Client phone number is required." };
//			clientAddress = new TextField(nameof(ClientAddress));
//			pickupTime = new TimeField(nameof(PickupTime));
//			facilityName = new TextField(nameof(FacilityName));
//			facilityPhone = new PhoneField(nameof(FacilityPhone));
//			facilityAddress = new TextField(nameof(FacilityAddress));
//			appoitmentTime = new TimeField(nameof(AppoitmentTime));

//			Fields = new FieldList(clientName, clientPhone, clientAddress, pickupTime,
//				facilityName, facilityPhone, facilityAddress, appoitmentTime);

//			if (source != null) {
//				Date = source.AppoitmentTime.Date;
//				ClientName = source.Client.FullName;
//				ClientPhone = source.Client.PhoneNumber;
//				ClientAddress = source.Client.Address;
//				PickupTime = source.PickupTime.Time;
//				FacilityName = source.Facility.Title;
//				FacilityPhone = source.Facility.PhoneNumber;
//				FacilityAddress = source.Facility.Address;
//				AppoitmentTime = source.AppoitmentTime.Time;
//			} else {
//				if (schedule == null) {	
//					schedule = AppScope.Instance.Schedule;
//				}
//				Date = schedule.Date ?? DateTime.Today;
//			}
//		}

//		DateField date;
//		public DateTime Date {
//			get => date;
//			set => SetProperty(date, value);
//		}

//		public string DateAsText {
//			get {
//				if (date.Value != null) {
//					return ((DateTime)date.Value).ToLongDateString();
//				} else {
//					return string.Empty;
//				}
//			}
//		}

//		TextField clientName;
//		public string ClientName {
//			get => clientName;
//			set => SetProperty(clientName, value);
//		}

//		PhoneField clientPhone;
//		public string ClientPhone {
//			get => clientPhone;
//			set => SetTextProperty(clientPhone, value);
//		}

//		TextField clientAddress;
//		public string ClientAddress {
//			get => clientAddress;
//			set => SetTextProperty(clientAddress, value);
//		}

//		TimeField pickupTime;
//		public TimeSpan PickupTime {
//			get => pickupTime;
//			set => SetProperty(pickupTime, value);
//		}

//		TextField facilityName;
//		public string FacilityName {
//			get => facilityName;
//			set => SetProperty(facilityName, value);
//		}

//		PhoneField facilityPhone;
//		public string FacilityPhone {
//			get => facilityPhone;
//			set => SetTextProperty(facilityPhone, value);
//		}

//		TextField facilityAddress;
//		public string FacilityAddress {
//			get => facilityAddress;
//			set => SetTextProperty(facilityAddress, value);
//		}

//		TimeField appoitmentTime;
//		public TimeSpan AppoitmentTime {
//			get => appoitmentTime;
//			set => SetProperty(appoitmentTime, value);
//		}

//		public override Task<Exception> Validate()
//		{
//			return base.Validate();
//		}

//		protected override async Task DoAccept()
//		{
//		}
//	}
//}
