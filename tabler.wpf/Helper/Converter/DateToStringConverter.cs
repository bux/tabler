using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
using tabler.wpf.Helper;

namespace tabler.wpf.Controls.Converter
{
    public class DateToStringConverter : MarkupExtension, IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime)
            {
                try
                {
                    return ((DateTime)value).GetDateTimeString_yyyyMMddhhmmssms();
                }
                catch (Exception)
                {
                    return "error! DateToStringConverter value: " + value;
                }
            }

            return null;
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
