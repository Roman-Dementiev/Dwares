using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using Dwares.Druid.Satchel;


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

		public string BusyMessage { get; private set; }

		public void StartBusy(string message = null) 
			=> StartBusy(null, null, message);

		public void StartBusy(Task task, CancellationTokenSource cts, string message)
		{
			busy.Add(new BusyState { Task = task, CTS = cts, Message = message });
			if (message != null) {
				BusyMessage = message;
			}
			PropertiesChanged(nameof(IsBusy), nameof(NotBusy), nameof(BusyMessage));
		}

		public void ClearBusy(bool reset = false)
		{
			if (reset) {
				while (busy.Count > 0)
					ClearBusy(false);
			} else {
				int last = busy.Count-1;
				if (last >= 0) {
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
				}
			}
			PropertiesChanged(nameof(IsBusy), nameof(NotBusy), nameof(BusyMessage));
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
