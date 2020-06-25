using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid;
using Lost.Services;
using Lost.Views;
using Lost.ViewModels;
using Lost.Storage;


namespace Lost
{
	public partial class App : Application
	{
		public App()
		{
			Dwares.Dwarf.Package.Init();
			Dwares.Druid.Package.Init();

			InitializeComponent();

			BindingContext = AppScope.Instance;
			this.AddDefaultViewLocators();

			DependencyService.Register<MockDataStore>();
			
			MainPage = new AppShell();
		}

		protected override async void OnStart()
		{
			AppStorage.Instance = new Storage.Air.AirStorage();
			
			var shift = await AppStorage.Instance.Initialize();
			ShiftViewModel.Instance.Initialize(shift);

			await AppStorage.Instance.ListShifts(PeriodsViewModel.Instance.Periods);
		}

		protected override void OnSleep()
		{
		}

		protected override void OnResume()
		{
		}
	}
}
