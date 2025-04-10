using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    class WinRateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double winRate)
            {
                return winRate switch
                {
                    < 10 => System.Windows.Application.Current.Resources["Brush.WinRateZeroPercent"] as SolidColorBrush,
                    >= 10 and < 20 => System.Windows.Application.Current.Resources["Brush.WinRateTenPercent"] as SolidColorBrush,
                    >= 20 and < 30 => System.Windows.Application.Current.Resources["Brush.WinRateTwentyPercent"] as SolidColorBrush,
                    >= 30 and < 40 => System.Windows.Application.Current.Resources["Brush.WinRateThirtyPercent"] as SolidColorBrush,
                    >= 40 and < 50 => System.Windows.Application.Current.Resources["Brush.WinRateFortyPercent"] as SolidColorBrush,
                    >= 50 and < 60 => System.Windows.Application.Current.Resources["Brush.WinRateFiftyPercent"] as SolidColorBrush,
                    >= 60 and < 70 => System.Windows.Application.Current.Resources["Brush.WinRateSixtyPercent"] as SolidColorBrush,
                    >= 70 and < 80 => System.Windows.Application.Current.Resources["Brush.WinRateSeventyPercent"] as SolidColorBrush,
                    >= 80 and < 90 => System.Windows.Application.Current.Resources["Brush.WinRateEightyPercent"] as SolidColorBrush,
                    >= 90 and < 100 => System.Windows.Application.Current.Resources["Brush.WinRateNinetyPercent"] as SolidColorBrush,
                    >= 10 => System.Windows.Application.Current.Resources["Brush.WinRateHundredPercent"] as SolidColorBrush,
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
