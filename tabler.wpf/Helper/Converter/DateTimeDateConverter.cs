using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;

namespace tabler.wpf.Controls.Converter
{
    public class DateTimeDateConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return ((DateTime)value).Date.ToString("yyyyy-MM-dd");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return null;
            }
            return value;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
