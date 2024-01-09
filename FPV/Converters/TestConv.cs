﻿using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace HMI.Converter
{
    [ValueConversion(typeof(object), typeof(object))]
    public class TestConv : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return true;
                else
                    return false;
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
}