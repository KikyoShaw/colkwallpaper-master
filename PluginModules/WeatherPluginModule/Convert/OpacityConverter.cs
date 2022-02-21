using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace WeatherPluginModule.Convert
{
    public class OpacityConvert : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value == null || value.ToString() == "")
                {
                    return 0;
                }

                int mode = System.Convert.ToInt32(value);
                double opacity =  1.0 - ((double)mode / 100.0);
                return opacity;
            }
            catch { }

            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
