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
using tabler.wpf.Container;

namespace tabler.wpf.Helper.Converter
{
    public class ValueToBrushConverter : MarkupExtension, IValueConverter

    {
        public static readonly IValueConverter Instance = new ValueToBrushConverter();
        private readonly static SolidColorBrush _textMissing = new SolidColorBrush(Colors.Orange);
        private readonly static SolidColorBrush _textOk = new SolidColorBrush(Colors.LightGreen);
        private readonly static SolidColorBrush _textChanged = new SolidColorBrush(Colors.LightYellow);
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            { 
                var cell = value as DataGridCell;

                var item = cell.DataContext as Key_ExtendedWithChangeTracking;
                if (item != null )
                {
                    ChangeTrackerString valueString;
                    if (item.Languages.TryGetValue((string)cell.Column.Header, out valueString))
                    {
                        if (string.IsNullOrEmpty(valueString.CurrentValue))
                        {
                            return _textMissing;
                        }
                        if (valueString.HasChanged)
                        {
                            return _textChanged;
                        }

                        return _textOk;
                    }
                }
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
