using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Dwares.Druid;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Beylen.Models
{
	public class Invoice : Model
	{
		//static ClassRef @class = new ClassRef(typeof(Invoice));

		public Invoice() : this(false) { }

		public Invoice(bool newInvoice)
		{
			//Debug.EnableTracing(@class);

			Articles = new ObservableCollection<Article>();

			if (newInvoice) {
				var appScope = AppScope.Instance;
				Date = appScope.OrderingDate;
				CarId = appScope.Car?.Id ?? string.Empty;
				Number = appScope.NextInvoiceNumber();
			}
		}

		public Invoice(RouteStop routeStop) :
			this(true)
		{
			Guard.ArgumentNotNull(routeStop, nameof(routeStop));

			var customer = routeStop.Place as Customer;
			Guard.ArgumentNotNull(customer, nameof(customer));

			RouteStop = routeStop;
			Customer = customer;
		}

		public Customer Customer {
			get => customer;
			set => SetProperty(ref customer, value);
		}
		Customer customer;

		public int Ordinal {
			get => ordinal;
			set => SetProperty(ref ordinal, value);
		}
		int ordinal;

		public int Seq {
			get => seq;
			set => SetProperty(ref seq, value);
		}
		int seq;

		public DateOnly Date {
			get => date;
			set => SetProperty(ref date, value);
		}
		DateOnly date;

		public string CarId {
			get => carId;
			set => SetProperty(ref carId, value);
		}
		string carId;

		public string Number {
			get => number;
			set => SetProperty(ref number, value);
		}
		string number;

		public string Notes {
			get => notes;
			set => SetProperty(ref notes, value);
		}
		string notes;

		public ObservableCollection<Article> Articles { get; }

		public RouteStop RouteStop {get; private set; }

		public async Task EnsureRouteStop()
		{
			if (RouteStop == null) {
				var route = AppScope.Instance.Route;
				RouteStop = new CustomerStop(route, Customer);
				try {
					await route.AddNew(RouteStop);
				}
				catch (Exception exc) {
					Debug.ExceptionCaught(exc);
					RouteStop = null;
					throw;
				}
			}
		}
	}
}
