using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Collections
{
	public class BatchCollectionChange : IDisposable
	{
		ISuspendableNotifyCollectionChanged suspendable;

		public BatchCollectionChange(object collection, bool requireSuspandable)
		{
			suspendable = collection as ISuspendableNotifyCollectionChanged;
			if (suspendable != null) {
				suspendable.SuspendNotifications();
			}
			else if (requireSuspandable) {
				throw new ArgumentException("BatchCollectionChange(): collection does not implement ISuspendableNotifyCollectionChanged");
			}
		}

		public BatchCollectionChange(ISuspendableNotifyCollectionChanged collection)
		{
			suspendable = collection ?? throw new ArgumentNullException(nameof(collection));
			suspendable.SuspendNotifications();
		}

#if DEBUG
		~BatchCollectionChange()
		{
			Debug.AssertIsNull(suspendable, "BatchCollectionChange.Dispose() was not called.");
		}
#endif
		public void Dispose()
		{
			suspendable.ResumeNotifications(false);
			suspendable = null;
		}

	}
}
