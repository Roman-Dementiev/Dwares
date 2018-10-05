using System;
using Dwares.Druid.Services;


namespace Dwares.Druid.Localization
{
	public interface ILocalizationSvc
	{
		event EventHandler LanguageChanged;

		string DefaultLanguage { get; }
		string CurrentLanguage { get; }
		void RequestLanguage(string language, Action callback = null);
	}

	public static class LocalizationSvc
	{
		static DependencyService<ILocalizationSvc> instance = new DependencyService<ILocalizationSvc>(true);
		public static ILocalizationSvc Instance = instance.Service;

		public static string DefaultLanguage => Instance.DefaultLanguage;
		public static string CurrentLanguage => Instance.CurrentLanguage;

		public static void RequestLanguage(string language, Action callback = null)
		{
			Instance.RequestLanguage(language, callback);
		}

		public static void AddListener(EventHandler listener)
		{
			Instance.LanguageChanged += listener;
		}

		public static void RemoveListener(EventHandler listener)
		{
			Instance.LanguageChanged -= listener;
		}
	}

	//public class LanguageChangedEventArgs: EventArgs
	//{
	//	public string NewLanguage { get; set; }
	//}

	//public delegate void LanguageChangedHandler(object sender, LanguageChangedEventArgs args);
	//public delegate void LanguageChangeListener(string newLanguage);
}
