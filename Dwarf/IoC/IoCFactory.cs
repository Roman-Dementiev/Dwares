#if IoC
using System;
using System.Collections.Generic;
using System.Reflection;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.IoC
{
	public abstract class IoCFactory
	{
		public abstract Type CreatesType { get; }

		public abstract object GetObject(Type requestedType, IoCContainer container, ParametersOverloads parameters = null);
	}

	public class IoCConstructor : IoCFactory
	{
		Type implementationType;
		ConstructorInfo constructor;

		protected IoCConstructor() { }

		public IoCConstructor(Type implementationType, ConstructorInfo constructor = null)
		{
			Initialize(implementationType, constructor);
		}

		protected void Initialize(Type implementationType, ConstructorInfo constructor = null)
		{
			this.implementationType = implementationType ?? throw new ArgumentNullException(nameof(implementationType));
			this.constructor = constructor;
		}

		public override Type CreatesType => implementationType;

		public override object GetObject(Type requestedType, IoCContainer container, ParametersOverloads parameters = null)
		{
			return container.ConstructType(requestedType,  implementationType, constructor, parameters);
		}
	}

	public class IoCSingleton: IoCConstructor
	{
		object instance;

		public IoCSingleton(Type implementationType, ConstructorInfo constructor = null) :
			base(implementationType, constructor)
		{
			this.instance = null;
		}

		public IoCSingleton(object instance)
		{
			this.instance = instance ?? throw new ArgumentNullException(nameof(instance));
			Initialize(instance.GetType());
		}

		public override Type CreatesType => instance.GetType();

		public override object GetObject(Type requestedType, IoCContainer container, ParametersOverloads parameters = null)
		{
			if (instance == null) {
				instance = base.GetObject(requestedType, container, parameters);
			}
			return instance;
		}
	}
}
#endif
