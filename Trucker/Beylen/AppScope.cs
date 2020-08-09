using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Beylen.Models;
using Beylen.Views;
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

			RegisterRoutes();
		}

		public AppMode? ConfigMode { get; set; }// = AppMode.Driver;
		public AppMode CurrentMode { get; set; }//= AppMode.Driver;
		public string Car { get; set; }

		public Shell AppShell { get; set; }


		public ContactCollection<Contact> Contacts { get; } = new ContactCollection<Contact>();
		public ContactCollection<Customer> Customers { get; } = new ContactCollection<Customer>();

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
				Car = car ?? "Car 1";
				Settings.Car = Car;
			}

			Application.Current.MainPage = AppShell;
		}

		public async Task Initialize()
		{
			var storage = AppStorage.Instance;
			await storage.Initialize();
			await storage.LoadContacts();
			await storage.LoadCustomers();
		}

		public async Task ReloadData()
		{
			Contacts.Clear();
			Customers.Clear();

			var storage = AppStorage.Instance;
			await storage.LoadContacts();
			await storage.LoadCustomers();
		}


		/* Xamarin.Forms Routing
		 */
		public Dictionary<string, Type> Routes { get; } = new Dictionary<string, Type>() {
			{ "orders", typeof(OrdersPage) },
			{ "shopping", typeof(ShoppingPage) },
			{ "storage", typeof(StoragePage) },
			{ "route", typeof(RoutePage) },
			{ "phones", typeof(ContactsPage) },
			{ "customers", typeof(CustomersPage) },
			{ "produce", typeof(ProducePage) },
			{ "settings", typeof(SettingsPage) },
			{ "about", typeof(AboutPage) }
		};

		public void RegisterRoutes()
		{
			foreach (var item in Routes) {
				Xamarin.Forms.Routing.RegisterRoute(item.Key, item.Value);
			}
		}


	}
}
