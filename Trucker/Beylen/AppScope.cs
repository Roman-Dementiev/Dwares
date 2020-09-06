using Beylen.Models;
using Beylen.ViewModels;
using Beylen.Views;
using Dwares.Druid;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Toolkit;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Beylen
{
	public enum AppMode
	{
		//Master,
		Market,
		Driver
	}

	public enum Stage
	{
		Preparing,
		Delivering,
		ClosingUp
	}


	public class AppScope : BindingScope
	{
		//static ClassRef @class = new ClassRef(typeof(AppScope));

		public static AppScope Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}
		static AppScope instance;

#if DEBUG
		static bool resetSettings = false;
		static bool resetDataProperties = false;
#else
		const bool resetSettings = false;
		const bool resetDataProperties = false;
#endif


		public AppScope() : base(null)
		{
			//Debug.EnableTracing(@class);

			Contacts = new ObservableCollection<Contact>();
			Customers = new ObservableCollection<Customer>();
			Produce = new ObservableCollection<Produce>();
			Invoices = new ObservableCollection<Invoice>();
			Places = new ObservableCollection<Place>();
			Route = new Route();

			foreach (var item in Routing) {
				Xamarin.Forms.Routing.RegisterRoute(item.Key, item.Value);
			}
		}

		public Shell AppShell { get; set; }

		public AppMode? ConfigMode { get; set; }//= AppMode.Driver;
		public AppMode CurrentMode { get; set; }//= AppMode.Driver;
		public Car Car { get; set; }
		//public DateOnly CurrentDate { get; set; }
		public Stage Stage { get; set;}
		public DateOnly OrderingDate { get; set; }
		public int OrderingLast { get; set; }


		public ObservableCollection<Contact> Contacts { get; }
		public ObservableCollection<Customer> Customers { get; }
		public ObservableCollection<Produce> Produce { get; }
		public ObservableCollection<Invoice> Invoices { get; }
		public ObservableCollection<Place> Places { get; }
		public Route Route { get; }



		public void Configure()
		{
			if (resetSettings) {
				Settings.Reset();
				resetSettings = false;
			}

			if (Settings.ApplicationMode == "Driver") {
				var car = Car.ById(Settings.Car) ?? Car.List[0];
				Configure(AppMode.Driver, car);
			} else {
				Configure(AppMode.Market, null);
			}
		}

		public void Configure(AppMode mode, Car car)
		{
			if (ConfigMode == null) {
				CurrentMode = mode;
				Settings.ApplicationMode = mode == AppMode.Market ? "Market" : "Driver";
			} else {
				CurrentMode = (AppMode)ConfigMode;
			}

			if (CurrentMode == AppMode.Market) {
				AppShell = new AppShell_Market();
				Car = null;
				Settings.Car = null;
			} else {
				AppShell = new AppShell_Driver();
				Car = car ?? Car.List[0];
				Settings.Car = Car.Id;
			}

			ClearData();
			Application.Current.MainPage = AppShell;
		}

		public async Task Initialize()
		{
			var storage = AppStorage.Instance;
			await storage.Initialize();
			await LoadData();
		}

		public async Task ReloadData()
		{
			ClearData();
			await LoadData();
		}

		public void ClearData()
		{
			Produce.Clear();
			Contacts.Clear();
			Customers.Clear();
			Places.Clear();
			Invoices.Clear();
			Route.Clear();
		}

		async Task LoadData()
		{
			await AppStorage.Instance.LoadData(Car?.Id, false, resetDataProperties);
			resetDataProperties = false;

			await InitScopeData();

			if (Route.Stops.Count > 0) {
				var firstStop = Route.Stops[0];
				if (firstStop.Kind != RouteStopKind.StartPoint || firstStop.Status > RoutеStopStatus.Arrived) {
					await Route.Start();
				} else {
					await Route.UpdateDurations();
				}
			}
		}

		public async Task InitScopeData()
		{
			if (CurrentMode == AppMode.Driver) {
				var stage = await AppStorage.GetStage(Car);
				var stageDate = await AppStorage.GetStageDate(Car);

				if (stage != null && stageDate == DateOnly.Today) {
					Stage = (Stage)stage;
				} else {
					Stage = Stage.Preparing;
				}
				OrderingDate = DateOnly.Today;

				await AppStorage.SetStage(Car, Stage);
				await AppStorage.SetStageDate(Car, OrderingDate);
			}
			else
			{
				var orderingDate = await AppStorage.GetOrderingDate();

				if (orderingDate == null) {
					OrderingDate = DateOnly.Today;
				} else {
					OrderingDate = (DateTime)orderingDate;

					if (OrderingDate < DateOnly.Today) {
						OrderingDate = DateOnly.Today;
					}
				}

				while (IsDayOff(OrderingDate)) {
					OrderingDate = OrderingDate.NextDay();
				}

				await AppStorage.SetOrderingDate(OrderingDate);
			}

			OrderingLast = 0;
			foreach (var invoice in Invoices) {
				if (invoice.Date != OrderingDate)
					continue;
				if (invoice.Number == null || invoice.Number.Length != 6) {
					Debug.Print($"Invalid invoice Number={Dw.ToString(invoice.Number)} in Invoce (RecordId={invoice.RecordId})");
					continue;
				}

				int last;
				if (int.TryParse(invoice.Number.Substring(invoice.Number.Length-2), out last) && last > OrderingLast) {
					OrderingLast = last;
				}
			}
		}

		static bool IsDayOff(DateOnly date)
		{
			//TODO
			var dayOfWeek = date.DayOfWeek;
			return dayOfWeek == DayOfWeek.Saturday || dayOfWeek == DayOfWeek.Sunday;
		}

		public string NextInvoiceNumber()
		{
			var str = string.Format("{0,2:D2}{1,2:D2}{2,2:D2}", OrderingDate.Month, OrderingDate.Day, OrderingLast + 1);
			return str;
		}


		public static Produce GetProduce(string name) => Instance.Produce.Lookup((p) => p.Name == name);
		public static Customer GetCustomer(string codeName) => Instance.Customers.Lookup((c) => c.CodeName == codeName);
		public static Place GetPlace(string codeName) => Instance.Places.Lookup((p) => p.CodeName == codeName);


		public async Task NewInvoice(Invoice invoice)
		{
			await invoice.EnsureRouteStop();

			//Invoices.Add(invoice);
			await AppStorage.Instance.NewInvoice(invoice);
		}

		public async Task UpdateInvoice(Invoice invoice)
		{
			await AppStorage.Instance.UpdateInvoice(invoice);
		}

		/* Xamarin.Forms Routing
		 */
		public Dictionary<string, Type> Routing { get; } = new Dictionary<string, Type>() {
			//{ "//driver/orders", typeof(OrdersPage) },
			//{ "//driver/route", typeof(RoutePage) },
			//{ "//driver/contacts/phones", typeof(ContactsPage) },
			//{ "//driver/contacts/customers", typeof(CustomersPage) },
			//{ "orders", typeof(OrdersPage) },
			{ "invoices", typeof(InvoicesPage) },
			{ "totals", typeof(TotalsPage) },
			{ "invoice", typeof(InvoiceForm) },
			{ "shopping", typeof(ShoppingPage) },
			{ "storage", typeof(StoragePage) },
			{ "route", typeof(RoutePage) },
			{ "phones", typeof(ContactsPage) },
			{ "customers", typeof(CustomersPage) },
			{ "routestop", typeof(RouteStopForm) },
			{ "produce", typeof(ProducePage) },
			{ "settings", typeof(SettingsPage) },
			{ "about", typeof(AboutPage) }
		};


	}
}
