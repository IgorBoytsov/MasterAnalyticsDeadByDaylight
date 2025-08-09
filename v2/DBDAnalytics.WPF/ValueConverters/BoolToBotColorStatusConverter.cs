using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class BoolToBotColorStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bot)
            {
                if (bot)
                    return System.Windows.Application.Current.Resources["Brush.IsBot.Converter"] as SolidColorBrush;
                else
                    return System.Windows.Application.Current.Resources["Brush.NoBo.Converter"] as SolidColorBrush;

            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
