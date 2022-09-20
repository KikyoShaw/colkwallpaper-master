using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ImagePluginModule.Convert
{
    public class MulEqualToVisibleConvert : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                //两个参数值相等就显示
                if (values[0] != null && values[1] != null)
                {
                    if (values[0].ToString() == values[1].ToString()) 
                    {
                        return Visibility.Visible;
                    }
                }                
            }
            catch { }
            return Visibility.Collapsed;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
