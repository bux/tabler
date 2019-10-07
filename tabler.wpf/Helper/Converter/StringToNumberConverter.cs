using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace tabler.wpf.Controls.Converter
{
    public class StringToNumberConverter : MarkupExtension, IValueConverter

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
            
            var s = value as string;
            if (s != null)
            {
                if (double.TryParse(s, NumberStyles.Any, new NumberFormatInfo() {NumberDecimalSeparator = ",", PercentDecimalSeparator = ",", CurrencyDecimalSeparator = ","}, out valueDbl))
                {}
            }

            if (valueDbl == 0)
            {
                return string.Empty;
                return DependencyProperty.UnsetValue;
            }

            return valueDbl;
            
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
            return null;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
