using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace tabler.wpf.Controls.Converter
{
    public class DoubleToStringConverterNoScientific : MarkupExtension, IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return string.Empty;
            }
            double valueDbl = 0;

            if (value is double)
            {
                valueDbl = (double)value;
            }
            if (value is int)
            {
                valueDbl = (int)value;
            }
            if (value is long)
            {
                valueDbl = (long)value;
            }

            return valueDbl.ToString(".################");
        }

        /// <summary>
        /// Convert Visibility to boolean 
        /// </summary>
        /// <param name="value"></param> 
        /// <param name="targetType"></param> 
        /// <param name="parameter"></param>
        /// <param name="culture"></param> 
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            double change = 0;
            double.TryParse((string)value, NumberStyles.Any, new NumberFormatInfo() { NumberDecimalSeparator = ",", PercentDecimalSeparator = ",", CurrencyDecimalSeparator = "," }, out change);
            return change;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
