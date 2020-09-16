using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Collections
{
	public class ObservableCollectionEx<T> : ObservableCollection<T>, ISuspendableNotifyCollectionChanged
	{
		//static ClassRef @class = new ClassRef(typeof(OrderableCollectionEx));

		public ObservableCollectionEx()
		{
			//Debug.EnableTracing(@class);
		}

		public bool HasPendingNotifications { get; private set; }

		public bool NotificationsAreSuspended {
			get => notificationsAreSuspended > 0;
		}
		int notificationsAreSuspended;

		public void SuspendNotifications()
		{
			notificationsAreSuspended++;
		}

		public void ResumeNotifications(bool force)
		{
			Debug.Assert(notificationsAreSuspended > 0);

			if (force || notificationsAreSuspended <= 0) {
				notificationsAreSuspended = 0;
			} else {
				notificationsAreSuspended--;
			}

			if (notificationsAreSuspended == 0 && HasPendingNotifications) {
				HasPendingNotifications = false;

				OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
				OnPropertyChanged(new PropertyChangedEventArgs(nameof(Count)));
			}
		}
	}
}
