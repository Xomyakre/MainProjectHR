using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MainProjectHR.Converters
{
    public class PasswordEmptyVisibilityConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is string password)
            {
                return string.IsNullOrEmpty(password) ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Visible;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 