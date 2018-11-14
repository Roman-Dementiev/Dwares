using System;
using Xamarin.Forms;


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
