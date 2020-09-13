using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Collections
{
	public enum MoveDirection
	{
		Up,
		Down
	}

	// NOTE: AutoOrdinals odoes not work for nested items in GroupedOrderableCollection<TGroup>.
	//       Use GroupedOrderableCollection<TGroup, TItem> instead.

	public class GroupedOrderableCollection<TGroup> : OrderableCollection<TGroup>, IGroupedOrderableCollection
		where TGroup : class, IOrderableCollection
	{
		//static ClassRef @class = new ClassRef(typeof(GroupedOrderableCollection));

		public new event EventHandler<GroupedOrderableCollectionChangedEventArgs> OrderChanged;

		public GroupedOrderableCollection()
		{
			//Debug.EnableTracing(@class);
		}

		public override void ChangeOrder(int oldFlatIndex, int newFlatIndex)
		{
			if (newFlatIndex == 0)
				return;

			var moveDirection = oldFlatIndex < newFlatIndex ? MoveDirection.Down : MoveDirection.Up;
			var priorFlatIndex = oldFlatIndex;
			var latterFlatIndex = newFlatIndex;

			var oldGroup = GetGroupFromFlatIndex(priorFlatIndex, moveDirection);
			var newGroup = GetGroupFromFlatIndex(latterFlatIndex, moveDirection);

			var changedItem = GetItemFromFlatIndex(priorFlatIndex);

			if (moveDirection == MoveDirection.Up) {
				// add one to where we delete, because we're increasing the index by inserting
				if (oldGroup.Equals(newGroup))
					priorFlatIndex += 1;
			} else {
				// add one to where we insert, because we haven't deleted the original yet
				latterFlatIndex += 1;
			}

			var priorNestedIndex = GetNestedItemIndexFromFlatIndex(priorFlatIndex);
			var latterNestedIndex = GetNestedItemIndexFromFlatIndex(latterFlatIndex);


			newGroup.Insert(latterNestedIndex, changedItem);
			((IList)oldGroup).RemoveAt(priorNestedIndex);

			//if (AutoOrdinals) {
			//	oldGroup.ResetOrdinals(OrdinalType.Nested);

			//	if (newGroup != oldGroup) {
			//		newGroup.ResetOrdinals(OrdinalType.Nested);
			//	}
			//}

			OrderChanged?.Invoke(
				this,
				new GroupedOrderableCollectionChangedEventArgs(changedItem,
																oldGroup,
																newGroup,
																priorNestedIndex,
																latterNestedIndex)
				);
		}

		public object GetItemFromFlatIndex(long flatIndex)
		{
			object item = null;

			do {
				int currentTotalIndex = -1;
				foreach (var group in this) {
					currentTotalIndex++;
					foreach (var i in group) {
						currentTotalIndex++;

						if (currentTotalIndex == flatIndex)
							item = i;
					}
				}

			} while (item == null);

			return item;
		}

		public TGroup GetGroupFromFlatIndex(int flatIndex, MoveDirection direction)
		{
			if (flatIndex < 1)
				return this[0];

			int currentFlatIndex = 0;
			foreach (var group in this) {
				if (currentFlatIndex == flatIndex
					&& direction == MoveDirection.Down)
					return group;

				currentFlatIndex++;


				foreach (var item in group) {
					if (currentFlatIndex == flatIndex)
						return group;

					currentFlatIndex++;
				}

				if (currentFlatIndex == flatIndex
					&& direction == MoveDirection.Up)
					return group;

			}

			return null;
		}

		public int GetNestedItemIndexFromFlatIndex(int flatIndex)
		{
			if (flatIndex < 1)
				return 0;

			int currentFlatIndex = 0;
			foreach (var group in this) {
				int nestedIndex = 0;
				currentFlatIndex++;

				foreach (var item in group) {
					if (currentFlatIndex == flatIndex)
						return nestedIndex++;

					currentFlatIndex++;
					nestedIndex++;
				}

				if (currentFlatIndex == flatIndex)
					return nestedIndex;

			}
			return -1;
		}

	}

	public class GroupedOrderableCollection<TGroup, TItem> : GroupedOrderableCollection<TGroup>
		where TGroup : class, IList<TItem>, IOrderableCollection
	{
		//static ClassRef @class = new ClassRef(typeof(GroupedOrderableCollection));

		public GroupedOrderableCollection()
		{
			//Debug.EnableTracing(@class);
		}

		protected override void HandleCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			base.HandleCollectionChanged(sender, e);

			if (e.Action == NotifyCollectionChangedAction.Move)
				return;

			if (e.OldItems != null) {
				foreach (var item in e.OldItems) {
					if (item is ObservableCollection<TItem> group) {
						group.CollectionChanged -= NestedCollectionChanged;
					}
				}
			}

			if (e.NewItems != null) {
				foreach (var item in e.NewItems) {
					if (item is ObservableCollection<TItem> group) {
						group.CollectionChanged += NestedCollectionChanged;
					}
				}
			}
		}

		protected void NestedCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
		{
			if (AutoOrdinals && sender is TGroup group) {
				group.ResetOrdinals(OrdinalType.Nested);
			}
		}
	}
}
