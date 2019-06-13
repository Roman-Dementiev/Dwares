using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;


namespace Dwares.Druid
{
	public interface IModel
	{
		event ModelChangedEventHandler ModelChanged;
	}

	public class Model : PropertyNotifier, IModel
	{
		//static ClassRef @class = new ClassRef(typeof(Model));

		public event ModelChangedEventHandler ModelChanged;
		HashSet<string> changedProperties = new HashSet<string>();

		public Model()
		{
			//Debug.EnableTracing(@class);
		}

		int notificationsSuspended = 0;
		protected bool NotificationsSuspended {
			get => notificationsSuspended > 0;
		}

		protected void SuspendNotifications()
		{
			notificationsSuspended++;
		}

		protected void ResumeNotifications()
		{
			Debug.Assert(notificationsSuspended > 0);
			if (notificationsSuspended <= 0)
				return;

			notificationsSuspended--;
			if (notificationsSuspended == 0 && changedProperties.Count > 0) {
				DoNotification(changedProperties);
				changedProperties.Clear();
			}
		}

		protected override void FirePropertyChanged([CallerMemberName] string propertyName = "")
		{
			if (NotificationsSuspended) {
				changedProperties.Add(propertyName);
			} else {
				DoNotification(new string[] { propertyName });
			}
		}

		protected override void PropertiesChanged(IEnumerable<string> names)
		{
			if (NotificationsSuspended) {
				changedProperties.UnionWith(names);
			} else {
				DoNotification(names);
			}
		}

		protected void DoNotification(IEnumerable<string> changedProperties)
		{
			base.PropertiesChanged(changedProperties);

			if (ModelChanged != null) {
				var e = new ModelChangedEventArgs(changedProperties);
				ModelChanged(this, e);
			}
		}
	}

	public class ModelChangedEventArgs : EventArgs
	{
		public ModelChangedEventArgs(IEnumerable<string> changedProperties)
		{
			ChangedProperties = changedProperties;
		}

		public virtual IEnumerable<string> ChangedProperties { get; }
	}

	public delegate void ModelChangedEventHandler(object sender, ModelChangedEventArgs e);
}
