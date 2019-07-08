using System;
using System.Collections.Generic;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	public class ColorCollection : List<NamedColor> //, IDictionary<string, Color>
	{
		//static ClassRef @class = new ClassRef(typeof(ColorCollection));

		public ColorCollection(string name, string design)
		{
			//Debug.EnableTracing(@class);
			Name = name;
			Design = design;
		}

		public string Name { get; }
		public string Design { get; }


		public virtual void Load(IDictionary<string, object> dict, Metadata metadata, object target)
		{
			foreach (var pair in dict) {
				if (pair.Value is NamedColor namedColor) {
					Add(namedColor);
				} else if (pair.Value is Color color) {
					Add(new NamedColor { Name = pair.Key, Value = color });
				} else if (metadata != null) {
					metadata.Set(pair.Key, pair.Value, target);
				}
			}
		}


		public virtual bool TryGetColor(string name, string variant, out Color color)
		{
			if (!string.IsNullOrEmpty(name))
			{ 
				string key;
				if (string.IsNullOrEmpty(variant)) {
					key = name;
				} else {
					key = $"{name}:{variant}";
				}

				if (TryGetValue(key, out color)) {
					return true;
				}
			}

			color = default;
			return false;
		}

		bool TryGetValue(string key, out Color color)
		{
			foreach (var item in this) {
				if (item.Name == key) {
					color = item.Value;
					return true;
				}
			}

			color = default;
			return false;
		}

		//public Color GetColor(string name, string variant = null, Color defaultValue = default)
		//{
		//	Color color;
		//	TryGetColor(name, variant, out color, defaultValue);
		//	return color;
		//}

		//public void Add(string key, Color value) => throw new NotImplementedException();
		//public bool ContainsKey(string key) => throw new NotImplementedException();
		//public bool Remove(string key) => throw new NotImplementedException();
		//public bool TryGetValue(string key, out Color value) => throw new NotImplementedException();
		//public void Add(KeyValuePair<string, Color> item) => throw new NotImplementedException();
		//public bool Contains(KeyValuePair<string, Color> item) => throw new NotImplementedException();
		//public void CopyTo(KeyValuePair<string, Color>[] array, int arrayIndex) => throw new NotImplementedException();
		//public bool Remove(KeyValuePair<string, Color> item) => throw new NotImplementedException();
		//IEnumerator<KeyValuePair<string, Color>> IEnumerable<KeyValuePair<string, Color>>.GetEnumerator() => throw new NotImplementedException();
	}
}
