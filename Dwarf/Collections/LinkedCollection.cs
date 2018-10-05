using System;
using System.Collections;
using System.Collections.Generic;


namespace Dwares.Dwarf.Collections
{
	public class LinkedCollection<TItem> : LinkedCollection<TItem, object>
	{
		public LinkedCollection() { }
		public LinkedCollection(IEnumerable<TItem> items, bool isReadOnly = false) : base(items, isReadOnly) { }
	}

	public class LinkedCollection<TItem, TTarget> : ICollection<TItem>
	{
		protected Node head;
		protected Node last;
		protected int count;

		public LinkedCollection()
		{
			head = new Node();
			last = head;
			count = 0;
		}

		public LinkedCollection(IEnumerable<TItem> items, bool isReadOnly = false) :
			this()
		{
			foreach (var item in items) {
				Add(item);
			}

			IsReadOnly = isReadOnly;
		}

		public bool IsReadOnly { get; set; } = false;
		public bool AddToStart { get; set; } = false;
		public TItem NotFound { get; set; } = default(TItem);

		public Node Head => head;
		public Node FirstNode => head.Next;
		public Node LastNode => last;
		public TItem FirstItem => head.Next != null ? head.Next.Item : NotFound;
		public TItem LastItem => last != head ? last.Item : NotFound;

		public int Count => count;

		public void AddFirst(TItem item)
		{
			if (IsReadOnly)
				throw new NotSupportedException(nameof(AddFirst));

			var node = new Node(item, head.Next);
			head.Next = node;
			if (last == head) {
				last = node;
			}

			count++;
		}

		public void AddLast(TItem item)
		{
			if (IsReadOnly)
				throw new NotSupportedException(nameof(AddLast));

			var node = new Node(item);
			last.Next = node;
			last = node;
			count++;
		}

		public void Add(TItem item)
		{
			if (IsReadOnly)
				throw new NotSupportedException(nameof(Add));

			if (AddToStart) {
				AddFirst(item);
			} else {
				AddLast(item);
			}
		}

		public bool Remove(TItem item)
		{
			if (IsReadOnly)
				throw new NotSupportedException(nameof(Remove));

			var renoved = Iterate((prev, node, nodeItem) => {
				if (nodeItem.Equals(item)) {
					prev.Next = node.Next;
					count--;
					return true;
				}
				return false;
			});

			return renoved != null;
		}

		public void Clear()
		{
			if (IsReadOnly)
				throw new NotSupportedException(nameof(Clear));

			head.Next = null;
			last = head;
		}

		public bool Contains(TItem item)
		{
			return Find(nodeItem => nodeItem.Equals(item)) != null;
		}

		public void CopyTo(TItem[] array, int arrayIndex)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (arrayIndex < 0)
				throw new ArgumentOutOfRangeException(nameof(arrayIndex));

			int index = arrayIndex;
			ForEach(item => {
				if (index >= array.Length)
					throw new ArgumentException();
				array[index++] = item;
			});
		}

		public void ForEach(Action<TItem> action)
		{
			Iterate((prev, node, unused) => {
				action(node.Item);
				return false;
			});
		}

		public TItem Find(Func<TItem, bool> func)
		{
			var found = Iterate((prev, node, unused) => func(node.Item));
			return (found != null) ? found.Item : NotFound;
		}

		public Node Iterate(Func<Node, Node, TTarget, bool> func)
		{
			return Iterate(func, out TTarget target);
		}

		public virtual Node Iterate(Func<Node, Node, TTarget, bool> func, out TTarget target)
		{
			target = default(TTarget);
			Node prev = head;
			for (var node = head.Next; node != null; node = node.Next) {
				if (func(prev, node, target)) {
					return node;
				}
				prev = node;
			}
			return null;
		}

		public IEnumerator<TItem> GetEnumerator() => new Enumerator(head);
		IEnumerator IEnumerable.GetEnumerator() => new Enumerator(head);

		public class Node
		{
			public Node(TItem item = default(TItem), Node next = null)
			{
				Item = item;
				Next = next;
			}

			public TItem Item { get; set; }
			public Node Next { get; set; }
		}

		internal class Enumerator : IEnumerator<TItem>
		{
			protected Node head;
			protected Node node;
			protected Node prev;

			public Enumerator(Node head)
			{
				this.head = head;
				Reset();
			}

			public void Dispose() { }

			public TItem Current => node != null ? node.Item : default(TItem);
			object IEnumerator.Current => Current;

			public bool MoveNext()
			{
				if (node != null) {
					prev = node;
					node = node.Next;
					return node != null;
				} else {
					return false;
				}
			}

			public void Reset()
			{
				node = head;
				prev = null;
			}
		}
	}
}
