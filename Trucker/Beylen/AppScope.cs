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

		public Contacts<Contact> Contacts { get; } = new Contacts<Contact>();
		public Contacts<Customer> Customers { get; } = new Contacts<Customer>();

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
		}

		/* Xamarin.Forms Routing
		 */

		public Dictionary<string, Type> Routes { get; } = new Dictionary<string, Type>() {
			{ "orders", typeof(ItemsPage) },
			{ "shopping", typeof(ItemsPage) },
			{ "storage", typeof(AboutPage) },
			{ "route", typeof(AboutPage) },
			{ "phones", typeof(ContactsPage) },
			{ "customers", typeof(AboutPage) },
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
