using System;
using Xamarin.Forms.Xaml;


namespace Dwares.Druid.Xaml
{
	public abstract class MarkupExtension<T> : IMarkupExtension<T>
	{
		public abstract T ProvideValue(IServiceProvider serviceProvider);
		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider) 
			=> (this as IMarkupExtension<T>).ProvideValue(serviceProvider);
	}
}
