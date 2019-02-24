﻿using System;
using System.Threading.Tasks;
using Dwares.Dwarf.Validation;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie.ViewModels
{
	public class AddBaseViewModel : FramedFormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(AddBaseViewModel));

		NumberField<int> year;
		TextField baseId;
		TripBase db;


		public AddBaseViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Add Database";

			year = new NumberField<int>("Year");
			baseId = new TextField("Base Id");
			Fields = new FieldList(year, baseId);

			Year = 2019;
			FullYearEnabled = true;
			FullYear = false;
			MonthIndex = 3;
			MonthEnabled = true;

			BaseId = "app";
		}

		public int Year {
			get => year;
			set => SetProperty(year, value);
		}

		public string YearText {
			get => year.Text;
			set => SeTextProperty(year, value);
		}


		public bool FullYearEnabled { get; }

		bool fullYear;
		public bool FullYear {
			get => fullYear;
			set {
				if (SetProperty(ref fullYear, value)) {
					MonthEnabled = !fullYear;
				}
			}
		}

		bool monthEnabled;
		public bool MonthEnabled {
			get => monthEnabled;
			set => SetProperty(ref monthEnabled, value);
		}

		public int MonthIndex { get; set; }

		public int Month {
			get => FullYear ? 0 : MonthIndex+1;
		}

		public string BaseId {
			get => baseId;
			set => SetProperty(baseId, value);
		}

		public override async Task<Exception> Validate()
		{
			var error = await base.Validate();
			if (error != null)
				return error;

			error = AppScope.CheckBaseIsNew(Year, Month, BaseId);
			if (error != null)
				return error;

			try {
				db = new TripBase(Rookie.AppScope.ApiKey, BaseId, Year, Month);
				await db.TripsTable.Probe();
			} catch (Exception exc) {
				error = new DwarfException("Can not connect to database", exc);
				db = null;
			}
			return error;
		}

		protected override async Task DoAccept()
		{
			Debug.AssertNotNull(db);

			await Rookie.AppScope.Instance.AddBase(db, Year, Month);
		}
	}

}
