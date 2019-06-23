using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;


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

		static Dictionary<string, UITheme> namedThemes = new Dictionary<string, UITheme>();

		Dictionary<string, Style> styles = new Dictionary<string, Style>();
		Dictionary<string, ImageSource> images = new Dictionary<string, ImageSource>();

		public UITheme(UITheme baseTheme = null)
		{
			//Debug.EnableTracing(@class);

			BaseTheme = baseTheme;
			BasedOn = baseTheme?.Name;
		}

		public UITheme(ResourceDictionary dict, UITheme baseTheme = null) :
			this(baseTheme)
		{
			object value;
			if (dict.TryGetValue("ThemeName", out value)) {
				Name = value.ToString();
			}

			if (dict.TryGetValue("BasedOn", out value)) {
				BasedOn = value.ToString();
				BaseTheme = null;
			}


			foreach (var pair in dict) {
				if (pair.Value is ImageSource image) {
					AddImage(pair.Key, image);
				} else if (pair.Value is Style style) {
					AddStyle(pair.Key, style);
				}
			}
		}

		string name;
		public string Name {
			get => name;
			set {
				if (value != name) {
					if (!string.IsNullOrEmpty(name) && namedThemes.ContainsKey(name)) {
						namedThemes.Remove(name);
					}

					name = value;

					if (!string.IsNullOrEmpty(name)) {
						namedThemes[name] = this;
					}
				}
			}
		}

		public static UITheme ByName(string name)
		{
			UITheme theme;
			if (!string.IsNullOrEmpty(name) && namedThemes.TryGetValue(name, out theme)) {
				return theme;
			} else {
				return null;
			}
		}

		public string BasedOn { get; private set; }

		UITheme baseTheme;
		public UITheme BaseTheme {
			get {
				if (baseTheme == null && !string.IsNullOrEmpty(BasedOn)) {
					baseTheme = ByName(BasedOn);
				}
				return baseTheme;
			}
			private set {
				baseTheme = value;
			}
		}

		//public ColorScheme ColorScheme { get; set; }

		public void AddImage(string name, ImageSource image)
		{
			images.Add(name, image);
		}

		public ImageSource GetImage(string name)
		{
			if (images.ContainsKey(name)) {
				return images[name];
			}

			return ArtProvider.Instance.GetImageSource(name);
		}

		public void AddStyle(string flavor, Style style)
		{
			Guard.ArgumentNotEmpty(flavor, nameof(flavor));
			Guard.ArgumentNotNull(style, nameof(style));

			styles[flavor] = style;
		}

		public void AddStyle(string flavor, Type type, params object[] propertiesAndValues)
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

			AddStyle(flavor, style);
		}

		public Style GetStyle(string flavor)
		{
			if (string.IsNullOrEmpty(flavor))
				return null;

			var baseStyle = BaseTheme?.GetStyle(flavor);

			if (styles.ContainsKey(flavor)) {
				var style = styles[flavor];
				if (baseStyle == null)
					return style;

				baseStyle.MergeStyle(style);
			}
			return baseStyle;
		}

		public bool Apply(VisualElement element, string flavor)
		{
			if (element == null || string.IsNullOrEmpty(flavor))
				return false;

			var style = GetStyle(flavor);
			if (style != null) {
				element.Style = style;
				return true;
			} else {
				Debug.Print($"Style '{flavor}' not found in UITheme");
				return false;
			}
		}

	}
}
