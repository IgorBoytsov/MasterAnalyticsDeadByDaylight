using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class StatusMatchIntToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
           if (value is int countKills)
            {
                return countKills switch
                {
                    0 => "Поражение",
                    1 => "Поражение",
                    2 => "Ничья",
                    3 => "Победа",
                    4 => "Победа",
                    _ => value.ToString(),
                };
            }
           return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
