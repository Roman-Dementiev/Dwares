//using System;
//using System.Collections.Generic;
//using Dwares.Dwarf;
//using Dwares.Dwarf.Toolkit;
//using Dwares.Dwarf.Runtime;
//using Xamarin.Forms;


//namespace Dwares.Druid.Satchel
//{
//	public class ColorCollection : List<NamedColor> //, IDictionary<string, Color>
//	{
//		//static ClassRef @class = new ClassRef(typeof(ColorCollection));

//		public ColorCollection()
//		{
//			//Debug.EnableTracing(@class);
//		}

//		//public ColorCollection(string name, string design)
//		//{
//		//	//Debug.EnableTracing(@class);
//		//	Name = name;
//		//	Design = design;
//		//}

//		//public string Name { get; }
//		//public string Design { get; }


//		//public virtual void Load<Target>(IDictionary<string, object> dict, Target target, Func<Target, string, object, bool> loadProperty = null)
//		//{
//		//	foreach (var pair in dict) {
//		//		if (pair.Value is NamedColor namedColor) {
//		//			Add(namedColor);
//		//		} else if (pair.Value is Color color) {
//		//			Add(new NamedColor { Name = pair.Key, Value = color });
//		//		} else {
//		//			if (target != null) {
//		//				if (loadProperty == null)
//		//					loadProperty = Reflection.TrySetPropertyValue<Target>;

//		//				loadProperty(target, pair.Key, pair.Value);
//		//			}
//		//		}
//		//	}
//		//}	

//		public static bool LoadColor(ColorCollection target, string key, object value)
//		{
//			if (value is NamedColor namedColor) {
//				target.Add(namedColor);
//				return true;
//			}
//			if (value is Color color) {
//				target.Add(new NamedColor { Name = key, Value = color });
//				return true;
//			}
//			return false;
//		}


//		public virtual bool TryGetColor(string name, string variant, out Color color)
//		{
//			if (!string.IsNullOrEmpty(name))
//			{ 
//				var colorName = new ColorName { Name = name, Variant = variant };
//				if (TryGetValue(colorName, out color)) {
//					return true;
//				}
//			}

//			color = default;
//			return false;
//		}

//		bool TryGetValue(string key, out Color color)
//		{
//			foreach (var item in this) {
//				if (item.Name == key) {
//					color = item.Value;
//					return true;
//				}
//			}

//			color = default;
//			return false;
//		}

//		//public Color GetColor(string name, string variant = null, Color defaultValue = default)
//		//{
//		//	Color color;
//		//	TryGetColor(name, variant, out color, defaultValue);
//		//	return color;
//		//}

//		//public void Add(string key, Color value) => throw new NotImplementedException();
//		//public bool ContainsKey(string key) => throw new NotImplementedException();
//		//public bool Remove(string key) => throw new NotImplementedException();
//		//public bool TryGetValue(string key, out Color value) => throw new NotImplementedException();
//		//public void Add(KeyValuePair<string, Color> item) => throw new NotImplementedException();
//		//public bool Contains(KeyValuePair<string, Color> item) => throw new NotImplementedException();
//		//public void CopyTo(KeyValuePair<string, Color>[] array, int arrayIndex) => throw new NotImplementedException();
//		//public bool Remove(KeyValuePair<string, Color> item) => throw new NotImplementedException();
//		//IEnumerator<KeyValuePair<string, Color>> IEnumerable<KeyValuePair<string, Color>>.GetEnumerator() => throw new NotImplementedException();
//	}
//}
