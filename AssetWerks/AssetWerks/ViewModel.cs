using System;
using AssetWerks.Model;


namespace AssetWerks
{
	public class ViewModel : NotifyPropertyChanged
	{
		bool isBusy;
		public bool IsBusy {
			get => isBusy;
			set {
				if (value != isBusy) {
					isBusy = value;
					FirePropertyChanged(nameof(IsBusy), nameof(IsReady));
				}
			}
		}

		public bool IsReady => !IsBusy;
	}
}
