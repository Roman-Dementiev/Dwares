using System;
using System.Collections.Generic;
using System.Threading;
using Xamarin.Forms;
using Dwares.Dwarf;

namespace Dwares.Druid.UI
{
	public class UIScheme
	{
		//static ClassRef @class = new ClassRef(typeof(ColorScheme));

		public UIScheme() : this(null) { }

		public UIScheme(UIScheme baseScheme)
		{
			//Debug.EnableTracing(@class);

			BaseScheme = baseScheme;
			Values = new Dictionary<string, object>();
		}

		public UIScheme BaseScheme { get; set; }
		public Dictionary<string, object> Values { get; }

		public bool HasValue(string key) => Values.ContainsKey(key);
		public void SetValue(string key, object value) => Values[key] = value;
		public void SetValue<T>(string key, T value) => Values[key] = value;

		public object GetValue(string key)
		{
			if (Values.ContainsKey(key)) {
				return Values[key];
			} else {
				return BaseScheme?.GetValue(key);
			}
		}

		public T GetValue<T>(string key, T defaultValue = default(T))
		{
			TryGetValue(key, out T value, defaultValue);
			return value;
		}

		public bool TryGetValue<T>(string key, out T value, T defaultValue=default(T))
		{
			if (Values.TryGetValue(key, out var obj)) {
				if (obj is T val) {
					value = val;
					return true;
				}
			}
			else if (BaseScheme != null) {
				if (BaseScheme.TryGetValue(key, out value))
					return true;
			}
			
			value = defaultValue;
			return false;
		}

		public bool HasColor(string key)
		{
			return TryGetValue(key, out Color color);
		}

		public void SetColor(string key, Color color)
		{
			Debug.Assert(!Values.ContainsKey(key) || Values[key] is Color);
			SetValue(key, color);
		}
		
		public Color GetColor(string key, Color defaultColor=default(Color))
		{
			return GetValue(key, defaultColor);
		}

		static UIScheme defaultScheme;
		static UIScheme Default => LazyInitializer.EnsureInitialized(ref defaultScheme, () => {
			var scheme = new UIScheme();
			scheme.SetColor(TabViewEx.kHeaderBackgroundColor, Xamarin.Forms.Color.Transparent);
			scheme.SetColor(TabViewEx.kHeaderTabTextColor, Xamarin.Forms.Color.Black);
			scheme.SetColor(TabViewEx.kHeaderSelectionUnderlineColor, Xamarin.Forms.Color.Black);
			scheme.SetValue<double>(TabViewEx.kHeaderSelectionUnderlineThickness, 1.0);
			scheme.SetValue<double>(TabViewEx.kHeaderSpacing, 8.0);
			return scheme;
		});

		static UIScheme currentScheme;
		static UIScheme Current {
			get => LazyInitializer.EnsureInitialized(ref currentScheme, () => Default);
			set => currentScheme = value;
		}

		public static string ColorKey(string @namespace, string className, string  colorName)
		{
			return Strings.JoinNonEmpty(".", @namespace, className, colorName);
		}

		internal static string DruidColorKey(string className, string colorName) => ColorKey("Druid", className, colorName);

		public static T Value<T>(string key, T defaultValue=default(T)) => Current.GetValue<T>(key, defaultValue);
		public static Color Color(string key, Color defaultColor=default(Color)) => Current.GetColor(key, defaultColor);
	}
}
