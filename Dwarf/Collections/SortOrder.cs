using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.Collections
{
	public class SortOrder<T> : IComparer<T>
	{
		public SortOrder(IComparer<T> baseComparer, bool descending = false) :
			this(null, baseComparer, descending)
		{
		}

		public SortOrder(Comparison<T> comparison, bool descending = false) :
			this(null, comparison, descending)
		{
		}

		public SortOrder(string name, IComparer<T> baseComparer, bool descending = false)
		{
			Debug.AssertNotNull(baseComparer);
			Name = name;
			BaseComparer = baseComparer;
			Descending = descending;
		}

		public SortOrder(string name, Comparison<T> comparison, bool descending = false) :
			this(name, Comparer<T>.Create(comparison), descending)
		{
		}

		public SortOrder(string name, SortOrder<T> order, bool descending = false) :
			this(name, order.BaseComparer, descending)
		{
		}

		public string Name { get; set; }
		public IComparer<T> BaseComparer { get; protected set; }
		public bool Descending { get; set; }

		public override string ToString()
		{
			if (String.IsNullOrEmpty(Name)) {
				return base.ToString();
			} else {
				return Name;
			}
		}

		public int Compare(T x, T y)
		{
			var result = BaseComparer.Compare(x, y);
			if (Descending)
				result = -result;
			return result;
		}
	}
}
