using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace AurorasEffect
{
    /// <summary>
    /// EffectView.xaml 的交互逻辑
    /// </summary>
    public class EffectConfigValue
    {

    }

    public partial class EffectView : UserControl
    {
        private AurorasEffect.Sharder.AurorasEffect _effect = new AurorasEffect.Sharder.AurorasEffect();
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public EffectConfigModule.SaveDesignWindow _saveDesignWindow = null;
        private string _defaultCfgPath = "";
        private bool _start = false;
        public Timer _timer = null;
        double _effectBreathe = 0;
        double _effectChangeSpeed = 0;
        double _timeTick = 0;
        private string _dllPath = "";
        int _monitorIndex = 0;

        public bool _bAutoBreathe = false;
        public double _curSpeed = 0.03;
        public double _curBreatheSpeed = 0.03;
        public double _curChangeSpeed = 0.03;
        public double _curAlpha = 1.0;
        public Size _imageSize = new Size(0, 0);
        public BitmapImage _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/AurorasEffect;Component/Resources/1.png"));
        private ViewModel.EffectViewModel vm = null;

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

            _imageSize.Width = _defaultImage.PixelWidth;
            _imageSize.Height = _defaultImage.PixelHeight;
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

        private void EffectView_Unloaded(object sender, RoutedEventArgs e)
        {
           
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

        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            if (_start)
            {
                _effectBreathe += _curBreatheSpeed;
                _effectChangeSpeed += _curChangeSpeed;

                {
                    _timeTick += _curSpeed;
                    _effect.Time = _timeTick;
                    if (_bAutoBreathe)
                    {
                        _effect.Alpha = _curAlpha + (Math.Sin(_effectBreathe) * 0.3);
                    }
                };
            }
        }

        private void EffectView_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = $@"pack://application:,,,/AurorasEffect;Component/Resources/config.json";


                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute))?.Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        var sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "AurorasEffect_v1.0", _monitorIndex);
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

            }
            catch { }
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                if (_start)
                {
                    _effectBreathe += _curBreatheSpeed;
                    _effectChangeSpeed += _curChangeSpeed;

                    {
                        _timeTick += _curSpeed;
                        _effect.Time = _timeTick;
                        if (_bAutoBreathe)
                        {
                            _effect.Alpha = _curAlpha + (Math.Sin(_effectBreathe) * 0.3);
                        }
                    };
                }
            });
        }

        internal void NotifyEffect(Dictionary<string, object> cfg)
        {
            try
            {
                if (cfg == null && cfg.Count == 0)
                    return;
                string command = cfg["command"].ToString();
                switch (command)
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

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                _timer.Start();
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _timer.Stop();
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

        private void RefreshEffect(object obj = null)
        {
            foreach (var item in _effectConfig.items)
            {
                string value = (item.GetValue());
                string key = (item.key);
                OnConfigValueChange(key, value);
            }
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
                case "Direction":
                    {
                    }
                    break;
                case "alpha":
                    {
                        double alpha = (double.Parse(value) / 100);
                        _curAlpha = alpha;
                        _effect.Alpha = _curAlpha;
                    }
                    break;
                case "speed":
                    {
                        double speed = (double.Parse(value) / 100);
                        _curSpeed = speed;
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
            }
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

        private double GetImageRatioHeight(double w)
        {
            return w * this.ActualHeight / this.ActualWidth;
        }

        private double GetImageRatioWidth(double h)
        {
            return h * this.ActualWidth / this.ActualHeight;
        }

    }

}
