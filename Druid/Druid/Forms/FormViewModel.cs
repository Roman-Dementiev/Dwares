using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;


namespace Dwares.Druid.Forms
{
	public class FormViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(FormViewModel));
		public const double FitContent = -1;
		public static double DefaultWidth { get; set; } = FitContent;
		public static double DefaultHeight { get; set; } = FitContent;

		public FormViewModel()
		{
			//Debug.EnableTracing(@class);
		}

		public FormViewModel(BindingScope parentScope) :
			base(parentScope)
		{
			//Debug.EnableTracing(@class);
		}

		public virtual double FormWidth { get; set; } = DefaultWidth;
		public virtual double FormHeight { get; set; } = DefaultHeight;

		public IFiledList Fields { get; protected set; }

		protected virtual Task DoAccept()
		{
			return null;
		}

		public virtual Task<Exception> Validate()
		{
			var error = Fields?.Validate();
			return Task.FromResult(error);
		}

		public virtual Task<List<Exception>> ValidateAll()
		{
			var errors = Fields?.ValidateAll();
			return Task.FromResult(errors);
		}

		public virtual async Task OnAccept()
		{
			Exception error = null;
			try {
				IsBusy = true;

				error = await Validate();
				if (error == null) {
					var task = DoAccept();
					if (task != null) {
						await task;
					}
				}
			}
			catch (Exception exc) {
				Debug.ExceptionCaught(exc);
				error = exc;
			}
			finally {
				IsBusy = false;
			}

			if (error != null) {
				await Alerts.ErrorAlert(error.Message);
				return;
			}

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

		public FormViewModel(BindingScope parentScope, TSource source = default(TSource)) :
			base(parentScope)
		{
			Source = source;
		}

		public TSource Source { get; protected set; }

		public bool IsNew => Source == null;
		public bool IsEditing => Source != null;
	}
}
