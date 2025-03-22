using DBDAnalytics.WPF.Enums;
using System.Globalization;
using System.Windows.Data;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class StatusMatchToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int countKillsOrTypeDeath && parameter is StatusMatchConverter statusEnum)
            {
                if (statusEnum == StatusMatchConverter.CountKills)
                {
                    return countKillsOrTypeDeath switch
                    {
                        0 => "Поражение",
                        1 => "Поражение",
                        2 => "Ничья",
                        3 => "Победа",
                        4 => "Победа",
                        _ => value.ToString(),
                    };
                }
                if (statusEnum == StatusMatchConverter.TypeDeath)
                {
                    return countKillsOrTypeDeath switch
                    {
                        1 => "Умер от крюка",
                        2 => "Умер от земли",
                        3 => "Умер от мементо",
                        4 => "Умер от способности",
                        5 => "Сбежал",
                        _ => value.ToString()
                    };
                }
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}