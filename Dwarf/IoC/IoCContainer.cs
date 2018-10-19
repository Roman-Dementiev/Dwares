#if IoC
using System;
using System.Reflection;
using System.Threading;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.IoC
{
	struct TypeRegistration
	{
		public Type Type { get; }
		public string Name { get; }
		public int Hash { get; }

		public TypeRegistration(Type type, string name = null)
		{
			Type = type ?? throw new ArgumentNullException(nameof(type));

			if (String.IsNullOrEmpty(name)) {
				Name = String.Empty;
				Hash = Type.FullName.GetHashCode();
			} else {
				Name = name;
				Hash = String.Concat(Type.FullName, "|", name).GetHashCode();
			}
		}

		public override int GetHashCode() => Hash;

		public override bool Equals(object obj)
		{
			if (obj is TypeRegistration namedType) {
				if (namedType.Type == Type) {
					return String.Compare(Name, namedType.Name, StringComparison.Ordinal) == 0;
				}
			}
			return false;
		}
	}

	public class IoCContainer
	{
		static IoCContainer instance;
		public static IoCContainer Instance => LazyInitializer.EnsureInitialized(ref instance);

		SafeDictionary<TypeRegistration, IoCFactory> registry;

		public IoCContainer() : this(null, null) { }

		public IoCContainer(IoCOptions options) : this(null, options) { }

		public IoCContainer(IoCContainer parent, IoCOptions options = null)
		{
			Parent = parent;
			Options = options ?? IoCOptions.Default;
		}

		public IoCContainer Parent { get; }
		public IoCOptions Options { get;  }

		void RegisterInternal(string regisrationName, Type registerType, Type implementationType, IoCFactory factory)
		{
			if (registerType == null || implementationType == null || implementationType.IsAbstract() || implementationType.IsInterface())
				IoCRegistrationException.Raise(registerType, implementationType);

			if (!IsValidAssignment(registerType, implementationType))
				IoCRegistrationException.Raise(registerType, implementationType);

			var typeRegistration = new TypeRegistration(registerType, regisrationName);
			registry[typeRegistration] = factory;
		}

		public void RegisterFactory(Type registerType, Type implementationType, string regisrationName = null)
		{
			RegisterInternal(regisrationName, registerType, implementationType, new IoCConstructor(implementationType));
		}

		public void RegisterSingleton(Type registerType, object singleton, string regisrationName = null)
		{
			if (singleton == null)
				throw new ArgumentNullException(nameof(singleton));

			RegisterInternal(regisrationName, registerType, singleton.GetType(), new IoCSingleton(singleton));
		}

		public void RegisterSingleton(Type registerType, Type implementationType, string regisrationName = null)
		{
			RegisterInternal(regisrationName, registerType, implementationType, new IoCSingleton(implementationType));
		}

		private static bool IsValidAssignment(Type registerType, Type implementationType)
		{
			//if (!registerType.IsGenericTypeDefinition()) {
			//	return registerType.IsAssignableFrom(implementationType);
			//} else {

			//	if (registerType.IsInterface()) {
			//		return implementationType.ImplementsInterface(registerType.Name);
			//	} else if (registerType.IsAbstract() && implementationType.BaseType() != registerType) {
			//		return false;
			//	}

			//	/// TODO: ???? 
			//}
			//return true;

			return registerType.IsAssignableFrom(implementationType);
		}

		public object Resolve(Type resolveType, string registrationName, ParametersOverloads parameters = null, IoCOptions options = null)
		{
			return ResolveInternal(new TypeRegistration(resolveType, registrationName), parameters, options);
		}

		public object Resolve(Type resolveType, ParametersOverloads parameters, IoCOptions options = null)
		{
			return ResolveInternal(new TypeRegistration(resolveType), parameters, options);
		}

		private object ResolveInternal(TypeRegistration registration, ParametersOverloads parameters, IoCOptions options)
		{
			if (options == null)
				options = Options;

			object resolved;
			// Attempt container resolution
			if (TryResolve(out resolved, registration, parameters, options))
				return resolved;

			// Attempt container resolution
			if (!string.IsNullOrEmpty(registration.Name)) {
				// Fail if requesting named resolution and settings set to fail if unresolved
				if (options.NamedResolutionFailureAction == NamedResolutionFailureAction.Fail)
					IoCResolutionException.Raise(registration.Type);

				// Attemped unnamed fallback container resolution if relevant and requested
				if (options.NamedResolutionFailureAction == NamedResolutionFailureAction.AttemptUnnamedResolution) {
					if (TryResolve(out resolved, new TypeRegistration(registration.Type), parameters, options))
						return resolved;
	
				}
			}

#if EXPRESSIONS
            // Attempt to construct an automatic lazy factory if possible
            if (IsAutomaticLazyFactoryRequest(registration.Type))
                return GetLazyAutomaticFactoryRequest(registration.Type);
#endif
#if ENUMERABLE_REQUESTS
			if (IsIEnumerableRequest(registration.Type))
				return GetIEnumerableRequest(registration.Type);
#endif
			// Attempt unregistered construction if possible and requested
			if ((options.UnregisteredResolutionAction == UnregisteredResolutionAction.AttemptResolve) 
				/*|| (registration.Type.IsGenericType() && options.UnregisteredResolutionAction == UnregisteredResolutionAction.GenericsOnly*/) {
				if (!registration.Type.IsAbstract() && !registration.Type.IsInterface())
					return ConstructType(registration.Type, registration.Type, null, parameters);
			}

			// Unable to resolve - throw
			return IoCResolutionException.Raise(registration.Type);
		}

		private bool TryResolve(out object resolved, TypeRegistration registration, ParametersOverloads parameters, IoCOptions options)
		{
			for (var container = this; container != null; container = container.Parent) {
				IoCFactory factory;
				if (registry.TryGetValue(registration, out factory)) {
					try {
						resolved = factory.GetObject(registration.Type, this, parameters);
						return true;
					}
					catch (IoCResolutionException) {
						throw;
					}
					catch (Exception ex) {
						IoCResolutionException.Raise(registration.Type, ex);
					}
				}

#if RESOLVE_OPEN_GENERICS
				// Attempt container resolution of open generic
				if (registration.Type.IsGenericType()) {
					var openTypeRegistration = new TypeRegistration(registration.Type.GetGenericTypeDefinition(),
																	registration.Name);

					if (_RegisteredTypes.TryGetValue(openTypeRegistration, out factory)) {
						try {
							return factory.GetObject(registration.Type, this, parameters, options);
						}
						catch (TinyIoCResolutionException) {
							throw;
						}
						catch (Exception ex) {
							IoCResolutionException.Raise(registration.Type, ex);
						}
					}
				}
#endif
			}

			resolved = null;
			return false;
		}

		public object ConstructType(Type requestedType, Type implementationType, ConstructorInfo constructor, ParametersOverloads parameters)
		{
			var typeToConstruct = implementationType;

#if RESOLVE_OPEN_GENERICS
            if (implementationType.IsGenericTypeDefinition())
            {
                if (requestedType == null || !requestedType.IsGenericType() || !requestedType.GetGenericArguments().Any())
                    throw new TinyIoCResolutionException(typeToConstruct);

                typeToConstruct = typeToConstruct.MakeGenericType(requestedType.GetGenericArguments());
            }
#endif
			return Constructor.New(typeToConstruct, constructor, parameters);
		}
	}

}
#endif
