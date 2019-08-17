//using System;
//using Dwares.Dwarf;


//namespace Dwares.Druid.Satchel
//{
//	public interface IAsset<T> where T : class
//	{
//		WeakReference<T> AssetRef { get; }

//		T AssetValue { get; }
//	}

//	public interface IResourceAsset<T> : IAsset<T> where T : class
//	{
//		ResourceId ResourceId { get; }
//	}


//	public abstract class Asset<T> : IAsset<T> where T : class
//	{
//		//static ClassRef @class = new ClassRef(typeof(Ассет));

//		public Asset()
//		{
//			//Debug.EnableTracing(@class);
//			AssetRef = new WeakReference<T>(null);
//		}

//		public WeakReference<T> AssetRef { get; }

//		public T AssetValue { 
//			get => this.GetAssetValue<T>(CreateAssetValue);
//		}

//		protected abstract T CreateAssetValue();
//	}

//	public abstract class ResourceAsset<T> : Asset<T>, IResourceAsset<T> where T : class
//	{
//		//static ClassRef @class = new ClassRef(typeof(Ассет));

//		public ResourceAsset(ResourceId resourceId)
//		{
//			//Debug.EnableTracing(@class);
//			ResourceId = resourceId;
//		}

//		public ResourceId ResourceId { get; }
//	}

//	public class ClassAsset<T> : Asset<T> where T : class
//	{
//		public ClassAsset(string _class)
//		{
//			Class = _class;
//		}

//		public string Class { get; }

//		protected override T CreateAssetValue()
//		{
//			var instance = AssetLocator.CreateInstance(Class);
//			if (instance is T assetValue) {
//				return assetValue;
//			} else {
//				return null;
//			}
//		}
//	}

//	public static partial class Extensions
//	{
//		public static T GetAssetValue<T>(this IAsset<T> asset, Func<T> createValue) where T : class
//		{
//			T value;
//			var @ref = asset.AssetRef;
//			if (!@ref.TryGetTarget(out value)) {
//				value = createValue();
//				@ref.SetTarget(value);
//			}
//			return value;
//		}
//	}
//}
