using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Buffy.Models;


namespace Buffy.ViewModels
{
	public class SummaryCell : PropertyNotifier
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryCell));

		public SummaryCell(BaseSummary summary)
		{
			//Debug.EnableTracing(@class);
			Summary = summary ?? throw new ArgumentNullException(nameof(summary));
			Summary.PropertyChanged += (s, e) => FirePropertyChanged(e.PropertyName);
		}

		BaseSummary Summary { get; }

		public string Title {
			get => Summary.Title;
		}

		public string Total {
			get => Summary.Total > 0 ? Summary.Total.ToString("C") : string.Empty;
		}

		public string Gallons {
			get {
				if (Summary.Gallons > 0) {
					return Summary.IsEstimated ? "~" : string.Empty + Summary.Gallons.ToString("N3") + "G";
				} else {
					return string.Empty;
				}
			}
		}

	}
}
