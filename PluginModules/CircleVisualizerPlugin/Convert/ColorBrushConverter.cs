using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media;

namespace CircleVisualizerPlugin.Convert
{
    public class ColorBrushConverter : IValueConverter
    {
        private static SolidColorBrush defaultBrush;

        public ColorBrushConverter()
        {
            try
            {
                defaultBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x34, 0x34, 0x34));
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("StringToBrushConvert " + e1.Message.ToString());
            }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                if (value != null)
                {
                    string strVal = value.ToString();
                    if (strVal.Length > 0)
                    {
                        BrushConverter brushConverter = new BrushConverter();
                        SolidColorBrush solidbrush = new SolidColorBrush();
                        solidbrush = brushConverter.ConvertFromString(strVal) as SolidColorBrush;

                        return solidbrush;
                    }
                }
            }
            catch (Exception e1)
            {
                System.Diagnostics.Debug.WriteLine("StringToBrushConvert" + e1.Message);
            }

            return defaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
