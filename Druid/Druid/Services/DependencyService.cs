using System;
using System.Threading;


namespace Dwares.Druid.Services
{
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

		public static TService GetInstance(ref DependencyService<TService> instance, bool required = true)
		{
			var _instance = LazyInitializer.EnsureInitialized(ref instance, () => new DependencyService<TService>(true, required));
			return _instance.Service;
		}

		public static void SetInstance(ref DependencyService<TService> instance, TService service)
		{
			var _instance = LazyInitializer.EnsureInitialized(ref instance, () => new DependencyService<TService>(false));
			_instance.Set(service);
		}

		public DependencyService() : this(true, true) { }

		public DependencyService(bool init, bool required=true)
		{
			if (init) {
				service = TryGet(required);
			}
		}

		public DependencyService(TService service)
		{
			Set(service);
		}

		private bool inited = false;
		private TService service = null;
		public TService Service {
			get => Get(Mode.Exlusive);
			set => Set(value);
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

	public class DependencyServiceNotFound : Exception
	{
		public const string DefaultMessageFormat = "Dependency Service not found: {0}";

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
}
