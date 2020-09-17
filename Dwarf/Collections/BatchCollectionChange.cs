using System;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Dwarf.Collections
{
	public class BatchCollectionChange : GuardDisposable<ISuspendableNotifyCollectionChanged>
	{
		public BatchCollectionChange(object collection, bool requireSuspandable) :
			base(collection, requireSuspandable)
		{
			Guarded?.SuspendNotifications();
		}

		public BatchCollectionChange(ISuspendableNotifyCollectionChanged collection) :
			base(collection)
		{
			Guarded.SuspendNotifications();
		}

		public override void Dispose()
		{
			Guarded?.ResumeNotifications(false);
			base.Dispose();
		}

	}
}
