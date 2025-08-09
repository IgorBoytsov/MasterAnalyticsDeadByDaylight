using System.Globalization;
using System.Windows.Data;

namespace DBDAnalytics.WPF.ValueConverters
{
    internal class NumberDigitConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double number)
            {
                CultureInfo customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
                customCulture.NumberFormat.NumberGroupSeparator = " "; 

                string formatString = "#,##0.################";

                string formattedNumber = number.ToString(formatString, customCulture);

                if (formattedNumber == "-0")
                {
                    return "0";
                }

                return formattedNumber;
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}