using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;

namespace Earth3DPlugin.ViewModel
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
        private static readonly Lazy<EffectViewModel> LazyVm = new Lazy<EffectViewModel>(() => new EffectViewModel());
        public static EffectViewModel Instance => LazyVm.Value;

        public Dictionary<string, EffectView> EffectManger = new Dictionary<string, EffectView>();

        public System.Drawing.FontConverter FontConverter { get; } = new System.Drawing.FontConverter();


        private Thickness _showMargin = new Thickness(0, 0, 0, 0);
        public Thickness ShowMargin
        {
            get => _showMargin;
            set => Set("ShowMargin", ref _showMargin, value);
        }

        private double _iShowWidth = 0;
        public double ShowWidth
        {
            get => _iShowWidth;
            set => Set("ShowWidth", ref _iShowWidth, value);
        }

        private double _iShowHeight= 0;
        public double ShowHeight
        {
            get => _iShowHeight;
            set => Set("ShowHeight", ref _iShowHeight, value);
        }

        private string _backColor = "#ffffff";
        public string BackColor
        {
            get => _backColor;
            set => Set("BackColor", ref _backColor, value);
        }
        private int _iBackOpacity = 0;
        public int BackOpacity
        {
            get => _iBackOpacity;
            set => Set("BackOpacity", ref _iBackOpacity, value);
        }

        private int _iStyleId = 30;
        public int StyleId
        {
            get => _iStyleId;
            set => Set("StyleId", ref _iStyleId, value);
        }
        private string _sCityCode = "";
        public string CityCode
        {
            get => _sCityCode;
            set => Set("CityCode", ref _sCityCode, value);
        }
        private string _txtColor = "#ffffff";
        public string TxtColor
        {
            get => _txtColor;
            set => Set("TxtColor", ref _txtColor, value);
        }
        private int _txtSize = 12;
        public int TxtSize
        {
            get => _txtSize;
            set => Set("TxtSize", ref _txtSize, value);
        }

        private int _iOpacity = 0;
        public int Opacity
        {
            get => _iOpacity;
            set => Set("Opacity", ref _iOpacity, value);
        }

        private string _shadowColor = "#000000";
        public string ShadowColor
        {
            get => _shadowColor;
            set => Set("ShadowColor", ref _shadowColor, value);
        }

        private int _iShadowDirection = 0;
        public int ShadowDirection
        {
            get => _iShadowDirection;
            set => Set("ShadowDirection", ref _iShadowDirection, value);
        }
        private int _iShadowDepth = 0;
        public int ShadowDepth
        {
            get => _iShadowDepth;
            set => Set("ShadowDepth", ref _iShadowDepth, value);
        }
        private int _iShadowOpacity = 0;
        public int ShadowOpacity
        {
            get => _iShadowOpacity;
            set => Set("ShadowOpacity", ref _iShadowOpacity, value);
        }
        private int _iShadowBlurRadius = 0;
        public int ShadowBlurRadius
        {
            get => _iShadowBlurRadius;
            set => Set("ShadowBlurRadius", ref _iShadowBlurRadius, value);
        }
        public EffectViewModel()
        {

        }

    }
}
