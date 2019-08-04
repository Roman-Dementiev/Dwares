using System;
using Dwares.Dwarf;
using Dwares.Dwarf.Runtime;
using Xamarin.Forms.Xaml;
using Xamarin.Forms;


namespace Dwares.Druid.Xaml
{
	public class Instance : IMarkupExtension
	{
		public string Class { set; get; }

		public object ProvideValue(IServiceProvider serviceProvider)
		{
			var type = PackageUnit.GetTypeByName(Class, Application.Current.GetType().Assembly);
			if (type != null) {
				try {
					var instance = Activator.CreateInstance(type);
					return instance;
				}
				catch (Exception ex) {
					Debug.ExceptionCaught(ex);
				}

			}
			return null;
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
