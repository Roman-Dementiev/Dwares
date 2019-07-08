using System;
using System.Collections.Generic;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.Collections
{
	public class NamedItemsCollection<T> : List<T>
	{
		public NamedItemsCollection() { }

		public NamedItemsCollection(IEnumerable<T> collection) : base(collection) { }

		public NamedItemsCollection(ICreateByName<T> factory)
		{
			Factory = factory;
		}

		public bool AllowEmptyNames { get; set; } = false;
		public bool UniqueNames { get; set; } = true;

		public ICreateByName<T> Factory { get; set; } = null;

		public new virtual bool Add(T item)
		{
			Guard.ArgumentNotNull(item, nameof(item));

			var name = item.GetName();
			Guard.Verify(AllowEmptyNames || !string.IsNullOrEmpty(name), 
				$"NamedItemsCollection.Add(): item {item} doesn't nave a name");

			if (UniqueNames && GetByName(name, out var found))
				return false;

			base.Add(item);
			return true;
		}

		public bool Add(string name)
		{
			if (name == null)
				throw new ArgumentNullException(nameof(name));

			if (UniqueNames && GetByName(name, out var found))
				return false;

			if (Factory != null) {
				var item = Factory.New(name);
				if (item != null) {
					return Add(item);
				}
			}
			return false;
		}

		public bool ContainsName(string name) 
			=> GetByName(name, out var found);

		public virtual T Get(string name) 
			=> GetByNameOrDefault(name);

		public T GetByNameOrDefault(string name, T defaultValue = default)
		{
			if (GetByName(name, out var found)) {
				return found;
			} else {
				return defaultValue;
			}
		}

		public bool GetByName(string name, out T found)
		{
			foreach (var item in this) {
				if (item.GetName() == name) {
					found = item;
					return true;
				}
			}
			found = default;
			return false;
		}
	}
}
