using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid.Satchel
{
	public interface IColorCollection
	{
		bool TryGetColor(string name, string variant, out Color color, Color defaultValue);
		Color GetColor(string name, string variant, Color defaultValue);
	}

	public class ColorCollection : IColorCollection, INameHolder
	{
		//static ClassRef @class = new ClassRef(typeof(ColorCollection));
		
		Dictionary<string, Color> colors = new Dictionary<string, Color>();
		Metadata metadata = new Metadata();

		protected ColorCollection()
		{
			//Debug.EnableTracing(@class);
		}

		public virtual void Load(IDictionary<string, object> dict)
		{
			foreach (var pair in dict) {
				if (pair.Value is Color color) {
					Add(pair.Key, color);
				} else if (pair.Value is string value) {
					SetMeta(pair.Key, value);
				}
			}
		}

		public virtual string Name => null;
		public virtual string Design => null;

		protected virtual void Add(string key, Color color)
		{
			colors.Add(key, color);
		}

		public virtual bool TryGetColor(string name, string variant, out Color color, Color defaultValue = default)
		{
			if (!string.IsNullOrEmpty(name))
			{ 
				string key;
				if (string.IsNullOrEmpty(variant)) {
					key = name;
				} else {
					key = $"{name}:{variant}";
				}

				if (colors.TryGetValue(key, out color)) {
					return true;
				}
			}

			color = defaultValue;
			return false;
		}

		public Color GetColor(string name, string variant = null, Color defaultValue = default)
		{
			Color color;
			TryGetColor(name, variant, out color, defaultValue);
			return color;
		}

		protected virtual void SetMeta(string key, string value)
			=> metadata.Set(key, value, this);

		public virtual string GetMeta(string key)
			=> metadata.GetAsString(key);

		protected void OnNameChanged<TCollection>(string oldName, string newName, Dictionary<string, TCollection> named)
			where TCollection : ColorCollection
		{
			if (!string.IsNullOrEmpty(oldName)) {
				named.Remove(oldName);
			}

			if (!string.IsNullOrEmpty(newName) && this is TCollection collection) {
				named[newName] = collection;
			}
		}

		protected static TCollection ByName<TCollection>(string name, Dictionary<string, TCollection> named)
			where TCollection : ColorCollection
		{
			TCollection collection;
			if (!string.IsNullOrEmpty(name) && named.TryGetValue(name, out collection)) {
				return collection;
			} else {
				return null;
			}
		}
	}
}
