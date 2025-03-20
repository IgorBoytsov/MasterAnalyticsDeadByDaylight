using DBDAnalytics.Application.DTOs;
using System.Globalization;
using System.Windows.Data;

namespace DBDAnalytics.WPF.ValueConverters
{
    class CountMatchToStringConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int countKillerMatches && values[1] is KillerDTO selectedKiller)
            {
                if (selectedKiller.IdKiller == 0)
                    return $"Количество матчей за всех убийц - {countKillerMatches}";
                else
                    return $"Количество матчей за персонажа {selectedKiller.KillerName} - {countKillerMatches}";
            }

            if (values[0] is int countSurvivorMatches && values[1] is SurvivorDTO selectedSurvivor)
            {
                if (selectedSurvivor.IdSurvivor == 0)
                    return $"Количество матчей за всех выживших - {countSurvivorMatches}";
                else
                    return $"Количество матчей за персонажа {selectedSurvivor.SurvivorName} - {countSurvivorMatches}";
            }

            return $"Ошибка, такие значения не обрабатываются.";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}