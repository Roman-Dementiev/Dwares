using System;
using System.Reflection;
using Dwares.Dwarf;


namespace Dwares.Druid.Satchel
{
	public abstract class WeakResouce<T> where T : class
	{
		//static ClassRef @class = new ClassRef(typeof(WeakResouce));

		public WeakResouce(ResourceId resourceId)
		{
			//Debug.EnableTracing(@class);
			Guard.ArgumentNotEmpty(resourceId, nameof(resourceId));

			ResourceId = resourceId;
		}

		public ResourceId ResourceId { get; }
		public string ResourceName => ResourceId.ResourceName;
		public Assembly Assembly => ResourceId.Assembly;
		public WeakReference<T> Ref { get; protected set; }

		public T Get()
		{
			T resource;
			if (!Ref.TryGetTarget(out resource)) {
				resource = LoadResource();
				Ref.SetTarget(resource);
			}
			return resource;
		}

		protected abstract T LoadResource(); 
	}
}
