using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ImagePluginModule
{
    public class EffectViewImpl
    {
        private static Dictionary<string, EffectView> _effectManger = new Dictionary<string, EffectView>();

        public static FrameworkElement CreateView(string guid, int monitorIndex, string dllPath)
        {
            if (_effectManger.ContainsKey(guid))
            {
                return _effectManger[guid];
            }
            else
            {
                _effectManger[guid] = new EffectView(monitorIndex, dllPath);
                return _effectManger[guid];
            }
        }

        public static void NotifyEffect(Dictionary<string, object> cfg)
        {
            try
            {
                if (!cfg.ContainsKey("guid"))
                    return;

                string guid = cfg["guid"].ToString();
                if (_effectManger.ContainsKey(guid))
                {
                    _effectManger[guid]?.NotifyEffect(cfg);
                }

            }
            catch { }
        }

    }
}