using System;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Druid;
using Lost.Models;
using Xamarin.Forms;
using System.Threading.Tasks;


namespace Lost.ViewModels
{
	public class ShiftViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(ShiftViewModel));

		public static ShiftViewModel Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}
		static ShiftViewModel instance;

		public ShiftViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Shift";
			Shift = null;

			ShiftActionCommand = new Command(OnShiftAction);
			BackActionCommand = new Command(OnBackAction, CanPerformBackAction);

			StartBusy("Initializing...");
		}

		public void Initialize(Shift shift)
		{
			Shift = shift;
			NotifyPropertiesChanged();

			ClearBusy();
			StartTimer();
		}

		public Shift Shift { get; private set; }

		public string Date {
			get => Shift != null ? Shift.Date.ToLongDateString() : string.Empty;
		}

		string TimeString(DateTime? time)
		{
			if (time == null)
				return string.Empty;

			return ((DateTime)time).ToShortTimeString();
		}

		public string ShiftStartTime {
			get => TimeString(Shift?.ShiftStartTime);
		}
		public string FirstPickupTime {
			get => TimeString(Shift?.FirstPickupTime);
		}
		public string LastDropoffTime {
			get => TimeString(Shift?.LastDropoffTime);
		}
		public string ShiftEndTime {
			get => TimeString(Shift?.ShiftEndTime);
		}

		public string ShiftActionName {
			get {
				switch (Shift?.State)
				{
				case ShiftState.None:
					return "Start Shift";

				case ShiftState.Enroute:
					return "First Pickup";

				case ShiftState.Working:
					return "Last Dropoff";
				
				case ShiftState.Finishing:
					return "End Shift";

				default:
				case ShiftState.Ended:
					return "New Shift";
				}
			}
		}

		public Command ShiftActionCommand { get; }
		public Command BackActionCommand { get; }

		public async void OnShiftAction()
		{
			Debug.Print("OnShiftAction");

			uint mileage = 0;
			//var time = CurrentDate + CurrentTime;
			var time = DateTime.Now.Date + CurrentTime;

			if (IsMileageRequired) {
				if (!uint.TryParse(Mileage, out mileage)) {
					await Alerts.ErrorAlert("Please enter correct mileage.");
					return;
				}
			}

			var storage = AppStorage.Instance;
			var shiftState = Shift?.State;

			switch (shiftState)
			{
			case ShiftState.None:
				Shift.StartShift(time, (int)mileage);
				await storage.StartShift();
				break;

			case ShiftState.Enroute:
				Shift.FirtPickup(time);
				await storage.FirstPickup();
				break;

			case ShiftState.Working:
				Shift.LastDropoff(time);
				await storage.LastDropoff();
				break;

			case ShiftState.Finishing:
				Shift.EndShift(time, (int)mileage);
				await storage.EndShift();
				break;

			default:
			case ShiftState.Ended:
				Shift = new Shift(CurrentDate);
				await storage.NewShift(Shift);
				break;

			}

			//if (shiftState == ShiftState.Ended) {
			//	StopTimer();
			//} else {
			//	StartTimer();
			//}

			Mileage = string.Empty;

			NotifyPropertiesChanged();
		}

		public void NotifyPropertiesChanged()
		{
			PropertiesChanged(
				nameof(Date),
				nameof(ShiftStartTime),
				nameof(FirstPickupTime),
				nameof(LastDropoffTime),
				nameof(ShiftEndTime),
				nameof(ShiftActionName),
				nameof(IsBackActionEnabled),
				nameof(CurrentDate),
				nameof(CurrentTime),
				nameof(Mileage),
				nameof(IsDateRequired),
				nameof(IsTimeRequired),
				nameof(IsTimerRunning),
				nameof(IsMileageRequired)
				);
		}

		public void OnBackAction()
		{
			Debug.Print("OnBackAction");

		}


		public bool IsBackActionEnabled => CanPerformBackAction();

		public bool CanPerformBackAction()
		{
			return Shift !=null && Shift.State != ShiftState.None;
		}

		public bool IsDateRequired {
			get => Shift == null || Shift.State == ShiftState.Ended;
		}

		public bool IsTimeRequired {
			get => Shift != null && Shift.State != ShiftState.Ended;
		}

		public bool IsMileageRequired {
			get => Shift?.State == ShiftState.None || Shift?.State == ShiftState.Finishing;
		}

		public DateTime CurrentDate {
			get => currentDate;
			set => SetCurrentDate(value, false);
		}
		DateTime currentDate;

		public TimeSpan CurrentTime {
			get => currentTime;
			set => SetCurrentTime(value, false);
		}
		TimeSpan currentTime;

		void SetCurrentDate(DateTime date, bool programmatic)
		{
			SetProperty(ref currentDate, date, nameof(CurrentDate));

			if (!programmatic) {
				SuspendTimer();
			}
		}


		void SetCurrentTime(TimeSpan time, bool programmatic)
		{
			SetProperty(ref currentTime, time, nameof(CurrentTime));

			if (!programmatic) {
				SuspendTimer();
			}
		}

		bool IsTimerRunning { get; set; }
		bool IsTimerSuspended { get; set; }

		void StartTimer()
		{
			//if (IsTimerRunning)
			//	return;

			IsTimerRunning = true;
			IsTimerSuspended = false;
			SetCurrentDate(DateTime.Now.Date, true);
			SetCurrentTime(DateTime.Now.TimeOfDay, true);

			Device.StartTimer(TimeSpan.FromSeconds(1), () => {
				if (!IsTimerSuspended) {
					SetCurrentDate(DateTime.Now.Date, true);
					SetCurrentTime(DateTime.Now.TimeOfDay, true);
				}
				return IsTimerRunning;
			});

		}

		void SuspendTimer(int seconds = 90)
		{
			IsTimerSuspended = true;
			Device.StartTimer(TimeSpan.FromSeconds(seconds), () => {
				IsTimerSuspended = false;
				return false;
			});
		}

		//void StopTimer()
		//{
		//	IsTimerRunning = false;
		//}

		public string Mileage { get; set; } = string.Empty;
	}
}
