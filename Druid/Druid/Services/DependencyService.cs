using System;
using Xamarin.Forms;


namespace Dwares.Druid.Services
{
	public static class DependencyService
	{
		public static Service GetService<Service>(bool required, bool global = true) where Service : class
		{
			var fetchTarget = global ? DependencyFetchTarget.GlobalInstance : DependencyFetchTarget.NewInstance;
			var service = Xamarin.Forms.DependencyService.Get<Service>(fetchTarget);
			if (required && service == null) {
				throw new DependencyServiceNotFound(typeof(Service));
			}
			return service;
		}

		public static Service GetInstance<Service>(ref Service instance, bool required = true) where Service : class
		{
			if (instance == null) {
				instance = GetService<Service>(required);
			}
			return instance;
		}

		public static Service GetInstance<Service, Default>(ref Service instance)
			where Service : class
			where Default : Service, new()
		{
			if (instance == null) {
				instance = GetService<Service>(required: false);

				if (instance == null) {
					SetInstance(ref instance, new Default());
				}
			}
			return instance;
		}

		public static void SetInstance<Service>(ref Service instance, Service service)
			where Service : class
		{
			instance = service;
			Xamarin.Forms.DependencyService.RegisterSingleton(instance);
		}
	}

	public class DependencyServiceNotFound : Exception
	{
		public Type ServiceType { get; }

		public DependencyServiceNotFound(Type serviceType)
		{
			ServiceType = serviceType;
		}

		public DependencyServiceNotFound(Type serviceType, Exception innerException) :
			base(null, innerException)
		{
			ServiceType = serviceType;
		}
	}
}
