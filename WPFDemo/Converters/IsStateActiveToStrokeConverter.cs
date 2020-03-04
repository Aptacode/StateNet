using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace WPFDemo.Converters
{
    public class IsStateActiveToStrokeConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isActive && isActive)
            {
                return new SolidColorBrush(Color.FromRgb(25, 200, 25));
            }

            return new SolidColorBrush(Color.FromRgb(0, 0, 0));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}