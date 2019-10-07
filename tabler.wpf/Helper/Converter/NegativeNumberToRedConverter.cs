using System;
using System.Drawing;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace tabler.wpf.Controls.Converter
{
    public class NegativeNumberToRedConverter : MarkupExtension, IValueConverter

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

            if (valueDbl == 0 )
            {
                return DependencyProperty.UnsetValue;
            }

            if (valueDbl > 0 )
            {
                return Brushes.MediumSeaGreen;
            }

            if (valueDbl < 0)
            {
                return Brushes.Orange;
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
