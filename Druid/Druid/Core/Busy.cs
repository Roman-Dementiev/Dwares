using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dwares.Druid
{
	public interface IBusy
	{
		bool IsBusy { get; set; }
		string BusyMessage { get; set; }

		void StartBusy(string message);
		void EndBusy();
	}


	public static class Busy
	{
		public static async Task<Exception> BusyTask(this IBusy busy, Action action, string busyMessage = null, bool alertOnException = true)
		{
			Debug.AssertNotNull(busy);
			Debug.AssertNotNull(action);
			if (busy.IsBusy) {
				//Debug.Fail("Can not perfotm another busy task while in busy state");
				return null;
			}

			busy.StartBusy(busyMessage);
			try {
				action();
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				if (alertOnException) {
					await Alerts.ExceptionAlert(exc);
				}
				return exc;
			}
			finally {
				busy.EndBusy();
			}
		}

		public static async Task<Exception> BusyTask(this IBusy busy, Func<Task> task, string busyMessage = null, bool alertOnException = true)
		{
			Debug.AssertNotNull(busy);
			Debug.AssertNotNull(task);
			if (busy.IsBusy) {
				//Debug.Fail("BusyTask called with other BusyTask in progress");
				return null;
			}

			busy.StartBusy(busyMessage);
			try {
				await task();
				return null;
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				if (alertOnException) {
					await Alerts.ExceptionAlert(exc);
				}
				return exc;
			}
			finally {
				busy.EndBusy();
			}
		}

		public static async Task<T> BusyTask<T>(this IBusy busy, Func<Task<T>> task, string busyMessage = null, bool alertOnException = true)
		{
			Debug.AssertNotNull(busy);
			Debug.AssertNotNull(task);
			if (busy.IsBusy) {
				//Debug.Fail("BusyTask called with other BusyTask in progress");
				return default;
			}

			busy.StartBusy(busyMessage);
			try {
				return await task();
			} 
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				if (alertOnException) {
					await Alerts.ExceptionAlert(exc);
				}
				return default;
			}
			finally {
				busy.EndBusy();
			}
		}

		public static async Task BusyTaskOnMainThread(this IBusy busy, Action action, string busyMessage = null, bool alertOnException = true)
		{
			Debug.AssertNotNull(busy);
			Debug.AssertNotNull(action);
			if (busy.IsBusy) {
				//Debug.Fail("BusyTask called with other BusyTask in progress");
				return;
			}

			busy.StartBusy(busyMessage);
			await Device.InvokeOnMainThreadAsync(async () => {
				try {
					action();
				}
				catch (Exception exc) {
					if (alertOnException) {
						Debug.ExceptionCaught(exc);
						await Alerts.ExceptionAlert(exc);
					}
				}
				finally {
					busy.EndBusy();
				}
			});
		}

		public static async Task BusyTaskOnMainThread(this IBusy busy, Func<Task> task, string busyMessage = null, bool alertOnException = true)
		{
			Debug.AssertNotNull(busy);
			Debug.AssertNotNull(task);
			if (busy.IsBusy) {
				//Debug.Fail("BusyTask called with other BusyTask in progress");
				return;
			}

			busy.StartBusy(busyMessage);
			await Device.InvokeOnMainThreadAsync(async () => {
				try {
					await task();
				}
				catch (Exception exc) {
					if (alertOnException) {
						Debug.ExceptionCaught(exc);
						await Alerts.ExceptionAlert(exc);
					}
				}
				finally {
					busy.EndBusy();
				}
			});
		}
	}

	public class BusyGuard : DisposableGuard<IBusy>
	{
		bool wasBusy;
		string oldMessage;

		public BusyGuard(IBusy busy, string busyMessage = null) :
			base(busy)
		{
			wasBusy = busy.IsBusy;
			oldMessage = busy.BusyMessage;

			busy.StartBusy(busyMessage);
		}

		public override void Dispose()
		{
			if (wasBusy) {
				Guarded.BusyMessage = oldMessage;
			} else {
				Guarded.EndBusy();
			}

			base.Dispose();
		}
	}
}
