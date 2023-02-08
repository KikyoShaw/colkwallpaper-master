using HeavenEffect.SharderEffect;
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

namespace HeavenEffect
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class EffectConfigValue
    {

    }
    public partial class EffectView : UserControl
    {
        private string _defaultCfgPath = "";
        int _monitorIndex = 0; //用来设置
        private ViewModel.EffectViewModel vm = null;
        private LightraysChromeKey _effect = null;
        private bool _start = false;
        double _timeTick = 0;
        int _onTimeCount = 0;
        double _angleBreathe = 0;
        public double _curSpeed = 0.03;
        public double _curBreatheSpeed = 0.03;
        private string _dllPath = "";
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public BitmapImage _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/HeavenEffect;Component/Resources/1.jpg"));
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public EffectConfigModule.SaveDesignWindow _saveDesignWindow = null;

        public bool _bAutoHue = false;
        public bool _bAutoRefracton = false;
        public bool _bAutoWobble2 = false;
        public bool _bAutoBreathe = false;
        public double _curAlpha = 1.0;
        public double _curHue = 0;
        public Timer _timer = null;
        public Size _imageSize = new Size(0, 0);

        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;
            //Back.Effect = _effect;
            _effect = Sharder;

            _timer = new Timer(30);
            _timer.Elapsed += OnTimer;

            vm.isHaveLavSplitter = WPFMediaKitCore.DirectShow.MediaPlayers.MediaUriPlayer.CheckSplitter();

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
                    _angleBreathe += _curBreatheSpeed;
                    //if (_onTimeCount % 2 == 0)
                    {
                        _effect.Time = _timeTick;
                        /*if (_bAutoBreathe)
                        {
                            _effect.Alpha = _curAlpha + (Math.Sin(_angleBreathe) * 0.3);
                        }*/
                    }
                    if (_bAutoHue)
                    {
                        if (_effect.EffHue + 0.5 > 360.0)
                            _effect.EffHue = 0;
                        else
                            _effect.EffHue += 0.5;
                    }
                }
            });
        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                try
                {
                    if (vm.isHaveLavSplitter)
                    {
                        LavVideoElement.Play();
                    }
                    else
                    {
                        VideoElement.Play();
                    }
                    _timer.Start();
                    //CompositionTarget.Rendering += CompositionTarget_Rendering;
                }
                catch/*(Exception ex) */
                {
                    //MessageBox.Show(ex.ToString());
                }
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                try
                {
                    if (vm.isHaveLavSplitter)
                    {
                        LavVideoElement.Pause();
                    }
                    else
                    {
                        VideoElement.Pause();
                    }
                    _timer.Stop();
                    //CompositionTarget.Rendering -= CompositionTarget_Rendering;
                }
                catch/* (Exception ex)*/
                {
                    //MessageBox.Show(ex.ToString());
                }
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
                case "ImgAlpha":
                    {
                        _effect.Alpha2 = (double.Parse(value) / 1000);
                    }
                    break;

                case "EffAlpha":
                    {
                        _effect.Tolerance = (double.Parse(value) / 1000);
                    }
                    break;
                case "Direction":
                    {
                        _effect.Direction = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "UpDownReverse":
                    {
                        _effect.UpDownReverse = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "ImgDirection":
                    {
                        _effect.ImgDirection = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "ImgUpDownReverse":
                    {
                        _effect.ImgUpDownReverse = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "Colours":
                    {
                        _effect.Colours = bool.Parse(value) ? 1.0 : 0.0;
                    }
                    break;
                case "imgHue":
                    {
                        _effect.ImgHue = (double)((int.Parse(value))) / 10.0;
                    }
                    break;
                case "imgSat":
                    {
                        _effect.ImgSat = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "imgLum":
                    {
                        _effect.ImgLum = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "EffHue":
                    {
                        _effect.EffHue = _curHue = (double)((int.Parse(value))) / 10.0;
                    }
                    break;
                case "EffSat":
                    {
                        _effect.EffSat = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "EffLum":
                    {
                        _effect.EffLum = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "AutoHue":
                    {
                        _bAutoHue = bool.Parse(value);
                        if (!_bAutoHue)
                        {
                            _effect.EffHue = _curHue;
                        }
                    }
                    break;
                case "EffectColor":
                    {
                        _effect.EffColor = (Color)ColorConverter.ConvertFromString(value);
                    }
                    break;
                case "UseLightraysProperty":
                    {
                        _effect.UseLightraysLum = (bool.Parse(value) ? 1 : 0);
                    }
                    break;
                case "speed":
                    {
                        double speed = (double.Parse(value) / 100);
                        _curSpeed = speed;
                        //_effect.Alpha = _curAlpha;
                    }
                    break;
                case "LightraysColor":
                    {
                         _effect.LightraysColor = (Color)ColorConverter.ConvertFromString(value);
                    }
                    break;

                case "LightraysHue":
                    {
                        _effect.LightraysHue = (double)((int.Parse(value))) / 10.0;
                    }
                    break;
                case "LightraysSat":
                    {
                        _effect.LightraysSat = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "LightraysLum":
                    {
                        _effect.LightraysLum = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "LightraysAlpha":
                    {
                        _effect.LightraysAlpha = (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "LightraysDensity":
                    {
                        _effect.LightraysDensity = (double)((int.Parse(value))) / 100.0;
                    }
                    break;
                case "LightraysOffX":
                    {
                        _effect.LightraysOffX = (double)((int.Parse(value))) / 100.0;
                    }
                    break;
                case "LightraysUpDown":
                    {
                        _effect.LightraysUpDownReverse = (bool.Parse(value) ? 1 : 0);
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
        private void RefreshVideo()
        {
            try
            {
                string strResPath = $@"{System.Environment.CurrentDirectory}\Resources\vd.mp4";
                if (!vm.isHaveLavSplitter)
                {
                    VideoElement.Stop();
                    VideoElement.Close();
                    VideoElement.Source = null;
                    GC.Collect();
                    VideoElement.Source = (new Uri(strResPath, UriKind.Relative));
                    VideoElement.Play();
                }
                else
                {
                    LavVideoElement.Stop();
                    LavVideoElement.Close();
                    LavVideoElement.Source = null;
                    GC.Collect();
                    //LavVideoElement.Loop = true;
                    //LavVideoElement.VideoRenderer = WPFMediaKitCore.DirectShow.MediaPlayers.VideoRendererType.VideoMixingRenderer9;
                    LavVideoElement.Source = new Uri(strResPath);

                }
            }
            catch (System.Exception ex)
            {
            }
        }
        private void EffectView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                string str = $@"pack://application:,,,/HeavenEffect;Component/Resources/config.json";

                _effect.ColorKey = (Color)ColorConverter.ConvertFromString("#000000");
                //_effect.Tolerance = 0.5;
                _effect.MaskColorChannel = 0.0;

                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "HeavenEffect_v1.0", _monitorIndex);
                        _effectConfig.SetConfigAction(OnConfigValueChange, RefreshEffect);
                        if (!string.IsNullOrEmpty(_defaultCfgPath))
                            _effectConfig.LoadConfigPath(_defaultCfgPath);
                        else
                            RefreshEffect();
                    }
                }

                try
                {
                    RefreshVideo();
                }
                catch { }

                if (_effect != null)
                {
                    _effect.ResolutionX = this.ActualWidth;
                    _effect.ResolutionY = this.ActualHeight;
                }
            }
            catch { }
        }

        private void VideoElement_MediaOpened(object sender, RoutedEventArgs e)
        {
        }

        private void VideoElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_start)
            {
                VideoElement.Stop();
                VideoElement.Play();
            }
        }
        private void VideoElement_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            //int I = 0;
        }

        private void RefreshEffect(object obj = null)
        {
            foreach (var item in _effectConfig.items)
            {
                string value = (item.GetValue());
                string key = (item.key);
                OnConfigValueChange(key, value);
            }
        }

        private void MediaUriElement_MediaFailed(object sender, WPFMediaKitCore.DirectShow.MediaPlayers.MediaFailedEventArgs e)
        {
            //MessageBox.Show($"{vm.sVideoPath} \r\n 此视频桌面不存在");
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                try
                {
                    vm.isHaveLavSplitter = false;
                    RefreshVideo();
                }
                catch (System.Exception ex)
                {

                }

            });
        }

        private void MediaUriElement_MediaOpened(object sender, RoutedEventArgs e)
        {
            if (_start)
            {
                //LavVideoElement.Play();
            }
        }
        private void MediaUriElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            if (_start)
            {
                //LavVideoElement.MediaPosition = 0;
                //LavVideoElement.Play();
            }
        }
        private void EffectView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_effect != null)
            {
                _effect.ResolutionX = this.ActualWidth;
                _effect.ResolutionY = this.ActualHeight;
            }
            LimitSize();
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
