﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI.Converter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class BoolToVisConverter1 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return Visibility.Visible;
                else 
                    return Visibility.Hidden;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}
