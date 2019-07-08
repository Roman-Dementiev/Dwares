using System;
using System.ComponentModel;
using System.Globalization;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Satchel
{
	[ContentProperty(nameof(Value))]
	[Xamarin.Forms.TypeConverter(typeof(NamedColorTypeConverter))]
	public class NamedColor
	{
		public NamedColor() { }
		public NamedColor(string value)
		{
			Value = Color.Red;
		}

		public string Name { get; set; }
		public Color Value { get; set; }

		public static implicit operator Color(NamedColor color) => color.Value;
	}


	[Xamarin.Forms.Xaml.TypeConversion(typeof(NamedColor))]
	public class NamedColorTypeConverter : Xamarin.Forms.TypeConverter, IValueConverter
	{
		public NamedColorTypeConverter() { }

		//public override bool CanConvertFrom(Type sourceType)
		//{
		//	Debug.Print("NamedColorTypeConverter.CanConvertFrom()");
		//	return base.CanConvertFrom(sourceType);
		//}

		//public override object ConvertFrom(CultureInfo culture, object o)
		//{
		//	Debug.Print("NamedColorTypeConverter.ConvertFrom()");
		//	return base.ConvertFrom(culture, o);
		//}

		//public override object ConvertFrom(object o)
		//{
		//	Debug.Print("NamedColorTypeConverter.ConvertFrom()");
		//	return base.ConvertFrom(o);
		//}

		public override object ConvertFromInvariantString(string str)
		{
			Debug.Print("NamedColorTypeConverter.ConvertFromInvariantString()");
			var value = TypeDescriptor.GetConverter(typeof(Color)).ConvertFromInvariantString(str);
			if (value is Color color) {
				return new NamedColor { Value = color };
			} else {
				return null;
			}
		}

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			var converted = TypeDescriptor.GetConverter(typeof(Color)).ConvertFromInvariantString(value.ToString());
			if (converted is Color color) {
				return new NamedColor() { Value = color };
			} else {
				return null;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
