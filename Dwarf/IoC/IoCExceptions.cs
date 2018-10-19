#if IoC
using System;
using System.Collections.Generic;
using System.Text;

namespace Dwares.Dwarf.IoC
{
	public class IoCException : Exception
	{
		public IoCException()
		{
			RegisterType = null;
		}

		public IoCException(Type registerType, string message) :
			base(message)
		{
			RegisterType = registerType;
		}

		public IoCException(Type registerType, string message, Exception innerException) :
			base(message, innerException)
		{
			RegisterType = registerType;
		}

		public Type RegisterType { get; protected set; }
	}


	public class IoCRegistrationException : IoCException
	{
		public IoCRegistrationException(Type registerType, string message)
			: base(registerType, message)
		{
		}
		
		public IoCRegistrationException(Type registerType, string message, Exception innerException)
			: base(registerType, message, innerException)
		{
		}

		public static Never Raise(Type registerType, string method, string messageFormat = null, Exception innerException = null)
		{
			var message = String.Format(messageFormat ?? CONVERT_ERROR_FORMAT, registerType?.FullName, method);

			IoCRegistrationException ex;
			if (innerException != null) {
				ex = new IoCRegistrationException(registerType, message, innerException);
			} else {
				ex = new IoCRegistrationException(registerType, message);
			}
			throw ex;
		}

		public static Never Raise(Type registerType, Type implementationType, string messageFormat = null, Exception innerException = null)
		{
			var message = String.Format(messageFormat ?? INVALID_REGISTRATION_FORMAt, registerType?.FullName, implementationType?.FullName);

			IoCRegistrationException ex;
			if (innerException != null) {
				ex = new IoCRegistrationException(registerType, message, innerException);
			} else {
				ex = new IoCRegistrationException(registerType, message);
			}
			throw ex;
		}

		public const string CONVERT_ERROR_FORMAT = "Cannot convert current registration of {0} to {1}";
		public const string INVALID_REGISTRATION_FORMAt = "Type {1} is not valid for a registration of type {0}";
	}

	public class IoCResolutionException : IoCException
	{
		public IoCResolutionException(Type unresolvedType, string message)
			: base(unresolvedType, message)
		{}

		public IoCResolutionException(Type unresolvedType, string message, Exception innerException)
			: base(unresolvedType, message, innerException)
		{}

		public Type UnresolvedType => RegisterType;

		public static Never Raise(Type unresolvedType, Exception innerException = null)
			=> Raise(unresolvedType, null, innerException);

		public static Never Raise(Type unresolvedType, string messageFormat, Exception innerException = null)
		{
			var message = String.Format(messageFormat ?? DEFAULT_FORMAT, unresolvedType?.FullName);

			IoCResolutionException ex;
			if (innerException != null) {
				ex = new IoCResolutionException(unresolvedType, message, innerException);
			} else {
				ex = new IoCResolutionException(unresolvedType, message);
			}
			throw ex;
		}

		public const string DEFAULT_FORMAT = "Unable to resolve type: {0}";
	}


	public class DependencyNotFound : IoCException
	{
		//public DependencyNotFound() { }

		public DependencyNotFound(Type dependencyType, string message) :
			base(dependencyType, message)
		{ }

		public DependencyNotFound(Type dependencyType, string message, Exception innerException) :
			base(dependencyType, message, innerException)
		{ }

		public Type DependencyType => RegisterType;

		public static Never Raise(Type dependencyType, Exception innerException = null)
			=> Raise(dependencyType, null, innerException);

		public static Never Raise(Type dependencyType, string messageFormat, Exception innerException = null)
		{
			var message = String.Format(messageFormat ?? DEFAULT_FORMAT, dependencyType?.FullName);

			DependencyNotFound ex;
			if (innerException != null) {
				ex = new DependencyNotFound(dependencyType, message, innerException);
			} else {
				ex = new DependencyNotFound(dependencyType, message);
			}
			throw ex;
		}

		public const string DEFAULT_FORMAT = "Dependency not found: dependencyType={0}";
	}


	public class InvalidDependencyType : IoCException
	{
		//public InvalidDependencyType() { }

		public InvalidDependencyType(Type dependencyType, Type actualType, string message) :
			base(dependencyType, message)
		{
			ActualType = actualType;
		}

		public InvalidDependencyType(Type dependencyType, Type actualType, string message, Exception innerException) :
			base(dependencyType, message, innerException)
		{
			ActualType = actualType;
		}

		public Type DependencyType => RegisterType;
		public Type ActualType { get; }

		public static Never Raise(Type dependencyType, Type actualType, Exception innerException = null)
			=> Raise(dependencyType, actualType, null, innerException);

		public static Never Raise(Type dependencyType, Type actualType, string messageFormat, Exception innerException = null)
		{
			var message = String.Format(messageFormat ?? DEFAULT_FORMAT, dependencyType?.FullName, actualType?.FullName);

			InvalidDependencyType ex;
			if (innerException != null) {
				ex = new InvalidDependencyType(dependencyType, actualType, message, innerException);
			} else {
				ex = new InvalidDependencyType(dependencyType, actualType, message);
			}
			throw ex;
		}

		public const string DEFAULT_FORMAT = "Invalid Dependency type: dependencyType={0}, actualType={1}";
	}
}
#endif
