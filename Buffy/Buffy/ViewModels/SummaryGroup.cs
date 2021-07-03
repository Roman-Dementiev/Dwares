using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Buffy.Models;


namespace Buffy.ViewModels
{
	public class SummaryGroup : ObservableCollection<SummaryCell>
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryGroup));

		public SummaryGroup(YearSummary summary)
		{
			//Debug.EnableTracing(@class);
			Summary = summary ?? throw new ArgumentNullException(nameof(summary));
			Summary.PropertyChanged += (s, e) => OnPropertyChanged(e);
		}

		public YearSummary Summary { get; }
		public int Year => Summary.Year;

		public string Title {
			get => Summary.Title;
		}

		public string Total {
			get => Summary.TotalStr();
		}

		public string Gallons {
			get => Summary.GallonsStr();
		}
	}
}
