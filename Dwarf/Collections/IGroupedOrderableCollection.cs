using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Collections
{
	/// <summary>
	/// Used by bound collections to expose item retrieval 
	/// </summary>
	public interface IGroupedOrderableCollection : IOrderableCollection
	{
		new event EventHandler<GroupedOrderableCollectionChangedEventArgs> OrderChanged;

		/// <summary>
		/// Get's the nested Item from an index of a flat list
		/// </summary>
		/// <param name="flatIndex"></param>
		/// <returns></returns>
		object GetItemFromFlatIndex(long flatIndex);
	}

	public sealed class GroupedOrderableCollectionChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Object that moved
		/// </summary>
		public object ChangedObject { get; set; }
		/// <summary>
		/// Old group
		/// </summary>
		public IList OldGroup { get; set; }
		/// <summary>
		/// New group
		/// </summary>
		public IList NewGroup { get; set; }

		/// <summary>
		/// Pior index from old group
		/// </summary>
		public int OldIndex { get; set; }
		/// <summary>
		/// New index in new group
		/// </summary>
		public int NewIndex { get; set; }

		public GroupedOrderableCollectionChangedEventArgs(
			object changedObject,
			IList oldGroup,
			IList newGroup,
			int oldIndex,
			int newIndex
			)
		{
			ChangedObject = changedObject;
			OldGroup = oldGroup;
			NewGroup = newGroup;
			OldIndex = oldIndex;
			NewIndex = newIndex;
		}
	}

}
