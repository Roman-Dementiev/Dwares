using System;
using System.Globalization;
using Xamarin.Forms;


namespace ACE
{
	public class TimeToStringConverter : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.Print("TimeToStringConverter.Convart: value={0}, targetType={1], parameter={2}", value, targetType, parameter);
			return "Time from converter";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			Debug.Print("TimeToStringConverter.ConvertBack: value={0}, targetType={1], parameter={2}", value, targetType, parameter);
			return new TimeSpan();
		}
	}
}
