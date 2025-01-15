using MasterAnalyticsDeadByDaylight.MVVM.Model.MSSQL_DB;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class CharacterUseOfferingVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //return value is Offering currentOffering && parameter is string param && ((currentOffering.IdRole == 2 && param == "Killer") || (currentOffering.IdRole == 3 && param == "Survivor") || currentOffering.IdRole == 5) ? Visibility.Visible : Visibility.Collapsed;
            if (value is Offering currentOffering && parameter is string param)
            {
                if (currentOffering.IdRole == 2 && param == "Killer")
                {
                    return Visibility.Visible;
                }
                if (currentOffering.IdRole == 3 && param == "Survivor")
                {
                    return Visibility.Visible;
                }
                if (currentOffering.IdRole == 5)
                {
                    return Visibility.Visible;
                }
                else
                {
                    return Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
