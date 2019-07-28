using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Satchel;


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
		static Style emptyStyle = new Style(typeof(VisualElement));

		Dictionary<string, Style> styles = new Dictionary<string, Style>();
		Dictionary<string, ImageSource> images = new Dictionary<string, ImageSource>();
		Metadata metadata = new Metadata();

		public UITheme(ResourceDictionary resources, UITheme baseTheme = null)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(resources, nameof(resources));

			//Resources = resources;
			BaseTheme = baseTheme;

			if (resources != null) {
				Load(resources);
			}

			if (!string.IsNullOrEmpty(ThemeName)) {
				namedThemes[ThemeName] = this;
			}

			//var name = ThemeName;
			//if (!string.IsNullOrEmpty(name)) {
			//	namedThemes[name] = this;
			//}

			//var colorScheme = GetValue<ColorScheme>("ColorScheme", false);
			////if (colorScheme == null) {
			////	colorScheme = new ColorSchema(resources);
			////}
			//ColorScheme = colorScheme;
		}

		public void Load(IDictionary<string, object> resources)
		{
			ResourcesLoader.Load(resources, this, metadata, LoadValue);
			//ColorScheme.BindingPalette = null;
		}

		static bool LoadValue(UITheme target, string key, object value)
		{
			if (value is Style style) {
				target.AddStyle(key, style);
				return true;
			}
			if (value is ImageSource image) {
				target.AddImage(key, image);
				return true;
			}
			//if (key == nameof(ColorScheme)) {
			//	ColorScheme.BindingPalette = target.ColorScheme = value as ColorScheme;
			//	return true;
			//}
			return false;
		}

		//ResourceDictionary Resources { get; }

		public ColorScheme ColorScheme { get; set; }

		//public string ThemeName {
		//	get => GetString(nameof(ThemeName), false);
		//}
		//public string BasedOn { 
		//	get => GetString(nameof(BasedOn), false);
		//}
		public string ThemeName { get; set; }
		public string BasedOn { get; set; }

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

		//public T GetValue<T>(string key, bool useBase, T defaultValue=default(T))
		//{
		//	if (!string.IsNullOrEmpty(key))
		//	{
		//		object value;
		//		if (Resources.TryGetValue(key, out value) && value is T val) {
		//			return val;
		//		} else if (useBase && BaseTheme != null) {
		//			return BaseTheme.GetValue<T>(key, true);
		//		}
		//	}
		//	return defaultValue;
		//}

		//public string GetString(string key, bool useBase)
		//	=> GetValue<string>(key, useBase);

		//public Color GetColor(string key, bool useBase = true)
		//	=> GetValue(key, useBase, Color.Transparent);

		public ImageSource GetImage(string key, bool useBase = true, bool useArtProvider = true)
		{
			//var image = GetValue<ImageSource>(key, useBase);
			//if (image == null && useArtProvider) {
			//	image = ArtProvider.Instance.GetImageSource(key);
			//}

			//return image;

			ImageSource image = null;
			if (images.ContainsKey(key)) { 
				image = images[key];
			}

			if (image == null && useArtProvider) {
				image = ImageProvider.Instance.GetImageSource(key);
			}

			return image;
		}

		public Style GetStyle(string key, bool useBase = true, bool notNull = false)
		{
			Style style = null;

			if (!string.IsNullOrEmpty(key))
			{
				styles.TryGetValue(key, out style);

				if (useBase) {
					var baseStyle = BaseTheme?.GetStyle(key);
					if (style != null) {
						style.MergeIn(baseStyle);
					} else {
						style = baseStyle;
					}
				}
			}

			if (style == null && notNull) {
				style = emptyStyle;
			}
			return style;
		}

		public void AddImage(string key, ImageSource image)
		{
			Guard.ArgumentNotEmpty(key, nameof(key));
			Guard.ArgumentNotNull(image, nameof(image));

			//Resources.Add(key, image);
			images.Add(key, image);
		}

		public void AddStyle(string key, Style style)
		{
			Guard.ArgumentNotEmpty(key, nameof(key));
			Guard.ArgumentNotNull(style, nameof(style));

			//Resources.Add(key, style);
			styles.Add(key, style);
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
			UITheme theme;
			if (!string.IsNullOrEmpty(name) && namedThemes.TryGetValue(name, out theme)) {
				return theme;
			} else {
				return null;
			}
		}

	}
}
