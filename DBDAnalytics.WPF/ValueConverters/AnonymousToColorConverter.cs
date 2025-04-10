using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class AnonymousToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string platformName)
            {
                return platformName switch
                {
                    "Анонимных: " => System.Windows.Application.Current.Resources["Brush.Anonymous"] as SolidColorBrush,
                    "Не анонимных: " => System.Windows.Application.Current.Resources["Brush.NoAnonymous"] as SolidColorBrush,
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