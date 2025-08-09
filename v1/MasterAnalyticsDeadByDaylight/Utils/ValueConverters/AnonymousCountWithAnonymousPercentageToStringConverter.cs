using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class AnonymousCountWithAnonymousPercentageToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int anonymousCount && values[1] is int allSurvivorCount && values[2] is double percent)
            {
                return $"Анонимных: {anonymousCount} из {allSurvivorCount} ({percent}%)";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
