using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Dwares.Druid.UI;
using Dwares.Druid.Satchel;
using ACE.Models;


namespace ACE.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
		public MainPage()
		{
			InitializeComponent();

			//TODO
			//InitDebugToolbar();

			Page[] pages;
			if (false/*DeviceEx.Idiom == TargetIdiom.Phone*/) {
				pages = new Page[] {
					//new PickupsListPage() { Title = "Pickups" },
					new SchedulePage() { Title = "Schedule" },
					new RoutePage() { Title = "Route" },
					new ContactsListPage(ContactType.ACE, true) { Title = "Contacts" },
				};
			} else {
				pages = new Page[] {
					//new PickupsListPage() { Title = "Pickups" },
					new SchedulePage() { Title = "Schedule" },
					new RoutePage() { Title = "Route" },
					new ContactsListPage(ContactType.ACE) { Title = "ACE" },
					new ContactsListPage(ContactType.Client) { Title = "Clients" },
					new ContactsListPage(ContactType.Office) { Title = "Offices" }
				};
			}

			foreach (var page in pages) {
				var navigationPage = new NavigationPage(page) {
					Title = page.Title
				};
				//navigationPage.Icon = new Xamarin.Forms.OnPlatform<FileImageSource> {  iOS = "tab_feed.png"}
				Children.Add(navigationPage);
			}
		}

		[System.Diagnostics.Conditional("DEBUG")]
		public void InitDebugToolbar()
		{
			foreach (var page in this.Children) {
				page.ToolbarItems.Add(new ToolbarItemEx { Text = "Device", Symbol = SymbolEx.CellPhone, Writ = "SelectDevice" });
			}
		}
	}
}