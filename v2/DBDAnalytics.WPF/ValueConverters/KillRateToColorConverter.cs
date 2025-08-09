using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class KillRateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double killRate)
            {
                return killRate switch
                {
                    < 1 => System.Windows.Application.Current.Resources["Brush.ZeroKills"] as SolidColorBrush,
                    >= 1 and < 2 => System.Windows.Application.Current.Resources["Brush.OneKills"] as SolidColorBrush,
                    >= 2 and < 3 => System.Windows.Application.Current.Resources["Brush.TwoKills"] as SolidColorBrush,
                    >= 3 and < 4 => System.Windows.Application.Current.Resources["Brush.ThreeKills"] as SolidColorBrush,
                    4 => System.Windows.Application.Current.Resources["Brush.FourKills"] as SolidColorBrush,
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