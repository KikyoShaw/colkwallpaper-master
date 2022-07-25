using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;

namespace DefaultPluginModule.ViewModel
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
        private bool _bShowDiyText = false;

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
        private int _iShowDiyWidth = 0;
        public int iShowDiyWidth
        {
            get { return _iShowDiyWidth; }
            set { Set("iShowDiyWidth", ref _iShowDiyWidth, value); }
        }

        private int _iShowDiyHeight = 0;
        public int iShowDiyHeight
        {
            get { return _iShowDiyHeight; }
            set { Set("iShowDiyHeight", ref _iShowDiyHeight, value); }
        }
        public bool bShowDiyText
        {
            get { return _bShowDiyText; }
            set { Set("bShowDiyText", ref _bShowDiyText, value); }
        }
        private bool _bShowDate = false;
        public bool bShowDate
        {
            get { return _bShowDate; }
            set { Set("bShowDate", ref _bShowDate, value); }
        }
        private bool _bShowTime = false;
        public bool bShowTime
        {
            get { return _bShowTime; }
            set { Set("bShowTime", ref _bShowTime, value); }
        }
        private bool _bShowWeek = false;
        public bool bShowWeek
        {
            get { return _bShowWeek; }
            set { Set("bShowWeek", ref _bShowWeek, value); }
        }
        private bool _bShowAnte = false;
        public bool bShowAnte
        {
            get { return _bShowAnte; }
            set { Set("bShowAnte", ref _bShowAnte, value); }
        }


        private string _sDiyText = "";
        public string sDiyText
        {
            get { return _sDiyText; }
            set { Set("sDiyText", ref _sDiyText, value); }
        }

        private DateTime _CurDateTime = DateTime.Now;
        public DateTime CurDateTime
        {
            get { return _CurDateTime; }
            set { Set("CurDateTime", ref _CurDateTime, value); }
        }

        private int _iOpacity = 0;
        public int iOpacity
        {
            get { return _iOpacity; }
            set { Set("iOpacity", ref _iOpacity, value); }
        }
        private int _iDiyOpacity = 0;
        public int iDiyOpacity
        {
            get { return _iDiyOpacity; }
            set { Set("iDiyOpacity", ref _iDiyOpacity, value); }
        }

        private System.Drawing.Font _showDiyfont = new System.Drawing.Font(
            new System.Drawing.FontFamily("Microsoft YaHei UI"), 25, System.Drawing.FontStyle.Bold);
        public System.Drawing.Font showDiyfont
        {
            get { return _showDiyfont; }
            set { Set("showDiyfont", ref _showDiyfont, value); }
        }

        private System.Drawing.Font _showfont = new System.Drawing.Font(
            new System.Drawing.FontFamily("Microsoft YaHei UI"), 25, System.Drawing.FontStyle.Bold);
        public System.Drawing.Font showfont
        {
            get { return _showfont; }
            set { Set("showfont", ref _showfont, value); }
        }
        private float _fFontSize = 25;
        public float fFontSize
        {
            get { return _fFontSize; }
            set { Set("fFontSize", ref _fFontSize, value); }
        }
        private float _ffDiyFontSize = 25;
        public float fDiyFontSize
        {
            get { return _ffDiyFontSize; }
            set { Set("fDiyFontSize", ref _ffDiyFontSize, value); }
        }
        private float _fDateFontSize = 25;
        public float fDateFontSize
        {
            get { return _fDateFontSize; }
            set { Set("fDateFontSize", ref _fDateFontSize, value); }
        }
        private float _fTimeFontSize = 25;
        public float fTimeFontSize
        {
            get { return _fTimeFontSize; }
            set { Set("fTimeFontSize", ref _fTimeFontSize, value); }
        }
        private string _backColor = "#ffffff";
        public string backColor
        {
            get { return _backColor; }
            set { Set("backColor", ref _backColor, value); }
        }
        private string _diyBackColor = "#ffffff";
        public string diyBackColor
        {
            get { return _diyBackColor; }
            set { Set("diyBackColor", ref _diyBackColor, value); }
        }
        private string _txtColor = "#ffffff";
        public string txtColor
        {
            get { return _txtColor; }
            set { Set("txtColor", ref _txtColor, value); }
        }
        private string _DateColor = "#ffffff";
        public string DateColor
        {
            get { return _DateColor; }
            set { Set("DateColor", ref _DateColor, value); }
        }
        private string _TimeColor = "#ffffff";
        public string TimeColor
        {
            get { return _TimeColor; }
            set { Set("TimeColor", ref _TimeColor, value); }
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
        private int _iShadowDepth= 0;
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
        private Thickness _ShowDiyMargin = new Thickness(0, 0, 0, 0);
        public Thickness ShowDiyMargin
        {
            get { return _ShowDiyMargin; }
            set { Set("ShowDiyMargin", ref _ShowDiyMargin, value); }
        }

        private Thickness _ShowMargin = new Thickness(0, 0, 0, 0);
        public Thickness ShowMargin
        {
            get { return _ShowMargin; }
            set { Set("ShowMargin", ref _ShowMargin, value); }
        }
        private int _iShadowBlurRadius = 0;
        public int iShadowBlurRadius
        {
            get { return _iShadowBlurRadius; }
            set { Set("iShadowBlurRadius", ref _iShadowBlurRadius, value); }
        }

        private string _sDateFormat= "yyyy/MM/dd";
        public string sDateFormat
        {
            get { return _sDateFormat; }
            set { Set("sDateFormat", ref _sDateFormat, value); }
        }
        private string _sTimeFormat = "HH:mm:ss";
        public string sTimeFormat
        {
            get { return _sTimeFormat; }
            set { Set("sTimeFormat", ref _sTimeFormat, value); }
        }
        private int _iWeekFormat = 0;
        public int iWeekFormat
        {
            get { return _iWeekFormat; }
            set { Set("iWeekFormat", ref _iWeekFormat, value); }
        }
        private string _sShowDateTime1 = "";
        public string sShowDateTime1
        {
            get { return _sShowDateTime1; }
            set { Set("sShowDateTime1", ref _sShowDateTime1, value); }
        }
        private string _sShowDateTime2 = "";
        public string sShowDateTime2
        {
            get { return _sShowDateTime2; }
            set { Set("sShowDateTime2", ref _sShowDateTime2, value); }
        }
        private int _iAnteFomrat = 0;
        public int iAnteFomrat
        {
            get { return _iAnteFomrat; }
            set { Set("iAnteFomrat", ref _iAnteFomrat, value); }
        }
        private bool _bFirstShowTime = true;
        public bool bFirstShowTime
        {
            get { return _bFirstShowTime; }
            set { Set("bFirstShowTime", ref _bFirstShowTime, value); }
        }
        private bool _bFirstShowAnte = true;
        public bool bFirstShowAnte
        {
            get { return _bFirstShowAnte; }
            set { Set("bFirstShowAnte", ref _bFirstShowAnte, value); }
        }
        public EffectViewModel()
        {

        }

    }
}
