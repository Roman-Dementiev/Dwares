using System;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Buffy.Models;


namespace Buffy.ViewModels
{
	public class FuelingCell : PropertyNotifier
	{
		//static ClassRef @class = new ClassRef(typeof(FuelingCell));

		public FuelingCell(Fueling fueling)
		{
			//Debug.EnableTracing(@class);
			Fueling = fueling ?? throw new ArgumentNullException(nameof(fueling));
			Fueling.PropertyChanged += (s, e) => FirePropertyChanged(e.PropertyName);
		}

		public Fueling Fueling { get; }

		public string VendorIcon {
			get => Fueling.VendorIcon;
		}

		public string VendorName {
			get => Fueling.VendorName;
		}

		public string Date {
			get => Fueling.Date.ToShortDateString();
		}

		public string Gallons {
			get => Fueling.Gallons > 0 ? Fueling.Gallons.ToString("N3") + "G" : string.Empty;
		}

		public string Price {
			get => Fueling.Price > 0 ? Fueling.Price.ToString("C") : string.Empty;
		}

		public string Total {
			get => Fueling.Total > 0 ? Fueling.Total.ToString("C") : string.Empty;
		}
	}
}
