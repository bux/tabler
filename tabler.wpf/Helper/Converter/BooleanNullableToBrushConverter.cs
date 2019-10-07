using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;
using tabler.Logic.Helper;

namespace tabler.wpf.Controls.Converter
{
    public class BooleanNullableToBrushConverter : MarkupExtension, IValueConverter

    {
        public static readonly IValueConverter Instance = new BooleanNullableToBrushConverter();
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                bool? bValue = null;
                if (value is bool)
                {
                    bValue = (bool)value;
                }
                else
                {
                    var ut = Nullable.GetUnderlyingType(targetType);
                    if (ut != null && ut == typeof(bool))
                    {
                        bValue = (bool?)value;
                    }
                    else
                    {
                        var s = value as string;
                        if (s != null)
                        {
                            var valueString = s.ToLowerInvariant();
                            if (valueString == "true")
                            {
                                bValue = true;
                            }
                            if (valueString == "false")
                            {
                                bValue = false;
                            }
                        }
                        else
                        {
                            return DependencyProperty.UnsetValue;
                        }
                    }
                }

                if (!bValue.HasValue)
                {
                    return DependencyProperty.UnsetValue;
                }

                if (bValue.Value)
                {
                    return new SolidColorBrush(Colors.ForestGreen);
                }
                return new SolidColorBrush(Colors.Orange);
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
