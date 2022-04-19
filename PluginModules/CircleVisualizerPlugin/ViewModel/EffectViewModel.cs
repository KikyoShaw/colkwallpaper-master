using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace CircleVisualizerPlugin.ViewModel
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

        private int _iShowHeight = 0;
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
        private int _iOpacity = 0;
        public int iOpacity
        {
            get { return _iOpacity; }
            set { Set("iOpacity", ref _iOpacity, value); }
        }

        private string _PenColor = "#ffffff";
        public string PenColor
        {
            get { return _PenColor; }
            set { Set("PenColor", ref _PenColor, value); }
        }
        private int _iPenSize = 3;
        public int iPenSize
        {
            get { return _iPenSize; }
            set { Set("iPenSize", ref _iPenSize, value); }
        }
        private int _iSwing = 50;
        public int iSwing
        {
            get { return _iSwing; }
            set { Set("iSwing", ref _iSwing, value); }
        }
        public EffectViewModel()
        {

        }

    }
}
