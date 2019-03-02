using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Dwarf.Validation;


namespace Dwares.Rookie.ViewModels
{
	public class GoOnOffWorkViewMode : FramedFormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(GoOnOffWork));

		TimeField time;
		NonNegativeNumberField<int> mileage;

		protected GoOnOffWorkViewMode(string title)
		{
			//Debug.EnableTracing(@class);

			Title = title;

			time = new TimeField("Time") { Value = DateTime.Now.TimeOfDay };
			mileage = new NonNegativeNumberField<int>("Milage") { Value = AppScope.LastMileage };
			Fields = new FieldList(time, mileage);
		}

		public override double FrameHeight => 240;

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

	}


	public class GoToWorkViewModel : GoOnOffWorkViewMode
	{
		//static ClassRef @class = new ClassRef(typeof(GoOnDutyViewModel));
		public GoToWorkViewModel() :
			base("Go To Work")
		{
			//Debug.EnableTracing(@class);
		}

		protected override async Task DoAccept()
		{
			var error = await AppScope.Instance.GoToWork(Time, Mileage);
			if (error != null) {
				throw error;
			}
		}
	}


	public class GoOffWorkViewModel : GoOnOffWorkViewMode
	{
		//static ClassRef @class = new ClassRef(typeof(GoOffDutyViewModel));
		public GoOffWorkViewModel() :
			base("Go Off Work")
		{
			//Debug.EnableTracing(@class);
		}

		protected override async Task DoAccept()
		{
			var error = await AppScope.Instance.GoOffWork(Time, Mileage);
			if (error != null) {
				throw error;
			}
		}
	}
}
