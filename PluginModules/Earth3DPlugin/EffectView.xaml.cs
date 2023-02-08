using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Effects;
using CefSharp;
using CefSharp.Wpf;
using CefTools;
using EffectConfigModule.Help;
using Timer = System.Timers.Timer;

namespace Earth3DPlugin
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class EffectConfigValue
    {

    }


    public partial class EffectView :  UserControl
    {
        [DllImport("user32")]
        public static extern bool ScreenToClient(IntPtr hwnd, ref System.Drawing.Point p);

        public IntPtr OwnHWnd = IntPtr.Zero;
        private double _dpi = 1.0;
        //private readonly MouseHookEventObject _mouseHookObject = new MouseHookEventObject();
        private readonly int _monitorIndex = 0; //用来设置
        private ViewModel.EffectViewModel vm = null;
        private bool _start = false;
        double _timeTick = 0;
        private readonly string _dllPath;
        private string _defaultCfgPath = "";
        public HYWEffectConfig.EffectConfigInfo EffectConfig = new HYWEffectConfig.EffectConfigInfo();
        public EffectConfigModule.ConfigWindow ConfigWindow = null;
        public Thickness Thickness = new Thickness(0,0,0,0);
        public Thickness Diythickness = new Thickness(0,0,0,0);
        public string Address = "";
        private readonly Timer _timer = null;
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
        public double _showRatio = 1.0;
        public bool _userShadow = false;
        DropShadowEffect _shadowEffect = new DropShadowEffect();

        Size _showSize = new Size(100, 100);
        Point _showPoint = new Point(0, 0);
        int _curZIndex = 0;
        private CefDisplayHandler _dh = null;
        bool _bAutoHue = false;
        double _curHue = 0;
        public double _curAutoHueSpeed = 0.01;

        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;

            _timer = new Timer(1000);
            _timer.Elapsed += OnTimer;
            //_timer.Start();
            Address = $@"{System.Environment.CurrentDirectory}\Resources\html\index.html";

            InitializeCefSettings();
            
            this.Loaded += EffectView_Loaded;
            this.Unloaded += EffectView_Unloaded;

            //Browers.ExecuteScriptAsync("jsValue='adc'");
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
                //_mouseHookObject.Start();
            }
        }

        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                this.Visibility = Visibility.Collapsed;
                _timer.Stop();
                //_mouseHookObject.Stop();
            }
        }

        void Reload()
        {
            if(!_delayLoad)
                return;


            if(Cef.IsInitialized)
            {
                //Browers.Navigate(_address);
                Browers.Load(Address);
            }
                
 
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

                if (_bAutoHue)
                {
                    if (BrowerEffect.Hue + _curAutoHueSpeed > 360.0)
                        BrowerEffect.Hue = 0;
                    else
                        BrowerEffect.Hue += _curAutoHueSpeed;
                }
            });
        }

        private double GetDpiFromVisual(System.Windows.Media.Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            }

            return dpiX / 96.0;
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

                        _dpi = cfg.ContainsKey("dpi") ? double.Parse(cfg["dpi"].ToString() ?? string.Empty) : GetDpiFromVisual(this);
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
                    case "saveconfig":
                        {
                            if (cfg.ContainsKey("path"))
                            {
                                HYWEffectConfig.EffectConfigTool.SaveConfig(EffectConfig);
                                var path = cfg["path"].ToString();
                                if (!string.IsNullOrEmpty(path))
                                {
                                    HYWEffectConfig.EffectConfigTool.SaveConfig(EffectConfig, path, true);
                                }
                            }
                        }
                        break;
                    case "switch": //切换
                        {
                            if (cfg.ContainsKey("path"))
                            {
                                _defaultCfgPath = cfg["path"].ToString();
                                EffectConfig.LoadConfigPath(_defaultCfgPath);
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
            if (EffectConfig == null || EffectConfig.items.Count == 0)
            {
                MessageBox.Show("此特效没设置选项");                
                return;
            }
            if (ConfigWindow != null)
            {
                ConfigWindow.Close();
                ConfigWindow = null;
            }

            if (ConfigWindow == null)
            {
                ConfigWindow = new EffectConfigModule.ConfigWindow();
                
                ConfigWindow.SetDataContext(EffectConfig);
            }
            ConfigWindow.Closing += ((s, e) =>
            {
            });

            ConfigWindow.Show();
        }

        public void SetPointLeft(double left)
        {
            _showPoint.X = left;
            //RefreshPostion();
        }

        public void SetPointTop(double top)
        {
            _showPoint.Y = top;
            //RefreshPostion();
        }

        public void SetWidthRatio(double widthRatio)
        {
            _showSize.Width = _showSize.Height = widthRatio;
            //RefreshPostion();
        }

        public void RefreshPostion()
        {
            try
            {

                var parendGrid = (Grid)this.Parent;
                {
                    //this.Width = (parendGrid.ActualWidth * (_showSize.Width / 1000.0));

                    vm.ShowWidth = vm.ShowHeight = 500 * _showRatio;

                    {
                        //double hov = ((double)_showPoint.X / 500 * (parendGrid.ActualWidth - this.Width));

                        double hov = _showPoint.X * (parendGrid.ActualWidth / 400.0);
                        Thickness.Left = hov;
                        if (Thickness.Left + this.Width > parendGrid.ActualWidth)
                        {
                            Thickness.Right = parendGrid.ActualWidth - (Thickness.Left + this.Width);
                        }
                        else
                        {
                            Thickness.Right = 0;
                        }

                    }

                    {
                        //double vec = ((double)_showPoint.Y / 500.0 * (parendGrid.ActualHeight - this.Height));
                        double vec = _showPoint.Y * (parendGrid.ActualHeight / 400.0);
                        Thickness.Top = vec;
                        if (Thickness.Top + this.Height > parendGrid.ActualHeight)
                        {
                            Thickness.Bottom = parendGrid.ActualHeight - (Thickness.Top + this.Height);
                        }
                        else
                        {
                            Thickness.Bottom = 0;
                        }
                    }

                    vm.ShowMargin = Thickness;
                   
                }

            }
            catch (Exception ex)
            {

            }

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
                        vm.BackOpacity = int.Parse(value);
                    }
                    break;
                case "HorizontalOffset":
                    {
                        SetPointLeft(double.Parse(value));
                        updatePostion = true;
                    }
                    break;
                case "VerticalOffset":
                    {
                        SetPointTop(double.Parse(value));
                        updatePostion = true;
                    }
                    break;
                case "ShowRatio":
                    {
                        _showRatio = double.Parse(value) / 1000;
                        if(Browers is { IsBrowserInitialized: true })
                        {
                            AutoZoom();
                        }

                        updatePostion = true;
                    }
                    break;
                case "Opacity":
                    {
                        vm.Opacity = 100 - int.Parse(value);
                    }
                    break;
                case "UseShadow":
                {
                    _userShadow = bool.Parse(value);
                    Clock.Effect = _userShadow ? _shadowEffect : null;
                }
                    break;
                case "ShadowColor":
                    {
                        _shadowEffect.Color = (Color)ColorConverter.ConvertFromString(value)!;
                    }
                    break;
                case "ShadowDirection":
                    {
                        _shadowEffect.Direction = double.Parse(value);
                    }
                    break;
                case "ShadowDepth":
                    {
                        _shadowEffect.ShadowDepth = double.Parse(value);
                    }
                    break;
                case "ShadowOpacity":
                    {
                        _shadowEffect.Opacity =  double.Parse(value) / 100.0;
                    }
                    break;
                case "ShadowBlurRadius":
                    {
                        _shadowEffect.BlurRadius = double.Parse(value);
                    }
                    break;
                //case "EdgeOpacity":
                //    {
                //        BrowerEffect.Alpha = double.Parse(value) / 100.0;
                //    }
                //    break;
                case "EffHue":
                    {
                        _curHue = (double)((int.Parse(value))) / 10.0;
                        if (!_bAutoHue)
                            BrowerEffect.Hue = _curHue;
                    }
                    break;
                case "EffSat":
                    {
                        BrowerEffect.Saturation = double.Parse(value) / 1000.0;
                    }
                    break;
                case "EffLum":
                    {
                        BrowerEffect.Luminosity = double.Parse(value) / 1000.0;
                    }
                    break;
                case "AutoHue":
                    {
                        _bAutoHue = bool.Parse(value);
                        if (!_bAutoHue)
                        {
                            BrowerEffect.Hue = _curHue;
                        }
                    }
                    break;
                case "AutoHueSpeed":
                    {
                        _curAutoHueSpeed = (double.Parse(value) / 100);
                    }
                    break;
            }
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                if (updatePostion)
                    RefreshPostion();
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

        private void OnHookMouseUp(object sender, MouseHook.MouseEventArgs e)
        {
            System.Drawing.Point pos = new System.Drawing.Point((int)(e.X), (int)(e.Y));
            ScreenToClient(OwnHWnd, ref pos);
            pos.X = (int)(pos.X / _dpi);
            pos.Y = (int)(pos.Y / _dpi);
            var pt = this.PointFromScreen(Browers.PointToScreen(new Point(0, 0)));
            int posX = (int)(pt.X / _dpi);
            int posY = (int)(pt.Y / _dpi);
            if ((posX <= pos.X && (posX + Browers.ActualWidth) >= pos.X) &&
                (posY <= pos.Y && (posY + Browers.ActualHeight) >= pos.Y))
            {
                switch (e.Button)
                {
                    case MouseHook.MouseButtons.Left:
                    {
                        int x = pos.X - posX;
                        int y = pos.Y - posY;
                        if (x >= 0 && y >= 0)
                        {
                            var m = new CefSharp.MouseEvent(x, y, CefEventFlags.LeftMouseButton);
                            //Browers?.GetBrowser().GetHost().SendMouseMoveEvent(m, false);
                            Browers?.GetBrowser().GetHost().SendMouseClickEvent(
                                m, MouseButtonType.Left, true, e.Clicks);
                        }
                    } 
                        break;
                }
            }
        }

        private void OnHookMouseDown(object sender, MouseHook.MouseEventArgs e)
        {
            System.Drawing.Point pos = new System.Drawing.Point((int)(e.X), (int)(e.Y));
            ScreenToClient(OwnHWnd, ref pos);
            pos.X = (int)(pos.X / _dpi);
            pos.Y = (int)(pos.Y / _dpi);
            var pt = this.PointFromScreen(Browers.PointToScreen(new Point(0, 0)));
            int posX = (int)(pt.X / _dpi);
            int posY = (int)(pt.Y / _dpi);
            if ((posX <= pos.X && (posX + Browers.ActualWidth) >= pos.X) &&
                (posY <= pos.Y &&(posY + Browers.ActualHeight) >= pos.Y))
            {
                switch (e.Button)
                {
                    case MouseHook.MouseButtons.Left:
                    {
                        int x = pos.X - posX;
                        int y = pos.Y - posY;
                        if (x >= 0 && y >= 0)
                        {
                            var m = new CefSharp.MouseEvent(x, y, CefEventFlags.LeftMouseButton);
                                //Browers?.GetBrowser().GetHost().SendMouseMoveEvent(m, false);
                                Browers?.GetBrowser().GetHost().SendMouseClickEvent(
                                    m, MouseButtonType.Left, false, e.Clicks);
                                //Thread.Sleep(50);
                                //Browers?.GetBrowser().GetHost().SendMouseClickEvent(
                                //m, MouseButtonType.Left, true, e.Clicks);
                        }
                    } 
                        break;
                }
            }
        }

        //private long LastMoveTime { get; set; } = 0;
        private void OnHookMouseMove(object sender, MouseHook.MouseEventArgs e)
        {
            //if (DateTime.Now.Ticks - LastMoveTime <= TimeSpan.FromSeconds(5).Ticks)
            //    return;
            System.Drawing.Point pos = new System.Drawing.Point((int)(e.X), (int)(e.Y));
            ScreenToClient(OwnHWnd, ref pos);
            pos.X = (int)(pos.X / _dpi);
            pos.Y = (int)(pos.Y / _dpi);
            var pt = this.PointFromScreen(Browers.PointToScreen(new Point(0, 0)));
            int posX = (int)(pt.X / _dpi);
            int posY = (int)(pt.Y / _dpi);
            if ((posX <= pos.X && (posX + Browers.ActualWidth) >= pos.X) &&
                (posY <= pos.Y && (posY + Browers.ActualHeight) >= pos.Y))
            {
                int x = pos.X - posX;
                int y = pos.Y - posY;
                if (x >= 0 && y >= 0)
                {
                    Browers?.GetBrowser()?.GetHost()?.SendMouseMoveEvent(
                        new CefSharp.MouseEvent(x, y, CefEventFlags.None), false);
                }
            }

            //LastMoveTime = DateTime.Now.Ticks;
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
                _dpi = GetDpiFromVisual(this);
                _dh ??= new CefDisplayHandler();
                Browers.DisplayHandler = _dh;
                Browers.LoadingStateChanged += Browser_LoadingStateChanged;
                OwnHWnd = (((HwndSource)PresentationSource.FromVisual(this))!).Handle;
                string str =$@"pack://application:,,,/Earth3DPlugin;Component/Resources/config.json";

                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute))?.Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        var sCfgData = r.ReadToEnd();
                        EffectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "Earth3DPlugin_v1.0", _monitorIndex, 1);
                        EffectConfig.SetConfigAction(OnConfigValueChange, RefreshEffect);
                        if (!string.IsNullOrEmpty(_defaultCfgPath))
                            EffectConfig.LoadConfigPath(_defaultCfgPath);
                        else
                            RefreshEffect();
                    }
                }

                //_mouseHookObject.Start();
                //_mouseHookObject.MouseMove += OnHookMouseMove;
                //_mouseHookObject.MouseDown += OnHookMouseDown;
                //_mouseHookObject.MouseUp += OnHookMouseUp;
            }
            catch { }
        }
        
        private void RefreshEffect(object obj = null)
        {
            
            foreach(var item in EffectConfig.items)
            {
                string value = (item.GetValue());
                string key = (item.key);
                OnConfigValueChange(key, value);
            }

            System.Windows.Application.Current.Dispatcher.InvokeAsync(RefreshPostion);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                RefreshEffect();
                RefreshPostion(); 
            });
        }

        public void AutoZoom()
        {
            if (_showRatio >= 1.0)
            {
                Browers.SetZoomLevel((_showRatio - 1.0) * 3.85);
            }
            else
            {
                Browers.SetZoomLevel(-((1.0 - _showRatio) * 7.5));
            }
        }

        private void Browser_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {      
            if (e.IsLoading == false)
            {
                if (_lasterror == 0)
                {
                    System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
                    {
                        try
                        {
                            //Loading.Visibility = Visibility.Collapsed;
                            //Error.Visibility = Visibility.Collapsed;
                            BrowersGrid.Visibility = Visibility.Visible;
                            AutoZoom();
                        }
                        catch (Exception ex)
                        {

                        }
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
                //Error.Visibility = Visibility.Visible;
                //Loading.Visibility = Visibility.Collapsed;
                //BrowersGrid.Visibility = Visibility.Collapsed;
            }
        }

        private void Browser_ConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
