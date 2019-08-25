using System;
using System.Collections.Generic;
using System.Threading;
using Dwares.Druid.Xaml;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.UI
{
	[ContentProperty(nameof(Themes))]
	public class UIThemeManager
	{
		//static ClassRef @class = new ClassRef(typeof(UIThemeManager));
		public event EventHandler CurrentThemeChanged;

		static UIThemeManager instance;
		public static UIThemeManager Instance {
			set => instance = value;
			get => LazyInitializer.EnsureInitialized(ref instance);
		}

		public UIThemeManager() : this(new ResourceDictionary()) { }
		public UIThemeManager(ResourceDictionary themes)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotNull(themes, nameof(themes));

			Themes = themes;
		}

		public ResourceDictionary Themes { get; }

		string defaultTheme = null;
		public string DefaultTheme {
			set {
				defaultTheme = value;
			}
			get {
				if (defaultTheme == null) {
					Themes.TryGetValue(nameof(DefaultTheme), out var name);
					defaultTheme = name?.ToString() ?? string.Empty;
				}
				return defaultTheme;
			}
		}

		UITheme currentTheme;
		public UITheme CurrentTheme {
			get {
				if (currentTheme == null) {
					currentTheme = GetTheme(DefaultTheme);
					CurrentThemeChanged?.Invoke(this, new EventArgs());
				}
				return currentTheme;
			}

			set {
				if (value != currentTheme) {
					currentTheme = value;
					CurrentThemeChanged?.Invoke(this, new EventArgs());
				}
			}
		}

		public bool SelectTheme(string themeName)
		{
			var theme = GetTheme(themeName);
			if (theme != null) {
				CurrentTheme = theme;
				return true;
			} else {
				return false;
			}

		}

		public UITheme GetTheme(string themeName)
		{
			if (string.IsNullOrEmpty(themeName))
				return null;

			if (Themes.TryGetValue(themeName, out var value) && value != null)
			{
				if (value is IAsset asset)
					value = asset.AssetValue;

				if (value is UITheme theme)
					return theme;
			
				Debug.Print($"UIThemeManager.GetTheme(): '{themeName}' is not UITheme");
			} else {
				Debug.Print($"UIThemeManager.GetTheme(): '{themeName}' not found");
			}

			return null;
		}

		public bool TryGetTheme(string themeName, out UITheme theme)
		{
			if (!string.IsNullOrEmpty(themeName) && Themes.TryGetValue(themeName, out var value)) {
				if (value is IAsset asset)
					value = asset.AssetValue;

				if (value is UITheme _theme) {
					theme = _theme;
					return true;
				}
			}

			theme = null;
			return false;
		}

		public List<string> GetThemeNames(out int indexOfCurrent, bool includeInternal = false)
		{
			var list = new List<string>();
			indexOfCurrent = -1;

			foreach (var key in Themes.Keys) {
				UITheme theme;
				if (TryGetTheme(key, out theme)) {
					if (theme.Internal && !includeInternal)
						continue;

					if (theme == CurrentTheme) {
						indexOfCurrent = list.Count;
					}
					list.Add(key);
				}
			}

			return list;
		}
	}
}
