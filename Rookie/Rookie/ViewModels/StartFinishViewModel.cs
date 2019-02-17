using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Dwarf.Validation;


namespace Dwares.Rookie.ViewModels
{
	public class StartFinishViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(StartFinishViewModel));

		TimeField time;
		IntegerField mileage;

		public StartFinishViewModel(string title)
		{
			//Debug.EnableTracing(@class);

			Title = title;

			time = new TimeField() { Value = DateTime.Now.TimeOfDay };
			mileage = new IntegerField() { Value = Rookie.AppScope.LastMileage, MsgInvalidEntryText = "Milage must be a positive number" };
			fields = new Validatables(time, mileage);
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
	}
}
