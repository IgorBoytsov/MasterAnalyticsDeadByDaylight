using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class TypeDeathToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string platformName)
            {
                return platformName switch
                {
                    "От крюка" => System.Windows.Application.Current.Resources["Brush.DeathByHook"] as SolidColorBrush,
                    "От земли" => System.Windows.Application.Current.Resources["Brush.DeathByGround"] as LinearGradientBrush,
                    "От мементо" => System.Windows.Application.Current.Resources["Brush.DeathByMementoMori"] as SolidColorBrush,
                    "От способности убийцы" => System.Windows.Application.Current.Resources["Brush.DeathByKillerAbility"] as LinearGradientBrush,
                    "Сбежал" => System.Windows.Application.Current.Resources["Brush.DeathByEscape"] as SolidColorBrush,
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