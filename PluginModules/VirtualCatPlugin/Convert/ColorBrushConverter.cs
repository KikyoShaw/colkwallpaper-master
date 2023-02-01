using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace VirtualCatPlugin.Convert
{
    public class ColorBrushConverter : IValueConverter
    {
        private static SolidColorBrush _defaultBrush;

        public ColorBrushConverter()
        {
            try
            {
                _defaultBrush = new SolidColorBrush(Color.FromArgb(0xff, 0x34, 0x34, 0x34));
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

            return _defaultBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
