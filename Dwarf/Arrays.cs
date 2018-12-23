using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf
{
	public static class Arrays
	{
		//static ClassRef @class = new ClassRef(typeof(Arrays));


		public static T[] Slice<T>(this T[] array, int start, int end)
		{
			if (array == null)
				throw new ArgumentNullException(nameof(array));
			if (start < 0 || start >= array.Length)
				throw new ArgumentOutOfRangeException(nameof(start));
			if (end < start || end > array.Length)
				throw new ArgumentOutOfRangeException(nameof(end));

			int len = end - start;
			T[] slice = new T[len];
			for (int i = 0; i < len; i++) {
				slice[i] = array[i + start];
			}
			return slice;
		}
	}
}
