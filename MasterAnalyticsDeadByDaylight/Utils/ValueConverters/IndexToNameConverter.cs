﻿using System.Globalization;
using System.Windows.Data;

namespace MasterAnalyticsDeadByDaylight.Utils.ValueConverters
{
    class IndexToNameConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int index && values[1] is string name)
            {
                return $"№ {index} - {name}";
            }
            return string.Empty;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
