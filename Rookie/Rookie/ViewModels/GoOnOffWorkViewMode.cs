﻿using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Dwarf.Validation;


namespace Dwares.Rookie.ViewModels
{
	public class GoOnOffWorkViewMode : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(GoOnOffWork));

		TimeField time;
		IntegerField mileage;

		protected GoOnOffWorkViewMode(string title)
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


	public class GoToWorkViewModel : StartFinishViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(GoOnDutyViewModel));
		public GoToWorkViewModel() :
			base("Go To Work")
		{
			//Debug.EnableTracing(@class);
		}


		protected override async Task DoAccept()
		{
			await Rookie.AppScope.Instance.GoToWork(Time, Mileage);
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
			await Rookie.AppScope.Instance.GoOffWork(Time, Mileage);
		}
	}
}
