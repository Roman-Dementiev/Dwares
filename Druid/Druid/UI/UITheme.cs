using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms;

namespace Dwares.Druid.UI
{
	public class UITheme
	{
		//static ClassRef @class = new ClassRef(typeof(UITheme));

		static UITheme current;
		public static UITheme Current { 
			get => current;
			set {
				if (value != current) {
					current = value;
					CurrentThemeChanged?.Invoke(typeof(UITheme), new EventArgs());
				}
			}
		}
		public static event EventHandler CurrentThemeChanged;

		public UITheme()
		{
			//Debug.EnableTracing(@class);
		}

		//public UITheme(ResourceDictionary resources)
		//{
		//}

		public void AddStyle(string name, Style style)
			=> AddStyle(name, null, style);

		public void AddStyle(Type type, Style style)
			=> AddStyle(null, type, style);

		public void AddStyle(string name, Type type, Style style)
		{
			Guard.ArgumentNotNull(style, nameof(style));

			if (type == null) {
				type = style.TargetType;
				Guard.ArgumentNotNull(type, nameof(type));
			}
			Guard.ArgumentIsValid(nameof(type), type.IsDerivedFrom(typeof(VisualElement)),
				$"UITheme.AddStyle: Invalid type={type}");


			var spec = new StyleSpec {
				Name = name,
				Style = style,
				BaseType = type
			};

			if (!string.IsNullOrEmpty(name)) {
				stylesByName.Add(name, spec);
			}
			
			if (!stylesByType.ContainsKey(type)) {
				stylesByType.Add(type, spec);
			}
		}

		public void AddStyle(string name, Type type, params object[] propertiesAndValues)
		{
			Guard.ArgumentNotNull(type, nameof(type));

			var style = new Style(type);
			for (int i = 1; i < propertiesAndValues.Length; i += 2) {
				var property = propertiesAndValues[i-1] as BindableProperty;
				if (property == null) {
					throw new ArgumentException("Invalid propertied in propertiesAndValues, must be BindableProperty");
				}

				style.Setters.Add(new Setter { Property = property, Value = propertiesAndValues[i]});			
			}

			AddStyle(name, type, style);
		}

		public Style GetStyleByName(string name)
		{
			if (stylesByName.ContainsKey(name)) {
				return stylesByName[name].Style;
			} else {
				return null;
			}
		}

		public Style GetStyleByType(Type type)
		{
			while (type != null) {
				if (stylesByType.ContainsKey(type)) {
					return stylesByType[type].Style;
				}

				if (type == typeof(VisualElement))
					break;
				type = type.BaseType;
			}

			return null;
		}

		public bool Apply(VisualElement element, string styleName)
		{
			Guard.ArgumentNotNull(element, nameof(element));

			Style style;
			if (string.IsNullOrEmpty(styleName)) {
				styleName = element.GetType().ToString(); // for debug message only
				style = GetStyleByType(element.GetType());
			} else {
				style = GetStyleByName(styleName);
			}

			if (style != null) {
				element.Style = style;
				return true;
			} else {
				Debug.Print("Style '{0}' not found in UITheme", styleName);
				return false;
			}
		}

		public bool Apply(VisualElement element, Type type)
		{
			Guard.ArgumentNotNull(element, nameof(element));

			var style = GetStyleByType(type ?? element.GetType());
			if (style != null) {
				element.Style = style;
				return true;
			} else {
				Debug.Print("Style for type {0} not found in UITheme", type);
				return false;
			}
		}

		public bool Apply(VisualElement element) => Apply(element, (Type)null);

		class StyleSpec
		{
			public Type BaseType { get; set; }
			public string Name { get; set; }
			public Style Style { get; set; }
		}

		Dictionary<string, StyleSpec> stylesByName = new Dictionary<string, StyleSpec>();
		Dictionary<Type, StyleSpec> stylesByType = new Dictionary<Type, StyleSpec>();
	}
}
