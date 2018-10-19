using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Dwares.Dwarf.Collections
{
	class SafeDictionary<TKey, TValue> : IDisposable, IDictionary<TKey, TValue>
	{
		private readonly ReaderWriterLockSlim padlock = new ReaderWriterLockSlim();
		private readonly IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

		public void Dispose()
		{
			Clear();
			//GC.SuppressFinalize(this);
		}

		static void Release(object value)
		{
			if (value is IDisposable disposable) {
				disposable.Dispose();
			}
		}

		//public bool IsReadOnly => dict.IsReadOnly;
		public bool IsReadOnly {
			get {
				padlock.EnterReadLock();
				try {
					return dict.IsReadOnly;
				}
				finally {
					padlock.ExitReadLock();
				}
			}
		}

		public int Count{
			get {
				padlock.EnterReadLock();
				try {
					return dict.Count;
				}
				finally {
					padlock.ExitReadLock();
				}
			}
		}

		public ICollection<TKey> Keys {
			get {
				padlock.EnterReadLock();
				try {
					return dict.Keys;
				}
				finally {
					padlock.ExitReadLock();
				}
			}
		}

		public ICollection<TValue> Values {
			get {
				padlock.EnterReadLock();
				try {
					return dict.Values;
				}
				finally {
					padlock.ExitReadLock();
				}
			}
		}

		public bool ContainsKey(TKey key)
		{
			padlock.EnterReadLock();
			try {
				return dict.ContainsKey(key);
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		public bool Contains(KeyValuePair<TKey, TValue> pair)
		{
			padlock.EnterReadLock();
			try {
				return dict.Contains(pair);
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		public TValue this[TKey key] {
			get => Get(key);
			set => Set(key, value);
		}

		public bool TryGetValue(TKey key, out TValue value)
		{
			padlock.EnterReadLock();
			try {
				return dict.TryGetValue(key, out value);
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		public TValue Get(TKey key)
		{
			padlock.EnterReadLock();
			try {
				return dict[key];
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		public void Set(TKey key, TValue value)
		{
			TValue oldValue;
			if (Set(key, value, out oldValue)) {
				Release(oldValue);
			}
		}

		public bool Set(TKey key, TValue newValue, out TValue oldValue)
		{
			padlock.EnterWriteLock();
			try {
				bool replaced = dict.TryGetValue(key, out oldValue);
				dict[key] = newValue;
				return replaced;
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		public void Add(TKey key, TValue value)
		{
			padlock.EnterWriteLock();
			try {
				dict.Add(key, value);
			}
			finally {
				padlock.ExitWriteLock();
			}
		}

		public void Add(KeyValuePair<TKey, TValue> pair)
		{
			padlock.EnterWriteLock();
			try {
				dict.Add(pair);
			}
			finally {
				padlock.ExitWriteLock();
			}
		}

		public bool Remove(TKey key, out TValue value)
		{
			padlock.EnterWriteLock();
			try {
				if (dict.TryGetValue(key, out value)) {
					dict.Remove(key);
					return true;
				} else {
					return false;
				}
			}
			finally {
				padlock.ExitWriteLock();
			}
		}

		public bool Remove(TKey key)
		{
			TValue value;
			if (Remove(key, out value)) {
				if (value is IDisposable disposable) {
					disposable.Dispose();
				}
				return true;
			} else {
				return false;
			}
		}

		public bool Remove(KeyValuePair<TKey, TValue> pair)
		{
			padlock.EnterWriteLock();
			try {
				return dict.Remove(pair);
			}
			finally {
				padlock.ExitWriteLock();
			}
		}

		public void Clear(out Disposables disposables)
		{
			padlock.EnterWriteLock();
			try {
				disposables = new Disposables();
				foreach (var pair in dict) {
					if (pair.Value is IDisposable disposable) {
						disposables.Attach(disposable);
					}
				}
				dict.Clear();
			}
			finally {
				padlock.ExitWriteLock();
			}
		}

		public void Clear()
		{
			Disposables disposables;
			Clear(out disposables);
			disposables?.Dispose();
		}

		public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
		{
			padlock.EnterReadLock();
			try {
				dict.CopyTo(array, arrayIndex);
			}
			finally {
				padlock.ExitReadLock();
			}

		}

		public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
		{
			padlock.EnterReadLock();
			try {
				return dict.GetEnumerator();
			}
			finally {
				padlock.ExitReadLock();
			}
		}

		IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
	}
}
