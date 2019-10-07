using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using tabler.Logic.Helper;

namespace tabler.wpf.Helper.Converter
{
    public class ValueToBrushConverter : MarkupExtension, IValueConverter

    {
        public static readonly IValueConverter Instance = new ValueToBrushConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            { 
                var cell = value as DataGridCell;
                if (cell == null)
                {
                    //return DependencyProperty.UnsetValue; 
                    return new SolidColorBrush(Colors.Orange);
                }

                if (cell.Content  == null)
                {
                   // return DependencyProperty.UnsetValue;
                    return new SolidColorBrush(Colors.Beige);

                }

                var tb = cell.Content as TextBlock;
                if (tb == null)
                {
                    return DependencyProperty.UnsetValue;
                    //return new SolidColorBrush(Colors.Beige);
                }

                if (!string.IsNullOrEmpty(tb.Text))
                {
                    return new SolidColorBrush(Colors.Green);
                    //return DependencyProperty.UnsetValue;
                }
                return new SolidColorBrush(Colors.YellowGreen);
               // return DependencyProperty.UnsetValue;

            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(ValueToBrushConverter)}.{nameof(Convert)} Exception: {ex}");
            }

            return Visibility.Visible;
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
            return false;
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
