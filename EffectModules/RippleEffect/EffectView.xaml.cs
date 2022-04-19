using EffectConfigModule.Help;
using RippleEffectModule.SharderEffect;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RippleEffectModule
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class EffectConfigValue
    {

    }
    public partial class EffectView :  UserControl
    {
        private string _defaultCfgPath = "";
        int _monitorIndex = 0; //用来设置
        private ViewModel.EffectViewModel vm = null;
        private RippleEffect _effect = new RippleEffect(160, 120);
        private bool _start = false;
        double _timeTick = 0;
        int _onTimeCount = 0;
        bool _lastLightningFlag = false;
        double _angleBreathe = 0;
        public double _curSpeed = 0.03;
        public double _curBreatheSpeed = 0.03;
        public double _curBreatheRang = 0.03;
        private string _dllPath = "";
        public Size _OffCentering = new Size(0.5, 0.5);
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public BitmapImage _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/RippleEffectModule;Component/Resources/1.jpg"));
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public IntPtr _ownHwnd = IntPtr.Zero;
        MouseHookManager _mouseHook = MouseHookManager.Instance;

        public bool _bAutoHue = false;
        public bool _bAutoRefracton = false;
        public bool _bAutoRipple = false;
        public bool _bAutoBreathe = false;
        public double _curAlpha = 1.0;
        public double _curDiskRadius = 1.0;
        public double _curHollowSize = 0.2;
        public double _curHue = 0;
        public Timer _timer = null;
        private double _dpi = 1.0;

        public int _curTimeCount = 0;
        public int _iRippleSec = 1;

        public Random _rnd = new Random();


        [DllImport("user32")]
        public static extern bool ScreenToClient(IntPtr hwnd, ref System.Drawing.Point p);
        [DllImport("user32")]
        public static extern bool ClientToScreen(IntPtr hwnd, ref System.Drawing.Point p);

        public double GetDpiFromVisual(System.Windows.Media.Visual visual)
        {
            var source = PresentationSource.FromVisual(visual);

            var dpiX = 96.0;

            if (source?.CompositionTarget != null)
            {
                dpiX = 96.0 * source.CompositionTarget.TransformToDevice.M11;
            }

            return dpiX / 96.0;
        }

        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;
            Back.Effect = _effect;

            _timer = new Timer(30);
            _timer.Elapsed += OnTimer;


            this.Loaded += EffectView_Loaded;
            this.Unloaded += EffectView_Unloaded;
            this.SizeChanged += EffectView_SizeChanged;


            _mouseHook.MouseUp += OnHookMouseUp;
            _mouseHook.Start();
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                if (_start)
                {
                    _onTimeCount++;
                    _timeTick += _curSpeed;
                    _angleBreathe += _curBreatheSpeed;
                    if (_onTimeCount> (_iRippleSec * 33))
                    {
                        _onTimeCount = 0;

                        float x = (float)_rnd.Next(0, 99999999) % (int)(this.ActualWidth);
                        float y = (float)_rnd.Next(0, 99999999) % (int)(this.ActualHeight);

                        x /= (int)(this.ActualWidth);
                        y /= (int)(this.ActualHeight);
                        _effect.Drop(x, y);
                    };
                }
            });
        }
        private void OnHookMouseUp(object sender, MouseHook.MouseEventArgs e)
        {
            System.Drawing.Point pt = new System.Drawing.Point((int)(e.X), (int)(e.Y));
            ScreenToClient(_ownHwnd, ref pt);
            pt.X = (int)(pt.X / _dpi);
            pt.Y = (int)(pt.Y / _dpi);
            double x = (double)pt.X / Back.RenderSize.Width;
            double y = (double)pt.Y / Back.RenderSize.Height;
            int lParam = (int)((short)pt.X) | (int)((short)pt.Y << 16);
            if (x >= 0 && x <= 1 && y >= 0 && y <= 1)
            {
                switch (e.Button)
                {
                    case MouseHook.MouseButtons.Left:
                        {
                            if (x >= 0 && x <= 1 && y >= 0 && y <= 1)
                                _effect.Drop((float)x, (float)y);
                        }

                        break;
                    case MouseHook.MouseButtons.Right:
                        {
                            /*PostMessage(new IntPtr(_web2Hwnd), WM_RBUTTONDOWN, 0x0001, lParam);
                            PostMessage(new IntPtr(_web2Hwnd), WM_RBUTTONUP, 0x0001, lParam);
                            PostMessage(webView.Handle, WM_RBUTTONDOWN, 0x0001, lParam);
                            PostMessage(webView.Handle, WM_RBUTTONUP, 0x0001, lParam);*/
                        }
                        break;
                    case MouseHook.MouseButtons.Middle:
                        {

                            /*PostMessage(new IntPtr(_web2Hwnd), WM_MBUTTONDOWN, 0x0001, lParam);
                            PostMessage(new IntPtr(_web2Hwnd), WM_MBUTTONDOWN, 0x0001, lParam);
                            PostMessage(webView.Handle, WM_MBUTTONDOWN, 0x0001, lParam);
                            PostMessage(webView.Handle, WM_MBUTTONDOWN, 0x0001, lParam);*/
                        }
                        break;
                }
            }
        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                _timer.Start();
                //_mouseHook.Start();
                //CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _timer.Stop();
                //_mouseHook.Stop();
                //CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>{
            if (_start)
            {
                _onTimeCount++;
                _timeTick += _curSpeed;
                _angleBreathe += _curBreatheSpeed;

               
                if (_onTimeCount % 2 == 0)
                {
                    //_effect.Time = _timeTick;
                };
            }
            //});
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
                            if (cfg.ContainsKey("dpi"))
                            {
                                _dpi = double.Parse(cfg["dpi"].ToString());
                            }
                            else
                            {
                                _dpi = GetDpiFromVisual(this);
                            }
                            RefreshEffect();
                        }
                        break;

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
            catch { }
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

        private void OnConfigValueChange(string key, string value)
        {

            switch (key)
            {
                case "ImageBk":
                    {
                        if (!string.IsNullOrEmpty(value))
                        {
                            if (File.Exists(value))
                            {
                                BitmapImage curImage = HYWEffectConfig.AssistHelp.LoadLocalImage(value, (int)this.ActualWidth, (int)this.ActualHeight);
                                if (curImage != null)
                                {
                                    ImgBk.ImageSource = curImage;
                                    return;
                                }
                            }
                        }

                        {
                            BitmapImage curImage = new BitmapImage(new Uri(@"pack://application:,,,/RippleEffectModule;Component/Resources/1.jpg"));
                            //BitmapImage curImage = new BitmapImage(new Uri(value));
                            if (curImage != null)
                            {
                                ImgBk.ImageSource = curImage;
                            }
                        }
                        GC.Collect();
                    }
                    break;
                case "ImageStrength":
                    {
                        int mode = int.Parse(value);
                        switch (mode)
                        {
                            case 1:
                                ImgBk.Stretch = Stretch.Fill;
                                break;
                            case 2:
                                ImgBk.Stretch = Stretch.Uniform;
                                break;
                            case 3:
                                ImgBk.Stretch = Stretch.UniformToFill;
                                break;
                            default:
                                ImgBk.Stretch = Stretch.None;
                                break;
                        }
                    }
                    break;
                case "AutoRipple":
                    _bAutoRipple = bool.Parse(value);
                    break;
                case "AutoSpan":
                    _iRippleSec = int.Parse(value);
                    break;
            }
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
                _dpi = GetDpiFromVisual(this);
                string str =$@"pack://application:,,,/RippleEffectModule;Component/Resources/config.json";
                _ownHwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;

                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "RippleEffectModule_v1.0", _monitorIndex);
                        _effectConfig.LoadConfigPath(_defaultCfgPath);
                        RefreshEffect();
                    }
                }

                //if (_effect != null)
                //{
                //    _effect.ResolutionX = this.ActualWidth;
                //    _effect.ResolutionY = this.ActualHeight;
                //}
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
        }
        private void EffectView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //if (_effect != null)
            //{
            //    _effect.ResolutionX = this.ActualWidth;
            //    _effect.ResolutionY = this.ActualHeight;
            //}
        }
    }
}