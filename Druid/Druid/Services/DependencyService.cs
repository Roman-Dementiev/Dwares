using System;
using System.Collections.Generic;
using System.Text;


namespace Dwares.Druid.Services
{
	public class DependencyServiceNotFound: Exception
	{
		public const string DefaultMessageFormat = "Depemdency Service not found: {0}";

		public Type ServiceType { get; }

		public DependencyServiceNotFound() { }

		public DependencyServiceNotFound(Type serviceType, string message) : base(message)
		{
			ServiceType = serviceType;
		}
		
		public DependencyServiceNotFound(Type serviceType, string message, Exception innerException) : base(message, innerException)
		{
			ServiceType = serviceType;
		}



		public static void Raise(Type serviceType, Exception innerException = null) => Raise(serviceType, null, innerException);

		public static void Raise(Type serviceType, string messageFormat, Exception innerException = null)
		{
			//Debug.Trace("DependencyService not found: {0}", serviceType?.FullName);
			var message = String.Format(messageFormat ?? DefaultMessageFormat, serviceType?.FullName);

			DependencyServiceNotFound ex;
			if (innerException != null) {
				ex = new DependencyServiceNotFound(serviceType, message, innerException);
			} else {
				ex = new DependencyServiceNotFound(serviceType, message);
			}
			throw ex;
		}
	}

	public class DependencyService<TService> where TService : class
	{
		public enum Mode
		{
			Exlusive,
			InitOnce,
			InitIfNull,
			InitAlwyas,
			GetOnly
		};

		private bool inited = false;
		private TService service = null;
		public TService Service {
			get => Get(Mode.Exlusive);
			set => Set(value);
		}

		public DependencyService(bool init = false)
		{
			//Debug.Trace("DependencyService() init={0}", init);
			if (init) {
				service = TryGet(true);
			}
		}

		public DependencyService(TService service)
		{
			//Debug.Print("DependencyService() service={0}", service);
			Set(service);
		}

		public TService TryGet(bool required)
		{
			try {
				var service = Xamarin.Forms.DependencyService.Get<TService>();
				if (service == null && required) {
					DependencyServiceNotFound.Raise(typeof(TService));
				}
				//Debug.Trace("DependencyService.TryGet({0}) => {1}", typeof(TService).FullName, service != null ? service.GetType().FullName : "null");
				return service;
			}
			catch (Exception ex) {
				if (required) {
					DependencyServiceNotFound.Raise(typeof(TService), ex);
					throw;
				}
				return null;
			}
		}

		public TService Get(Mode mode = Mode.InitOnce, bool required=false)
		{
			switch (mode)
			{
			case Mode.InitOnce:
				if (!inited) {
					service = TryGet(required);
				}
				break;

			case Mode.InitIfNull:
				if (service == null) {
					service = TryGet(required);
				}
				break;

			case Mode.InitAlwyas:
				service = TryGet(required);
				break;

			case Mode.GetOnly:
				return TryGet(required);
			}

			return service;
		}

		public void Set(TService service)
		{
			this.service = service;
			inited = true; 
		}

	}
}
