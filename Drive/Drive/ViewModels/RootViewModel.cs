using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
using Xamarin.Forms;

namespace Drive.ViewModels
{
	public enum RootTab
	{
		Schedule,
		Route,
		Contacts
	}

	public class RootViewModel : ViewModel
	{
		Style tabButton_Default;
		Style tabButton_Active;

		public RootViewModel()
		{
			Title = "ZenRide";
			
			MessageBroker.SubscribePageSizeMessage(this, OnPageSizeMessage);

			tabButton_Default = UITheme.Current.GetStyleByName("TabButton-default");
			tabButton_Active = UITheme.Current.GetStyleByName("TabButton-active");
		}

		StackOrientation buttonsOrientation;
		public StackOrientation ButtonsOrientation {
			get => buttonsOrientation;
			set => SetProperty(ref buttonsOrientation, value);
		}


		RootTab? activeTab;
		public RootTab? ActiveTab {
			get => activeTab;
			set => SetPropertyEx(ref activeTab, value, nameof(ActiveTab),
				nameof(ScheduleIcon), nameof(RouteIcon), nameof(ContactsIcon),
				nameof(ScheduleTabStyle), nameof(RouteTabStyle), nameof(ContactsTabStyle),
				nameof(ContentBorderColor));
		}

		public string ScheduleIcon {
			get => ActiveTab == RootTab.Schedule ? "Schedule_active" : "Schedule";
		}
		public string RouteIcon {
			get => ActiveTab == RootTab.Route ? "Route_active" : "Route";
		}
		public string ContactsIcon {
			get => ActiveTab == RootTab.Contacts ? "Contacts_active" : "Contacts";
		}

		Style TabButtonStyle(RootTab tab) {
			return ActiveTab == tab ? tabButton_Active : tabButton_Default;
		}

		public Style ScheduleTabStyle {
			get => TabButtonStyle(RootTab.Schedule);
		}
		public Style RouteTabStyle {
			get => TabButtonStyle(RootTab.Route);
		}
		public Style ContactsTabStyle {
			get => TabButtonStyle(RootTab.Contacts);
		}

		public Color ContentBorderColor {
			get => ActiveTab == RootTab.Schedule ? Color.Transparent : Color.Black;
		}

		View content;
		public View Content {
			get => content;
			set => SetProperty(ref content, value);
		}



		void GoToTab(RootTab tab, object contentViewModel)
		{
			if (tab == ActiveTab)
				return;

			ActiveTab = tab;

			ContentViewEx contentView = null;
			if (contentViewModel != null) {
				contentView = Forge.CreateView(contentViewModel) as ContentViewEx;
			}

			Content = contentView;

			var mainPage = Application.Current.MainPage;
			if (mainPage != null) {
				mainPage.SetToolbarItems(contentView?.ToolbarItems);
				//mainPage.SetPageTitle(contentView?.Title);
			}
		}

		public void OnGoToSchedule() 
			=> GoToTab(RootTab.Schedule, typeof(ScheduleViewModel));

		public void OnGoToRoute() 
			=> GoToTab(RootTab.Route, typeof(RouteViewModel));

		public void OnGoToContacts() 
			=> GoToTab(RootTab.Contacts, typeof(ContactsViewModel));
	
	
		void OnPageSizeMessage(PageSizeMessage message)
		{
			Debug.Print($"RootViewModel.OnPageSizeMessage(): PageWidthh={message.PageWidth} PageHeight={message.PageHeight}");

			if (message.PageWidth < message.PageHeight) {
				ButtonsOrientation = StackOrientation.Vertical;
			} else {
				ButtonsOrientation = StackOrientation.Horizontal;
			}
		}
	}

}
