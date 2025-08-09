using DBDAnalytics.WPF.Enums;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class StatusMatchToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int countKillsOrTypeDeath && parameter is StatusMatchConverter statusEnum)
            {
                if (statusEnum == StatusMatchConverter.CountKills)
                {
                    return countKillsOrTypeDeath switch
                    {
                        0 => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"), // Красный
                        1 => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"), // Красный
                        2 => (Brush)new BrushConverter().ConvertFrom("#FFD9C10B"), // Желтый
                        3 => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"), // Зеленый
                        4 => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"), // Зеленый
                        _ => (Brush)new BrushConverter().ConvertFrom("#FF020101"), // Черный,
                    };
                }
                if (statusEnum == StatusMatchConverter.TypeDeath)
                {
                    return countKillsOrTypeDeath switch
                    {
                        1 => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"), // Красный,
                        2 => (Brush)new BrushConverter().ConvertFrom("#FFBE6B24"), // Коричневый,
                        3 => (Brush)new BrushConverter().ConvertFrom("#FFF11313"), // Ярко красный,
                        4 => (Brush)new BrushConverter().ConvertFrom("#FF630F0F"), // Бордовый ( Темно красный ),
                        5 => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"), // Зеленый,
                        _ => (Brush)new BrushConverter().ConvertFrom("#FF020101"), // Черный,
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