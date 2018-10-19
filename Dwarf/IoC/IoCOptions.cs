#if IoC
using System;
using Dwares.Dwarf.Runtime;


namespace Dwares.Dwarf.IoC
{
	public enum UnregisteredResolutionAction
	{
		/// <summary>
		/// Fail resolution if type not explicitly registered
		/// </summary>
		Fail,

		/// <summary>
		/// Attempt to resolve type, even if the type isn't registered.
		/// 
		/// Registered types/options will always take precedence.
		/// </summary>
		AttemptResolve,

		/// <summary>
		/// Attempt to resolve unregistered type if requested type is generic
		/// and no registration exists for the specific generic parameters used.
		/// 
		/// Registered types/options will always take precedence.
		/// </summary>
		//GenericsOnly

		Default = Fail
	}

	public enum NamedResolutionFailureAction
	{
		Fail,
		AttemptUnnamedResolution,
		Default = Fail
	}

	public class IoCOptions: ConstructorOptions
	{
		public UnregisteredResolutionAction UnregisteredResolutionAction { get; set; }
		public NamedResolutionFailureAction NamedResolutionFailureAction { get; set; }

		public static readonly IoCOptions Default = new IoCOptions();
	}
}
#endif
