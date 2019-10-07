using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace tabler.wpf.Helper.Converter
{
    public class CompleteConverter : IValueConverter
    {
        // This converter changes the value of a Tasks Complete status from true/false to a string value of
        // "Complete"/"Active" for use in the row group header.
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            bool complete = (bool)value;
            if (complete)
                return "Translation Complete";
            else
                return "Translation unfinished";
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            string strComplete = (string)value;
            if (strComplete == "Complete")
                return true;
            else
                return false;
        }
    }
}
