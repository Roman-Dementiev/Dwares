﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dwares.Dwarf.Collections;
using Dwares.Dwarf.Validation;
using Dwares.Druid.Support;


namespace Dwares.Druid.Forms
{
	public class FormScope : BindingScope
	{
		public const string ValidationError = "Validation error";
		protected Validatables validatables = null;

		public FormScope() :
			base(AppScope)
		{ }

		public FormScope(BindingScope parentScope) :
			base(parentScope)
		{ }

		protected virtual bool DoValidate(out IList<string> errors)
		{
			if (validatables != null && !validatables.Validate()) {
				errors = validatables.Errors;
				return false;
			}

			errors = null;
			return true;
		}

		protected virtual Task DoAccept()
		{
			return null;
		}

		protected virtual async Task<bool> Validate()
		{
			bool valid = DoValidate(out var errors);
			if (!valid) {
				var message = Collection.First(errors) ?? ValidationError;
				await Alerts.ErrorAlert(message);
			}

			return valid;
		}


		public virtual async void OnAccept()
		{
			bool valid = await Validate();
			if (!valid)
				return;

			var task = DoAccept();
			if (task != null)
				await task;


			await Navigator.PopPage();
		}

		public virtual async void OnCancel()
		{
			await OnDismiss();
		}

		public virtual async Task OnDismiss()
		{
			await Navigator.PopPage();
		}
	}
}