using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Satchel;
using Dwares.Druid.Painting;

namespace Dwares.Druid.UI
{
	public class UITheme
	{
		//static ClassRef @class = new ClassRef(typeof(UITheme));

		static Style emptyStyle = new Style(typeof(VisualElement));

		public UITheme(ResourceDictionary resources, UITheme baseTheme = null)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(resources, nameof(resources));

			Resources = resources;
			BaseTheme = baseTheme;
		}

		public ResourceDictionary Resources { get; }

		public string ThemeName {
			get => GetString(nameof(ThemeName));
		}
		public string BasedOn {
			get => GetString(nameof(BasedOn));
		}

		UITheme baseTheme;
		public UITheme BaseTheme {
			get {
				if (baseTheme == null) {
					baseTheme = ByName(BasedOn);
				}
				return baseTheme;
			}
			private set {
				baseTheme = value;
			}
		}

		public string GetString(string key, bool convert = false)
		{
			if (Resources.TryGetValue(key, out object value) && value != null) {
				if (value is string str)
					return str;
				if (convert)
					return value.ToString();
			}
			return null;
		}

		public ImageSource GetImageSource(string key, bool useBase = true)
		{
			if (string.IsNullOrEmpty(key))
				return null;

			if (Resources.TryGetValue(key, out object value)) {
				if (value is ImageSource imageSource)
					return imageSource;

				if (value is string art) {
					var artImageSource = ArtBroker.Instance.GetImageSource(art);
					if (artImageSource != null)
						return artImageSource;

					key = art;
				}
			}

			if (useBase) {
				var baseTheme = BaseTheme;
				return baseTheme?.GetImageSource(key, true);
			}

			return null;
		}

		Style TryGetStyle(string flavor)
		{
			object value;
			if (!string.IsNullOrEmpty(flavor) && Resources.TryGetValue(flavor, out value)) {
				return value as Style;
			} else {
				return null;
			}
		}

		public Style GetStyle(string flavor, bool useBase = true, bool notNull = false)
		{
			var style = TryGetStyle(flavor);

			if (useBase) {
				var baseStyle = BaseTheme?.GetStyle(flavor);
				if (style != null) {
					style.MergeIn(baseStyle);
				} else {
					style = baseStyle;
				}
			}

			if (style == null && notNull) {
				style = emptyStyle;
			}
			return style;
		}

		public void AddStyle(string key, Style style)
		{
			Guard.ArgumentNotEmpty(key, nameof(key));
			Guard.ArgumentNotNull(style, nameof(style));

			Resources.Add(key, style);
		}

		public void AddStyle(string key, Type type, params object[] propertiesAndValues)
		{
			Guard.ArgumentNotNull(type, nameof(type));

			var style = new Style(type);
			for (int i = 1; i < propertiesAndValues.Length; i += 2) {
				var property = propertiesAndValues[i - 1] as BindableProperty;
				if (property == null) {
					throw new ArgumentException("Invalid propertied in propertiesAndValues, must be BindableProperty");
				}

				style.Setters.Add(new Setter { Property = property, Value = propertiesAndValues[i] });
			}

			AddStyle(key, style);
		}

		public static UITheme ByName(string name)
		{
			return UIThemeManager.Instance.GetTheme(name);
		}

		public static UITheme Default {
			get {
				var manager = UIThemeManager.Instance;
				return manager.GetTheme(manager.DefaultTheme);
			}
		}

		public static UITheme Current {
			get => UIThemeManager.Instance.CurrentTheme;
			set => UIThemeManager.Instance.CurrentTheme = value;
		}

		public static void OnCurrentThemeChanged(Action action)
		{
			UIThemeManager.Instance.CurrentThemeChanged += (sender, args) => action?.Invoke();

		}
	}
}
