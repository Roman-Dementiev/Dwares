using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;


namespace Ziply.Models
{
	public static class Settings
	{
		const string SharedName = "Ziply";
		public static Color DefaultRecipientColor = Color.Black;
		public static Color UnusedRecipientColor = Color.Gray;

		public static string Recipient1Name {
			get => Preferences.Get(nameof(Recipient1Name), string.Empty);
			set => Preferences.Set(nameof(Recipient1Name), value);
		}
		public static string Recipient1Phone {
			get => Preferences.Get(nameof(Recipient1Phone), string.Empty);
			set => Preferences.Set(nameof(Recipient1Phone), value);
		}
		public static Color Recipient1Color {
			get => Color.FromHex(Preferences.Get(nameof(Recipient1Color), DefaultRecipientColor.ToHex()));
			set => Preferences.Set(nameof(Recipient1Color), value.ToHex());
		}

		public static string Recipient2Name {
			get => Preferences.Get(nameof(Recipient2Name), string.Empty);
			set => Preferences.Set(nameof(Recipient2Name), value);
		}
		public static string Recipient2Phone {
			get => Preferences.Get(nameof(Recipient2Phone), string.Empty);
			set => Preferences.Set(nameof(Recipient2Phone), value);
		}
		public static Color Recipient2Color {
			get => Color.FromHex(Preferences.Get(nameof(Recipient2Color), DefaultRecipientColor.ToHex()));
			set => Preferences.Set(nameof(Recipient2Color), value.ToHex());
		}

		public static Recipient Recipient1
		{
			get {
				var name = Recipient1Name;
				var phone = Recipient1Phone;
				var color = Recipient1Color;
				if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone)) {
					return new Recipient {
						Name = name,
						Phone = phone,
						Color = color
					};
				} else {
					return null;
				}
			}

			set {
				if (value != null) {
					Recipient1Name = value.Name;
					Recipient1Phone = value.Phone;
					Recipient1Color = value.Color ?? DefaultRecipientColor;
				} else {
					Preferences.Clear(nameof(Recipient1Name));
					Preferences.Clear(nameof(Recipient1Phone));
					Preferences.Clear(nameof(Recipient1Color));
				}
			}
		}

		public static Recipient Recipient2 {
			get {
				var name = Recipient2Name;
				var phone = Recipient2Phone;
				var color = Recipient2Color;
				if (!string.IsNullOrEmpty(name) && !string.IsNullOrEmpty(phone)) {
					return new Recipient {
						Name = name,
						Phone = phone,
						Color = color
					};
				} else {
					return null;
				}
			}

			set {
				if (value != null) {
					Recipient2Name = value.Name;
					Recipient2Phone = value.Phone;
					Recipient2Color = value.Color ?? DefaultRecipientColor;
				} else {
					Preferences.Clear(nameof(Recipient2Name));
					Preferences.Clear(nameof(Recipient2Phone));
					Preferences.Clear(nameof(Recipient2Color));
				}
			}
		}

		public static string DestinationAddress {
			get => Preferences.Get(nameof(DestinationAddress), string.Empty);
			set => Preferences.Set(nameof(DestinationAddress), value);
		}
	}
}
