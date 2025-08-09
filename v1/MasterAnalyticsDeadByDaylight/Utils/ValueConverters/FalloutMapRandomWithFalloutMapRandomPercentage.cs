using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class FalloutMapRandomWithFalloutMapRandomPercentage : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int falloutMapRandomCount && values[1] is double falloutMapRandomPercent)
            {
                return $"Без подношений: {falloutMapRandomCount} ( {falloutMapRandomPercent} % )";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
