using System.Globalization;
using System.Windows.Data;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class BoolToBotStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool bot)
            {
                if (bot)
                    return "Ливнул";
                else
                    return "Не ливнул";
            }

            return "Необработанный тип";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}