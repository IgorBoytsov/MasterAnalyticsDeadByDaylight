using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class RecentGeneratorToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double recentGenerators)
            {
                return recentGenerators switch
                {
                    < 1 => System.Windows.Application.Current.Resources["Brush.ZeroGenerator"] as SolidColorBrush,
                    >= 1 and < 2 => System.Windows.Application.Current.Resources["Brush.OneGenerator"] as SolidColorBrush,
                    >= 2 and < 3 => System.Windows.Application.Current.Resources["Brush.TwoGenerator"] as SolidColorBrush,
                    >= 3 and < 4 => System.Windows.Application.Current.Resources["Brush.ThreeGenerator"] as SolidColorBrush,
                    >= 4 and < 5 => System.Windows.Application.Current.Resources["Brush.FourGenerator"] as SolidColorBrush,
                    5 => System.Windows.Application.Current.Resources["Brush.FiveGenerator"] as SolidColorBrush,
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