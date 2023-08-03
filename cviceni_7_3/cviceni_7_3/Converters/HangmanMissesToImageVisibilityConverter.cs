using System;
using System.Globalization;
using Xamarin.Forms;

namespace cviceni_7_3
{
    internal class HangmanMissesToImageVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int misses)
            {
                if (int.TryParse(parameter?.ToString(), out int requiredMisses))
                    return misses >= requiredMisses;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
