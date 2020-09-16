using System;
using System.Collections.Specialized;


namespace Dwares.Dwarf.Collections
{
	public interface ISuspendableNotifyCollectionChanged : INotifyCollectionChanged
	{
		bool NotificationsAreSuspended {  get; }
		void SuspendNotifications();
		void ResumeNotifications(bool force);
	}
}
