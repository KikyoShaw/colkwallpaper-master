using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ImagePluginModule.Convert
{
    public class BoolToVisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null)
                    return false;

                bool bVisible = bool.Parse(value.ToString());
                if (parameter !=null && parameter.ToString() == "-")
                    return bVisible ? Visibility.Collapsed : Visibility.Visible;

                return bVisible ? Visibility.Visible : Visibility.Collapsed;
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("BoolToVisibleConvert " + e1.Message);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class CompareToBoolConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && parameter != null)
                {
                    return value.ToString() == parameter.ToString() ? true : false;
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("CompareToVisibleConvert " + e1.Message);
            }
            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class CompareToVisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && parameter != null)
                {
                    return value.ToString() == parameter.ToString() ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("CompareToVisibleConvert " + e1.Message);
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class UnCompareToVisibleConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && parameter != null)
                {
                    return value.ToString() != parameter.ToString() ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("CompareToVisibleConvert " + e1.Message);
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
