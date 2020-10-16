using Dwares.Druid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Dwares.Druid.ViewModels;
using Ziply.Models;

namespace Ziply.ViewModels
{
	public class BaseViewModel : ViewModel
	{
		protected const string BMK = "At3kj4rBGQ5lVXSMcxAoYc7AQ2tLFhbyfikyPfaEbXuw03XiRTGCWAdYeiUzqFNa";
		protected const string Unknown = "?????";


		protected BaseViewModel(double expireMinutes)
		{
			ExpirePeriod = TimeSpan.FromMinutes(expireMinutes);
			RefreshCommand = new Command(async () => await Refresh(false));
			ShareCommand = new Command(async () => await Share());
			SendToRecipient1Command = new Command(async () => Recipient1 = await SendToRecipient(Recipient1));
			SendToRecipient2Command = new Command(async () => Recipient2 = await SendToRecipient(Recipient2));

			Recipient1 = new Recipient {
				Name = "Andriy",
				Phone = "267-707-7788"
			};
		}

		public TimeSpan ExpirePeriod { get; set; }
		public DateTime? LastRefreshed { get; protected set; }

		public Command RefreshCommand { get; set; }
		public Command ShareCommand { get; set; }
		public Command SendToRecipient1Command { get; set; }
		public Command SendToRecipient2Command { get; set; }

		public string ButtonText {
			get => buttonText;
			set => SetProperty(ref buttonText, value);
		}
		string buttonText;

		protected void SetButtonText(string text)
		{
			if (string.IsNullOrEmpty(text))
				text = Unknown;

			ButtonText = $" {text} ";
		}

		public Recipient Recipient1 {
			get => recipient1;
			set => SetPropertyEx(ref recipient1, value, nameof(Recipient1), nameof(Recipient1Title), nameof(Recipient1Color));
		}
		static Recipient recipient1;

		public Recipient Recipient2 {
			get => recipient2;
			set => SetPropertyEx(ref recipient2, value, nameof(Recipient2), nameof(Recipient2Title), nameof(Recipient2Color));
		}
		static Recipient recipient2;

		public string Recipient1Title {
			get => Recipient.GetTitle(Recipient1);
		}
		public Color? Recipient1Color {
			get => Recipient.GetColor(Recipient1, DefaultRecipientColor, UnusedRecipientColor);
		}

		public string Recipient2Title {
			get => Recipient.GetTitle(Recipient2);
		}
		public Color? Recipient2Color {
			get => Recipient.GetColor(Recipient2, DefaultRecipientColor, UnusedRecipientColor);
		}

		public Color DefaultRecipientColor {
			get => Color.Black;
		}

		public Color UnusedRecipientColor {
			get => Color.Gray;
		}


		public virtual async Task Activate()
		{
			if (CheckIfExpired()) {
				await Refresh(true);
			}
		}

		public virtual Task Refresh(bool silent)
		{
			return Task.CompletedTask;
		}

		public bool CheckIfExpired()
		{
			if (LastRefreshed == null)
				return true;

			var lastRefreshed = (DateTime)LastRefreshed;
			var now = DateTime.Now;
			if (now > lastRefreshed) {
				return (now - lastRefreshed) >= ExpirePeriod;
			} else {
				return false;
			}
		}


		protected virtual string GetMessageText()
		{
			return null;
		}

		async Task Share()
		{
			string text = GetMessageText();
			if (text == null)
				return;

			await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest {
				//Subject = "Current location",
				Text = text
			});
		}

		async Task<Recipient> SendToRecipient(Recipient recipient)
		{
			if (recipient == null) {
				try {
					var contact = await Contacts.PickContactAsync();
					if (contact == null)
						return null;

					var contactPhone = contact.Numbers.FirstOrDefault();
					if (contactPhone == null)
						return null;

					recipient = new Recipient {
						Name = contact.Name,
						Phone = contactPhone.PhoneNumber
					};
				}
				catch (TaskCanceledException) {
					return null;
				}
				catch (Exception exc) {
					await Alerts.ExceptionAlert(exc);
					return null;
				}
			}

			try {
				string text = GetMessageText();
				if (text != null) {
					var message = new SmsMessage(text, recipient.Phone);
					await Sms.ComposeAsync(message);
				}
			}
			catch (FeatureNotSupportedException) {
				await Alerts.Error("Sending SMS is not supported on this device.");
			}
			catch (Exception exc) {
				await Alerts.ExceptionAlert(exc);
			}


			return recipient;
		}

		public async Task ClearRecipient(bool recipient2)
		{
			if (await Alerts.ConfirmAlert("Do you want to clear recipient?")) {
				if (recipient2) {
					Recipient2 = null;
				} else {
					Recipient1 = null;
				}
			}
		}
	}
}
