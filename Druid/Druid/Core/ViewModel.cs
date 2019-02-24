using System;


namespace Dwares.Druid
{
	public class ViewModel : BindingScope
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

		bool isBusy;
		public bool IsBusy {
			get => isBusy;
			set {
				if (value != isBusy) {
					isBusy = value;
					PropertiesChanged(nameof(IsBusy), nameof(NotBusy));
				}
			}
		}
		public bool NotBusy => !IsBusy;
	}
}
