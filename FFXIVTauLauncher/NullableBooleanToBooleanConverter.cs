using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace FFXIVTauLauncher
{
    public class NullableBooleanToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            { return b; }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is bool b)
            { return b; }
            return false;
        }
    }
}