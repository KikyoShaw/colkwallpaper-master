using EffectConfigModule.Help;
using RainingSimpleEffect.SharderEffect;
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

namespace RainingSimpleEffect
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

        private string _defaultCfgPath = "";
        int _monitorIndex = 0; //用来设置
        private ViewModel.EffectViewModel vm = null;
        private RainingSimpleEffect.SharderEffect.RainingSimple _effect = new RainingSimpleEffect.SharderEffect.RainingSimple();
        private bool _start = false;
        private Point dropReflect = new Point(100, 0);
        double _timeTick = 18;
        int _onTimeCount = 0;
        bool _lastLightningFlag = false;
        double _angleBreathe = 0;
        public double _curSpeed = 0.03;
        public double _curBreatheSpeed = 0.03;
        public double _curBreatheRang = 0.03;
        private string _dllPath = "";
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public BitmapImage _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/RainingSimpleEffect;Component/Resources/1.jpg"));
        public BitmapImage _noise = new BitmapImage(new Uri(@"pack://application:,,,/RainingSimpleEffect;Component/Resources/noise.jpg"));
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public EffectConfigModule.SaveDesignWindow _saveDesignWindow = null;

        public bool _bAutoHue = false;
        public bool _bAutoRefracton = false;
        public bool _bAutoWobble2 = false;
        public bool _bAutoBreathe = false;
        public double _curAlpha = 1.0;
        public double _curDiskRadius = 1.0;
        public double _curHollowSize = 0.2;
        public double _curHue = 0;
        public Timer _timer = null;
        public Size _imageSize = new Size(0, 0);
        public double _zoomMove = 0;

        MouseHookEventObject _mouseHookObject = new MouseHookEventObject();
        public IntPtr _ownHwnd = IntPtr.Zero;
        private double _dpi = 1.0, _screenDpi = 1.0;
        public Point _lastOff = new Point(0,0);
        public Point _CurOff = new Point(0,0);
        public double _maxTimeTick = 39999.0;

        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;
            _effect.IChannel1 = new ImageBrush(_noise);
            Back.Effect = _effect;

            _timer = new Timer(30);
            _timer.Elapsed += OnTimer;


            this.Loaded += EffectView_Loaded;
            this.Unloaded += EffectView_Unloaded;
            this.SizeChanged += EffectView_SizeChanged;

            _imageSize.Width = _defaultImage.PixelWidth;
            _imageSize.Height = _defaultImage.PixelHeight;
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                if (_start)
                {
                    _onTimeCount++;
                    _timeTick += _curSpeed;

                    if (_timeTick >= _maxTimeTick)
                        _timeTick = 0;

                    _angleBreathe += _curBreatheSpeed;
                    //if (_onTimeCount % 2 == 0)
                    {
                        _effect.Time = _timeTick;
                        SetCurOffX();
                        SetCurOffY();
                        /*if (_bAutoBreathe)
                        {
                            _effect.BlueRadius = _curDiskRadius + (200-_curDiskRadius)* _curBreatheRang * (Math.Sin(_angleBreathe));
                        }*/


                        //if (_bAutoHue)
                        //{
                        //    if (_effect.EffHue + 0.5 > 360.0)
                        //        _effect.EffHue = 0;
                        //    else
                        //        _effect.EffHue += 0.5;
                        //}
                    };
                }
            });
        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                _timer.Start();
                _mouseHookObject.Start();
                //CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _timer.Stop();
                _mouseHookObject.Stop();
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

                /*if (_bAutoBreathe)
                {
                    _effect.BlueRadius = _curDiskRadius + (Math.Sin(_angleBreathe) * _curBreatheRang);
                }*/

                if (_onTimeCount % 2 == 0)
                {
                    _effect.Time = _timeTick;
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
                                _screenDpi = double.Parse(cfg["dpi"].ToString());
                            }
                            _dpi = GetDpiFromVisual(this);
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
                    case "design": //打开配置
                        ShowDesign();
                        break;
                    case "switch": //切换
                        {
                            if (cfg.ContainsKey("path"))
                            {
                                string strPath = cfg["path"].ToString();

                                if (!string.IsNullOrEmpty(strPath) && File.Exists(strPath))
                                {
                                    string ext = System.IO.Path.GetExtension(strPath);
                                    if (ext.IndexOf("json", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                        ext.IndexOf("cfg", StringComparison.CurrentCultureIgnoreCase) != -1)
                                    {

                                        _defaultCfgPath = strPath;
                                        _effectConfig.LoadConfigPath(_defaultCfgPath);
                                        RefreshEffect();
                                    }
                                    else if (ext.IndexOf("bmp", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                        ext.IndexOf("png", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                        ext.IndexOf("jpg", StringComparison.CurrentCultureIgnoreCase) != -1 ||
                                        ext.IndexOf("jpeg", StringComparison.CurrentCultureIgnoreCase) != -1)
                                    {
                                        foreach (var item in _effectConfig.items)
                                        {
                                            if (string.Compare(item.key, "ImageBk", StringComparison.CurrentCultureIgnoreCase) == 0)
                                            {
                                                item.SetValue(strPath);
                                                OnConfigValueChange("ImageBk", strPath);
                                                HYWEffectConfig.EffectConfigTool.SaveConfig(_effectConfig);
                                                break;
                                            }
                                        }
                                    }
                                }
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
            if (_saveDesignWindow != null)
            {
                _saveDesignWindow.Close();
                _saveDesignWindow = null;
            }

            if (_configWindow == null)
            {
                _configWindow = new EffectConfigModule.ConfigWindow(this);
                
                _configWindow.SetDataContext(_effectConfig);
            }
            _configWindow.Closing += ((s, e) =>
            {
                _configWindow = null;
            });

            _configWindow.Show();
        }

        private void ShowDesign()
        {
            if (_effectConfig == null || _effectConfig.items.Count == 0)
            {
                MessageBox.Show("此特效没设置选项");
                return;
            }

            HYWEffectConfig.EffectConfigTool.SaveConfig(_effectConfig, _effectConfig.effectConfigPath);
            if (_saveDesignWindow != null)
            {
                _saveDesignWindow.Close();
                _saveDesignWindow = null;
            }
            if (_configWindow != null)
            {
                _configWindow.Close();
                _configWindow = null;
            }

            if (_saveDesignWindow == null)
            {
                _saveDesignWindow = new EffectConfigModule.SaveDesignWindow(_effectConfig, this);
                _saveDesignWindow.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            }
            _saveDesignWindow.Closing += ((s, e) =>
            {
                _saveDesignWindow = null;
            });

            _saveDesignWindow.Topmost = true;
            _saveDesignWindow.Show();
            
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
                                    _imageSize.Width = curImage.PixelWidth;
                                    _imageSize.Height = curImage.PixelHeight;
                                    LimitSize();
                                    GC.Collect();
                                    return;
                                }
                            }
                        }

                        {
                            if (_defaultImage != null)
                            {
                                ImgBk.ImageSource = _defaultImage;
                                _imageSize.Width = _defaultImage.PixelWidth;
                                _imageSize.Height = _defaultImage.PixelHeight;
                            }
                            LimitSize();
                            GC.Collect();
                        }
                    }
                    break;
                case "ImageStrength":
                    {
                        int mode = int.Parse(value);
                        switch (mode)
                        {
                            case 1:
                                ImgBk.Stretch = Stretch.Fill;
                                br.Stretch = Stretch.Fill;
                                break;
                            case 2:
                                ImgBk.Stretch = Stretch.Uniform;
                                br.Stretch = Stretch.Uniform;
                                break;
                            case 3:
                                ImgBk.Stretch = Stretch.UniformToFill;
                                br.Stretch = Stretch.UniformToFill;
                                break;
                            default:
                                ImgBk.Stretch = Stretch.Uniform;
                                br.Stretch = Stretch.None;
                                break;
                        }
                    }
                    break;
                case "speed":
                    {
                        double speed = (double.Parse(value) / 1000);
                        _curSpeed = speed;
                        //_effect.Alpha = _curAlpha;
                    }
                    break;
                
                case "Direction":
                    {
                        _effect.Direction = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "colorTemperature":
                    {
                        _effect.UsePostProcessing = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;

                case "imgHue":
                    {
                        _effect.ImgHue = (double)((int.Parse(value))) / 10.0;
                    }
                    break;
                case "imgSat":
                    {
                        _effect.ImgSat = (double)((int.Parse(value))) / 1000.0 + (_lastLightningFlag ? 0.8 : 0);
                    }
                    break;
                case "imgLum":
                    {
                        _effect.ImgLum = (double)((int.Parse(value))) / 1000.0 + (_lastLightningFlag ? 0.17 : 0);
                    }
                    break;
               
                case "dropSize":
                    {
                        _effect.DropSize = (double)((int.Parse(value)));
                    }
                    break;
                case "dropReflectX":
                    {
                        dropReflect.X = (double)((int.Parse(value)));
                        _effect.DropReflect = dropReflect;
                    }
                    break;
                case "dropReflectY":
                    {
                        dropReflect.Y = (double)((int.Parse(value)));
                        _effect.DropReflect = dropReflect;
                    }
                    break;
                case "BlueRadius":
                    {
                        _effect.BlueRadius = _curDiskRadius = (double)((int.Parse(value)));
                    }
                    break;
                case "BlurContrast":
                    {
                        _effect.BlurContrast =  (double)((int.Parse(value)))/1000.0;
                    }
                    break;
                case "BlurPrecision":
                    {
                        _effect.BlurPrecision =  (double)((int.Parse(value)))/10.0;
                    }
                    break;
                //case "BlurMode":
                //    {
                //        _effect.BlurMode = ((int.Parse(value)));
                //    }
                //    break;
                case "rainSpeed":
                    {
                        _effect.RainSpeed = ((double.Parse(value)) / 100.0);
                    }
                    break;
                case "AutoBlur":
                    {
                        _bAutoBreathe = bool.Parse(value);
                        if (!_bAutoBreathe)
                            _effect.BlueRadius = _curDiskRadius;
                    }
                    break;
                case "AutoBlurSpeed":
                    {
                        double speed = (double.Parse(value) / 1000);
                        _curBreatheSpeed = speed;
                    }
                    break;
                case "AutoBlurRange":
                    {
                        _curBreatheRang = (double.Parse(value) ); 
                    }
                    break;
                case "lightningLevel":
                    {
                         bool curLightningFlag = (double.Parse(value)) > 0;
                        _effect.LightningTime = (double.Parse(value));
                        
                        /*if (_lastLightningFlag!= curLightningFlag)
                        {
                            _lastLightningFlag = curLightningFlag;
                            if (curLightningFlag)
                            {
                                _effect.ImgLum += 0.17;
                                _effect.ImgSat += 0.8;
                            }
                            else
                            {
                                _effect.ImgLum -= 0.17;
                                _effect.ImgSat -= 0.8;
                            }
                        }*/
                    }
                    break;
                case "ZoomEffectMove":
                    {
                        _effect.ZoomMove = _zoomMove = (double.Parse(value)) / 1000;

                    }
                    break;
                case "dropRainZoom":
                    {
                        _effect.DropRainZoom = (double.Parse(value)) / 10;

                    }
                    break;
                case "ZoomImageMove":
                    {
                        _effect.ZoomMovePic = (double.Parse(value)) / 1000;
                    }
                    break;
                case "rainIntensity":
                    {
                        _effect.Rainintensity = (double.Parse(value)) / 1000;
                    }
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

                string str =$@"pack://application:,,,/RainingSimpleEffect;Component/Resources/config.json";


                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "RainingSimpleEffect_v1.0", _monitorIndex);
                        _effectConfig.SetConfigAction(OnConfigValueChange, RefreshEffect);
                        if (!string.IsNullOrEmpty(_defaultCfgPath))
                            _effectConfig.LoadConfigPath(_defaultCfgPath);
                        else
                            RefreshEffect();
                    }
                }

                if (_effect != null)
                {
                    _effect.ResolutionX = this.ActualWidth;
                    _effect.ResolutionY = this.ActualHeight;
                }
                _ownHwnd = ((HwndSource)PresentationSource.FromVisual(this)).Handle;
                _mouseHookObject.Start();
                _mouseHookObject.MouseMove += OnHookMouseMove;
            }
            catch { }
        }
        public void SetCurOffX()
        {
            if(_lastOff.X!=_CurOff.X)
            {
                _lastOff.X = _CurOff.X;
                _effect.Offx = _lastOff.X;
            }
        }
        public void SetCurOffY()
        {
            if (_lastOff.Y != _CurOff.Y)
            {
                _lastOff.Y = _CurOff.Y;
                _effect.Offy = _lastOff.Y;
            }
        }
        private void OnHookMouseMove(object sender, MouseHook.MouseEventArgs e)
        {
            //System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            //{
            System.Drawing.Point pt = new System.Drawing.Point((int)(e.X), (int)(e.Y));
            ScreenToClient(_ownHwnd, ref pt);
            pt.X = (int)(pt.X / _dpi);
            pt.Y = (int)(pt.Y / _dpi);
            if (pt.X < 0 || pt.X > this.ActualWidth || pt.Y < 0 || pt.Y >= this.ActualHeight)
                return;
            double x = (double)pt.X / (this.ActualWidth);
            double y = (double)pt.Y / (this.ActualHeight);
            _CurOff.X = Math.Max(Math.Min(x, 1), 0);
            _CurOff.Y = Math.Max(Math.Min(y, 1), 0);
            //});
        }
        private void RefreshEffect(object obj = null)
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
            if (_effect != null)
            {
                _effect.ResolutionX = this.ActualWidth;
                _effect.ResolutionY = this.ActualHeight;

                LimitSize();
            }
        }
        double GetImageRatioHeight(double w)
        {
            return w * this.ActualHeight / this.ActualWidth;
        }
        double GetImageRatioWidth(double h)
        {
            return h * this.ActualWidth / this.ActualHeight;
        }
        private void LimitSize()
        {
            if (this.ActualWidth > _imageSize.Width)
            {

                Back.Width = Math.Min(_imageSize.Width, 3000);
                Back.Height = GetImageRatioHeight(Back.Width);
            }
            else
            {
                Back.Width = Math.Min(this.ActualWidth, 3000);
                Back.Height = GetImageRatioHeight(Back.Width);

            }
        }
    }
}