using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.UI
{
	public class CheckedChangedEventArgs: EventArgs
	{
		public bool IsChecked { get; set; }
	}

	public interface IToggleControl
	{
		event EventHandler<CheckedChangedEventArgs> CheckedChanged;

		// TODO
		//bool? IsChecked { get; set; }
		//bool IsThreeState { get; set; }

		bool IsChecked { get; set; }
	}
}
