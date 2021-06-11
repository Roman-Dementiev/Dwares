//using System;
//using Windows.UI.Xaml;
//using Windows.Globalization;
//using Xamarin.Forms;
//using Dwares.Dwarf;
//using Dwares.Druid.Localization;


//[assembly: Dependency(typeof(Dwares.Druid.UWP.LocalizationSvc))]
//namespace Dwares.Druid.UWP
//{
//	public class LocalizationSvc : ResourceMapListener, ILocalizationSvc
//	{
//		public static ClassRef @class => new ClassRef(typeof(LocalizationSvc));

//		public event EventHandler LanguageChanged;
//		private Action requestCallback;

//		public LocalizationSvc()
//		{
//			Init("Language", OnResourceMapLanguageChanged, Window.Current.Content.Dispatcher);
//			InitCurrentLanguage();
//			Register();

//			//Debug.EnableTracing(@class);
//			//Debug.EnableTracing(typeof(LocalizationSvc));
//		}

//		void InitCurrentLanguage()
//		{
//			var initialtLanguage = ApplicationLanguages.Languages[0];
//			CurrentLanguage = SelectLanguage(initialtLanguage);
//			if (CurrentLanguage != initialtLanguage) {
//				ApplicationLanguages.PrimaryLanguageOverride = CurrentLanguage;
//			}
//		}

//		static string SelectLanguage(string languageTag)
//		{
//			if (Language.IsWellFormed(languageTag)) {
//				foreach (var lang in ApplicationLanguages.ManifestLanguages) {
//					if (languageTag == lang)
//						return languageTag;
//				}

//				int dash = languageTag.IndexOf('-');
//				if (dash >- 0) {
//					languageTag = languageTag.Substring(0, dash);
//					foreach (var lang in ApplicationLanguages.ManifestLanguages) {
//						if (languageTag == lang)
//							return languageTag;
//					}
//				}
//			}

//			return ApplicationLanguages.ManifestLanguages[0];
//		}

//		public string DefaultLanguage => ApplicationLanguages.ManifestLanguages[0];
//		public string CurrentLanguage { get; private set; }

//		public void RequestLanguage(string language, Action callback = null)
//		{
//			if (String.IsNullOrEmpty(language)) {
//				language = DefaultLanguage;
//			}
//			if (language != CurrentLanguage) {
//				//Debug.Trace(@class, nameof(RequestLanguage), "{0}", language);
//				requestCallback = callback;
//				ApplicationLanguages.PrimaryLanguageOverride = language;
//			}
//		}


//		void OnResourceMapLanguageChanged(string resourceKey, string value)
//		{
//			//Localizer.LocalizeTargets();

//			if (CurrentLanguage != ApplicationLanguages.Languages[0]) {
//				CurrentLanguage = ApplicationLanguages.Languages[0];

//				requestCallback?.Invoke();

//				if (LanguageChanged != null) {
//					LanguageChanged.Invoke(this, EventArgs.Empty);
//				}
//			}
//		}

//	}
//}
