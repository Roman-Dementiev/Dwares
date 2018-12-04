using System;


namespace Dwares.Druid
{
	public class ViewModel : BindingScope
	{
		public ViewModel() : base(AppScope) { }

		public ViewModel(BindingScope parentScope) :
			base(parentScope)
		{ }

		string title = string.Empty;
		public string Title {
			get => title;
			set => SetProperty(ref title, value);
		}
	}
}
