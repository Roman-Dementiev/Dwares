using System;
using System.Threading.Tasks;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Validation;


namespace Dwares.Druid.Forms
{
	public class FormViewModel : ViewModel
	{
		//public const string ValidationError = "Validation error";
		protected Validatables fields = null;

		public FormViewModel() { }

		public FormViewModel(BindingScope parentScope) :
			base(parentScope)
		{ }

		protected virtual Task DoAccept()
		{
			return null;
		}

		protected virtual Task<Exception> Validate()
		{
			var error = fields?.Validate();
			return Task.FromResult(error);
		}


		public virtual async Task OnAccept()
		{
			var error = await Validate();
			if (error != null) {
				await Alerts.ErrorAlert(error.Message);
				return;
			}

			var task = DoAccept();
			if (task != null)
				await task;


			await Navigator.PopPage();
		}

		public virtual async Task OnCancel()
		{
			await OnDismiss();
		}

		public virtual async Task OnDismiss()
		{
			await Navigator.PopPage();
		}
	}

	public class FormViewModel<TSource> : FormViewModel
	{
		public FormViewModel() { }

		public FormViewModel(TSource source)
		{
			Source = source;
		}

		public FormViewModel(BindingScope parentScope, TSource source) :
			base(parentScope)
		{
			Source = source;
		}

		public TSource Source { get; protected set; }

		public bool IsNew => Source == null;
		public bool IsEditing => Source != null;
	}
}
