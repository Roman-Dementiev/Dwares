using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Dwarf.Validation;


namespace Dwares.Rookie.ViewModels
{
	public class GoOnOffWorkViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(GoOnOffWork));

		DateField date;
		TimeField time;
		NonNegativeNumberField<int> mileage;

		public GoOnOffWorkViewModel(bool toWork)
		{
			//Debug.EnableTracing(@class);

			ToWork = toWork;
			Title = toWork ? "Go To Work" : "Go Off Work";
			AcceptText = toWork ? "To Work" : "Off Work";

			date = new DateField("Date") { Value = DateTime.Today };
			time = new TimeField("Time") { Value = DateTime.Now.TimeOfDay };
			mileage = new NonNegativeNumberField<int>("Mileage") { Value = AppScope.LastMileage };
			Fields = new FieldList(time, mileage);
		}

		public override double FormHeight => FitContent;
		public bool BorderIsVisible => true;

		public bool ToWork { get; }
		public string AcceptText{ get; }

		public DateTime Date {
			get => date;
			set => SetProperty(date, value);
		}

		public TimeSpan Time {
			get => time;
			set => SetProperty(time, value);
		}

		public int Mileage {
			get => mileage;
			set => SetProperty(mileage, value);
		}

		public string MileageText {
			get => mileage.Text;
			set => SeTextProperty(mileage, value);
		}


		protected override async Task DoAccept()
		{
			Exception error;
			var dt = Date.Add(Time);
			if (ToWork) {
				error = await AppScope.Instance.GoToWork(dt, Mileage);
			} else {
				error = await AppScope.Instance.GoOffWork(dt, Mileage);
			}
			if (error != null) {
				throw error;
			}
		}
	}


	//public class GoToWorkViewModel : GoOnOffWorkViewMode
	//{
	//	//static ClassRef @class = new ClassRef(typeof(GoOnDutyViewModel));
	//	public GoToWorkViewModel() :
	//		base(true)
	//	{
	//		//Debug.EnableTracing(@class);
	//	}

	//	protected override async Task DoAccept()
	//	{
	//		var error = await AppScope.Instance.GoToWork(Time, Mileage);
	//		if (error != null) {
	//			throw error;
	//		}
	//	}
	//}


	//public class GoOffWorkViewModel : GoOnOffWorkViewMode
	//{
	//	//static ClassRef @class = new ClassRef(typeof(GoOffDutyViewModel));
	//	public GoOffWorkViewModel() :
	//		base(false)
	//	{
	//		//Debug.EnableTracing(@class);
	//	}

	//	protected override async Task DoAccept()
	//	{
	//		var error = await AppScope.Instance.GoOffWork(Time, Mileage);
	//		if (error != null) {
	//			throw error;
	//		}
	//	}
	//}
}
