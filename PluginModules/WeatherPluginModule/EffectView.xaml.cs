using CefSharp;
using CefSharp.Wpf;
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

namespace WeatherPluginModule
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
        private bool _delayLoad = false;
        bool dealyReload = false;
        int _lasterror = 0;

        public EffectView(int monitorIndex, string dllPath)
        {
            InitializeCefSettings();
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
        private void InitializeCefSettings()
        {
            //全局下Cef只能被初始化一次
            try
            {
                if (Cef.IsInitialized != true)
                {
                    CefSettings settings = new CefSettings();
                    //允许调用JS函数调用后端代码

                    string appdataDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                    settings.CachePath = System.IO.Path.Combine(appdataDir, @"ColkWallpaper\webviewCache");

                    //CefSharpSettings.LegacyJavascriptBindingEnabled = true;
                    //CefSharpSettings.WcfEnabled = true;
                    //禁用代理设置
                    //settings.CefCommandLineArgs.Add("no-proxy-server", "1");
                    //禁用硬件加速
                    //settings.CefCommandLineArgs.Add("disable-gpu", "0");
                    //settings.CefCommandLineArgs.Add("disable-gpu-compositing", "1");
                    //settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
                    settings.BackgroundColor = 0;

                    settings.CefCommandLineArgs.Add("--disable-gpu-shader-disk-cache");
                    settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
                    settings.CefCommandLineArgs.Add("--disable-web-security", "1");//关闭同源策略,允许跨域
                                                                                   //settings.CefCommandLineArgs.Add("windowless_frame_rate", "60");

                    //string debugPort = "8088";
                    settings.BackgroundColor = Cef.ColorSetARGB(01, 0, 0, 0);
                    //if (!string.IsNullOrWhiteSpace(debugPort))
                    //{
                    //    //example-port: 8088
                    //    if (int.TryParse(debugPort, out int value))
                    //        settings.RemoteDebuggingPort = value;
                    //}


                    Cef.Initialize(settings);


                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                //_shieldMouseInteraction = false;
            }
        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;                
                this.Visibility = Visibility.Visible;
                dealyReload = true;
                _timer.Start();
                //Reload();
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                this.Visibility = Visibility.Collapsed;

                _timer.Stop();
            }
        }

        void Reload()
        {
            if(!_delayLoad)
                return;

            string address = $@"https://i.tianqi.com/?c=code&a=getcode&id={vm.iStyleId}";
            if (!string.IsNullOrEmpty(vm.sCityCode))
            {
                address += $@"&py={vm.sCityCode}";
            }
            if (vm.txtSize != 12)
            {
                address += $@"&site={vm.txtSize}";
            }
            if (!string.IsNullOrEmpty(vm.txtColor))
            {
                string color = vm.txtColor.Replace("#FF", "");
                address += $@"&color={color}";
            }

            if(Cef.IsInitialized)
                Browers.Load(address);
 
        }
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                _timeTick++;
                if (!_delayLoad &&_timeTick >= 1)
                {
                    _delayLoad = true;
                    _timeTick = 0;
                    dealyReload = false;
                    Reload();
                    return;
                }
                else
                {
                    if (dealyReload)
                    {
                        _timeTick = 0;
                        Reload();
                        dealyReload = false;
                    }
                    if (_timeTick > 1000 * 60 * 30)
                    {
                        _timeTick = 0;
                        Reload();
                    }
                }   

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
                    case "saveconfig": //打开配置
                        {
                            if (cfg.ContainsKey("path"))
                            {
                                HYWEffectConfig.EffectConfigTool.SaveConfig(_effectConfig);
                                var path = cfg["path"].ToString();
                                HYWEffectConfig.EffectConfigTool.SaveConfig(_effectConfig, path);
                            }
                        }
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
                
                _configWindow.SetDataContext(_effectConfig);
            }
            _configWindow.Closing += ((s, e) =>
            {
            });

            _configWindow.Show();
        }
        
        private void SetPluginPosition()
        {
            double hov = ((double)_HovPos / 500 * (this.ActualWidth - vm.iShowWidth)) ;
            double vec = ((double)_VerPos / 500.0 * (this.ActualHeight - vm.iShowHeight)) ;
            _thickness.Left = hov;
            _thickness.Top = vec;
            vm.ShowMargin = _thickness;
        }
        private void OnConfigValueChange(string key, string value)
        {

            bool updatePostion = false;
            switch (key)
            {
                case "zIndex":
                    {
                        this.SetValue(Panel.ZIndexProperty, int.Parse(value));
                    }
                    break;

                case "BackColor":
                    {
                        vm.BackColor = value;
                    }
                    break;
                case "BackOpacity":
                    {
                        vm.iBackOpacity = int.Parse(value);
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
                case "ShowWidth":
                    {
                        vm.iShowWidth = int.Parse(value);
                        updatePostion = true;
                    }
                    break;
                case "ShowHeight":
                    {
                        vm.iShowHeight = int.Parse(value);
                        updatePostion = true;
                    }
                    break;
                case "CityCode":
                    {
                        vm.sCityCode = value;
                        dealyReload = true;
                    }
                    break;
                case "StyleId":
                    {
                        vm.iStyleId = int.Parse(value);
                        dealyReload = true;
                    }
                    break;
                case "FontSize":
                    {
                        vm.txtSize = int.Parse(value);
                        dealyReload = true;
                    }
                    break;
                case "FontColor":
                    {
                        vm.txtColor = value;
                        dealyReload = true;
                    }
                    break;
                case "Opacity":
                    {
                        vm.iOpacity = 100 - int.Parse(value);
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
                        vm.iShadowOpacity = 100 - int.Parse(value);
                    }
                    break;
                case "ShadowBlurRadius":
                    {
                        vm.iShadowBlurRadius = int.Parse(value);
                    }
                    break;
            }
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (updatePostion)
                    SetPluginPosition();
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


                Error.Visibility = Visibility.Collapsed;
                Loading.Visibility = Visibility.Visible;

                Browers.LoadingStateChanged += Browser_LoadingStateChanged;
                Browers.ConsoleMessage += Browser_ConsoleMessage;
                Browers.LoadError += Browser_LoadError;
                //Main.Children.Add(_browers);
                //Browers.Address = "https://i.tianqi.com/?c=code&a=getcode&id=94&color=888";

                string str =$@"pack://application:,,,/WeatherPluginModule;Component/Resources/config.json";


                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "WeatherPluginModule_v1.0", _monitorIndex, 1);

                        _effectConfig.SetConfigAction(OnConfigValueChange, RefreshEffect);
                        if (!string.IsNullOrEmpty(_defaultCfgPath))
                            _effectConfig.LoadConfigPath(_defaultCfgPath);
                        else
                            RefreshEffect();

                    }
                }
                
            }
            catch { }
        }
        
        private void RefreshEffect(object obj = null)
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

        
        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {      
            if (e.IsLoading == false)
            {
                if (_lasterror == 0)
                {
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        Loading.Visibility = Visibility.Collapsed;
                        Error.Visibility = Visibility.Collapsed;
                        BrowersGrid.Visibility = Visibility.Visible;
                    });
                    return;
                }
            }
            else
            {
            }

        }

        private void Browser_LoadError(object sender, LoadErrorEventArgs e)
        {
            switch (e.ErrorCode)
            {
                case CefErrorCode.None:
                    break;
                case CefErrorCode.TimedOut:
                case CefErrorCode.ConnectionFailed:
                case CefErrorCode.NameNotResolved:
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        _lasterror = (int)e.ErrorCode;
                        //Error.Visibility = Visibility.Collapsed;
                        //BrowersGrid.Visibility = Visibility.Collapsed;
                        //Loading.Visibility = Visibility.Visible;
                        OnLoadedFail();
                    });
                    break;
                default:
                    break;
            }
        }
        private void OnLoadedFail()
        {
            //if (BrowersGrid.Visibility != Visibility.Visible)
            {
                Error.Visibility = Visibility.Visible;
                Loading.Visibility = Visibility.Collapsed;
                BrowersGrid.Visibility = Visibility.Collapsed;
            }
        }
        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
