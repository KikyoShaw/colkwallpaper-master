using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;

namespace ImagePluginModule.Convert
{
    public class TimeStringConvert : IMultiValueConverter
    {
        static string[] format = {
             "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday",
             "Sun.", "Mon.", "Tue.", "Wed.", "Thu.", "Fri.", "Sat.", 
             "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六", 
        };
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string ret = "";
            try
            {
                //两个参数值相等就显示
                if (values[0] != null && values[1] != null && values[0] is DateTime)
                {
                    DateTime dt = (DateTime)values[0];
                    string fm = (string)values[1];
                    if (dt != null)
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(fm))
                                fm = "HH:mm:ss";
                            ret = dt.ToString(fm);
                        }
                        catch { }
                    }
                    //week
                    if (values.Count() >= 3 && values[2] != null )
                    {
                        bool isShowWeek = bool.Parse(values[2].ToString());
                        if(isShowWeek)
                        {

                            int weekFormatType = 0;
                            if (values.Count() >= 4 && values[3] != null)
                            {
                                weekFormatType = int.Parse(values[3].ToString());
                            }
                            ret += ", ";
                            int week = (int)dt.DayOfWeek;
                            switch (weekFormatType)
                            {
                                case 1:
                                    ret += format[week];
                                    break;
                                case 2:
                                    ret += format[week + 7];
                                    break;
                                case 3:
                                    ret += format[week + 7 * 2];
                                    break;
                                default:
                                    ret += dt.ToString("dddd");
                                    break;
                            }

                        }
                    }
                }
            }
            catch { }
            return ret;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }

    }
}