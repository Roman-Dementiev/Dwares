using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Dwares.Druid.Satchel;
using Xamarin.Forms.Xaml;


namespace Dwares.Druid.Xaml
{
	public class Instance : IMarkupExtension
	{
		public string Class { set; get; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			return AssetLocator.CreateInstance(Class);
		}
	}

	public class Instance<T> : Instance, IMarkupExtension<T> where T : class
	{
		T IMarkupExtension<T>.ProvideValue(IServiceProvider serviceProvider)
		{
			var instance = base.ProvideValue(serviceProvider);
			if (instance is T value) {
				return value;
			} else {
				Debug.Print($"Dwares.Druid.Xaml.Instance.ProvideValue(): invalid instance Type={Class}, expected {typeof(T)}");
				return null;
			}
		}
	}

}
