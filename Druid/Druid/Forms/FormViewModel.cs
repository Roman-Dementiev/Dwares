﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf;
using Dwares.Druid.ViewModels;


namespace Dwares.Druid.Forms
{
	public class FormViewModel : ViewModel
	{
		//static ClassRef @class = new ClassRef(typeof(FormViewModel));

		public FormViewModel()
		{
			//Debug.EnableTracing(@class);
		}

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
				StartBusy();

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
				EndBusy();
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

		public TSource Source { get; protected set; }

		public bool IsNew => Source == null;
		public bool IsEditing => Source != null;
	}
}
