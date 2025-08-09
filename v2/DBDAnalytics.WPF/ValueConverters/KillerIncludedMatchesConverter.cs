using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class KillerIncludedMatchesConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool existKillerInMatches)
            {
                if (existKillerInMatches)
                    return System.Windows.Application.Current.Resources["Brush.ComboBox.ExistKillerInMatches.CheckMark"] as SolidColorBrush;
                else
                    return System.Windows.Application.Current.Resources["Brush.ComboBox.NoExistKillerInMatches.CheckMark"] as SolidColorBrush;
            }
            return Brushes.Transparent;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}