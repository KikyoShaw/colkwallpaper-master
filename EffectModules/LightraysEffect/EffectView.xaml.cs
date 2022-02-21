using LightraysEffect.SharderEffect;
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

namespace LightraysEffect
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
        private Lightrays _effect = new Lightrays();
        private bool _start = false;
        double _timeTick = 0;
        int _onTimeCount = 0;
        double _angleBreathe = 0;
        double _angleChangeSpeed = 0;
        double _autoColor = 0;
        private string _dllPath = "";
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public BitmapImage _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/LightraysEffect;Component/Resources/1.jpg"));
        public EffectConfigModule.ConfigWindow _configWindow = null;

        public bool _bAutoBreathe = false;
        public bool _bAutoChangeColor = false;
        public double _curAlpha = 1.0;
        public double _curSpeed = 0.03;
        public double _curBreatheSpeed = 0.03;
        public double _curChangeSpeed = 0.03;
        public Color _chageColor = new Color();
        public Color _orgColor = new Color();
        public Color _curColor = new Color();
        public List<Color> _argColor = new List<Color>();
        public Timer _timer = null;
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
        }

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                if (_start)
                {
                    _onTimeCount++;
                    _angleBreathe += _curBreatheSpeed;
                    _angleChangeSpeed += _curChangeSpeed;

                    //if (_onTimeCount % 2 == 0)
                    {
                        _timeTick += _curSpeed;
                        _effect.Time = _timeTick;
                        if (_bAutoBreathe)
                        {
                            _effect.Alpha = _curAlpha + (Math.Sin(_angleBreathe)*0.3  );
                        }
                        if (_bAutoChangeColor&& _argColor.Count>0)
                        {
                            Color tmpColor = _argColor[(int)Math.Abs(255.0 * (float)Math.Sin(_angleChangeSpeed))];
                            _effect.Color = tmpColor;
                        }
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
                //CompositionTarget.Rendering += CompositionTarget_Rendering;
            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _timer.Stop();
                //CompositionTarget.Rendering -= CompositionTarget_Rendering;
            }
        }
        private void CompositionTarget_Rendering(object sender, EventArgs e)
        {
            //System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>{
            if (_start)
            {
                _onTimeCount++;
                _angleBreathe += _curBreatheSpeed;
                _angleChangeSpeed += _curChangeSpeed;

                //if (_onTimeCount % 2 == 0)
                {
                    _timeTick += _curSpeed;
                    _effect.Time = _timeTick;
                    if (_bAutoBreathe)
                    {
                        _effect.Alpha = _curAlpha + (Math.Sin(_angleBreathe) * 0.3);
                    }
                    if (_bAutoChangeColor && _argColor.Count > 0)
                    {
                        Color tmpColor = _argColor[(int)Math.Abs(255.0 * (float)Math.Sin(_angleChangeSpeed))];
                        _effect.Color = tmpColor;
                    }
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

        private void SetColorChange()
        {
            int nRp, nGp, nBp, nRcount, nGcount, nBcount;
            nRcount = _orgColor.R * 256;
            nGcount = _orgColor.G * 256;
            nBcount = _orgColor.B * 256;
            nRp = -(nRcount - _chageColor.R * 256) / 255;
            nGp = -(nGcount - _chageColor.G * 256) / 255;
            nBp = -(nBcount - _chageColor.B * 256) / 255;
            int i = 0;
            
            if (_argColor.Count == 0)
            {
                for (; i < 256; i++)
                {
                    _argColor.Add(Color.FromRgb((byte)(nRcount / 256), (byte)(nGcount / 256), (byte)(nBcount / 256)));
                    nRcount += nRp;
                    nGcount += nGp;
                    nBcount += nBp;
                }

            }
            else
            {
                for (; i < 256; i++)
                {
                    _argColor[i] = Color.FromRgb((byte)(nRcount / 256), (byte)(nGcount / 256), (byte)(nBcount / 256));
                    nRcount += nRp;
                    nGcount += nGp;
                    nBcount += nBp;
                }

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
                                    return;
                                }
                            }
                        }

                        {
                            BitmapImage curImage = new BitmapImage(new Uri(@"pack://application:,,,/LightraysEffect;Component/Resources/1.jpg"));
                            //BitmapImage curImage = new BitmapImage(new Uri(value));
                            if (curImage != null)
                            {
                                ImgBk.ImageSource = curImage;
                            }
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
                case "DropDown":
                    {
                        _effect.DropDown = (double)((bool.Parse(value) ? 1.0 : 0.0));
                    }
                    break;
                case "color":
                    {
                        _orgColor = (Color)ColorConverter.ConvertFromString(value);
                        _effect.Color = _orgColor;
                        if(_bAutoChangeColor)
                        {
                            SetColorChange();
                        }
                    }
                    break;
                case "userChangeColor":
                    {
                        _bAutoChangeColor = bool.Parse(value);
                        if (!_bAutoChangeColor)
                        {
                            _effect.Color = _orgColor;
                            _curColor = _orgColor;
                        }
                        else
                        {
                            SetColorChange();
                        }
                    }
                    break;
                case "changeColor":
                    {
                        Color crColor = (Color)ColorConverter.ConvertFromString(value);
                        _chageColor = crColor;
                        if (_bAutoChangeColor)
                        {
                            SetColorChange();
                        }
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
                        //_effect.Alpha = _curAlpha;
                    }
                    break;
                case "orgX":
                    {
                        double orgX = (double.Parse(value) / 100);                        
                        _effect.OrgX = orgX;
                    }
                    break;
                case "breathe":
                    {
                        _bAutoBreathe = bool.Parse(value);
                        if (!_bAutoBreathe)
                        {
                            _effect.Alpha = _curAlpha; 
                        }
                    }
                    break;
                case "breatheSpeed":
                    {
                        _curBreatheSpeed = (double.Parse(value) / 100);
                    }
                    break;
                case "changeColorSpeed":
                    {
                        _curChangeSpeed = (double.Parse(value) / 100);
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
                string str =$@"pack://application:,,,/LightraysEffect;Component/Resources/config.json";


                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "LightraysEffect_v1.0", _monitorIndex);
                        _effectConfig.LoadConfigPath(_defaultCfgPath);
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
        private void EffectView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            if (_effect != null)
            {
                _effect.ResolutionX = this.ActualWidth;
                _effect.ResolutionY = this.ActualHeight;
            }
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
    }
}
