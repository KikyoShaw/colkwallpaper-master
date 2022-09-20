
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

namespace ImagePluginModule
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
        public EffectConfigModule.MultiConfigWindow _configWindow = null;
        public Thickness _thickness = new Thickness(0,0,0,0);
        public Thickness _diythickness = new Thickness(0,0,0,0);

        public Timer _timer = null;
        Dictionary<string, ImagePlugin> _pluginDict = new Dictionary<string, ImagePlugin>();
        public EffectView(int monitorIndex, string dllPath)
        {
            _monitorIndex = monitorIndex;
            DataContext = vm = ViewModel.EffectViewModel.Instance;
            InitializeComponent();

            _dllPath = dllPath;

            _timer = new Timer(30);
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
                this.Visibility = Visibility.Visible;                
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

        private void OnTimer(object sender, ElapsedEventArgs e)
        {
            System.Windows.Application.Current?.Dispatcher?.InvokeAsync(() =>
            {
                foreach (var ctrlItem in _pluginDict)
                {
                    ctrlItem.Value?.OnAnimationTime();
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
                                HYWEffectConfig.EffectConfigTool.SaveMultiConfig(_effectConfig);
                                var path = cfg["path"].ToString();
                                HYWEffectConfig.EffectConfigTool.SaveMultiConfig(_effectConfig, path, true);
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
                _configWindow = new EffectConfigModule.MultiConfigWindow();
                
                _configWindow.SetDataContext(_effectConfig);
            }
            _configWindow.Closing += ((s, e) =>
            {
            });

            _configWindow.Show();
        }
        
        private void SetPluginPosition()
        {
            //double hov = ((double)_HovPos / 500 * (this.ActualWidth - vm.iShowWidth)) ;
            //double vec = ((double)_VerPos / 500.0 * (this.ActualHeight - vm.iShowHeight)) ;
            //_thickness.Left = hov;
            //_thickness.Top = vec;
            //vm.ShowMargin = _thickness;
        }
        private void InsertNewPlugin(string key)
        {
            if (_pluginDict.ContainsKey(key))
            {
                return;
            }
            var pluginCtrl = new ImagePlugin();

            _pluginDict.Add(key, pluginCtrl);
            this.plugin.Children.Add(pluginCtrl);
            RefreshPlugin(key);
        }

        private void RemovePlugin(string key)
        {
            if (_pluginDict.ContainsKey(key))
            {
                this.plugin.Children.Remove(_pluginDict[key]);
                _pluginDict.Remove(key);
                return;
            }
        }

        //remove all 
        private void ResetPlugin()
        {
            Dictionary<string, ImagePlugin> newDict = new Dictionary<string, ImagePlugin>(); 
            foreach (var pitem in _pluginDict)
            {
                if(pitem.Key != "default")
                {
                    this.plugin.Children.Remove(pitem.Value);
                }
                else
                {
                    newDict.Add(pitem.Key, pitem.Value);
                }
            }
            _pluginDict.Clear();
            _pluginDict = newDict;
        }
        private void OnAddCfg(string key)
        {
            InsertNewPlugin(key);
        }
        private void OnRemoveCfg(string key)
        {
            RemovePlugin(key);
        }
        private void OnSwtichCfg(string key)
        {

        }

        public void RefreshPlugin(string key)
        {
            foreach (var item in _effectConfig.items)
            {
                string keyvalue = _effectConfig.GetMultiValue(key, item.key, item.defaultValue);
                OnValueChange(key, item.key, keyvalue);
            }

        }
        private void RefreshEffect(object obj = null) 
        {
            ResetPlugin();
            foreach(var multiCfg in _effectConfig.vMultiConfigHeadItem)
            {
                InsertNewPlugin(multiCfg.key);
                RefreshPlugin(multiCfg.key);
            }

        }
        private void OnValueChange(string cfgKey, string key, string value)
        {
            if(_pluginDict.ContainsKey(cfgKey))
            {
                _pluginDict[cfgKey].OnValueChange(key, value);
            }
        }
        private void OnConfigValueChange(string key, string value)
        {

            string curConfigKey = _effectConfig.curConfigKey;
            if(string.IsNullOrEmpty(curConfigKey))
            {
                curConfigKey = "default";
            }
            OnValueChange(curConfigKey, key, value);
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
                this.SetValue(Panel.ZIndexProperty, 500);
                string str =$@"pack://application:,,,/ImagePluginModule;Component/Resources/config.json";

                InsertNewPlugin("default");
                string sCfgData = "";
                using (Stream stream = Application.GetResourceStream(new Uri(str, UriKind.RelativeOrAbsolute)).Stream)
                {
                    using (StreamReader r = new StreamReader((Stream)stream, Encoding.UTF8))
                    {
                        sCfgData = r.ReadToEnd();
                        _effectConfig = HYWEffectConfig.EffectConfigTool.EffectMultiConfigParseString(sCfgData, _dllPath, "ImagePluginModule_v1.0", _monitorIndex, 1);

                        _effectConfig.SetConfigAction(OnConfigValueChange, RefreshEffect);
                        _effectConfig.SetMultiConfigAction(OnAddCfg, OnRemoveCfg, OnSwtichCfg);
                        if (!string.IsNullOrEmpty(_defaultCfgPath))
                            _effectConfig.LoadConfigPath(_defaultCfgPath);
                        else
                            RefreshEffect();
                    }
                }
                
            }
            catch { }
        }



        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var pitem in _pluginDict)
                {
                    pitem.Value.RefreshPostion();
                }
            });
        }
    }
}
