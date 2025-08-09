using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class StatusMatchIntToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int countKills)
            {
                return countKills switch
                {
                    0 => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"), // Красный
                    1 => (Brush)new BrushConverter().ConvertFrom("#FFC41D1D"), // Красный
                    2 => (Brush)new BrushConverter().ConvertFrom("#FFD9C10B"), // Желтый
                    3 => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"), // Зеленый
                    4 => (Brush)new BrushConverter().ConvertFrom("#FF1FA818"), // Зеленый
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
