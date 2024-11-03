using System;
using System.Globalization;
using System.Windows.Data;

namespace CounterStrikeHunter.Model.Converter
{
    [ValueConversion(typeof(bool), typeof(bool))]
    public class InvBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool _bool)
            {
                return !_bool;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
