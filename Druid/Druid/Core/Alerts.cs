using System;
using System.Threading.Tasks;
using Xamarin.Forms;


namespace Dwares.Druid
{
	public enum AlertPlacement
	{
		OnMainPage,
		OnNavigationPage,
		OnCurrentPage,
		OnContentPage
	}

	public static class Alerts
	{
		public static string ErrorAlertTitle { get; set; } = "Error";
		public static string ConfirmAlertTitle { get; set; } = "Confirm";
		//public static string MessageAlertTitle { get; set; } = "Message";
		public static string DetailsSeparator { get; set; } = ": ";

		public static string DismissString { get; set; } = "OK";
		public static string AcceptString { get; set; } = "Accept";
		public static string CancelString { get; set; } = "Cancel";
		public static string ConfirmString { get; set; } = "Yes";
		public static string RejectString { get; set; } = "No";

		public static AlertPlacement Placement { get; set; } = AlertPlacement.OnMainPage;

		static Page CallerPage {
			get {
				Page page = null;
				switch (Placement)
				{
				case AlertPlacement.OnNavigationPage:
					page = Navigator.NavigationPage;
					break;
				case AlertPlacement.OnCurrentPage:
					page = Navigator.CurrentPage;
					break;
				case AlertPlacement.OnContentPage:
					page = Navigator.ContentPage;
					break;
				}

				return page ?? Application.Current.MainPage;
			}
		}

		public static async Task DisplayAlert(string title, string message, string dismiss = null)
		{
			if (title == null)
				title = "";
			if (message == null)
				message = "";
			if (string.IsNullOrEmpty(dismiss))
				dismiss = DismissString;

			await CallerPage.DisplayAlert(title, message, dismiss);
		}

		public static async Task<bool> DisplayAlert(string title, string message, string accept, string cancel)
		{
			if (message == null)
				message = "";
			if (string.IsNullOrEmpty(accept))
				accept = AcceptString;
			if (string.IsNullOrEmpty(cancel))
				cancel = CancelString;

			return await CallerPage.DisplayAlert(title, message, accept, cancel);
		}

		public static Task ErrorAlert(string message, string dismiss = null)
		{
			return DisplayAlert(ErrorAlertTitle, message, dismiss);
		}

		public static Task Error(string messageFormat, params object[] args)
		{
			var message = string.Format(messageFormat, args);
			return ErrorAlert(message);
		}

		public static Task ErrorDetails(string message, string details, string separator = null)
		{
			if (string.IsNullOrEmpty(details)) {
				return ErrorAlert(message);
			} else {
				if (string.IsNullOrEmpty(separator))
					separator = DetailsSeparator;
				return ErrorAlert(message+separator+details);
			}
		}

		public static async Task<bool> ConfirmAlert(string message, string confirm = null, string reject = null)
		{
			if (string.IsNullOrEmpty(confirm))
				confirm = ConfirmString;
			if (string.IsNullOrEmpty(reject))
				reject = RejectString;

			return await DisplayAlert(ConfirmAlertTitle, message, confirm, reject);
		}

	}
}
