using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class PageLimitConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int currentPage && values[1] is int totalPages &&
                values[2] is string action)
            {
                if (action == "Предыдущая")
                {
                    return currentPage > 1;
                }
                else if (action == "Следующая")
                {
                    return currentPage < totalPages;
                }
            }
            return false;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
