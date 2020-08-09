using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Dwares.Druid.Satchel;
using Dwares.Dwarf;

namespace Dwares.Druid
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
			get => busy.Count > 0;
		}
		public bool NotBusy => !IsBusy;

		public string BusyMessage {
			get => busyMessage;
			set => SetProperty(ref busyMessage, value);
		}
		string busyMessage;

		public void StartBusy(string message = null) 
			=> StartBusy(null, null, message);

		public void StartBusy(Task task, CancellationTokenSource cts, string message)
		{
			busy.Add(new BusyState { Task = task, CTS = cts, Message = message });
			if (message != null) {
				BusyMessage = message;
			}
			PropertiesChanged(nameof(IsBusy), nameof(NotBusy));
		}

		public void ClearBusy(bool reset = false)
		{
			if (busy.Count == 0) {
				Debug.Print("ViewModel.ClearBusy(): ViewModel not in Busy state");
				return;
			}

			int last = busy.Count-1;
			busy[last].Cancel();
			busy.RemoveAt(last);

			BusyMessage = null;
			while (last > 0) {
				last--;
				if (busy[last].Message != null) {
					BusyMessage = busy[last].Message;
					break;
				}
			}
			PropertiesChanged(nameof(IsBusy), nameof(NotBusy));
		}

		public void ResetBusy()
		{
			if (busy.Count == 0)
				return;

			for (int i = busy.Count-1; i >= 0; i--) {
				busy[i].Cancel();
			}
			busy.Clear();
			BusyMessage = null;
			PropertiesChanged(nameof(IsBusy), nameof(NotBusy));
		}


		internal struct BusyState
		{
			internal Task Task { get; set; }
			internal CancellationTokenSource CTS { get; set; }
			internal string Message { get; set; }

			internal void Cancel()
			{
				if (CTS != null && Task?.IsCompleted == false) {
					CTS.Cancel();
				}
			}
		}

		List<BusyState> busy = new List<BusyState>();
	}
}
