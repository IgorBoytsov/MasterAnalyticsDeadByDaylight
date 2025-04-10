using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class PlatformToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string platformName)
            {
                return platformName switch
                {
                    "Steam" => System.Windows.Application.Current.Resources["Brush.Steam"] as SolidColorBrush,
                    "Кроссплатформа" => System.Windows.Application.Current.Resources["Brush.Crosplatform"] as SolidColorBrush,
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