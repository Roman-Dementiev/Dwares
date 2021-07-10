using Dwares.Druid.UI;
using Dwares.Dwarf;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Drive.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : ContentPageEx
	{
		public MainPage()
		{
			//AboutCommand = new PushPageCommand<AboutPage>(this);
			AboutCommand = new GoToPageCommand<AboutPage>(nameof(AboutPage));
			SettingsCommand = new GoToPageCommand<SettingsPage>(nameof(SettingsPage));

			BindingContext = this;
			InitializeComponent();
		}

		public Command AboutCommand { get; }
		public Command SettingsCommand { get; }

	}

	internal class PushPageCommand<T> : Command where T: Page, new()
	{
		public PushPageCommand(Page parent) :
			base(async () => {
				await parent.Navigation.PushAsync(new T());
			})
		{ }
	}

	internal class GoToPageCommand<T> : Command where T : Page, new()
	{
		public GoToPageCommand(string path) :
			base(async () => {
				await Shell.Current.GoToAsync(path);
			})
		{ }
	}
}
