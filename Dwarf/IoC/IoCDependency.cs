#if IoC
using System;
using System.Threading;
using Dwares.Dwarf.Runtime;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.IoC
{
	public enum IoCDependencyInit
	{
		Exclusive,
		Immediate,
		Once,
		IfNull,
		Default = Once
	}

	public class IoCDependency<T> : IValueHolder<T> where T : class
	{
		public static T GetDependency(ref IoCDependency<T> dependency)
		{
			var dep = LazyInitializer.EnsureInitialized(ref dependency, () => new IoCDependency<T>());
			return dep.Value;
		}

		public static void SetDependency(ref IoCDependency<T> dependency, T value)
		{
			var dep = LazyInitializer.EnsureInitialized(ref dependency, () => new IoCDependency<T>(IoCDependencyInit.Exclusive));
			dep.Value = value;
		}

		public IoCDependency(IoCContainer container = null) :
			this(IoCDependencyInit.Default, true, container, null)
		{ }


		public IoCDependency(IoCDependencyInit initMode, bool required = true, IoCContainer container = null) :
			this(initMode, required, container, null)
		{}

		public IoCDependency(ParametersOverloads parameters, bool required = true, IoCContainer container = null) :
			this(IoCDependencyInit.Immediate, required, container, parameters)
		{}

		protected IoCDependency(IoCDependencyInit initMode, bool required, IoCContainer container, ParametersOverloads parameters)
		{
			this.container = container;
			InitMode = initMode;
			Required = required;

			if (initMode == IoCDependencyInit.Immediate) {
				value = Resolve(parameters);
			}
		}

		IoCContainer container = null;
		public IoCContainer Container => container ?? IoCContainer.Instance;

		public Type DependencyType => typeof(T);
		public IoCDependencyInit InitMode { get; }
		public bool Required { get; set; } = true;


		private bool inited = false;
		private T value = null;
		public T Value {
			get => Get();
			set => Set(value);
		}

		protected T Resolve(ParametersOverloads parameters)
		{
			try {
				var value = Container.Resolve(DependencyType, parameters);
				if (value == null && Required) {
					DependencyNotFound.Raise(DependencyType);
				}
				if (value is T resolved) {
					return resolved;
				} else {
					InvalidDependencyType.Raise(DependencyType, value.GetType());
				}
			}
			catch (Exception ex) {
				Debug.ExceptionCaught(ex);
				if (Required) {
					DependencyNotFound.Raise(typeof(T), ex);
				}
			}
			return null;
		}

		public T Get(ParametersOverloads parameters = null)
		{
			if (!inited) {
				switch (InitMode)
				{
				case IoCDependencyInit.Immediate:
				case IoCDependencyInit.Once:
					if (!inited) {
						value = Resolve(parameters);
					}
					break;

				case IoCDependencyInit.IfNull:
					if (value == null) {
						value = Resolve(parameters);
					}
					break;
				}
			}

			return value;
		}

		public void Set(T value)
		{
			this.value = value;
			inited = true;
		}
	}

}
#endif
