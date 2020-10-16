using System;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Ziply.Models
{
	public class Recipient
	{
		//static ClassRef @class = new ClassRef(typeof(Contact));

		public Recipient()
		{
			//Debug.EnableTracing(@class);
		}

		public string Name { get; set; }
		public string Phone { get; set; }

		public Color? Color { get; set; }


		public static string GetTitle(Recipient contact)
		{
			if (contact != null)
			{
				if (!string.IsNullOrEmpty(contact.Name))
					return contact.Name;

				if (!string.IsNullOrEmpty(contact.Phone))
					return contact.Phone;
			}

			return string.Empty;
		}

		public static Color GetColor(Recipient contact, Color defaultColor, Color unusedColor)
		{
			if (contact == null) {
				return unusedColor;
			} else if (contact.Color == null) {
				return defaultColor;
			} else {
				return (Color)contact.Color;
			}
		}

	}
}
