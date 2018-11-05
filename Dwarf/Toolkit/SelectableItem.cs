using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Toolkit
{
	public class SelectableItem : PropertyNotifier, ISelectable
	{
		bool isSelected;
		public bool IsSelected {
			get => isSelected;
			set {
				if (value != isSelected) {
					isSelected = value;
					OnSelectedChanged();
				}
			}
		}

		protected virtual void OnSelectedChanged()
		{
			FirePropertyChanged(nameof(IsSelected));
		}
	}
}
