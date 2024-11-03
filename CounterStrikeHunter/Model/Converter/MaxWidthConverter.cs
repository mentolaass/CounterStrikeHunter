using System.Globalization;
using System.Windows.Data;
using System;

namespace CounterStrikeHunter.Model.Converter
{
    [ValueConversion(typeof(double), typeof(double))]
    public class MaxWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is double totalWidth)
            {
                double fixedWidth = 200;

                return (totalWidth - fixedWidth);
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
