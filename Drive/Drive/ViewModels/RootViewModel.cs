using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
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
		public RootViewModel()
		{
			MessageBroker.SubscribePageSizeMessage(this, OnPageSizeMessage);
			OnGoToSchedule();
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
				nameof(ScheduleTabTextColor), nameof(RouteTabTextColor), nameof(ContactsTabTextColor),
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

		public Color ScheduleTabTextColor {
			get => ActiveTab == RootTab.Schedule ? AppScope.ActiveBottomButtonColor : AppScope.DefaultBottomButtonColor;
		}
		public Color RouteTabTextColor {
			get => ActiveTab == RootTab.Route ? AppScope.ActiveBottomButtonColor : AppScope.DefaultBottomButtonColor;
		}
		public Color ContactsTabTextColor {
			get => ActiveTab == RootTab.Contacts ? AppScope.ActiveBottomButtonColor : AppScope.DefaultBottomButtonColor;
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
