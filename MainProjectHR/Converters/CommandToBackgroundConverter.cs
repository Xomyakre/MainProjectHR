using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace MainProjectHR.Converters
{
    public class CommandToButtonStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var currentCommand = value as ICommand;
            var targetCommand = parameter as ICommand;

            // Если кнопка активная, возвращаем нужный стиль (цвет фона и текста)
            if (currentCommand == targetCommand)
            {
                return new ButtonStyle
                {
                    Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#3f51b580")), // Ваш фон
                    Foreground = new SolidColorBrush(Colors.White) // Белый цвет текста
                };
            }

            // Для неактивных кнопок, возвращаем стандартные значения
            return new ButtonStyle
            {
                Background = new SolidColorBrush(Colors.Transparent), // Прозрачный фон для неактивных
                Foreground = new SolidColorBrush(Colors.Black) // Черный цвет текста для неактивных
            };
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class ButtonStyle
    {
        public Brush Background { get; set; }
        public Brush Foreground { get; set; }
    }
}
