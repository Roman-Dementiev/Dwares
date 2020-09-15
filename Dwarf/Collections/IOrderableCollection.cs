using System;
using System.Collections;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Collections
{
	public enum OrdinalType
	{
		Default,
		Nested
	}

	public interface IOrderableCollection : IList
	{
		/// <summary>
		/// Event fired when the items in the collection are re-ordered.
		/// </summary>
		event EventHandler OrderChanged;

		/// <summary>
		/// Used to change the item orders in an enumerable
		/// </summary>
		/// 
		/// The old index of the item.
		/// 
		/// 
		/// The new index of the item.
		/// 
		void ChangeOrder(int oldIndex, int newIndex);

		void ResetOrdinals(OrdinalType ordinalType);
	}

}
