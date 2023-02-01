using System.Collections.Generic;
using System.Windows;

namespace VirtualCatPlugin
{
    public class EffectViewImpl
    {
        private static readonly Dictionary<string, EffectView> EffectManger = new Dictionary<string, EffectView>();

        public static FrameworkElement CreateView(string guid, int monitorIndex, string dllPath)
        {
            if (EffectManger.ContainsKey(guid))
            {
                return EffectManger[guid];
            }
            else
            {
                EffectManger[guid] = new EffectView(monitorIndex, dllPath);
                return EffectManger[guid];
            }
        }

        public static void NotifyEffect(Dictionary<string, object> cfg)
        {
            try
            {
                if (!cfg.ContainsKey("guid"))
                    return;

                string guid = cfg["guid"].ToString();
                if (guid != null && EffectManger.ContainsKey(guid))
                {
                    EffectManger[guid]?.NotifyEffect(cfg);
                }

            }
            catch { }
        }

    }
}