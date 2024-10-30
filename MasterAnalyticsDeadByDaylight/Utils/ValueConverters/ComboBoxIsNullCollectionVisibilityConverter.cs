using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class ComboBoxIsNullCollectionVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int CountItemCollection)
            {
                return CountItemCollection switch
                {
                    0 => false,
                    _ => true,
                };
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}