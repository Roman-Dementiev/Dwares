using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Druid.Services
{
	public interface ILocalizationService
	{
		event EventHandler LanguageChanged;

		string DefaultLanguage { get; }
		string CurrentLanguage { get; }
		void RequestLanguage(string language, Action callback = null);
	}


	public static class LocalizationService
	{
		public static ILocalizationService Instance {
			get => DependencyService.GetInstance<ILocalizationService>(ref instance);
			set => DependencyService.SetInstance(ref instance, value);
		}
		static ILocalizationService instance;

		public static string DefaultLanguage {
			get => Instance?.DefaultLanguage;
		}

		public static string CurrentLanguage {
			get => Instance?.CurrentLanguage;
		}

		public static void RequestLanguage(string language, Action callback = null)
		{
			Instance?.RequestLanguage(language, callback);
		}

		public static void AddListener(EventHandler listener)
		{
			var instance = Instance;
			if (instance != null) {
				instance.LanguageChanged += listener;
			}
		}

		public static void RemoveListener(EventHandler listener)
		{
			var instance = Instance;
			if (instance != null) {
				instance.LanguageChanged -= listener;
			}
		}
	}
}
