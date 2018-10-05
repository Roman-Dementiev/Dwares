using System;
using System.Collections;
using System.Collections.Generic;


namespace Dwares.Dwarf.Collections
{
	public class WeakCollection : WeakCollection<object>
	{
		public WeakCollection() { }
	}


	public class WeakCollection<TItem> : ICollection<TItem> where TItem : class
	{
		Collection collection;

		public WeakCollection()
		{
			collection = new Collection();
		}

		IEnumerable<WeakReference<TItem>> References => collection;

		public bool IsReadOnly => collection.IsReadOnly;
		public int Count => collection.Recount();
		public void Cleanup() => collection.Recount();
		public void Clear() => collection.Clear();

		public bool Contains(TItem item)
		{
			return collection.Iterate((prev, node, target) => (target == item)) != null;
		}

		public void CopyTo(TItem[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));

			int index = arrayIndex;
			collection.Iterate((prev, node, target) => {
				if (index >= array.Length)
					throw new ArgumentException();
				array[index++] = target;
				return false;
			});
		}

		public void Add(TItem item)
		{
			collection.Add(new WeakReference<TItem>(item));
		}

		public bool Remove(TItem item)
		{
			var found = collection.Iterate((prev, node, target) => {
				if (target == item) {
					prev.Next = node.Next;
					node.Next = null;
					return true;
				} else {
					return false;
				}
			});
			return found != null;
		}

		public void ForEach(Action<TItem> action)
		{
			collection.Iterate((prev, node, target) => {
				action(target);
				return false;
			});
		}

		public TItem Find(Func<TItem, bool> func)
		{
			collection.Iterate((prev, node, target) => func(target), out TItem found);
			return found;
		}

		public IEnumerator<TItem> GetEnumerator() => new Enumerator(collection.Head);
		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(collection.Head);

		class Collection : LinkedCollection<WeakReference<TItem>, TItem>
		{
			public int Recount()
			{
				count = 0;
				ForEach(node => count++);
				return count;
			}

			public override Node Iterate(Func<Node, Node, TItem, bool> func, out TItem item)
			{
				Node prev = head;
				for (var node = head.Next; node != null; node = node.Next) {
					if (node.Item.TryGetTarget(out item)) {
						if (func(prev, node, item)) {
							return node;
						}
						prev = node;
					} else {
						prev.Next = node.Next;
					}
				}
				item = null;
				return null;
			}
		}

		internal class Enumerator : LinkedCollection<WeakReference<TItem>, TItem>.Enumerator, IEnumerator<TItem>
		{
			TItem current = null;

			public Enumerator(LinkedCollection<WeakReference<TItem>, TItem>.Node head) : base(head) { }

			public new TItem Current => current;
			object IEnumerator.Current => current;

			public new bool MoveNext()
			{
				if (node != null) {
					prev = node;

					for (; ; ) {
						node = prev.Next;
						if (node == null)
							break;

						if (node.Item.TryGetTarget(out current)) {
							return true;
						} else {
							prev.Next = node.Next;
							node.Next = null;
						}
					}
				}

				current = null;
				return false;
			}

			public new void Reset()
			{
				base.Reset();
				current = null;
			}
		}
	}
}
