using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using tabler.Logic.Helper;

namespace tabler.wpf.Controls.Converter
{
    public class BooleanToVisibilityConverter : MarkupExtension, IValueConverter

    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool bValue = false;
                if (value is bool)
                {
                    bValue = (bool)value;
                }

                if (parameter != null && (string)parameter == "true")
                {
                    bValue = !bValue;
                }

                return bValue ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                Logger.LogError($"{nameof(BooleanToVisibilityConverter)}.{nameof(Convert)} Exception: {ex}");
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
            if (value is Visibility)
            {
                return (Visibility)value == Visibility.Visible;
            }
            else
            {
                return false;
            }
        }
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
