using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Satchel
{
	public interface ISelectionHandler
	{
		void OnSelectedChanged(ref object selectedItem, int selectedIndex);
	}
}
