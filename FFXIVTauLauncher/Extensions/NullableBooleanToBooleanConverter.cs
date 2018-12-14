using System;
using System.Globalization;
using System.Windows.Data;

namespace FFXIVTauLauncher.Extensions
{
    public class NullableBooleanToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is bool b)
            { return b; }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo language)
        {
            if (value is bool b)
            { return b; }
            return false;
        }
    }
}