using System;
using Dwares.Dwarf;


namespace Dwares.Druid.Xaml
{
	public interface IAsset
	{
		object AssetValue { get; }
	}

	public interface IAsset<T>
	{
		T AssetValue { get; }
	}

	//public interface IResourceAsset<T> : IAsset<T>
	//{
	//	ResourceId ResourceId { get; }
	//}

	public abstract class AssetBase<T> where T : class
	{
		//static ClassRef @class = new ClassRef(typeof(Ассет));
		protected WeakReference<T> @ref;

		public AssetBase()
		{
			//Debug.EnableTracing(@class);
			@ref = new WeakReference<T>(null);
		}

		protected abstract T CreateValue();

		protected virtual T GetValue()
		{
			T value;
			if (!@ref.TryGetTarget(out value)) {
				value = CreateValue();
				@ref.SetTarget(value);
			}
			return value;
		}
	}

	public class Asset : AssetBase<object>, IAsset
	{
		//static ClassRef @class = new ClassRef(typeof(Ассет));

		public Asset()
		{
			//Debug.EnableTracing(@class);
		}

		public string Class { get; set; }

		public object AssetValue => GetValue();

		protected override object CreateValue()
		{
			return AssetLocator.CreateInstance(Class);
		}
	}


	public class Asset<T> : AssetBase<T>, IAsset, IAsset<T> where T : class
	{
		public Asset() { }

		public string Class { get; set; }

		public T AssetValue => GetValue();
		object IAsset.AssetValue => GetValue();

		protected override T CreateValue()
		{
			var obj = AssetLocator.CreateInstance(Class);
			if (obj is T value)
				return value;

			Debug.Print($"Invalid type of asset value {obj.GetType()} (expected {typeof(T)})");
			return null;
		}
	}

	public abstract class ResourceAsset<T> : AssetBase<T>, IAsset, IAsset<T> where T : class
	{
		//static ClassRef @class = new ClassRef(typeof(Ассет));

		public ResourceAsset(ResourceId resourceId)
		{
			//Debug.EnableTracing(@class);
			ResourceId = resourceId;
		}

		public ResourceId ResourceId { get; }
		public T AssetValue => GetValue();
		object IAsset.AssetValue => GetValue();
	}
}
