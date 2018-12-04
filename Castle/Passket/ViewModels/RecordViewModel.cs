using System;
using Dwares.Dwarf;
using Dwares.Druid;
using Passket.Models;


namespace Passket.ViewModels
{
	public class RecordViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(RecordViewModel));

		public RecordViewModel(Record source)
		{
			//Debug.EnableTracing(@class);

			Source = source ?? throw new ArgumentNullException(nameof(source));
			Title = source?.Name;
		}

		public Record Source { get; }

		bool showEmptyFields = false;
		public bool ShowEmptyFields {
			get => showEmptyFields;
			set => SetProperty(ref showEmptyFields, value); 
		}

		public async void OnEditRecord()
		{
			var viewModel = new RecordEditModel(Source);
			var page = viewModel.CreatePage();
			await Navigator.PushPage(page);
		}
	}
}
