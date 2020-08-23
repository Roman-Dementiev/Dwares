//using System;
//using System.Collections.ObjectModel;
//using System.Threading.Tasks;
//using Dwares.Dwarf;


//namespace Beylen.Models
//{
//	public class StoredCollection<T> : ObservableCollection<T>
//	{
//		//static ClassRef @class = new ClassRef(typeof(StoredCollection));

//		public StoredCollection(Func<Task<Exception>> addToStorageMethod = null)
//		{
//			//Debug.EnableTracing(@class);

//			AddToStorageMethod = addToStorageMethod;
//		}

//		Func<Task<Exception>> AddToStorageMethod { get; }

//		public virtual async Task<Exception> Add(T item, bool addToStorage)
//		{
//			try {
//				if (addToStorage && AddToStorageMethod != null) {
//					var result = await AddToStorageMethod();
//					return result;
//				}

//				base.Add(item);
//			}
//			catch (Exception exc) {
//				return exc;
//			}

//			return null;
//		}

//		private new void Add(T item) => base.Add(item);
//	}
//}
