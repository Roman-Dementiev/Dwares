﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;


namespace Dwares.Druid.ViewModels
{
	public class ViewModel : BindingScope, ITitleHolder
	{
		public ViewModel() : base(ApplicationScope) { }

		public ViewModel(BindingScope parentScope) :
			base(parentScope)
		{ }

		string title = string.Empty;
		public string Title {
			get => title;
			set => SetProperty(ref title, value);
		}

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
			set => IsBusy = false;
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


		public async Task BusyAction(Action action)
		{
			Debug.AssertNotNull(action);

			try {
				IsBusy = true;
				action();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				await Alerts.ExceptionAlert(exc);
			}
			finally {
				IsBusy = false;
			}
		}

		public async Task BusyTask(Func<Task> task)
		{
			Debug.AssertNotNull(task);

			try {
				IsBusy = true;
				await task();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				await Alerts.ExceptionAlert(exc);
			}
			finally {
				IsBusy = false;
			}
		}

		public async Task<T> BusyTask<T>(Func<Task<T>> task)
		{
			Debug.AssertNotNull(task);

			try {
				IsBusy = true;
				return await task();
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				await Alerts.ExceptionAlert(exc);
				return default;
			}
			finally {
				IsBusy = false;
			}
		}
	}
}