using System;
using Windows.Foundation.Collections;
using Windows.ApplicationModel.Resources.Core;
using Windows.UI.Core;
using Dwares.Dwarf;


namespace Dwares.Druid.UWP
{
	public class ResourceMapListener
	{
		public delegate void HandlerProc(string resourceKey, string value);

		public string ResourceKey { get; protected set; }
		public HandlerProc Handler { get; protected set; }
		public CoreDispatcher Dispatcher { get; protected set; }
		public ResourceContext Context { get; protected set; }
		CoreDispatcherPriority Priority { get; set; } = CoreDispatcherPriority.Normal;

		public ResourceMapListener(HandlerProc handler, CoreDispatcher dispatcher, ResourceContext context = null) :
			this(null, handler, dispatcher, context)
		{ }

		public ResourceMapListener(string resourceKey, HandlerProc handler, CoreDispatcher dispatcher, ResourceContext context = null)
		{
			Init(resourceKey, handler, dispatcher, context);
		}

		protected ResourceMapListener() { }
		~ResourceMapListener() => UnRegister();

		protected void Init(string resourceKey, HandlerProc handler, CoreDispatcher dispatcher, ResourceContext context = null)
		{
			ResourceKey = resourceKey;
			Handler = handler ?? throw new ArgumentNullException(nameof(handler));
			Dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
			Context = context ?? ResourceContext.GetForCurrentView();
		}

		public bool IsRegistered { get; private set; } = false;

		public void Register()
		{
			if (!IsRegistered) {
				Context.QualifierValues.MapChanged += OnResourceMapChanged;
				IsRegistered = true;
			}
		}

		public void UnRegister()
		{
			if (IsRegistered) {
				Context.QualifierValues.MapChanged -= OnResourceMapChanged;
				IsRegistered = false;
			}
		}

		async void OnResourceMapChanged(IObservableMap<string, string> sender, IMapChangedEventArgs<string> e)
		{
			try {
				var value = sender.ContainsKey(e.Key) ? sender[e.Key] : String.Empty;

				await Dispatcher.RunAsync(Priority, () => Handler(e.Key, value));
			}
			catch (Exception ex) {
				Debug.Print(ex.Message);
				// TODO
			}
		}
	}
}
