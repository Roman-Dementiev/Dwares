﻿using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Buffy.Models;


namespace Buffy.ViewModels
{
	public class SummaryCell : PropertyNotifier
	{
		//static ClassRef @class = new ClassRef(typeof(SummaryCell));

		public SummaryCell(SummaryRecord summary)
		{
			//Debug.EnableTracing(@class);
			Summary = summary ?? throw new ArgumentNullException(nameof(summary));
			Summary.PropertyChanged += (s, e) => FirePropertyChanged(e.PropertyName);
		}

		SummaryRecord Summary { get; }

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
