using System;
using System.Collections.Generic;
using System.Collections;
using System.Threading;
using Xamarin.Forms;
using Dwares.Dwarf;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Services;


namespace Dwares.Druid.Localization
{
	public delegate void LocalizationHandlerProc(object target, string uid);

	public class Localizer
	{
		public static ClassRef @class => new ClassRef(typeof(Localizer));
		public static Localizer Instance = LazyInitializer.EnsureInitialized(ref instance);
		static Localizer instance = null;

		public static readonly BindableProperty KeyProperty = BindableProperty.CreateAttached(
			"Key",
			typeof(string),
			typeof(Localizer),
			null
		);
		public static void SetKey(BindableObject obj, string key) => obj.SetValue(KeyProperty, key);
		public static string GetKey(BindableObject obj) => (string)obj.GetValue(KeyProperty);

		public static readonly BindableProperty LocalizableProperty = BindableProperty.CreateAttached(
			"Localizable",
			typeof(string),
			typeof(Localizer),
			null
		);
		public static void SetLocalizable(BindableObject obj, string propertyName) => obj.SetValue(LocalizableProperty, propertyName);
		public static string GetLocalizable(BindableObject obj) => (string)obj.GetValue(LocalizableProperty);

		class HandlerEntry
		{
			public Type BaseType { get; set; }
			public String Property { get; set; }
			public LocalizationHandlerProc Handler { get; set; }
		}

		Dictionary<Type, HandlerEntry> handlers = new Dictionary<Type, HandlerEntry>();
		WeakCollection targets;
		public string DefaultLocalizableProperty { get; set; } = nameof(Localizables);

		//public static Localizer Initialize(Element ui)
		//{
		//	if (instance != null)
		//		throw new Exception("Localizer already initialized");
		//	if (ui == null)
		//		throw new ArgumentNullException(nameof(ui));

		//	var localizer = new Localizer(true);
		//	localizer.targets.Add(ui);

		//	return localizer;
		//}

		public static void Localize(object target, string uid = null) => Instance.LocalizeObject(target, uid, null);

		public Localizer() : this(true) {}

		public Localizer(bool useUiHandlers)
		{
			handlers = new Dictionary<Type, HandlerEntry>();
			targets = new WeakCollection();

			if (useUiHandlers) {
				AddUiHandlers();
			}

			LocalizationService.AddListener(OnLanguageChanged);
		}

		void OnLanguageChanged(object sender, EventArgs args)
		{
			LocalizeTargets();
		}

		public void LocalizeTargets()
		{
			foreach (var target in targets) {
				Localize(target);
			}
		}

		public static void AddTarget(object target, bool localize = true)
		{
			Instance.targets.Add(target);
			if (localize) {
				Localize(target);
			}
		}

		public static bool RemoveTarget(object target)
		{
			return Instance.targets.Remove(target);
		}

		public void AddHandler(Type type, string property)
		{
			var entry = new HandlerEntry() { BaseType = type, Property = property };
			handlers.Add(type, entry);
		}

		public void AddHandler(Type type, LocalizationHandlerProc handler)
		{
			var entry = new HandlerEntry() { BaseType = type, Handler = handler };
			handlers.Add(type, entry);
		}

		//public void AddHandler(Type type, Type baseType)
		//{
		//	var entry = handlers[baseType];
		//	handlers[type] = entry;
		//}

		public void InvokeHandler(object target, string uid)
		{
			var entry = Reflection.EvalForType(target.GetType(), (type) => handlers.GetValue(type));
			if (entry != null) {
				entry.Handler?.Invoke(target, uid);
				if (entry.Property != null) {
					LocalizeProperty(target, entry.Property, uid);
				}
			}
		}

		void GetKeyAndLocalizable(object target, ref string key, ref string localizableProperty)
		{
			if (target is BindableObject bindable) {
				if (key == null) {
					key = GetKey(bindable);

					if (String.IsNullOrEmpty(key)) {
						key = Bind.GetUid(bindable);
					}
				}

				if (localizableProperty == null) {
					localizableProperty = GetLocalizable(bindable);
				}
			}	

			if (localizableProperty == null) {
				localizableProperty = DefaultLocalizableProperty;
			}
		}

		public void LocalizeObject(object target, string uid = null, string localizableProperty = null)
		{
			if (target == null)
				return;

			if (target is ILocalizable localizable) {
				localizable.Localize();
				return;
			}


			GetKeyAndLocalizable(target, ref uid, ref localizableProperty);

			InvokeHandler(target, uid);

			if (localizableProperty != null) {
				LocalizeProperty(target, localizableProperty, uid);
			}

			if (target is IEnumerable enumerable) {
				foreach (var obj in enumerable) {
					LocalizeObject(obj, null, null);
				}
			}
		}

		public void LocalizeProperty(object target, string property, string uid)
		{
			if (target == null)
				return;

			var propertyInfo = Reflection.GetProperty(target, property);
			if (propertyInfo == null)
				return;

			var propertyType = Reflection.GetPropertyType(target, propertyInfo);
			if (propertyType == typeof(string)) {
				if (propertyInfo.CanWrite) {
					var value = ResourceService.GetString(uid);
					Reflection.SetPropertyValue(target, propertyInfo, value);
				}
			} else if (propertyInfo.CanRead) {
				var propertyValue = Reflection.GetPropertyValue(target, propertyInfo);
				Localize(propertyValue, uid);
			}
		}

		bool uiHandlersAdded = false;
		void AddUiHandlers()
		{
			if (uiHandlersAdded)
				return;
			uiHandlersAdded = true;

			AddHandler(typeof(Page), nameof(Page.Title));
			AddHandler(typeof(Button), nameof(Button.Text));
		}
	}
}
