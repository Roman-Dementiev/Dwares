using System;
using Dwares.Dwarf;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;


namespace Dwares.Druid.Xaml
{
	public class BindColor : IMarkupExtension<Color>
	{
		//static ClassRef @class = new ClassRef(typeof(BindColor));

		public BindColor()
		{
			//Debug.EnableTracing(@class);
		}


		Color IMarkupExtension<Color>.ProvideValue(IServiceProvider serviceProvider)
		{
			//if (TypedBinding == null)
			//	return new Binding(Path, Mode, Converter, ConverterParameter, StringFormat, Source) {
			//		UpdateSourceEventName = UpdateSourceEventName,
			//		FallbackValue = FallbackValue,
			//		TargetNullValue = TargetNullValue,
			//	};

			//TypedBinding.Mode = Mode;
			//TypedBinding.Converter = Converter;
			//TypedBinding.ConverterParameter = ConverterParameter;
			//TypedBinding.StringFormat = StringFormat;
			//TypedBinding.Source = Source;
			//TypedBinding.UpdateSourceEventName = UpdateSourceEventName;
			//TypedBinding.FallbackValue = FallbackValue;
			//TypedBinding.TargetNullValue = TargetNullValue;
			//return TypedBinding;
			return default;
		}

		object IMarkupExtension.ProvideValue(IServiceProvider serviceProvider)
		{
			return (this as IMarkupExtension<BindingBase>).ProvideValue(serviceProvider);
		}
	}
}
