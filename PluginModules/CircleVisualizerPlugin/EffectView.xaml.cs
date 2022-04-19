using CircleVisualizerPlugin.Shaders;
using EffectConfigModule.SpectrumHelp;
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

namespace CircleVisualizerPlugin
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
        ColorKeyAlphaEffect4 _effect = new ColorKeyAlphaEffect4();
        private bool _start = false;
        double _timeTick = 0;
        private string _dllPath = "";
        private string _defaultCfgPath = "";
        public HYWEffectConfig.EffectConfigInfo _effectConfig = new HYWEffectConfig.EffectConfigInfo();
        public EffectConfigModule.ConfigWindow _configWindow = null;
        public BitmapImage _noise = new BitmapImage(new Uri(@"pack://application:,,,/CircleVisualizerPlugin;Component/Resources/ColorRange.png"));
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
        double _curHue = 0;
        int _showPercent = 0;
        bool _bAutoHue = false;
        CircleVisualizer _SpectrumrCircleCtrl =new CircleVisualizer();
        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;

            _timer = new Timer(1000);
            _timer.Elapsed += OnTimer;
            //_timer.Start();
            _SpectrumrCircleCtrl.Init();

            this.Loaded += EffectView_Loaded;
            this.Unloaded += EffectView_Unloaded;

            _effect.Input1 = new ImageBrush(_noise);
            _effect.Tolerance = 1.0;
            main.Effect = _effect;

        }

        private void Start()
        {
            if (!_start)
            {
                _start = !_start;
                _timer.Start();
                this.Visibility = Visibility.Visible;
                _SpectrumrCircleCtrl.Start();

            }
        }
        private void Stop()
        {
            if (_start)
            {
                _start = !_start;
                _SpectrumrCircleCtrl.Stop();
                _timer.Stop();
                this.Visibility = Visibility.Collapsed;
            }
        }
        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                if (_bAutoHue)
                {
                    if (_effect.Hue + 0.5 > 360.0)
                        _effect.Hue = 0;
                    else
                        _effect.Hue += 0.5;
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

        private void SetPluginPosition()
        {
            double size = Math.Min(this.ActualWidth, this.ActualHeight);
            int realSize = (int)(_showPercent*size / 1000.0);
            vm.iShowWidth = realSize;
            vm.iShowHeight = realSize;
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
                case "LineColor":
                    {
                        vm.PenColor = (value);
                        _effect.ColorKey = (Color)ColorConverter.ConvertFromString(value); 
                    }
                    break;
                case "LineSize":
                    {
                        vm.iPenSize = int.Parse(value);
                    }
                    break;
                case "LineSwing":
                    {
                        vm.iSwing = int.Parse(value);
                    }
                    break;
                case "Opacity":
                    {
                        vm.iOpacity = int.Parse(value);
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
                case "ShowSize":
                    {
                        _showPercent = int.Parse(value);
                        updatePostion = true;
                    }
                    break;
                case "Colorize":
                    {
                        _effect.Colorize = bool.Parse(value) ? 1.0 : 0.0;
                    }
                    break;
                case "Hue":
                    {
                        _curHue = (double)((int.Parse(value))) / 10.0;;
                        if (!_bAutoHue)
                            _effect.Hue = _curHue;

                    }
                    break;
                case "Sat":
                    {
                        _effect.Sat =  (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "Lum":
                    {
                        _effect.Lum =  (double)((int.Parse(value))) / 1000.0;
                    }
                    break;
                case "Tolerance":
                    {
                        _effect.Tolerance = (1000- int.Parse(value)) / 1000.0;
                    }
                    break;
                case "AtuoHue":
                    {
                        _bAutoHue = bool.Parse(value);
                        if (!_bAutoHue)
                        {
                            _effect.Hue = _curHue;
                        }
                    }
                    break;
                case "Shake":
                    {
                        _SpectrumrCircleCtrl.Shake = bool.Parse(value);
                    }
                    break;
                case "ShakeSpeed":
                    {
                        _SpectrumrCircleCtrl.offsetSpeed = float.Parse(value) / (float)100.0;
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
                main.Children.Add(_SpectrumrCircleCtrl);

                string str =$@"pack://application:,,,/CircleVisualizerPlugin;Component/Resources/config.json";


                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectConfigParseString(sCfgData, _dllPath, "CircleVisualizerPlugin_v1.0", _monitorIndex, 1);
                        
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
