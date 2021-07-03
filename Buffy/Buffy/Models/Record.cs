using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Buffy.Models
{
	public class Record : PropertyNotifier
	{
		//static ClassRef @class = new ClassRef(typeof(BaseRecord));

		public Record()
		{
			//Debug.EnableTracing(@class);
		}

		public decimal Gallons {
			get => gallons;
			set => SetProperty(ref gallons, value);
		}
		decimal gallons;

		public decimal Price {
			get => price;
			set => SetProperty(ref price, value);
		}
		decimal price;

		public decimal Total {
			get => total;
			set => SetProperty(ref total, value);
		}
		decimal total;

		public string TotalStr()
		{
			return Total > 0 ? Total.ToString("C") : string.Empty;
		}

		public string PriceStr(bool isAverage = false)
		{
			if (Price > 0) {
				return isAverage ? "~" : string.Empty + Price.ToString("C");
			} else {
				return string.Empty;
			}
		}

		public string GallonsStr(bool isEstimated = false)
		{
			if (Gallons > 0) {
				return isEstimated ? "~" : string.Empty + Gallons.ToString("N3") + "G";
			} else {
				return string.Empty;
			}
		}
	}
}
