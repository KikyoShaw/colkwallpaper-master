using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DefaultPluginModule
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class EffectConfigValue
    {

    }
    public partial class EffectView :  UserControl
    {
        int _monitorIndex = 0; //用来设置
        private ViewModel.EffectViewModel vm = null;
        private bool _start = false;
        double _timeTick = 0;
        private string _dllPath = "";
        private string _defaultCfgPath = "";
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public Thickness _thickness = new Thickness(0,0,0,0);
        public Thickness _diythickness = new Thickness(0,0,0,0);
        public Timer _timer = null;
        private int _diyHovPos = 0;
        private int _diyVerPos = 0;
        private int _HovPos = 0;
        private int _VerPos = 0;
        private int _dateFomrat = 0;
        private int _timeFomrat = 0;
        private string _sDateFomrat = "";
        private string _sTimeFomrat = "";
        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;

            _timer = new Timer(1000);
            _timer.Elapsed += OnTimer;
            //_timer.Start();

            this.Loaded += EffectView_Loaded;
            this.Unloaded += EffectView_Unloaded;
        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                _timer.Start();
                this.Visibility = Visibility.Visible;

            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _timer.Stop();
                this.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                vm.CurDateTime = DateTime.Now;
                UpdateDateTime();
            });
        }

        internal void NotifyEffect(Dictionary<string, object> cfg)
        {
            try
            {
                if (cfg == null && cfg.Count == 0)
                    return;
                string comand = cfg["command"].ToString();
                switch (comand)
                {
                    case "init": //开始
                        {
                            if (cfg.ContainsKey("infos"))
                            {
                                _defaultCfgPath = cfg["infos"].ToString();
                            }                            
                        }
                        break; ;
                    case "start": //开始
                        Start();
                        break;
                    case "stop":  //停止
                        Stop();
                        break;
                    case "exit":  //退出
                        Stop();
                        break;
                    case "config": //打开配置
                        ShowConfig();
                        break;
                    case "switch": //切换
                        {
                            if (cfg.ContainsKey("path"))
                            {
                                _defaultCfgPath = cfg["path"].ToString();
                                _effectConfig.LoadConfigPath(_defaultCfgPath);
                                RefreshEffect();
                            }
                        }
                        break;
                }
            }
            catch (Exception ex)
            {
                int i = 0;
                //LogHelper.LogError(ex.ToString());
            }
        }

        private void ShowConfig()
        {
            if (_effectConfig == null || _effectConfig.items.Count == 0)
            {
                MessageBox.Show("此特效没设置选项");                
                return;
            }
            if (_configWindow != null)
            {
                _configWindow.Close();
                _configWindow = null;
            }

            if (_configWindow == null)
            {
                _configWindow = new EffectConfigModule.ConfigWindow();
                _configWindow.actValueChange += OnConfigValueChange;
                _configWindow.SetDataContext(_effectConfig);
            }
            _configWindow.Closing += ((s, e) =>
            {
            });

            _configWindow.Show();
        }

        static string[] weekFormat = {
             "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday",
             "Sun.", "Mon.", "Tue.", "Wed.", "Thu.", "Fri.", "Sat.",
             "星期日", "星期一", "星期二", "星期三", "星期四", "星期五", "星期六",
        };
        static string[] AnteFormat = {
             "AM", "PM",
            "Am", "Pm",
            "am", "pm",
            "A.", "P.",
            "上午", "下午" ,
        };
        private void UpdateDateTime()
        {
            string date = "", time = "";
            DateTime dt = (DateTime)vm.CurDateTime;
            if (vm.bShowDate)
            {
                string fm = (string)vm.sDateFormat;
                if (dt != null)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(fm))
                            fm = "yyyy/MM/dd";
                        date = vm.CurDateTime.ToString(fm);
                    }
                    catch { }
                }

                //week
                if (vm.bShowWeek)
                {
                   
                       
                        date += ", ";
                        int week = (int)dt.DayOfWeek;
                        switch (vm.iWeekFormat)
                        {
                            case 1:
                                date += weekFormat[week];
                                break;
                            case 2:
                                date += weekFormat[week + 7];
                                break;
                            case 3:
                                date += weekFormat[week + 7 * 2];
                                break;
                            default:
                                date += dt.ToString("dddd");
                                break;
                        }

                }
            }
            if(vm.bShowTime)
            {
                string fm = (string)vm.sTimeFormat;
                if (dt != null)
                {
                    try
                    {
                        if (string.IsNullOrEmpty(fm))
                            fm = "HH:mm:ss";
                        time = dt.ToString(fm);
                    }
                    catch { }
                }

                //week
                if (vm.bShowAnte)
                //week
                {
                    string ante = "";
                    int at = (int)dt.Hour > 12 ? 1 : 0;
                    switch (vm.iAnteFomrat)
                    {
                        case 1:
                            ante += AnteFormat[at];
                            break;
                        case 2:
                            ante += AnteFormat[at + 2];
                            break;
                        case 3:
                            ante += AnteFormat[at + 2 * 2];
                            break;
                        case 4:
                            ante += AnteFormat[at + 2 * 3];
                            break;
                        case 5:
                            ante += AnteFormat[at + 2 * 4];
                            break;
                        default:
                            ante += dt.ToString("tt");
                            break;
                    }

                    if (vm.bFirstShowAnte)
                    {
                        time = ante + " " + time;
                    }
                    else
                    {
                        time = time + " " + ante;
                    }
                }
            }
            if(vm.bFirstShowTime)
            {
                vm.sShowDateTime1 = time;
                vm.sShowDateTime2 = date;
            }
            else
            {
                vm.sShowDateTime2 = time;
                vm.sShowDateTime1 = date;

            }
            
        }
        private void SetDiyPosition()
        {

            double hov = ((double)_diyHovPos / 500 * (this.ActualWidth - DiyTxt.ActualWidth));
            double vec = ((double)_diyVerPos / 500.0 * (this.ActualHeight - DiyTxt.ActualHeight));
            _diythickness.Left = hov;
            _diythickness.Top = vec;
            vm.ShowDiyMargin = _diythickness;
        }
        private void SetPluginPosition()
        {
            UpdateDateTime();
            double hov = ((double)_HovPos / 500 * (this.ActualWidth - plugInfo.ActualWidth)) ;
            double vec = ((double)_VerPos / 500.0 * (this.ActualHeight - plugInfo.ActualHeight)) ;
            _thickness.Left = hov;
            _thickness.Top = vec;
            vm.ShowMargin = _thickness;
        }
        private void OnConfigValueChange(string key, string value)
        {

            bool updatediyPostion = false;
            bool updatePostion = false;
            switch (key)
            {
                case "DiyHorizontalOffset":
                    {
                        _diyHovPos = int.Parse(value);
                        updatediyPostion = true;
                        //vm.ShowMargin = _thickness;
                    }
                    break;
                case "DiyVerticalOffset":
                    {
                        _diyVerPos = int.Parse(value);
                        updatediyPostion = true;
                    }
                    break;
                case "HorizontalOffset":
                    {
                        _HovPos = int.Parse(value);
                        updatePostion = true;
                        //vm.ShowMargin = _thickness;
                    }
                    break;
                case "VerticalOffset":
                    {
                        _VerPos = int.Parse(value);
                        updatePostion = true;
                    }
                    break;
                case "ShowDiyText":
                    {
                        vm.bShowDiyText = bool.Parse(value);
                        DiyTxt.UpdateLayout();
                        diyInfo.UpdateLayout();
                        updatediyPostion = true;
                    }
                    break;
                case "ShowDate":
                    {
                        vm.bShowDate = bool.Parse(value);
                        DateTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "ShowTime":
                    {
                        vm.bShowTime = bool.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "DiyText":
                    {
                        vm.sDiyText = value;
                        DiyTxt.UpdateLayout();
                        diyInfo.UpdateLayout();
                        updatediyPostion = true;
                    }
                    break;
                case "DiyShowFont":
                    {
                        vm.showDiyfont = vm.FontConverter.ConvertFromString(value) as System.Drawing.Font;
                        DiyTxt.UpdateLayout();
                        diyInfo.UpdateLayout();
                        updatediyPostion = true;
                    }
                    break;
                case "ShowFont":
                    {
                        vm.showfont = vm.FontConverter.ConvertFromString(value) as System.Drawing.Font;
                        DateTxt.UpdateLayout();
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "DiyFontColor":
                    {
                        vm.txtColor = value;
                    }
                    break;
                case "DateFontColor":
                    {
                        vm.DateColor = value;
                    }
                    break;
                case "TimeFontColor":
                    {
                        vm.TimeColor = value;
                    }
                    break;
                case "DiyFontSize":
                    {
                        vm.fDiyFontSize = (float.Parse(value));
                        DiyTxt.UpdateLayout();
                        diyInfo.UpdateLayout();
                        updatediyPostion = true;
                    }
                    break;
                case "DateFontSize":
                    {
                        vm.fDateFontSize = (float.Parse(value));
                        DateTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "TimeFontSize":
                    {
                        vm.fTimeFontSize = (float.Parse(value));
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "FontOpacity":
                    {
                        vm.iOpacity = 100 - int.Parse(value);
                    }
                    break;
                case "DiyFontOpacity":
                    {
                        vm.iDiyOpacity = 100 - int.Parse(value);
                    }
                    break;
                case "ShadowColor":
                    {
                        vm.ShadowColor = value;
                    }
                    break;
                case "ShadowDirection":
                    {
                        vm.iShadowDirection = int.Parse(value);
                    }
                    break;
                case "ShadowDepth":
                    {
                        vm.iShadowDepth = int.Parse(value);
                    }
                    break;
                case "ShadowOpacity":
                    {
                        vm.iShadowOpacity = 100 - int.Parse(value) ;
                    }
                    break;
                case "ShadowBlurRadius":
                    {
                        vm.iShadowBlurRadius = int.Parse(value) ;
                    }
                    break;
                case "DateFomrat":
                    {
                        _dateFomrat =  int.Parse(value);

                        switch (_dateFomrat)
                        {
                            case 1:
                                vm.sDateFormat = "yyyy/MM/dd";
                                break;

                            case 2:
                                vm.sDateFormat = @"yyyy-MM-dd";
                                break;
                            case 3:
                                vm.sDateFormat = @"yyyy年MM月dd日";
                                break;

                            case 4:
                                vm.sDateFormat = "MM/dd";
                                break;

                            case 5:
                                vm.sDateFormat = @"MM-dd";
                                break;
                            case 6:
                                vm.sDateFormat = @"MM月dd日";
                                break;
                            default:
                                vm.sDateFormat = _sDateFomrat;
                                break;

                        }
                        DateTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "TimeFomrat":
                    {
                        _timeFomrat = int.Parse(value);

                        switch (_timeFomrat)
                        {
                            case 1:
                                vm.sTimeFormat = "HH:mm:ss";
                                _timer.Interval = 1000;
                                break;

                            case 2:
                                vm.sTimeFormat = @"h:mm:ss";
                                _timer.Interval = 1000;
                                break;
                            case 3:
                                vm.sTimeFormat = @"HH:mm";
                                _timer.Interval = 10000;
                                break;
                            case 4:
                                vm.sTimeFormat = @"h:mm";
                                _timer.Interval = 10000;
                                break;
                            default:
                                vm.sTimeFormat = _sTimeFomrat;
                                _timer.Interval = 1000;
                                break;

                        }
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "DateFomratText":
                    {
                        _sDateFomrat = value;
                        if (_dateFomrat == 0)
                        {
                            vm.sDateFormat = _sDateFomrat;
                            DateTxt.UpdateLayout();
                            plugInfo.UpdateLayout();
                            updatePostion = true;
                        }
                    }
                    break;
                case "TimeFomratText":
                    {
                        _sTimeFomrat = value;
                        if (_timeFomrat == 0)
                        {
                            vm.sTimeFormat = _sTimeFomrat;
                            TimeTxt.UpdateLayout();
                            plugInfo.UpdateLayout();
                            updatePostion = true;
                        }
                    }
                    break;
                case "ShowWeek":
                    {
                        {

                            vm.bShowWeek = bool.Parse(value);
                            TimeTxt.UpdateLayout();
                            plugInfo.UpdateLayout();
                            updatePostion = true;
                        }
                    }
                    break;
                case "WeekFomrat":
                    {
                        vm.iWeekFormat = int.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "ShowAnte":
                    {

                        vm.bShowAnte = bool.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "AnteFomrat":
                    {
                        vm.iAnteFomrat = int.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "FirstTime":
                    {
                        vm.bFirstShowTime = bool.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
                case "FirstAnte":
                    {
                        vm.bFirstShowAnte = bool.Parse(value);
                        TimeTxt.UpdateLayout();
                        plugInfo.UpdateLayout();
                        updatePostion = true;
                    }
                    break;
            }
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (updatePostion)
                    SetPluginPosition();
                if (updatediyPostion)
                    SetDiyPosition();
            });
        }

        internal void MouseEvent(List<int> mouseEvent)
        {
            try
            {
                if (mouseEvent == null && mouseEvent.Count < 5)
                    return;
            }
            catch { }
        }

        private void EffectView_Unloaded(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch { }
            //vm._cleanState(); 
        }

        private void EffectView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                string str =$@"pack://application:,,,/DefaultPluginModule;Component/Resources/config.json";


                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "DefaultPluginModule_v1.0", _monitorIndex, 1);
                        
                        _effectConfig.LoadConfigPath(_defaultCfgPath);

                    }
                }
                
            }
            catch { }
        }
        
        private void RefreshEffect()
        {
            
            foreach(var item in _effectConfig.items)
            {
                string value = (item.GetValue());
                string key = (item.key);
                OnConfigValueChange(key, value);
            }

            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                SetPluginPosition();
            });
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                RefreshEffect();
                SetPluginPosition(); 
            });
        }
    }
}
