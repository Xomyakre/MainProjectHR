using System;
using System.Globalization;
using System.Windows.Data;

namespace MainProjectHR.Converters
{
    public class InitialsConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string fullName && !string.IsNullOrEmpty(fullName))
            {
                var nameParts = fullName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (nameParts.Length >= 2)
                {
                    return $"{nameParts[0][0]}{nameParts[1][0]}".ToUpper();
                }
                else if (nameParts.Length == 1)
                {
                    return nameParts[0][0].ToString().ToUpper();
                }
            }
            return "??";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 