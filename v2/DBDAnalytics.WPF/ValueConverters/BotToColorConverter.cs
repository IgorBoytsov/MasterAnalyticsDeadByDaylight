using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class BotToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string platformName)
            {
                return platformName switch
                {
                    "Отключились: " => System.Windows.Application.Current.Resources["Brush.Bot"] as SolidColorBrush,
                    "Не отключились: " => System.Windows.Application.Current.Resources["Brush.NoBot"] as SolidColorBrush,
                    _ => () => Brushes.Transparent
                };
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}