using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Forms;
using Dwares.Dwarf.Validation;
using System.Threading.Tasks;
using Dwares.Rookie.Bases;


namespace Dwares.Rookie.ViewModels
{
	public class AddTripViewModel : FormViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(AddTripViewModel));

		PositiveNymbeFrield<int> tripNumber;
		TimeField started;
		TimeField finished;
		PositiveDecimalField distance;
		CurrencyField meter;
		CurrencyField cash;
		CurrencyField tolls;
		//bool isCredit;

		public AddTripViewModel()
		{
			//Debug.EnableTracing(@class);

			Title = "Add Trip";

			tripNumber = new PositiveNymbeFrield<int>("Trip Number") { IsRequired = true };
			started = new TimeField("Started") { Value = DateTime.Now.TimeOfDay };
			finished = new TimeField("Finished") { Value = DateTime.Now.TimeOfDay };
			distance = new PositiveDecimalField("Dustance", decimalPoints: 1);
			meter = new CurrencyField("Meter");
			cash = new CurrencyField("Cash");
			tolls = new CurrencyField("Tolls");

			Fields = new FieldList(tripNumber, started, finished, distance, meter, cash, tolls);
		}

		public int TripNumber {
			get => tripNumber;
			set => SetProperty(tripNumber, value);
		}

		public string TripNumberText {
			get => tripNumber.Text;
			set => SeTextProperty(tripNumber, value);
		}

		public TimeSpan Started {
			get => started;
			set => SetProperty(started, value);
		}

		public DateTime StartedDateTime => started.DateTime;

		public TimeSpan Finished {
			get => finished;
			set => SetProperty(finished, value);
		}

		public DateTime FinishedDateTime => finished.DateTime;

		public decimal Distance {
			get => distance;
			set => SetProperty(distance, value);
		}

		public string DistanceText {
			get => distance.Text;
			set => SeTextProperty(distance, value);
		}

		public decimal Meter {
			get => meter;
			set => SetProperty(meter, value);
		}

		public string MeterText {
			get => meter.Text;
			set => SeTextProperty(meter, value);
		}

		public decimal Cash {
			get => cash;
			set => SetProperty(cash, value);
		}

		public string CashText {
			get => cash.Text;
			set => SeTextProperty(cash, value);
		}

		public decimal Tolls {
			get => tolls;
			set => SetProperty(tolls, value);
		}

		public string TollsText {
			get => tolls.Text;
			set => SeTextProperty(tolls, value);
		}

		public bool IsCredit { get; set; }

		protected override Task DoAccept()
		{ 
			var tripRecprd = new TripRecord {
				TripNumber = TripNumber,
				Started = StartedDateTime,
				Finished = FinishedDateTime,
				Distance = Distance,
				Meter = Meter,
				Cash = Cash,
				Expences = Tolls,
				IsCredit = IsCredit
			};

			return AppScope.Instance.AddTrip(tripRecprd);
		}
	}
}
