using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;
using Dwares.Druid.Support;


namespace Dwares.Druid.UI
{
	public class ContentPageEx : ContentPage
	{
		public ContentPageEx() { }

		public ContentPageEx(BindingScope scope)
		{
			Scope = scope;
		}

		public BindingScope Scope {
			get => BindingContext as BindingScope;
			set => BindingContext = value;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			Scope?.UpdateCommands();
		}
	}
}
