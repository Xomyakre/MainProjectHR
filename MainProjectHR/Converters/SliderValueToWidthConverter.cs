using System;
using System.Globalization;
using System.Windows.Data;

namespace MainProjectHR.Converters
{
    public class SliderValueToWidthConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double doubleValue)
            {
                // Преобразуем значение слайдера (0-10) в проценты (0-100%)
                return (doubleValue * 10) + "%";
            }
            return "0%";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
} 