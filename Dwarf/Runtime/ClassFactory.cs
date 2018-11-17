using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;


namespace Dwares.Dwarf.Runtime
{
	public class ClassFactory
	{
		static ClassFactory instance;
		public static ClassFactory Instance => LazyInitializer.EnsureInitialized(ref instance);

		/*public*/ List<IClassLocator> Locators { get; } = new List<IClassLocator>();

		public void Add(IClassLocator locator) {
			Locators.Add(locator ?? throw new ArgumentNullException(nameof(locator)));
		}

		public object LocateFoRef(Type targetType, object reference, Func<Type, object> func)
		{
			foreach (var locator in Locators) {
				if (locator.TargetType != targetType)
					continue;

				var type = locator.LocateForRef(reference);
				if (type == null)
					continue;

				var obj = func(type);
				if (obj != null)
					return obj;
			}

			return null;
		}

		public object LocateForRefType(Type targetType, Type refType, Func<Type, object> func)
		{
			foreach (var locator in Locators) {
				if (locator.TargetType != targetType)
					continue;

				var type = locator.LocateForRefType(refType);
				if (type == null)
					continue;

				var obj = func(type);
				if (obj != null)
					return obj;
			}

			return null;
		}


		public object CreateForRef(Type targetType, object reference)
			=> LocateFoRef(targetType, reference, Construct);

		public object CreateForRefType(Type targetType, Type refType)
			=> LocateForRefType(targetType, refType, Construct);

		public static object Construct(Type type)
		{
			var ctor = Reflection.GetDefaultConstructor(type);
			if (ctor != null) {
				return ctor.Invoke(Reflection.cNoArgs);
			} else {
				return null;
			}
		}

		public static void AddLocator(IClassLocator locator) 
			=> Instance.Add(locator);

		public static TargetType Create<TargetType>(object typeOrReference)
			=> (TargetType)Create(typeof(TargetType), typeOrReference);

		public static object Create(Type targetType, object typeOrReference)
		{
			if (typeOrReference is Type refType) {
				return Instance.CreateForRefType(targetType, refType);
			} else {
				return Instance.CreateForRef(targetType, typeOrReference);
			}
		}
	}
}
