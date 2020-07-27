using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid;


namespace Beylen.Models
{
	public class CardModel : Model, ISelectable
	{
		//static ClassRef @class = new ClassRef(typeof(CardModel));

		public CardModel()
		{
			//Debug.EnableTracing(@class);
		}

		public bool IsSelected {
			get => isSelected;
			set {
				if (SetProperty(ref isSelected, value)) {
					OnSelectedChanged();
				}
			}
		}
		bool isSelected;

		protected virtual void OnSelectedChanged() { }
	}
}
