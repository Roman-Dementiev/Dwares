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
		//Style tabButton_Default;
		//Style tabButton_Active;

		public RootViewModel()
		{
			Title = "ZenRide";
			
			MessageBroker.SubscribePageSizeMessage(this, (message) => {
				//Debug.Print($"RootViewModel.OnPageSizeMessage(): PageWidthh={message.PageWidth} PageHeight={message.PageHeight}");
				IsLandscape = message.PageWidth > message.PageHeight; 
			});

			//tabButton_Default = UITheme.Current.GetStyleByName("TabButton-default");
			//tabButton_Active = UITheme.Current.GetStyleByName("TabButton-active");
		}

		//void OnOrientationChanged()
		//{

		//	if (IsLandscape) {
		//		ControlBarRow = 1;
		//		ControlBarColumn = 0;
		//		ButtonsOrientation = StackOrientation.Horizontal;
		//	} else {
		//		ControlBarRow = 0;
		//		ControlBarColumn = 1;
		//		ButtonsOrientation = StackOrientation.Vertical;
		//	}
		//}

		bool isLandscape = false;
		public bool IsLandscape {
			get => isLandscape;
			private set {
				if (SetProperty(ref isLandscape, value)) {
					PropertiesChanged(nameof(TabButtonsOrientation), nameof(ControlBarRow), nameof(ControlBarColumn));
					if (ContentViewModel != null) {
						Controls = Forge.CreateView(ContentViewModel.ControlsViewType(IsLandscape), ContentViewModel);
					}
				}
			}
		}

		public StackOrientation TabButtonsOrientation {
			get => IsLandscape ? StackOrientation.Horizontal : StackOrientation.Vertical;
		}

		public int ControlBarRow {
			get => IsLandscape ? 1 : 0;
		}

		public int ControlBarColumn {
			get => IsLandscape ? 0 : 1;
		}


		RootTab? activeTab;
		public RootTab? ActiveTab {
			get => activeTab;
			set => SetPropertyEx(ref activeTab, value, nameof(ActiveTab),
				nameof(ScheduleTabIsActive), nameof(RouteTabIsActive), nameof(ContactsTabIsActive)
				);
		}

		public bool ScheduleTabIsActive {
			get => ActiveTab == RootTab.Schedule;
		}
		public bool RouteTabIsActive {
			get => ActiveTab == RootTab.Route;
		}
		public bool ContactsTabIsActive {
			get => ActiveTab == RootTab.Contacts;
		}

		ITabContentViewModel ContentViewModel { get; set; }

		View content;
		public View Content {
			get => content;
			set => SetProperty(ref content, value);
		}

		View controls;
		public View Controls {
			get => controls;
			set => SetProperty(ref controls, value);
		}


		void GoToTab(RootTab tab, ITabContentViewModel contentViewModel)
		{
			if (tab == ActiveTab)
				return;

			ActiveTab = tab;
			ContentViewModel = contentViewModel;

			Content = Forge.CreateView(ContentViewModel.ContentViewType(), ContentViewModel);
			Controls = Forge.CreateView(ContentViewModel.ControlsViewType(IsLandscape), ContentViewModel);

			var mainPage = Application.Current.MainPage;
			if (mainPage != null && content is ContentViewEx contentEx) {
				mainPage.SetToolbarItems(contentEx.ToolbarItems);
				//mainPage.SetPageTitle(contentEx.Title);
			}

		}

		public void OnGoToSchedule() 
			=> GoToTab(RootTab.Schedule, new ScheduleViewModel());

		public void OnGoToRoute() 
			=> GoToTab(RootTab.Route, new RouteViewModel());

		public void OnGoToContacts() 
			=> GoToTab(RootTab.Contacts, new ContactsViewModel());
	
	
	}

}
