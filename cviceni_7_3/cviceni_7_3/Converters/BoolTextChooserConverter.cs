using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Xamarin.Forms;

namespace cviceni_7_3
{
    internal class BoolTextChooserConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (parameter.GetType() == typeof(string[]) && ((string[])parameter).Length == 2)
            {
                if (value is true)
                    return ((string[])parameter)[0];
                else
                    return ((string[])parameter)[1];
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
