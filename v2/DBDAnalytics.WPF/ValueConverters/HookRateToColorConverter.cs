using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    class HookRateToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double hookRate)
            {
                return hookRate switch
                {
                    < 1 => System.Windows.Application.Current.Resources["Brush.ZeroHooks"] as SolidColorBrush,
                    >= 1 and < 2 => System.Windows.Application.Current.Resources["Brush.OneHooks"] as SolidColorBrush,
                    >= 2 and < 3 => System.Windows.Application.Current.Resources["Brush.TwoHooks"] as SolidColorBrush,
                    >= 3 and < 4 => System.Windows.Application.Current.Resources["Brush.ThreeHooks"] as SolidColorBrush,
                    >= 4 and < 5 => System.Windows.Application.Current.Resources["Brush.FourHooks"] as SolidColorBrush,
                    >= 5 and < 6 => System.Windows.Application.Current.Resources["Brush.FiveHooks"] as SolidColorBrush,
                    >= 6 and < 7 => System.Windows.Application.Current.Resources["Brush.SixHooks"] as SolidColorBrush,
                    >= 7 and < 8 => System.Windows.Application.Current.Resources["Brush.SevenHooks"] as SolidColorBrush,
                    >= 8 and < 9 => System.Windows.Application.Current.Resources["Brush.EightHooks"] as SolidColorBrush,
                    >= 9 and < 10 => System.Windows.Application.Current.Resources["Brush.NineHooks"] as SolidColorBrush,
                    >= 10 and < 11 => System.Windows.Application.Current.Resources["Brush.TenHooks"] as SolidColorBrush,
                    >= 11 and < 12 => System.Windows.Application.Current.Resources["Brush.ElevenHooks"] as SolidColorBrush,
                    12 => System.Windows.Application.Current.Resources["Brush.TwelveHooks"] as SolidColorBrush,
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