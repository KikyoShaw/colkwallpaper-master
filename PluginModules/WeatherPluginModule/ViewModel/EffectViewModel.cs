using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace WeatherPluginModule.ViewModel
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


        private Thickness _ShowMargin = new Thickness(0, 0, 0, 0);
        public Thickness ShowMargin
        {
            get { return _ShowMargin; }
            set { Set("ShowMargin", ref _ShowMargin, value); }
        }

        private int _iShowWidth = 0;
        public int iShowWidth
        {
            get { return _iShowWidth; }
            set { Set("iShowWidth", ref _iShowWidth, value); }
        }

        private int _iShowHeight= 0;
        public int iShowHeight
        {
            get { return _iShowHeight; }
            set { Set("iShowHeight", ref _iShowHeight, value); }
        }

        private string _BackColor = "#ffffff";
        public string BackColor
        {
            get { return _BackColor; }
            set { Set("BackColor", ref _BackColor, value); }
        }
        private int _iBackOpacity = 0;
        public int iBackOpacity
        {
            get { return _iBackOpacity; }
            set { Set("iBackOpacity", ref _iBackOpacity, value); }
        }

        private int _iStyleId = 30;
        public int iStyleId
        {
            get { return _iStyleId; }
            set { Set("iStyleId", ref _iStyleId, value); }
        }
        private string _sCityCode = "";
        public string sCityCode
        {
            get { return _sCityCode; }
            set { Set("sCityCode", ref _sCityCode, value); }
        }
        private string _txtColor = "#ffffff";
        public string txtColor
        {
            get { return _txtColor; }
            set { Set("txtColor", ref _txtColor, value); }
        }
        private int _txtSize = 12;
        public int txtSize
        {
            get { return _txtSize; }
            set { Set("txtSize", ref _txtSize, value); }
        }

        private int _iOpacity = 0;
        public int iOpacity
        {
            get { return _iOpacity; }
            set { Set("iOpacity", ref _iOpacity, value); }
        }

        private string _ShadowColor = "#000000";
        public string ShadowColor
        {
            get { return _ShadowColor; }
            set { Set("ShadowColor", ref _ShadowColor, value); }
        }

        private int _iShadowDirection = 0;
        public int iShadowDirection
        {
            get { return _iShadowDirection; }
            set { Set("iShadowDirection", ref _iShadowDirection, value); }
        }
        private int _iShadowDepth = 0;
        public int iShadowDepth
        {
            get { return _iShadowDepth; }
            set { Set("iShadowDepth", ref _iShadowDepth, value); }
        }
        private int _iShadowOpacity = 0;
        public int iShadowOpacity
        {
            get { return _iShadowOpacity; }
            set { Set("iShadowOpacity", ref _iShadowOpacity, value); }
        }
        private int _iShadowBlurRadius = 0;
        public int iShadowBlurRadius
        {
            get { return _iShadowBlurRadius; }
            set { Set("iShadowBlurRadius", ref _iShadowBlurRadius, value); }
        }
        public EffectViewModel()
        {

        }

    }
}
