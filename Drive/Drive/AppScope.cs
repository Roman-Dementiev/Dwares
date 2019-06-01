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
		}

		public Contacts Contacts { get; } = new Contacts();
		public Schedule Schedule { get; } = new Schedule();


		public async Task Initialize()
		{
			var storage = AppStorage.Instance;

			await storage.Initialize();
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

		//async Task GoToPage(object contentViewModel)
		//{
		//	//Debug.Print($"AppScope.GoToPage(): viewModel={contentViewModel}");

		//	var page = CreatePage(contentViewModel);
		//	await Navigator.ReplaceTopPage(page);
		//}

		//public async void OnGoToSchedule() => await GoToPage(typeof(ScheduleViewModel));

		//public async void OnGoToRoute() => await GoToPage(typeof(RouteViewModel));

		//public async void OnGoToContacts() => await GoToPage(typeof(ContactsViewModel));

		// TODO: Move to some Theme related class?
		public static readonly Color DefaultBottomButtonColor = Color.Black;
		public static readonly Color ActiveBottomButtonColor  = new Color(0, 148.0/255.0, 255.0/255.0); // $0094FF
		public static readonly Thickness MainPanelMargin = new Thickness(-16, -16, -16, 4);
	}
}
