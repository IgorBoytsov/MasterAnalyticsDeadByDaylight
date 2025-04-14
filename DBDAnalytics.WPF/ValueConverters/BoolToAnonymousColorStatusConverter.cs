using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class BoolToAnonymousColorStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool anonymous)
            {
                if (anonymous)
                    return System.Windows.Application.Current.Resources["Brush.IsAnonymous.Converter"] as SolidColorBrush;
                else
                    return System.Windows.Application.Current.Resources["Brush.NoAnonymous.Converter"] as SolidColorBrush;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}