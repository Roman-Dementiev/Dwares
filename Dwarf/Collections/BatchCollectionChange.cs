using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Collections
{
	public class BatchCollectionChange : IDisposable
	{
		ISuspendableNotifyCollectionChanged collection;

		public BatchCollectionChange(ISuspendableNotifyCollectionChanged collection)
		{
			this.collection = collection ?? throw new ArgumentNullException(nameof(collection));
			collection.SuspendNotifications();
		}

#if DEBUG
		~BatchCollectionChange()
		{
			Debug.AssertIsNull(collection, "BatchCollectionChange.Dispose() was not called.");
		}
#endif
		public void Dispose()
		{
			collection.ResumeNotifications(false);
			collection = null;
		}
	}
}
