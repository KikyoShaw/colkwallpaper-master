using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Media.Imaging;

namespace ImagePluginModule.ViewModel
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected internal virtual void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //注意：值发生变化的时候，才抛通知的
        public bool Set<T>(string propertyName, ref T field, T newValue = default(T))
        {
            try
            {
                if (EqualityComparer<T>.Default.Equals(field, newValue) == false)
                {
                    field = newValue;
                    RaisePropertyChanged(propertyName);
                    return true;
                }
            }
            catch { }
            return false;
        }
    }

    class EffectViewModel : ViewModelBase
    {
        private static Lazy<EffectViewModel> lazyVM = new Lazy<EffectViewModel>(() => new EffectViewModel());
        public static EffectViewModel Instance => lazyVM.Value;

        public Dictionary<string, EffectView> _effectManger = new Dictionary<string, EffectView>();

        private System.Drawing.FontConverter _fontConverter = new System.Drawing.FontConverter();

        public System.Drawing.FontConverter FontConverter => _fontConverter;

        BitmapImage _defaultImage = null;//new BitmapImage(new Uri(@"pack://application:,,,/ImagePluginModule;Component/Resources/1.png"));
        public EffectViewModel()
        {
            _defaultImage = new BitmapImage(new Uri(@"pack://application:,,,/ImagePluginModule;Component/Resources/1.png"));
            _defaultImage.Freeze();
        }

        public BitmapImage GetDefaultBitmap()
        {
            return _defaultImage;
        }

    }
}
