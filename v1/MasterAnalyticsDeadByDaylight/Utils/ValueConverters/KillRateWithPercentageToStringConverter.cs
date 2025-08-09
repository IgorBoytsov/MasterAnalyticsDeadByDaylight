using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class KillRateWithPercentageToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is double killRatePercentage && values[1] is double killRate)
            {
                return $"Киллрейт: {killRate} ( {killRatePercentage}% )";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
