using System;
using System.Threading;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Dwarf.Toolkit;
using Dwares.Druid.Satchel;


namespace Dwares.Druid.ViewModels
{
	public class ViewModel : PropertyNotifier, IBusy
	{
		public ViewModel() { }

		public string Title {
			get => title;
			set => SetProperty(ref title, value);
		}
		string title = string.Empty;

		public bool IsBusy {
			get => busyState != null;
			set {
				if (value) {
					StartBusy();
				} else {
					EndBusy();
				}
			}
		}
		BusyState? busyState;

		public bool IsNotBusy {
			get => busyState == null;
			set => IsBusy = !value;
		}

		public string BusyMessage {
			get => busyMessage;
			set => SetProperty(ref busyMessage, value);
		}
		string busyMessage;

		public void StartBusy(string message = null)
		{
			if (busyState == null) {
				StartBusy(null, null, message);
			} else {
				if (message != null) {
					BusyMessage = message;
				}
			}
		}

		public void StartBusy(Task task, CancellationTokenSource cts, string message)
		{
			Debug.Assert(busyState == null);
			if (busyState != null)
				return;

			busyState = new BusyState { Task = task, CTS = cts };
			if (message != null) {
				BusyMessage = message;
			}
			FirePropertiesChanged(nameof(IsBusy), nameof(IsNotBusy));
		}

		public void EndBusy()
		{
			if (busyState == null) {
				Debug.Print("ViewModel.EndBusy(): ViewModel not in Busy state");
				return;
			}

			busyState?.Cancel();
			busyState = null;
			BusyMessage = null;

			FirePropertiesChanged(nameof(IsBusy), nameof(IsNotBusy));
		}

		internal struct BusyState
		{
			internal Task Task { get; set; }
			internal CancellationTokenSource CTS { get; set; }

			internal void Cancel()
			{
				if (CTS != null && Task?.IsCompleted == false) {
					CTS.Cancel();
				}
			}
		}


		//public async Task<Exception> BusyAction(Action action, string busyMessage = null, bool alertOnError = true)
		//{
		//	Debug.AssertNotNull(action);
		//	if (IsBusy) {
		//		//Debug.Fail("Can not perfotm another busy task while in busy state");
		//		return null;
		//	}

		//	StartBusy(busyMessage);
		//	try {
		//		action();
		//		return null;
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		if (alertOnError) {
		//			await Alerts.ExceptionAlert(exc);
		//		}
		//		return exc;
		//	}
		//	finally {
		//		EndBusy();
		//	}
		//}

		//public async Task<Exception> BusyActionOnMainThread(Action action, string busyMessage = null, bool alertOnError = true)
		//{
		//	Debug.AssertNotNull(action);
		//	if (IsBusy) {
		//		//Debug.Fail("Can not perfotm another busy task while in busy state");
		//		return null;
		//	}

		//	StartBusy(busyMessage);
		//	try {
		//		action();
		//		return null;
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		if (alertOnError) {
		//			await Alerts.ExceptionAlert(exc);
		//		}
		//		return exc;
		//	}
		//	finally {
		//		EndBusy();
		//	}
		//}
		//public async Task<Exception> BusyTask(Func<Task> task, string busyMessage = null, bool alertOnError = true)
		//{
		//	Debug.AssertNotNull(task);
		//	if (IsBusy) {
		//		//Debug.Fail("Can not perfotm another busy task while in busy state");
		//		return null;
		//	}

		//	StartBusy(busyMessage);
		//	try {
		//		await task();
		//		return null;
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		if (alertOnError) {
		//			await Alerts.ExceptionAlert(exc);
		//		}
		//		return exc;
		//	}
		//	finally {
		//		EndBusy();
		//	}
		//}

		//public async Task<T> BusyFunc<T>(Func<Task<T>> task, string busyMessage = null, bool alertOnError = true)
		//{
		//	Debug.AssertNotNull(task);
		//	if (IsBusy) {
		//		Debug.Fail("Can not call another busy function while in busy state");
		//		return default;
		//	}

		//	StartBusy(busyMessage);
		//	try {
		//		return await task();
		//	}
		//	catch (Exception exc) {
		//		Debug.ExceptionCaught(exc);
		//		if (alertOnError) {
		//			await Alerts.ExceptionAlert( exc);
		//		}
		//		return default;
		//	}
		//	finally {
		//		EndBusy();
		//	}
		//}

		public bool CanPerformAction() => IsNotBusy;
		public bool CanPerformAction(object param) => IsNotBusy;
	}

}
