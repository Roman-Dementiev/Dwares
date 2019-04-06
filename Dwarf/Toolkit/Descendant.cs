using System;


namespace Dwares.Dwarf.Toolkit
{
	public interface IDescendant
	{
		object Parent { get; }
	}

	public static class Descendant
	{
		public static object GetParent(object obj)
		{
			if (obj is IDescendant descendant) {
				return descendant.Parent;
			} else {
				return null;
			}
		}
	}
}
