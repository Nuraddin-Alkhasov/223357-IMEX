﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI.Converter
{
    [ValueConversion(typeof(object), typeof(Visibility))]
    public class BoolToEnConverter0 : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return false;
                else 
                    return true;
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }
}