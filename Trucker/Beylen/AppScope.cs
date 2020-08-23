using Beylen.Models;
using Beylen.Views;
using Dwares.Druid;
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

	public class AppScope : BindingScope
	{
		//static ClassRef @class = new ClassRef(typeof(AppScope));

		public static AppScope Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}
		static AppScope instance;

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

		public AppMode? ConfigMode { get; private set; }// = AppMode.Driver;
		public AppMode CurrentMode { get; private set; }//= AppMode.Driver;
		public string Car { get; private set; }



		public ObservableCollection<Contact> Contacts { get; }
		public ObservableCollection<Customer> Customers { get; }
		public ObservableCollection<Produce> Produce { get; }
		public ObservableCollection<Invoice> Invoices { get; }
		public ObservableCollection<Place> Places { get; }
		public Route Route { get; } = new Route();

		public Place StartPoint { get; set; }
		public Place EndPoint { get; set; }
		//public DateOnly ClosedDate { get; set; }
		public DateOnly OrderingDate { get; set; }
		public int OrderingLast { get; set; }


		public void Configure()
		{
			Configure(Settings.ApplicationMode ?? "Market", Settings.Car);
		}

		public void Configure(string mode, string car)
		{
			if (ConfigMode == null) {
				CurrentMode = mode == "Market" ? AppMode.Market : AppMode.Driver;
				Settings.ApplicationMode = mode;
			} else {
				CurrentMode = (AppMode)ConfigMode;
			}

			if (CurrentMode == AppMode.Market) {
				AppShell = new AppShell_Market();
				Settings.Car = null;
			} else {
				AppShell = new AppShell_Driver();
				Car = car ?? "Car A";
				Settings.Car = Car;
			}

			Application.Current.MainPage = AppShell;
		}

		public async Task Initialize()
		{
			var storage = AppStorage.Instance;
			await storage.Initialize();
			await storage.LoadData();

			await InitOrdering();
		}

		public async Task ReloadData()
		{
			Produce.Clear();
			Contacts.Clear();
			Customers.Clear();
			Places.Clear();
			Invoices.Clear();
			Route.Clear();

			await AppStorage.Instance.LoadData();
		}

		public async Task InitOrdering()
		{
			var closedDate = AppStorage.GetClosedDate();
			var orderingDate = AppStorage.GetOrderingDate();
			var orderingLast = AppStorage.GetOrderingLast();

			if (orderingDate == null)
			{
				if (closedDate == null) {
					OrderingDate = DateOnly.Today;
				} else {
					OrderingDate = ((DateOnly)closedDate).NextDay();
				}
				OrderingLast = 0;
			}
			else {
				OrderingDate = (DateTime)orderingDate;
				if (closedDate != null && OrderingDate <= (DateOnly)closedDate) {
					OrderingDate = ((DateOnly)closedDate).NextDay();
					OrderingLast = 0;
				}
			}

			if (OrderingDate.DayOfWeek == DayOfWeek.Sunday) {
				OrderingDate = OrderingDate.NextDay();
				OrderingLast = 0;
			}

			await AppStorage.SetOrderingDate(OrderingDate);
			await AppStorage.SetOrderingLast(OrderingLast);
		}

		
		public static Customer GetCustomer(string name) => Instance.Customers.GetByCodeName(name); // for MockStorage only


		/* Xamarin.Forms Routing
		 */
		public Dictionary<string, Type> Routing { get; } = new Dictionary<string, Type>() {
			//{ "//driver/orders", typeof(OrdersPage) },
			//{ "//driver/route", typeof(RoutePage) },
			//{ "//driver/contacts/phones", typeof(ContactsPage) },
			//{ "//driver/contacts/customers", typeof(CustomersPage) },
			{ "orders", typeof(OrdersPage) },
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
