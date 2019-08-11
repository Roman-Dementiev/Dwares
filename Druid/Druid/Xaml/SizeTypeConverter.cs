using System;
using System.Globalization;
using Dwares.Dwarf;
using Xamarin.Forms;


namespace Dwares.Druid.Xaml
{
	[Xamarin.Forms.Xaml.TypeConversion(typeof(Size))]
	public class SizeTypeConverter : TypeConverter
	{
		//static ClassRef @class = new ClassRef(typeof(SizeTypeConverter));

		public SizeTypeConverter()
		{
			//Debug.EnableTracing(@class);
		}

		public override object ConvertFromInvariantString(string value)
		{
			if (value != null) {
				string[] split = value.Split(',');
				switch (split.Length) {
				case 1:
					if (double.TryParse(split[0], NumberStyles.Number, CultureInfo.InvariantCulture, out double size))
						return new Size(size, size);
					break;

				case 2:
					if (double.TryParse(split[0], NumberStyles.Number, CultureInfo.InvariantCulture, out double width) &&
						double.TryParse(split[1], NumberStyles.Number, CultureInfo.InvariantCulture, out double height))
						return new Size(width, height);
					break;
				}
			}

			throw new InvalidOperationException($"Cannot convert \"{value}\" into {typeof(Size)}");
		}
	}
}
