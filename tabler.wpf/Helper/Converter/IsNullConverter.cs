using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Markup;
namespace tabler.wpf.Controls.Converter
{
    public class LIsNullConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {

            if (value == null)
            {
                return true;
            }

            var isString = value is string;

            if (isString && string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }
            
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return true;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
