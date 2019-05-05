using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AssetWerks.Model
{
	public class IconGroup : TitleHolder, IList<Icon>
	{
		public IconGroup(string tutle) : base(tutle) { }

		public List<Icon> Icons { get; } = new List<Icon>();

		public Icon this[int index] {
			get => Icons[index];
			set => Icons[index] = value;
		}
		public int Count => Icons.Count;
		public bool IsReadOnly => false;
		public void Clear() => Icons.Clear();
		public void Add(Icon icon) => Icons.Add(icon);
		public bool Contains(Icon icon) => Icons.Contains(icon);
		public int IndexOf(Icon icon) => Icons.IndexOf(icon);
		public void Insert(int index, Icon icon) => Icons.Insert(index, icon);
		public bool Remove(Icon icon) => Icons.Remove(icon);
		public void RemoveAt(int index) => Icons.RemoveAt(index);
		IEnumerator IEnumerable.GetEnumerator() => Icons.GetEnumerator();
		public IEnumerator<Icon> GetEnumerator() => Icons.GetEnumerator();
		public void CopyTo(Icon[] array, int arrayIndex) => Icons.CopyTo(array, arrayIndex);
	}
}
