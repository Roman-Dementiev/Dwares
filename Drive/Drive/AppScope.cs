using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Drive.Models;
using Xamarin.Forms;
using Dwares.Druid.UI;
using Drive.ViewModels;


namespace Drive
{
	public class AppScope : BindingScope
	{
		//static ClassRef @class = new ClassRef(typeof(AppScope));

		static AppScope instance;
		public static AppScope Instance {
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		public AppScope() : base(null)
		{
			//Debug.EnableTracing(@class);

			Tags = new Tags();
			Contacts = new Contacts();
			Schedule = new Schedule();
		}

		public Tags Tags { get; }
		public Contacts Contacts { get; }
		public Schedule Schedule { get; }
		public Route Route {
			get => Schedule.Route; 
		}

		public async Task Initialize()
		{
			var storage = AppStorage.Instance;

			await storage.Initialize();
			await storage.LoadTags();
			await storage.LoadContacts();
			await storage.LoadSchedule();
		}

		public static Page CreatePage(object contentViewModel)
		{
			View contentView;
			var page = Forge.CreateContentPage<FramedPage>(contentViewModel, out contentView);

			if (Device.Idiom == TargetIdiom.Desktop) {
				contentView.WidthRequest = 360;
				contentView.HeightRequest = 640;
				page.DecorationLayout = DecorationLayout.Center;
			} else {
				page.DecorationLayout = DecorationLayout.FullScreen;
				page.BorderIsVisible = false;
			}

			return page;
		}

		public async void OnSettings()
		{
			Debug.Print("AppScope.OnSettings()");

			var page = CreatePage(typeof(SettingsViewModel));
			await Navigator.PushPage(page);
		}

		public void OnAbout()
		{
			Debug.Print("AppScope.OnAbout()");
		}
	}
}
