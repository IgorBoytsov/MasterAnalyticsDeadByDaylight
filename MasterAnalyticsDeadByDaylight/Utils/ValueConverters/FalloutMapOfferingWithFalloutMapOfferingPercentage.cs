using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class FalloutMapOfferingWithFalloutMapOfferingPercentage : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int falloutMapOfferingCount && values[1] is double falloutMapOfferingPercent)
            {
                return $"С подношениями: {falloutMapOfferingCount} ( {falloutMapOfferingPercent} % )";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
