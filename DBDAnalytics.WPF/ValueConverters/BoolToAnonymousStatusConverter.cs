using System.Globalization;
using System.Windows.Data;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class BoolToAnonymousStatusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool anonymous)
            {
                if (anonymous)
                    return "Анонимный";
                else
                    return "Не анонимный";
            }
           
            return "Необработанный тип";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}