using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PhotoOrg
{
    public class MarginConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double dispImageWidth)
            {
                double margin = dispImageWidth + 100; // Calculate the desired margin based on the width of DispImage
                return new Thickness(margin, 10, 0, 0); // Return the margin as a Thickness object
            }

            return new Thickness(10, 10, 0, 0); // Default margin if the value is not a valid width
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
