using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace WeatherPluginModule.Convert
{

    public class FontNameConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is System.Drawing.Font )
                {
                    return new System.Windows.Media.FontFamily(((System.Drawing.Font)value).FontFamily.Name);
                }

            }
            catch { }

            return new System.Windows.Media.FontFamily("Microsoft YaHei UI");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class FontSizeConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is System.Drawing.Font)
                {
                    return ((System.Drawing.Font)value).Size;
                }

            }
            catch { }

            return 25.0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
    public class FontBoldConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is System.Drawing.Font)
                {
                    return ((System.Drawing.Font)value).Bold ? FontWeights.Bold : FontWeights.Normal;
                }

            }
            catch { }

            return FontWeights.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }

    public class FontItalicConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null && value is System.Drawing.Font)
                {
                    return ((System.Drawing.Font)value).Italic ? FontStyles.Italic : FontStyles.Normal;
                }

            }
            catch { }

            return FontStyles.Normal;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}