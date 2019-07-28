using System;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid;
using Dwares.Druid.Satchel;
using Dwares.Druid.UI;
using Xamarin.Forms;

namespace Drive.ViewModels
{
	public enum RootContentType
	{
		Schedule,
		Route,
		Contacts
	}

	interface IRootContentViewModel
	{
		Type ContentViewType();
		Type ControlsViewType(bool landscape);
	}

	public class ActiveContentMessage
	{
		public RootContentType? ActiveContent { get; set; }
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
					PropertiesChanged(nameof(ControlBarRow), nameof(ControlBarColumn),
						nameof(NavigationBarRow), nameof(NavigationBarColumn),
						nameof(NavigationBarOrientation), nameof(NavButtonsOrientation));
					if (ContentViewModel != null) {
						Controls = Forge.CreateView(ContentViewModel.ControlsViewType(IsLandscape), ContentViewModel);
					}
				}
			}
		}

		public int ControlBarRow {
			get => IsLandscape ? 1 : 0;
		}

		public int ControlBarColumn {
			get => IsLandscape ? 0 : 1;
		}

		public int NavigationBarRow {
			get => IsLandscape ? 1 : 2;
		}

		public int NavigationBarColumn {
			get => IsLandscape ? 2 : 1;
		}

		public StackOrientation NavigationBarOrientation {
			get => IsLandscape ? StackOrientation.Vertical : StackOrientation.Horizontal;
		}

		public StackOrientation NavButtonsOrientation {
			get => IsLandscape ? StackOrientation.Horizontal : StackOrientation.Vertical;
		}


		RootContentType? activeContent;
		public RootContentType? ActiveContent {
			get => activeContent;
			set {
				if (SetProperty(ref activeContent, value)) {
					var message = new ActiveContentMessage() { ActiveContent = activeContent };
					MessageBroker.Send(message);
				}
			}
		}

		//public bool ScheduleTabIsActive {
		//	get => ActiveTab == RootTab.Schedule;
		//}
		//public bool RouteTabIsActive {
		//	get => ActiveTab == RootTab.Route;
		//}
		//public bool ContactsTabIsActive {
		//	get => ActiveTab == RootTab.Contacts;
		//}

		IRootContentViewModel ContentViewModel { get; set; }

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


		void GoTo(RootContentType contentType, IRootContentViewModel contentViewModel)
		{
			if (contentType == ActiveContent)
				return;

			ActiveContent = contentType;
			ContentViewModel = contentViewModel;

			Content = Forge.CreateView(ContentViewModel.ContentViewType(), ContentViewModel);
			Controls = Forge.CreateView(ContentViewModel.ControlsViewType(IsLandscape), ContentViewModel);

			var mainPage = Application.Current.MainPage;
			if (mainPage != null && Content is ContentViewEx contentEx) {
				mainPage.SetToolbarItems(contentEx.ToolbarItems);
				//mainPage.SetPageTitle(contentEx.Title);
			}
		}

		public void OnGoToSchedule() 
			=> GoTo(RootContentType.Schedule, new ScheduleViewModel());

		public void OnGoToRoute() 
			=> GoTo(RootContentType.Route, new RouteViewModel());

		public void OnGoToContacts() 
			=> GoTo(RootContentType.Contacts, new ContactsViewModel());
	
	
	}

}
