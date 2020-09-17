using System;
using Dwares.Dwarf;


namespace Dwares.Dwarf.Toolkit
{
	public class GuardDisposable<T> : IDisposable
	{
		protected T Guarded { get; private set; }
		protected bool IsDisposed { get; private set; } = false;

		public GuardDisposable(object obj, bool required)
		{
			if (obj is T guarded) {
				Guarded = guarded;
			} else {
				Guarded = default;
				if (required) {
					throw new ArgumentNullException(nameof(obj));
				}
			}
		}

		public GuardDisposable(T obj)
		{
			Guarded = obj ?? throw new ArgumentNullException(nameof(obj));
		}

#if DEBUG
		~GuardDisposable()
		{
			Debug.Assert(IsDisposed, "DebugDisposable.Dispose() was not called.");
		}
#endif
		public virtual void Dispose()
		{
			Debug.Assert(!IsDisposed, "DebugDisposable.Dispose() already was called.");
			if (IsDisposed)
				return;

			if (Guarded is IDisposable disposable) {
				disposable.Dispose();
			}

			Guarded = default;
			IsDisposed = true;
		}
	}

	public class GuardDisposable : GuardDisposable<object>
	{
		public GuardDisposable(object obj, bool required = true) :
			base(obj, required)
		{
		}
	}
}
