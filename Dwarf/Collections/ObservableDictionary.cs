using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;

namespace Dwares.Dwarf.Collections
{
	public class ObservableDictionary<TKey, TValue> : PropertyNotifier, IDictionary<TKey, TValue>, ISuspendableNotifyCollectionChanged
	{
		//static ClassRef @class = new ClassRef(typeof(ObservableDictionary));

		public event NotifyCollectionChangedEventHandler CollectionChanged;

		private IDictionary<TKey, TValue> dict;

		//protected ObservableDictionary(IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer = null)
		//{
		//	//Debug.EnableTracing(@class);

		//	if (comparer == null) {
		//		dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
		//	} else if (dictionary == null) {
		//		dict = new Dictionary<TKey, TValue>(comparer);
		//	} else {
		//		dict = new Dictionary<TKey, TValue>(dictionary, comparer);
		//	}
		//}

		protected ObservableDictionary(IDictionary<TKey, TValue> dictionary)
		{
			//Debug.EnableTracing(@class);

			dict = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
		}

		public ObservableDictionary() :
			this(new Dictionary<TKey, TValue>())
		{
		}

		public ObservableDictionary(IEqualityComparer<TKey> comparer) :
			this(new Dictionary<TKey, TValue>(comparer))
		{
		}

		public ObservableDictionary(int capacity) :
			this(new Dictionary<TKey, TValue>(capacity))
		{
		}

		public ObservableDictionary(int capacity, IEqualityComparer<TKey> comparer) :
			this(new Dictionary<TKey, TValue>(capacity, comparer))
		{
		}

		public int Count => dict.Count;
		public ICollection<TKey> Keys => dict.Keys;
		public ICollection<TValue> Values => dict.Values;
		public bool IsReadOnly => dict.IsReadOnly;

		public TValue this[TKey key] {
			get => dict[key];
			set {
				if (dict.ContainsKey(key)) {
					var oldValue = dict[key];
					dict[key] = value;
					FireCollectionChange_Replace(key, value, oldValue);
				} else {
					Add(key, value);
				}
			}
		}

		public bool ContainsKey(TKey key) => dict.ContainsKey(key);
		public bool Contains(KeyValuePair<TKey, TValue> item) => dict.Contains(item);

		public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) => dict.CopyTo(array, arrayIndex);

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();
		IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();


		public void Clear()
		{
			dict.Clear();
			FireCollectionChange_Reset();
		}

		public void Add(TKey key, TValue value)
		{
			Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		public void Add(KeyValuePair<TKey, TValue> item)
		{
			dict.Add(item);
			FireCollectionChange_Add(item);
		}

		public bool Remove(TKey key)
		{
			if (dict.ContainsKey(key)) {
				var value = dict[key];
				if (dict.Remove(key)) { // Should be true
					FireCollectionChange_Remove(key, value);
					return true;
				}
			}
			return false;
		}

		public bool Remove(KeyValuePair<TKey, TValue> item)
		{
			if (dict.Remove(item)) {
				FireCollectionChange_Remove(item);
				return true;
			} else {
				return false;
			}
		}


		protected void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
		{
			if (NotificationsAreSuspended) {
				HasPendingNotifications = true;
			} else {
				CollectionChanged?.Invoke(this, args);
			}
		}

		protected void FireCollectionChange_Add(KeyValuePair<TKey, TValue> item)
		{
			FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, item));
			FirePropertiesChanged(nameof(Count), nameof(Keys), nameof(Values), $"Item[{item.Key.ToString()}]");
		}

		protected void FireCollectionChange_Add(TKey key, TValue value)
		{
			FireCollectionChange_Add(new KeyValuePair<TKey, TValue>(key, value));
		}

		protected void FireCollectionChange_Remove(KeyValuePair<TKey, TValue> item)
		{
			FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, item));
			FirePropertiesChanged(nameof(Count), nameof(Keys), nameof(Values), $"Item[{item.Key.ToString()}]");
		}

		protected void FireCollectionChange_Remove(TKey key, TValue value)
		{
			FireCollectionChange_Remove(new KeyValuePair<TKey, TValue>(key, value));
		}

		protected void FireCollectionChange_Replace(KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem)
		{
			FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, newItem, oldItem));
			FirePropertiesChanged(nameof(Values), $"Item[{oldItem.Key.ToString()}]");
		}

		protected void FireCollectionChange_Replace(TKey key, TValue newValue, TValue oldValue)
		{
			var newItem = new KeyValuePair<TKey, TValue>(key, newValue);
			var oldItem = new KeyValuePair<TKey, TValue>(key, oldValue);
			FireCollectionChange_Replace(newItem, oldItem);
		}

		protected void FireCollectionChange_Reset()
		{
			FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
			FirePropertiesChanged(nameof(Count), nameof(Keys), nameof(Values));
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
				FireCollectionChange_Reset();
			}
		}
	}
}
